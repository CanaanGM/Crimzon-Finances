using Application.DTOs;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Folders
{
    public class FolderValidator : AbstractValidator<FolderWriteDto>
    {

        public FolderValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
