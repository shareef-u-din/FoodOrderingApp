using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CustomerMaps : Profile
    {
        public CustomerMaps()
        {
            CreateMap<Customer, CustomerResponse>();
        }
    }
}
