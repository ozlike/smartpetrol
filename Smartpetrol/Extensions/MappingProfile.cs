using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Smartpetrol.Configuration;
using Smartpetrol.Data;
using Smartpetrol.Models.Books;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(x => x.ReservationEndTime, o => o.MapFrom(t => t.ReservationTime == null ? null :
                   (DateTime?)t.ReservationTime.Value.AddHours(GlobalValues.ReservationHours)));

            CreateMap<BookViewModel, Book>()
                .ForMember(x => x.Tenant, o => o.Ignore())
                .ForMember(x => x.TenantId, o => o.Ignore())
                .ForMember(x => x.Status, o => o.Ignore())
                .ForMember(x => x.RentalTime, o => o.Ignore())
                .ForMember(x => x.ReservationTime, o => o.Ignore());

            CreateMap<User, UserModel>()
                .ForMember(x => x.RoleFullNames, o => o.Ignore());
        }        
    }
}
