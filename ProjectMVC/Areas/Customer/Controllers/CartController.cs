using DataAccessLayer.Implementation;
using DataAccessLayer.Services;
using Entities.Models;
using Entities.Reposatories;
using Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Utilities;

namespace ProjectMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly CartServices cartservices;
        public CartController(IUnitOfWork unitOfWork, ICompositeViewEngine viewEngine, CartServices cartServices)
        {
            _unitOfWork = unitOfWork;
            _viewEngine = viewEngine;
            cartservices = cartServices;
        }

        public IActionResult Index()
        {
            var shoppingCartVM = GetShoppingCartVM();
            ViewBag.IsCartPage = true;
            return View(shoppingCartVM);
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var shoppingCartVM = GetShoppingCartVM();
            PopulateOrderHeader(shoppingCartVM);
            ViewBag.IsCartPage = true; 
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == claim.Value, icludeWord: "Product");

            shoppingCartVM.OrderHeader.OrderStatus = SD.Proccessing;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            CalculateTotalPrice(shoppingCartVM);
            CreateOrderHeader(shoppingCartVM);
            CreateOrderDetails(shoppingCartVM);
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CouponCode")))
            {
                HttpContext.Session.Remove("CouponCode");
            }
            _unitOfWork.complete();
            return View("confirm");
        }


        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetByID(u => u.Id == id);
            
             _unitOfWork.OrderHeader.updateOrderStates(id, SD.Approve);
            
            List<ShopingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.removeRange(shoppingCarts);
            _unitOfWork.complete();

            return View("Confirm", id);
        }

        [HttpPost]
        public IActionResult Plus(int cartId)
        {
            UpdateCartItemCount(cartId, 1);
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        [HttpPost]
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            if (cartFromDb.Count > 1)
            {
                UpdateCartItemCount(cartId, -1);
            }
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            _unitOfWork.ShoppingCart.remove(cartFromDb);
            _unitOfWork.complete();
            return PartialView("_CartWrapper", GetShoppingCartVM());
        }

        [HttpGet]
        public IActionResult GetCartItems()
        {
            var shoppingCartVM = GetShoppingCartVM();
            string cartItemsHtml = RenderViewToString("_CartItems", shoppingCartVM);

            return Json(new
            {
                cartItemsHtml = cartItemsHtml,
                total = shoppingCartVM.TotalCarts,
                itemCount = shoppingCartVM.CartList.Sum(c => c.Count)
            });
        }

        private ShoppingCartVM GetShoppingCartVM()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.applicationUserId == userId, icludeWord: "Product"),
                OrderHeader = new(),
                TotalCarts = 0
            };
            double GiftTotalPrices = 0;
            foreach (var cart in shoppingCartVM.CartList)
            {
                if (cart.ProductClassification==1)
                {
                    GiftTotalPrices+=cart.Product.Price;
                }
                shoppingCartVM.TotalCarts += (cart.Product.Price * cart.Count);
            }
            shoppingCartVM.TotalCarts-=GiftTotalPrices;

            return shoppingCartVM;
        }

        private void PopulateOrderHeader(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetByID(x => x.Id == claim.Value);

            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.Address = shoppingCartVM.OrderHeader.ApplicationUser.Address;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.Phone = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.Country = shoppingCartVM.OrderHeader.ApplicationUser.Country;
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State;

           CalculateTotalPrice(shoppingCartVM);

        }

        private void CalculateTotalPrice(ShoppingCartVM shoppingCartVM)
        {
            //foreach (var item in shoppingCartVM.CartList)
            //{
            //    shoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
            //}
            var couponCode = HttpContext.Session.GetString("CouponCode");
            Coupon coupon = _unitOfWork.Coupon.GetByCouponCode(couponCode);
           
                //var order=shoppingCartVM.OrderHeader;
                //shoppingCartVM.OrderHeader.Coupon=coupon;
                //shoppingCartVM.OrderHeader.CouponID=coupon.Id;
                //double discount = order.TotalPrice * ((double)coupon.DiscountAmount / 100);

                //order.TotalPrice-=discount;
                //if (order.TotalPrice < 0)
                //{
                //    order.TotalPrice = 0;
                //}
                cartservices.FinalTotalPrice(shoppingCartVM, coupon);
         
        }

        private void CreateOrderHeader(ShoppingCartVM shoppingCartVM)
        {
            _unitOfWork.OrderHeader.add(shoppingCartVM.OrderHeader);
            _unitOfWork.complete();
        }

        private void CreateOrderDetails(ShoppingCartVM shoppingCartVM)
        {
            foreach (var item in shoppingCartVM.CartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderId = shoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };

                _unitOfWork.OrderDetail.add(orderDetail);
            }
            _unitOfWork.complete();
        }


        private void UpdateCartItemCount(int cartId, int countChange)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.GetByID(u => u.ID == cartId);
            cartFromDb.Count += countChange;
            _unitOfWork.complete();
        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}