﻿using Application.Contracts.Responses;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class RestaurantAddressMap : Profile
    {
        public RestaurantAddressMap()
        {
            CreateMap<RestaurantAddress, RestaurantAddressResponse>();
        }
    }
}