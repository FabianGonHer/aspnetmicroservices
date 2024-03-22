using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Category
	{
		[Key]
		public int CategoryId { get; set; }

		[Required]
		public string? CategoryName { get; set; }

		public virtual ICollection<Product>? Products { get; set; }
	}
}

