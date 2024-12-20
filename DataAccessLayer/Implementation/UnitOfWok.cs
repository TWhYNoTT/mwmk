using DataAccessLayer.Data;
using Entities.Models;
using Entities.Reposatories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Implementation
{
    public class UnitOfWok : IUnitOfWork
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IAdminRepo Admin { get; private set; }
        public ICouponRepo Coupon { get; private set; }
        public IMarketerRepo Marketer { get; private set; }
        public IUserMessageRepo UserMessages { get; private set; }
        public Ifavourite_productRepo favourite_product { get; private set; }
        public IShopingCartRepo ShoppingCart { get; private set; }
        public IOrderHeaderRepo OrderHeader { get; private set; }
        public IUserRepo UserRepo { get; private set; }
        public IOrderDetailRepo OrderDetail { get; private set; }
        public IApplicationUserRepo ApplicationUser { get; private set; }
        public UnitOfWok(ApplicationDbContext context) 
        {
            _context = context;
            favourite_product=new favourite_productRepo(context);
            UserMessages=new UserMessageRepo(context);
            Category =new CategoryRepo(context);
            Product=new ProductRepo(context);
            Coupon=new CouponRepo(context);
            Marketer=new MarketerRepo(context);
            Admin =new AdminRepo(context);
            ShoppingCart=new ShoppingCartRepo(context);
            OrderHeader=new OredrHeaderRepo(context);   
            OrderDetail=new OredrDetailRepo(context);
            ApplicationUser=new ApplicationUserRepo(context);
        }
        

        public int complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
