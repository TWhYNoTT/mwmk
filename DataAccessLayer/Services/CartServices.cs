using DataAccessLayer.Implementation;
using Entities.Models;
using Entities.Reposatories;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class CartServices
    {
        private readonly IUnitOfWork unitOfWork;
        public CartServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public ShoppingCartVM FinalTotalPrice(ShoppingCartVM shoppingCartVM,Coupon coupon)
        {
            double GiftTotalPrices = 0;

            foreach (var cart in shoppingCartVM.CartList)
            {
                if (cart.ProductClassification==1)
                {
                    GiftTotalPrices+=cart.Product.Price;
                }
                shoppingCartVM.OrderHeader.TotalPrice += (cart.Product.Price * cart.Count);
            }
            shoppingCartVM.OrderHeader.TotalPrice-=GiftTotalPrices;
            if (coupon!=null&&shoppingCartVM.OrderHeader.TotalPrice >= coupon.MinPurchaseAmount)
            {
                var order = shoppingCartVM.OrderHeader;
                shoppingCartVM.OrderHeader.Coupon=coupon;
                shoppingCartVM.OrderHeader.CouponID=coupon.Id;
                double discount = order.TotalPrice * ((double)coupon.DiscountAmount / 100);

                shoppingCartVM.OrderHeader.TotalPrice-=discount;
                if (order.TotalPrice < 0)
                {
                    order.TotalPrice = 0;
                }

            }
            return shoppingCartVM;
        }
    }
}
