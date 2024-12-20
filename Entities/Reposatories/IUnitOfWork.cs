using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reposatories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        IAdminRepo Admin { get; }
        IMarketerRepo Marketer { get; }
        IUserMessageRepo UserMessages { get; }
        ICouponRepo Coupon { get; }
        IShopingCartRepo ShoppingCart { get; }
        Ifavourite_productRepo favourite_product { get; }
        IOrderHeaderRepo OrderHeader { get; }
        IOrderDetailRepo OrderDetail { get; }
        IApplicationUserRepo ApplicationUser { get; }

        int complete();
    }
}
