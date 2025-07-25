using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Repository.Contract;
using TalabatAPIs.Dtos;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository,IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCart(string cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            return Ok(cart is null ? new CustomerCart(cartId) : cart);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateCart(CustomerCartDto cart)
        {
            var mappedCart =_mapper.Map<CustomerCartDto,CustomerCart>(cart); 
            var UpdatedOrCreatedCart = await _cartRepository.UpdateCartAsync(mappedCart);
            if (UpdatedOrCreatedCart is null) return BadRequest(new ApiResponse(400));
            return Ok(UpdatedOrCreatedCart);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCart(string cartId)
        {
            var deleted = await _cartRepository.DeleteCartAsync(cartId);
            return deleted;
        }
    }
}
