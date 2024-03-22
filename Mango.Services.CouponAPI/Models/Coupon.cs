using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.CouponAPI.Models
{
    // This annotation is to validate postgres table name.
    [Dapper.Contrib.Extensions.Table(@"""Coupons""")]
    public class Coupon
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string? CouponCode { get; set; }

		[Required]
		public double DiscountAmount { get; set; }

		public int MinAmount { get; set; }
	}
}

