using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using findox.Domain.Models.Dto;
using FluentValidation;

namespace findox.Domain.Validator
{
    public class PermissionValidator : AbstractValidator<PermissionDto>
    {
        public PermissionValidator()
        {
            RuleFor(x => x.DocumentId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty().When(x => !x.GroupId.HasValue);
            RuleFor(x => x.GroupId).NotEmpty().When(x => !x.UserId.HasValue);
        }
    }
}