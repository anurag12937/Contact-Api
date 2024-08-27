using ContactsManagement.Core.Interfaces;
using ContactsManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContatcsManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactsManagementService _contactService;

        public ContactController(ILogger<ContactController> logger, IContactsManagementService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet]
        [Route("GetContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseModel>> GetContacts()
        {
            var response = await _contactService.GetAsync().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }
        [HttpPost]
        [Route("CreateAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateAsync(ContactsModel contactsModel)
        {
            var fluentValidator = contactsModel.Validate();
            if (!fluentValidator.IsValid) return BadRequest(fluentValidator.Errors.FirstOrDefault().ErrorMessage);
            var response = await _contactService.CreateAsync(contactsModel);
            return response != null ? Ok(response) : NotFound();
        }

        [HttpPut]
        [Route("UpdateAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateAsync(ContactsModel contactsModel)
        {
            var fluentValidator = contactsModel.Validate();
            if (!fluentValidator.IsValid) return BadRequest(fluentValidator.Errors.FirstOrDefault().ErrorMessage);

            var response = await _contactService.UpdateAsync(contactsModel);
            return response != null ? Ok(response) : NotFound();
        }
        [HttpDelete]
        [Route("DeleteAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            if (id == 0) { return BadRequest(); }

            var response = await _contactService.DeleteAsync(id);
            return response != null ? Ok(response) : NotFound();
        }

    }
}
