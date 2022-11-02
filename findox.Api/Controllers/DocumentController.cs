using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace findox.Api.Controllers
{
/// <summary>
    /// The documents controller.
    /// </summary>
    [Route("v1/documents")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IPermissionService _permissionService;

        public DocumentController(IDocumentService documentService, IPermissionService permissionService)
        {
            _documentService = documentService;
            _permissionService = permissionService;
        }

        #region CREATE

        [HttpPost("")]
        [Authorize(Roles = "manager, admin")]
        public async Task<ActionResult> Create(IFormCollection metadata, IFormFile file)
        {
            var serviceRequest = new DocumentServiceRequest(metadata, file, User);
            var serviceResponse = await _documentService.Create(serviceRequest);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion CREATE

        #region READ

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult> ReadAll()
        {
            var serviceRequest = new DocumentServiceRequest() { PrincipalUser = User };
            var serviceResponse = await _documentService.ReadAllPermissioned(serviceRequest);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> ReadById(long id)
        {
            var serviceRequest = new DocumentServiceRequest() { Id = id, PrincipalUser = User };
            var serviceResponse = await _documentService.ReadByIdPermissioned(serviceRequest);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpGet("/v1/documents/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadPermissionsByDocumentId(long id)
        {
            var serviceResponse = await _permissionService.ReadByDocumentId(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion READ

        #region UPDATE

        [HttpPut("/v1/documents/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] DocumentDto documentDto)
        {
            if (id != documentDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {documentDto.Id}");

            var serviceRequest = new DocumentServiceRequest(documentDto);
            var serviceResponse = await _documentService.Update(serviceRequest);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion UPDATE

        #region DELETE

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteById(long id)
        {
            var serviceRequest = new DocumentServiceRequest(id);
            var serviceResponse = await _documentService.DeleteById(serviceRequest);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/documents/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByDocumentId(long id)
        {
            var serviceResponse = await _permissionService.DeleteByDocumentId(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion DELETE
    }
}