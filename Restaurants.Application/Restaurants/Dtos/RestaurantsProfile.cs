﻿using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(
                r => r.Address,
                opt =>
                    opt.MapFrom(src => new Address
                    {
                        City = src.City,
                        Street = src.Street,
                        PostalCode = src.PostalCode,
                    })
            );

        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(
                d => d.City,
                opt => opt.MapFrom(r => r.Address == null ? null : r.Address.City)
            )
            .ForMember(
                d => d.Street,
                opt => opt.MapFrom(r => r.Address == null ? null : r.Address.Street)
            )
            .ForMember(
                d => d.PostalCode,
                opt => opt.MapFrom(r => r.Address == null ? null : r.Address.PostalCode)
            )
            .ForMember(d => d.Dishes, opt => opt.MapFrom(r => r.Dishes));
    }
}