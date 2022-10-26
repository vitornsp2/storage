using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace findox.Api.Controllers
{
    [Route("v1/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        public UserController(IUserGroupService userGroupService, IPermissionService permissionService, IUserService userService)
        {
            _userGroupService = userGroupService;
            _permissionService = permissionService;
            _userService = userService;
        }

        #region CREATE

        [HttpPost("")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                var serviceRequest = new UserServiceRequest(userDto);
                var serviceResponse = await _userService.Create(serviceRequest);
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

        [HttpPost("/v1/users/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUserUserGroup([FromRoute] long id, [FromBody] UserGroupDto userGroupDto)
        {
            try
            {
                if (id != userGroupDto.UserId) return BadRequest($"Id mismatch: Route {id} ≠ Body {userGroupDto.UserId}");

                var serviceRequest = new UserGroupServiceRequest(userGroupDto);
                var serviceResponse = await _userGroupService.Create(serviceRequest);
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

        [HttpPost("/v1/users/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUserPermission([FromRoute] long id, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                if (id != permissionDto.UserId) return BadRequest($"Id mismatch: Route {id} ≠ Body {permissionDto.UserId}");

                var serviceRequest = new PermissionServiceRequest(permissionDto);
                var serviceResponse = await _permissionService.Create(serviceRequest);
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
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadAll()
        {
            try
            {
                var serviceRequest = new UserServiceRequest();
                var serviceResponse = await _userService.ReadAll(serviceRequest);
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
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadById(long id)
        {
            try
            {
                var serviceRequest = new UserServiceRequest(id);
                var serviceResponse = await _userService.ReadById(serviceRequest);
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
                        response.Success(serviceResponse.AllDto);
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

        [HttpPut("/v1/users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] UserDto userDto)
        {
            try
            {
                if (id != userDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {userDto.Id}");

                var serviceRequest = new UserServiceRequest(userDto);
                var serviceResponse = await _userService.Update(serviceRequest);
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
                        response.Success("User updated.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpPut("/v1/users/{id}/password")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdatePassword([FromRoute] long id, [FromBody] UserDto userDto)
        {
            try
            {
                if (id != userDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {userDto.Id}");

                var serviceRequest = new UserServiceRequest(userDto);
                var serviceResponse = await _userService.UpdatePassword(serviceRequest);
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
                        response.Success("User password updated.");
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
                var serviceRequest = new UserServiceRequest(id);
                var serviceResponse = await _userService.DeleteById(serviceRequest);
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
                        response.Success("User deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpDelete("/v1/users/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupsByUserId(long id)
        {
            try
            {
                var serviceRequest = new UserGroupServiceRequest(id);
                var serviceResponse = await _userGroupService.DeleteByUserId(serviceRequest);
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
                        response.Success("UserGroups deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpDelete("/v1/users/{uid}/userGroups/{mid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupByUserGroupId(long uid, long mid)
        {
            try
            {
                var serviceRequest = new UserGroupServiceRequest(mid);
                var serviceResponse = await _userGroupService.DeleteById(serviceRequest);
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
                        response.Success("UserGroup deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }


        [HttpDelete("/v1/users/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByUserId(long id)
        {
            try
            {
                var serviceRequest = new PermissionServiceRequest(id);
                var serviceResponse = await _permissionService.DeleteByUserId(serviceRequest);
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

        [HttpDelete("/v1/users/{uid}/permissions/{pid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionByPermissionId(long uid, long pid)
        {
            try
            {
                var serviceRequest = new PermissionServiceRequest(pid);
                var serviceResponse = await _permissionService.DeleteById(serviceRequest);
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
                        response.Success("Permission deleted.");
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