using BLL.DTOs.Basket;
using BLL.Managers.BasketManager;
using E_Learning_API.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Learning_API.Controllers
{
  
[Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseApiController
    {
        private readonly IBasketManager _basketManager;

        public CartController(IBasketManager basketManager)
        {
            _basketManager = basketManager;
        }

        // GET: api/cart
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseFormat<CustomerBasketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<CustomerBasketDto>>> CreateBasket()
        {
            var basket = await _basketManager.CreateBasketAsync();
            return OkResponse(basket, "Shopping cart created successfully");
        }

        // GET: api/cart/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<CustomerBasketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<CustomerBasketDto>>> GetBasket(string id)
        {
            var basket = await _basketManager.GetBasketAsync(id);

            if (basket == null)
                return NotFoundResponse<CustomerBasketDto>($"Shopping cart with ID {id} not found");

            return OkResponse(basket, "Shopping cart");
        }

        // Post: api/cart/{id}
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<CustomerBasketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<CustomerBasketDto>>> UpdateBasket(string id, [FromBody] List<BasketItemDto> items)
        {
            if (items == null || items.Count == 0)
                return BadRequestResponse<CustomerBasketDto>("Items list cannot be empty");

            var updatedBasket = await _basketManager.UpdateBasketAsync(id, items);

            if (updatedBasket == null)
                return NotFoundResponse<CustomerBasketDto>($"Shopping cart with ID {id} not found");

            return OkResponse(updatedBasket, "Shopping cart updated successfully");
        }

        // DELETE: api/cart/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseFormat<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseFormat<object>>> DeleteBasket(string id)
        {
            var result = await _basketManager.DeleteBasketAsync(id);

            if (!result)
                return NotFoundResponse<object>($"Shopping cart with ID {id} not found");

            return OkResponse((object) null, "Shopping cart deleted successfully");
        }
    }
}
