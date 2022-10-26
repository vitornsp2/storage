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
            try
            {
                var serviceRequest = new DocumentServiceRequest(metadata, file, User);
                var serviceResponse = await _documentService.Create(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success(serviceResponse.Item);
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        #endregion CREATE

        #region READ

        [HttpGet("")]
        [Authorize]
        public async Task<ActionResult> ReadAll()
        {
            try
            {
                var serviceRequest = new DocumentServiceRequest() { PrincipalUser = User };
                var serviceResponse = await _documentService.ReadAllPermissioned(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success(serviceResponse.List);
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> ReadById(long id)
        {
            try
            {
                var serviceRequest = new DocumentServiceRequest() { Id = id, PrincipalUser = User };
                var serviceResponse = await _documentService.ReadByIdPermissioned(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        if (serviceResponse.FileContentResult is not null) return serviceResponse.FileContentResult;
                        else return StatusCode(500, response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpGet("/v1/documents/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadPermissionsByDocumentId(long id)
        {
            try
            {
                var serviceRequest = new PermissionServiceRequest(id);
                var serviceResponse = await _permissionService.ReadByDocumentId(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success(serviceResponse.List);
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        #endregion READ

        #region UPDATE

        [HttpPut("/v1/documents/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] DocumentDto documentDto)
        {
            try
            {
                if (id != documentDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {documentDto.Id}");

                var serviceRequest = new DocumentServiceRequest(documentDto);
                var serviceResponse = await _documentService.Update(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success("Document updated.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        #endregion UPDATE

        #region DELETE

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteById(long id)
        {
            try
            {
                var serviceRequest = new DocumentServiceRequest(id);
                var serviceResponse = await _documentService.DeleteById(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success("Document deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpDelete("/v1/documents/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByDocumentId(long id)
        {
            try
            {
                var serviceRequest = new PermissionServiceRequest(id);
                var serviceResponse = await _permissionService.DeleteByDocumentId(serviceRequest);
                var response = new ControllerResponse();
                switch (serviceResponse.Outcome)
                {
                    case OutcomeType.Error:
                        response.Error();
                        return StatusCode(500, response);
                    case OutcomeType.Fail:
                        response.Fail(serviceResponse.ErrorMessage);
                        return BadRequest(response);
                    case OutcomeType.Success:
                        response.Success("Permissions deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        #endregion DELETE
    }
}