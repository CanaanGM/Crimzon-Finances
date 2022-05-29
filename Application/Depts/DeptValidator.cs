using Application.DTOs;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Depts
{
    public class DeptValidator : AbstractValidator<DeptWriteDto>
    {
        public DeptValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PaidOff).NotEmpty();
            RuleFor(x => x.DateMade).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.AmountRemaining).NotEmpty();
        }
    }
}
