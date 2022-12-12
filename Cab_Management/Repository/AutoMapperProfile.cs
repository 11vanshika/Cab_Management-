using AutoMapper;
using Domain;

namespace Repository
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserView, UserDisplay>();
        }
    }
}