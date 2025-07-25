using System.ComponentModel.DataAnnotations;

namespace TalabatAPIs.Dtos
{
    public class CartItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(.1,double.MaxValue,ErrorMessage ="Please Enter A valid Price .")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Please Enter A valid Quantity .")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Brand { get; set; }
    }
}
