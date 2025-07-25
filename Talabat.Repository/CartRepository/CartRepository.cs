using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Cart;
using Talabat.Core.Repository.Contract;

namespace Talabat.Repository.CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        public CartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<CustomerCart?> GetCartAsync(string cartId)
        {
            var cart = await _database.StringGetAsync(cartId);
            return cart.IsNullOrEmpty ? null :JsonSerializer.Deserialize<CustomerCart?>(cart);
        }


        public async Task<CustomerCart?> UpdateCartAsync(CustomerCart cart)
        {
            var CreateOrUpddate = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (!CreateOrUpddate) return null;
            var updatedOrCreatedCart = await GetCartAsync(cart.Id);
            return updatedOrCreatedCart;
        }
        public Task<bool> DeleteCartAsync(string cartId)
        {
            var deleted = _database.KeyDeleteAsync(cartId);
            return deleted;
        }
    }
}
