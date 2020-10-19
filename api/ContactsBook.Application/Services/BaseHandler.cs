using ContactsBook.Application.Dtos;
using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using System;
using ContactsBook.Common.Exceptions;
using ContactsBook.Application.Exceptions;
using System.Linq;

namespace ContactsBook.Application.Services
{
    public abstract class BaseHandler
    {
        public BaseHandler(IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            UnitOfWork = unitOfWork;
            EventBus = eventBus;
        }

        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IEventBus EventBus { get; private set; }


        protected void ValidateEntity(IModel model)
        {
            if (model == null)
                throw new InvalidEntityException("Invalid entity");

            var validations = model.GetValidations();

            if (validations.Any())
                throw new EntityValidationException(validations);
        }

        protected void UoWExecute(Action action)
        {
            UnitOfWork.StartChanges();

            try
            {
                action();

                UnitOfWork.CommitChanges();
            }
            catch (Exception)
            {
                UnitOfWork.RollbackChanges();
                EventBus.DiscardEvents();
                throw;
            }

            EventBus.PublishAsync();
        }
    }
}
