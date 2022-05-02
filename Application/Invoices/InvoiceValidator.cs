using Application.DTOs;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Invoices
{
    public class InvoiceValidator : AbstractValidator<PhotoWriteDto>
    {
        public InvoiceValidator()
        {
            RuleFor(x=>x.Files).NotEmpty();
        }
    }
    public class InvoiceUpdateValidator : AbstractValidator<PhotoUpdateDto>
    {
        public InvoiceUpdateValidator()
        {
            RuleFor(x => x.File).NotEmpty();
        }
    }


}
