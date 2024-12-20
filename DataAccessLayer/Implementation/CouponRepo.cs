using DataAccessLayer.Data;
using Entities.Models;
using Entities.Reposatories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Implementation
{
    public class CouponRepo : GenaricRepo<Coupon>, ICouponRepo
    {
        private readonly ApplicationDbContext _context;
        public CouponRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
		public Coupon GetByCouponCode(string code)
		{
			return _context.Coupons.FirstOrDefault(c => c.Code == code);
		}
	}
}
