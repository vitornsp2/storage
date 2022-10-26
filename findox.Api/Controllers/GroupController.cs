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
            try
            {
                var serviceRequest = new GroupServiceRequest(groupDto);
                var serviceResponse = await _groupService.Create(serviceRequest);
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

        [HttpPost("/v1/groups/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateGroupPermission([FromRoute] long id, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                if (id != permissionDto.GroupId) return BadRequest($"Id mismatch: Route {id} ≠ Body {permissionDto.GroupId}");

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
                var serviceRequest = new GroupServiceRequest();
                var serviceResponse = await _groupService.ReadAll(serviceRequest);
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
                var serviceRequest = new GroupServiceRequest(id);
                var serviceResponse = await _groupService.ReadById(serviceRequest);
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

        [HttpPut("/v1/groups/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Update([FromRoute] long id, [FromBody] GroupDto groupDto)
        {
            try
            {
                if (id != groupDto.Id) return BadRequest($"Id mismatch: Route {id} ≠ Body {groupDto.Id}");

                var serviceRequest = new GroupServiceRequest(groupDto);
                var serviceResponse = await _groupService.Update(serviceRequest);
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
                        response.Success("Group updated.");
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
                var serviceRequest = new GroupServiceRequest(id);
                var serviceResponse = await _groupService.DeleteById(serviceRequest);
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
                        response.Success("Group deleted.");
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }

        [HttpDelete("/v1/groups/{id}/userGroups")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUserGroupsByGroupId(long id)
        {
            try
            {
                var serviceRequest = new UserGroupServiceRequest(id);
                var serviceResponse = await _userGroupService.DeleteByGroupId(serviceRequest);
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

        [HttpDelete("/v1/groups/{id}/permissions")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionsByGroupId(long id)
        {
            try
            {
                var serviceRequest = new PermissionServiceRequest(id);
                var serviceResponse = await _permissionService.DeleteByGroupId(serviceRequest);
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

        [HttpDelete("/v1/groups/{gid}/permissions/{pid}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeletePermissionByPermissionId(long gid, long pid)
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