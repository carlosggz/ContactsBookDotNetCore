using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsBook.Api.Models;
using ContactsBook.Application.Dtos;
using ContactsBook.Application.Exceptions;
using ContactsBook.Application.Services;
using ContactsBook.Application.Services.AddContact;
using ContactsBook.Application.Services.DeleteContact;
using ContactsBook.Application.Services.GetContact;
using ContactsBook.Application.Services.GetContacts;
using ContactsBook.Application.Services.UpdateContact;
using ContactsBook.Common.Exceptions;
using ContactsBook.Common.Logger;
using ContactsBook.Common.Mailer;
using ContactsBook.Common.Repositories;
using ContactsBook.Domain.Common;
using ContactsBook.Domain.Contacts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContactsBook.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly IAppLogger _logger;
        private readonly IMediator _mediator;

        public ContactsController(IAppLogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Add a new contact
        /// </summary>
        /// <param name="model">Contact details</param>
        /// <returns>Id of the new contact</returns>
        [HttpPost]
        public async Task<ActionResult<ApiContactResultModel>> Add([FromBody] ContactsModel model)
        {
            var result = new ApiContactResultModel();
            var contactId = new IdValueObject().Value;

            try
            {
                model.Id = contactId;
                await _mediator.Send(new AddContactCommand(model));
                result.Success = true;
                result.ContactId = contactId;
                return Ok(result);
            }
            catch (InvalidEntityException ex)
            {
                result.Errors = new List<string> { ex.Message };
                return BadRequest(result);
            }
            catch (EntityValidationException ex)
            {
                result.Errors = ex.ValidationResults.Select(x => x.ErrorMessage);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                result.Errors = new List<string> { ex.Message };
                _logger.Error(ex, "Error adding contact");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Update a contact details
        /// </summary>
        /// <param name="model">Contact details</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiContactResultModel>> Update([FromBody] ContactsModel model)
        {
            var result = new ApiContactResultModel();

            try
            {
                await _mediator.Send(new UpdateContactCommand(model));
                result.Success = true;
                result.ContactId = model.Id;
                return Ok(result);
            }
            catch (InvalidEntityException ex)
            {
                result.Errors = new List<string> { ex.Message };
                return BadRequest(result);
            }
            catch (EntityValidationException ex)
            {
                result.Errors = ex.ValidationResults.Select(x => x.ErrorMessage);
                return BadRequest(result);
            }
            catch (EntityNotFound ex)
            {
                result.Errors = new List<string> { ex.Message };
                return NotFound(result);
            }
            catch (Exception ex)
            {
                result.Errors = new List<string> { ex.Message };
                _logger.Error(ex, "Error updating contact");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiContactResultModel>> Delete([FromRoute] string id)
        {
            var result = new ApiContactResultModel();

            try
            {
                await _mediator.Send(new DeleteContactCommand(id));
                result.Success = true;
                result.ContactId = id;
                return Ok(result);
            }
            catch (InvalidEntityException ex)
            {
                result.Errors = new List<string> { ex.Message };
                return BadRequest(result);
            }
            catch (EntityNotFound ex)
            {
                result.Errors = new List<string> { ex.Message };
                return NotFound(result);
            }
            catch (Exception ex)
            {
                result.Errors = new List<string> { ex.Message };
                _logger.Error(ex, "Error deleting contact");
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Get the details of a contact
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ContactsModel>> Get([FromRoute] string id)
        {
            try
            {
                var contact = await _mediator.Send(new GetContactQuery(id));
                return Ok(contact);
            }
            catch (InvalidEntityException)
            {
                return BadRequest();
            }
            catch (EntityNotFound)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting contact");
                return BadRequest();
            }
        }

        /// <summary>
        /// Search contacts
        /// </summary>
        /// <param name="criteria">Criteria</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchResults<ContactDto>>> Search([FromBody] ContactsSearchCriteriaModel criteria)
        {
            try
            {
                var results = await _mediator.Send(new GetContactsQuery(criteria.PageNumber, criteria.PageSize, criteria.Text));
                return Ok(results);
            }
            catch (InvalidParametersException)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error searching contacts");
                return BadRequest();
            }
        }
    }
}
