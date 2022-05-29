
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
            CreateMap<Purchase, PurchaseReadDto>()
                .ForMember(e=>e.Invoice,
                o=>o.MapFrom(e=>e.Invoice))
                .ReverseMap();

            CreateMap<Purchase, PurchaseUpdateDto>()
                .ReverseMap();
            CreateMap<AppUser, AppRegisterDto>().ReverseMap();
            
            CreateMap<Photo, PhotoReadDto>()
                .ForMember(c=>c.ImageBase64,e=>e.MapFrom(x=>x.Bytes))
                .ReverseMap();
            CreateMap<Photo, PhotoWriteDto>().ReverseMap();
            CreateMap<Photo, Photo>().ReverseMap();


            CreateMap<Dept, DeptReadDto>().ReverseMap();
            CreateMap<Dept, DeptWriteDto>().ReverseMap();

            //CreateMap<AppUser, >().ReverseMap();
        }
    }
}
