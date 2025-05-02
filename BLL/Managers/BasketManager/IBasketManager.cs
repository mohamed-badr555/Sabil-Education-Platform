using BLL.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.BasketManager
{
    public interface IBasketManager
    {
        Task<CustomerBasketDto?> CreateBasketAsync();
        Task<CustomerBasketDto?> GetBasketAsync(string basketId);
        Task<CustomerBasketDto?> UpdateBasketAsync(string basketId, List<BasketItemDto> items);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
