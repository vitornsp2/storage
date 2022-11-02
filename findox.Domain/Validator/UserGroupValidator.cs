using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using FluentValidation;

namespace findox.Domain.Validator
{
    public class UserGroupValidator: AbstractValidator<UserGroupDto>
    {
        public UserGroupValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.GroupId).NotEmpty();
        }
    }
}