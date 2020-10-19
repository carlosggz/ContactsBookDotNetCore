using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ContactsBook.Api.Helpers;
using ContactsBook.Application.Services;
using ContactsBook.Application.Services.AddContact;
using ContactsBook.Common.Events;
using ContactsBook.Common.Logger;
using ContactsBook.Common.Mailer;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain;
using ContactsBook.Domain.Contacts;
using ContactsBook.Infrastructure.EventsBus;
using ContactsBook.Infrastructure.Logger;
using ContactsBook.Infrastructure.Mailer;
using ContactsBook.Infrastructure.Repositories.EF;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ContactsBook.Api
{
    public class Startup
    {
        const string PolicyName = "localhost";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(AddContactCommand));

            services.AddControllers();
            
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Contacts Book API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            var section = Configuration.GetSection("SmtpSettings");
            services.Configure<SmtpConfiguration>(section);
            
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped<IEventBus>(x => new EventBus(ReflectionHelpers.EventsSubscribers));
            services.AddScoped<IAppLogger>(x => new NLogLogger(Configuration.GetValue<string>("App:LoggerName")));
            services.AddScoped<IMailer>(x => new SmtpMailer(section.Get<SmtpConfiguration>()));
            services.AddScoped<IContactsRepository, ContactsRepository>();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    PolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            services
                .AddDbContext<ContactsBookContext>(options => {
                    if (Environment.IsEnvironment("Testing")) 
                        options.UseInMemoryDatabase(databaseName: "Test");
                    else 
                        options.UseSqlServer(Configuration["App:ConnectionString"]);
                  }, ServiceLifetime.Scoped);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                app.UseDeveloperExceptionPage();
                ApplyMigrations(app);
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts Book API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(PolicyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ApplyMigrations(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetService<IAppLogger>();
                

                using (var context = serviceScope.ServiceProvider.GetService<ContactsBookContext>())
                {
                    logger.Debug("Running migrations...");
                    context.Database.Migrate();
                    logger.Debug("Migrations completed");
                }                
            }
        }
    }
}
