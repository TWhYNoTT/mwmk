using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reposatories
{
    public interface ICouponRepo : IGenaricRepo<Coupon>
    {
		Coupon GetByCouponCode(string code);
	}
}
