using Application.DTOs;

using Domain;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Purchases
{
    public class PurchaseValidator : AbstractValidator<PurchaseWriteDto>
    {
        public PurchaseValidator()
        {
            //! Price in $ isn't manditory and so is the invoice

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Seller).NotEmpty();
            RuleFor(x => x.PurchaseDate).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PaymentMethod).NotEmpty();
            RuleFor(x => x.Reccuring).NotEmpty();
        }
    }
}
