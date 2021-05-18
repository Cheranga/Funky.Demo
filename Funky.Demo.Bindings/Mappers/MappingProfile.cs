using AutoMapper;
using Funky.Demo.Messages;
using Funky.Demo.Requests;

namespace Funky.Demo.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AcceptOrderRequest, CreateOrderMessage>();
        }
    }
}