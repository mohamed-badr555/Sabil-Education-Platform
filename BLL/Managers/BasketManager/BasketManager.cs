using AutoMapper;
using BLL.DTOs.Basket;
using DAL.Data.Models;
using DAL.Data.Models.Basket;
using DAL.Repositories;

using DAL.Repositories.BasketRepo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Managers.BasketManager
{
    public class BasketManager : IBasketManager
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IMapper _mapper;

        private readonly string? _baseUrl;
        public BasketManager(IConfiguration configuration ,
            IBasketRepository basketRepository,
            IGenericRepository<Course> courseRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _baseUrl = configuration["ApplicationSettings:BaseUrl"];
        }

        public async Task<CustomerBasketDto?> CreateBasketAsync()
        {
            var basket = await _basketRepository.CreateBasketAsync();
            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto?> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto?> UpdateBasketAsync(string basketId, List<BasketItemDto> items)
        {
            // Get existing basket
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
                return null;

            // Clear all items and add new ones
            basket.Items.Clear();
            basket.TotalPrice = 0;
            basket.TotalPriceBefore = 0;
            basket.IsVerified = false;

            // Process and validate each course item
            foreach (var item in items)
            {
                // We're only supporting Course type (1)
                if (item.Type == 1)
                {
                    // Parse the itemId as int to fetch the course
                    //if (int.TryParse(item.ItemId, out int courseId))
                    //{
                        var course = await _courseRepository.GetByIdAsync(item.ItemId);
                        if (course != null)
                        {
                            var basketItem = new BasketItem
                            {
                                ItemId = item.ItemId,
                                ItemName = course.Title,
                                ItemUrl = $"{_baseUrl}/courses/{course.Id}",
                                ImageUrl = course.ThumbnailUrl,
                                Price = course.Price,
                                PriceBefore = course.Price, // You might have a discount logic here
                                Quantity = item.Quantity,
                                Type = 1 // Course
                            };

                            basket.Items.Add(basketItem);
                            basket.TotalPrice += basketItem.Price * basketItem.Quantity;
                            basket.TotalPriceBefore += basketItem.PriceBefore * basketItem.Quantity;
                        }
                    //}
                }
            }

            // Update the basket in Redis
            await _basketRepository.UpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }
    }

 
}
