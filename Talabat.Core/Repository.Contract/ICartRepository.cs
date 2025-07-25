using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Cart;

namespace Talabat.Core.Repository.Contract
{
    public interface ICartRepository
    {
        Task<CustomerCart?> GetCartAsync(string cartId);
        Task<CustomerCart?> UpdateCartAsync(CustomerCart cart);
        Task<bool> DeleteCartAsync(string cartId);
    }
}
