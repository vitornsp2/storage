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
    [Route("v1/sessions")]
    [ApiController]
    [Authorize]
    public class SessionController : ControllerBase
    {
        private readonly IUserService _userService;

        public SessionController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] UserSessionDto userSessionDto)
        {
            try
            {
                var serviceRequest = new UserServiceRequest(userSessionDto);
                var serviceResponse = await _userService.CreateSession(serviceRequest);
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
                        response.Success(serviceResponse.Token);
                        return Ok(response);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);
        }
    }
}