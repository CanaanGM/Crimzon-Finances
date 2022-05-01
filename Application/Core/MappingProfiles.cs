
using Application.DTOs;

using AutoMapper;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //CreateMap<>
            CreateMap<Purchase, Purchase>();
            CreateMap<Transfer, Transfer>();

            //DTOz
            CreateMap<Transfer, TransferWriteDto>().ReverseMap();
            CreateMap<Transfer, TransferReadDto>().ReverseMap();
            CreateMap<Purchase, PurchaseWriteDto>().ReverseMap();
            CreateMap<Purchase, PurchaseReadDto>().ReverseMap();
            CreateMap<AppUser, AppRegisterDto>().ReverseMap();
            //CreateMap<AppUser, >().ReverseMap();
        }
    }
}
