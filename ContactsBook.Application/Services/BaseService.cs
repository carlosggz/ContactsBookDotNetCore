using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Common.Events;
using ContactsBook.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactsBook.Application.Services
{
    public class BaseService
    {
        public BaseService(IUnitOfWork unitOfWork, IEventBus eventBus)
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
                throw;
            }

            EventBus.PublishAsync();
        }
    }
}
