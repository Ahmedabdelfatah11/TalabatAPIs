using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Cart;

namespace TalabatAPIs.Dtos
{
    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
