﻿using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery(string? SearchPhrase)
    : IRequest<IEnumerable<RestaurantDto>> { }
