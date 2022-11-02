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
            var serviceResponse = await _userService.CreateSession(userSessionDto);
            
            if(serviceResponse.hasValidationErros)
                return BadRequest(serviceResponse);
            else if(serviceResponse.hasErros)
                return StatusCode(500, serviceResponse);
            
            return Ok(serviceResponse);
        }
    }
}