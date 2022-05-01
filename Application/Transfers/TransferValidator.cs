using Application.DTOs;

using Domain;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transfers
{
    public class TransferValidator : AbstractValidator<TransferWriteDto>
    {
        public TransferValidator()
        {
            //! Price in $ isn't manditory and so is the invoice

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.DateWasMade).NotEmpty();
            RuleFor(x => x.FromBank).NotEmpty();
            RuleFor(x => x.FromAccount).NotEmpty();
            RuleFor(x => x.Reciever).NotEmpty();
            RuleFor(x => x.RecieverAccount).NotEmpty();
            RuleFor(x => x.TransferType).NotEmpty();

        }
    }
}
