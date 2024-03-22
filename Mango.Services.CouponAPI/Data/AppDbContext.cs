using System.Data;
using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext: DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		// Create for Dapper.
		public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Coupon> Coupons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Coupon>().HasData(new List<Coupon>
			{
				new Coupon
				{
					Id = 1,
					CouponCode = "10OFF",
					DiscountAmount = 10,
					MinAmount = 20
				},
				new Coupon
				{
                    Id = 2,
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 60
                }
			});
		}
	}
}

