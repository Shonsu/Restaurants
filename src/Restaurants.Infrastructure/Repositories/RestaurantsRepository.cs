﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constans;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure;

internal class RestaurantsRepository(RestaurantsDBContext dbContext) : IRestaurantsRepository
{
    public async Task<int> CountUserRestaurants(string userId)
    {
        return await dbContext.Restaurants.Where(r => r.OwnerId == userId).CountAsync();
    }

    public async Task<int> CreateRestaurantAsync(Restaurant restaurant)
    {
        await dbContext.Restaurants.AddAsync(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task DeleteRestaurantAsync(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection sortDirection
    )
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = dbContext.Restaurants.Where(r =>
            searchPhraseLower == null
            || r.Name.ToLower().Contains(searchPhraseLower)
            || r.Description.ToLower().Contains(searchPhraseLower)
        );
        var totalCount = await baseQuery.CountAsync();

        if (sortBy is not null)
        {
            var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(RestaurantDto.Name), restaurant => restaurant.Name },
                { nameof(RestaurantDto.Description), restaurant => restaurant.Description },
                { nameof(RestaurantDto.Category), restaurant => restaurant.Category }
            };

            baseQuery = sortDirection switch
            {
                SortDirection.Ascending => baseQuery.OrderBy(columnSelector[sortBy]),
                SortDirection.Descending or _ => baseQuery.OrderByDescending(columnSelector[sortBy])
                // _ => throw new NotImplementedException(),
            };
        }
        var restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int restaurantId)
    {
        return await dbContext
            .Restaurants //.AsNoTracking()
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }

    public async Task<bool> RestaurantExistAsync(int restaurantId)
    {
        return await dbContext.Restaurants.AnyAsync(r => r.Id == restaurantId);
    }

    public async Task SaveChangesAsync() => await dbContext.SaveChangesAsync();

    public async Task<bool> UpdateRestaurantAsync(int id, Object restaurant)
    {
        Restaurant? restaurantToUpdate = await dbContext.Restaurants.FirstOrDefaultAsync(r =>
            r.Id == id
        );
        if (restaurantToUpdate == null)
        {
            return false;
        }
        dbContext.Entry(restaurantToUpdate).CurrentValues.SetValues(restaurant);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
