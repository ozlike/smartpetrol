using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Smartpetrol.Data;
using Smartpetrol.Models.Books;
using Smartpetrol.Models.Users;

namespace Smartpetrol.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookViewModel>();
            CreateMap<BookViewModel, Book>();

            CreateMap<User, UserModel>()
                .ForMember(x => x.RoleFullNames, o => o.Ignore());
        }
    }
}
