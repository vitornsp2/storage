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
            var serviceResponse = await _userService.Create(userDto);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpPost("/v1/users/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUserUserGroup([FromRoute] long id, [FromBody] UserGroupDto userGroupDto)
        {
            if (id != userGroupDto.UserId) return BadRequest($"Id mismatch: Route {id} ≠ Body {userGroupDto.UserId}");

            var serviceResponse = await _userGroupService.Create(userGroupDto);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpPost("/v1/users/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUserPermission([FromRoute] long id, [FromBody] PermissionDto permissionDto)
        {
            if (id != permissionDto.UserId) return BadRequest($"Id mismatch: Route {id} ≠ Body {permissionDto.UserId}");

            var serviceResponse = await _permissionService.Create(permissionDto);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        #endregion CREATE

        #region READ

        [HttpGet("")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadAll()
        {
            var serviceResponse = await _userService.ReadAll();

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadById(long? id)
        {
            var serviceResponse = await _userService.ReadById(id);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        #endregion READ

        #region UPDATE

        [HttpPut("/v1/users/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {userDto.Id}");

            var serviceResponse = await _userService.Update(userDto);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpPut("/v1/users/{id}/password")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdatePassword([FromRoute] long id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {userDto.Id}");

            var serviceResponse = await _userService.UpdatePassword(userDto);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        #endregion UPDATE

        #region DELETE

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteById(long? id)
        {
            var serviceResponse = await _userService.DeleteById(id);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/users/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupsByUserId(long id)
        {
            var serviceResponse = await _userGroupService.DeleteByUserId(id);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/users/{uid}/userGroups/{mid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupByUserGroupId(long uid, long mid)
        {
            var serviceResponse = await _userGroupService.DeleteByGroupId(mid);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }


        [HttpDelete("/v1/users/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByUserId(long id)
        {
            var serviceResponse = await _permissionService.DeleteByUserId(id);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/users/{uid}/permissions/{pid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionByPermissionId(long uid, long pid)
        {
            var serviceResponse = await _permissionService.DeleteById(pid);

            if (serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if (serviceResponse.hasValidationErros)
                return StatusCode(500, serviceResponse);

            return Ok(serviceResponse);
        }

        #endregion DELETE
    }
}