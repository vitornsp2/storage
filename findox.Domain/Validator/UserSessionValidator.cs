using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using FluentValidation;

namespace findox.Domain.Validator
{
    public class UserSessionValidator : AbstractValidator<UserSessionDto>
    {
        public UserSessionValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}