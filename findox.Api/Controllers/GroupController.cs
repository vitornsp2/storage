using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Interfaces.Service;
using findox.Domain.Models.Dto;
using findox.Domain.Models.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace findox.Api.Controllers
{
    [Route("v1/groups")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {

        private readonly IGroupService _groupService;
        private readonly IUserGroupService _userGroupService;
        private readonly IPermissionService _permissionService;

        public GroupController(IGroupService groupService, IUserGroupService userGroupService, IPermissionService permissionService)
        {
            _groupService = groupService;
            _userGroupService = userGroupService;
            _permissionService = permissionService;
        }

        #region CREATE

        [HttpPost("")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create([FromBody] GroupDto groupDto)
        {
            var serviceResponse = await _groupService.Create(groupDto);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpPost("/v1/groups/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateGroupPermission([FromRoute] long id, [FromBody] PermissionDto permissionDto)
        {
            if (id != permissionDto.GroupId) return BadRequest($"Id mismatch: Route {id} ≠ Body {permissionDto.GroupId}");

            var serviceResponse = await _permissionService.Create(permissionDto);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion CREATE

        #region READ

        [HttpGet("")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadAll()
        {
            var serviceResponse = await _groupService.ReadAll();
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ReadById(long id)
        {
            var serviceResponse = await _groupService.ReadById(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion READ

        #region UPDATE

        [HttpPut("/v1/groups/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] GroupDto groupDto)
        {
            if (id != groupDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {groupDto.Id}");

            var serviceResponse = await _groupService.Update(groupDto);
            
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
            var serviceResponse = await _groupService.DeleteById(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/groups/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupsByGroupId(long id)
        {
            var serviceResponse = await _userGroupService.DeleteByGroupId(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/groups/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByGroupId(long id)
        {
            var serviceResponse = await _permissionService.DeleteByGroupId(id);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        [HttpDelete("/v1/groups/{gid}/permissions/{pid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionByPermissionId(long gid, long pid)
        {
            var serviceResponse = await _permissionService.DeleteById(pid);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }

        #endregion DELETE
    }
}