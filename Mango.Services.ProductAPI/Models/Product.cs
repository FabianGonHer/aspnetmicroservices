using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ProductAPI.Models
{
    public class Product
	{
		[Key]
		public int ProductId { get; set; }
		[Required]
		public string? Name { get; set; }
		[Range(1, 1000)]
		public double Price { get; set; }
		public string? Description { get; set; }
		[ForeignKey("Category")]
		public int CategoryId { get; set;}
		public string? ImageUrl { get; set; }

        public virtual Category? Category { get; set; }
    }
}

