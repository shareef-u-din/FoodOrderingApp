using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MenuMap : Profile
    {
        public MenuMap()
        {
            CreateMap<Menu, MenuResponse>();
            CreateMap<Menu, RestaurantMenuResponse>();
            CreateMap<MenuCategory, MenuCategoryResponse>();
            CreateMap<Cuisine, CuisineResponse>();
            CreateMap<MenuCategory, RestaurantMenuCategoryResponse>();
            CreateMap<Cuisine, CuisineResponse>();
        }
    }
}
