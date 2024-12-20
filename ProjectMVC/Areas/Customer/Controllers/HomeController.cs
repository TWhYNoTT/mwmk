using Entities.Reposatories;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Data;
using Utilities;
using System.Linq;
using Entities.ViewModels;
using DataAccessLayer.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;

namespace ProjectMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
   //[Authorize(Roles = "Customer, Marketer")]
    public class HomeController : Controller
    {
     

        private readonly IUnitOfWork unitOfWork;
        private readonly RoleService _roleService;
        public HomeController(IUnitOfWork unitOfWork, RoleService roleService)
        {
            this.unitOfWork = unitOfWork;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            string userId = GetUserId();
            var viewModel = new HomeViewModel()
            {
                FeaturedCategories = unitOfWork.Category.GetAll().Take(5).ToList(),
                TopOffers = unitOfWork.Product.GetAll(icludeWord: "Reviews").OrderByDescending(p => p.Discount).Take(5).ToList(),
                NewArrivals = unitOfWork.Product.GetAll(icludeWord: "Reviews").OrderByDescending(p => p.DataInserted).Take(5).ToList(),
                PopularProducts = unitOfWork.Product.GetAll(icludeWord: "Reviews").OrderByDescending(p => (p.TotalNumberOfPieces- p.NumberOfPiecesAvailable)).Take(5).ToList(),
                favourite_products=unitOfWork.favourite_product.GetFavoriteProductIds(userId),
                HomeBackgroundImage = unitOfWork.Admin.GetHomeBackgroundImage()?.ToList() ?? new List<string>(),
                TextInHome = unitOfWork.Admin.GetTextInHome()?.ToList() ?? new List<string>(),
                AppearRateAndReview=unitOfWork.Admin.AppearRateReview()
            };


            // I am in review and rate
            var allProducts = unitOfWork.Product.GetAll(icludeWord: "Reviews");
            ViewBag.AppearRateAndReview=unitOfWork.Admin.AppearRateReview();
            ViewBag.AverageRatings = allProducts.ToDictionary(
                p => p.Id,
                p => CalculateAverageRating(p.Reviews)
            );

            bool ismarketer = false;
            if (User.Identity.IsAuthenticated)
            {
                // Check if the user is an Marketer
                ismarketer = User.IsInRole("Marketer");
            }
            ViewBag.IsMarketer=ismarketer;

            return View(viewModel);
        }
        
        private string GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Home(string categoryName, string searchTerm, double? minPrice, double? maxPrice, string sortBy, int page = 1, int pageSize = 12)
        {
            IEnumerable<Product> products = unitOfWork.Product.GetAll((p) => true, icludeWord: "Reviews");
            string userId = GetUserId();
            if (!string.IsNullOrEmpty(categoryName))
            {
                products = products.Where(p => p.Category.name == categoryName);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                            || (p.Description != null && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            switch (sortBy)
            {
                case "price_asc"://form low to height
                    products = products.OrderBy(p => p.Price);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "new_Arr":
                    products = products.OrderByDescending(p => p.DataInserted);
                    break;
                case "best-seller":
                    products = products.OrderBy(p => (p.TotalNumberOfPieces- p.NumberOfPiecesAvailable));
                    break;
                case "discount_Prod":
                    products=products.OrderByDescending(p => p.Discount);
                    break;
                default:
                    products = products.OrderBy(p => p.Id);
                    break;
            }



            var total = products.Count();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            var result = products.Select(p => new
            {
                Product = p,
                AverageRating = CalculateAverageRating(p.Reviews)
            }).Skip((page - 1) * pageSize)
             .Take(pageSize)
             .ToList();

            ViewBag.AverageRatings = result.ToDictionary(r => r.Product.Id, r => r.AverageRating);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentCategory = categoryName;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            ViewBag.favourite_products=unitOfWork.favourite_product.GetFavoriteProductIds(userId);
            ViewBag.Categories = unitOfWork.Category.GetAll().Select(c => c.name).Distinct().ToList();
            var allProducts = unitOfWork.Product.GetAll(icludeWord: "Reviews");
            ViewBag.AverageRatings = allProducts.ToDictionary(
                p => p.Id,
                p => CalculateAverageRating(p.Reviews)
            );
            ViewBag.AppearRateAndReview=unitOfWork.Admin.AppearRateReview();
            bool ismarketer = false;
            if (User.Identity.IsAuthenticated)
            {
                // Check if the user is an admin
                ismarketer = User.IsInRole("Marketer");
            }
            ViewBag.IsMarketer=ismarketer;

            return View(result.Select(r => r.Product).ToList());
        }

        [HttpGet]
        [Authorize]
        public IActionResult FavProd()
        {
            string userId = GetUserId();
            List<int> fav_prodsIds = unitOfWork.favourite_product.GetFavoriteProductIds(userId);

            // Check if there are any favorite product IDs
            if (fav_prodsIds == null || fav_prodsIds.Count == 0)
            {
                ViewBag.NoFavorites = true; // Set a ViewBag flag to indicate no favorites
                return View("FavProd", new List<Product>()); // Pass an empty list
            }

            // Filter products by the list of favorite product IDs
            List<Product> fav_prods = unitOfWork.Product
                .GetProductsByIds(fav_prodsIds)
                .ToList();
            ViewBag.NoFavorites = false;
            return View("FavProd", fav_prods);
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int productId, int quantity = 1, string size = "", string color = "",short ProClass=0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "You must be logged in to add items to the cart." });
            }

            string userId = GetUserId();
            var product = unitOfWork.Product.GetByID(p => p.Id == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            // Set default color and size if not provided
            if (string.IsNullOrEmpty(color))
            {
                color = product.Color?.FirstOrDefault() ?? "";
            }

            if (string.IsNullOrEmpty(size))
            {
                size = product.Size?.FirstOrDefault() ?? "";
            }

            // Check if the cart item already exists
            var cartItem = unitOfWork.ShoppingCart.GetByID(u => u.applicationUserId == userId&& u.ProductId == productId
                 && u.Size==size
                 && u.Color == color);

            if (cartItem == null)
            {
                // Create a new cart item
                cartItem = new ShopingCart
                {
                    ProductId = productId,
                    ProductClassification=ProClass,
                    applicationUserId = userId,
                    Count = quantity,
                    Color = color,
                    Size = size
                };
                unitOfWork.ShoppingCart.add(cartItem);
            }
            else
            {
                // Update the quantity of the existing cart item
                unitOfWork.ShoppingCart.IncreaseCount(cartItem, quantity);
            }

            // Save changes to the database
            unitOfWork.complete();

            return Json(new { success = true, message = "Product added to cart successfully." });
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalis(ShopingCart shoppinCart)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppinCart.applicationUserId = claim.Value;
            var cartFromDatabase = unitOfWork.ShoppingCart.GetByID(
                u => u.applicationUserId == claim.Value && u.ProductId == shoppinCart.ProductId);

            if (cartFromDatabase == null)
            {
                unitOfWork.ShoppingCart.add(shoppinCart);
                HttpContext.Session.SetInt32(SD.SessionKey,
                    unitOfWork.ShoppingCart.GetAll(x => x.applicationUserId == claim.Value).ToList().Count()
                );
                unitOfWork.complete();
            }
            else
            {
                unitOfWork.ShoppingCart.IncreaseCount(cartFromDatabase, shoppinCart.Count);
                unitOfWork.complete();
            }
           

            return RedirectToAction("index","Cart");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ToggleFavorite(int productId)
        {
            string userId = GetUserId();
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Account", new { area = "Identity" });
            //}
            bool favorite = unitOfWork.favourite_product.IsProductInUserFavorites(productId, userId);
            if (favorite==true)
            {
                var favoriteProduct= unitOfWork.favourite_product.GetFavoriteProduct(productId, userId);
                unitOfWork.favourite_product.remove(favoriteProduct);
                unitOfWork.complete();
                //unitOfWork.favourite_product.RemoveProductFromFavourite(productId, userId);
            }
            else
            {
                var favoriteProduct = new favourite_product
                {
                    applicationUserId = userId,
                    ProductId = productId
                };
                unitOfWork.favourite_product.add(favoriteProduct);
                unitOfWork.complete();
                //unitOfWork.favourite_product.AddProductToFavourite(productId, userId);

            }
            return Ok();
        }


        //public IActionResult Detalis(int ProductID, short ProductClassification = 0)
        //{
        //    ShopingCart shopincartvm = new ShopingCart()
        //    {
        //        ProductClassification=ProductClassification,
        //        ProductId = ProductID,
        //        Product = unitOfWork.Product.GetByID(p => p.Id == ProductID, icludeWord: "Category,Reviews"),
        //        Count = 1,
        //    };
        //    return View("Details", shopincartvm);
        //}

        public IActionResult Details(int ProductID)
        {
            var product = unitOfWork.Product.GetByID(p => p.Id == ProductID, icludeWord: "Category,Reviews");

            if (product == null)
            {
                // Handle the case when the product is not found
                return NotFound("The product does not exist."); // You can customize this response.
            }

            ShopingCart shopincartvm = new ShopingCart()
            {
                ProductId = ProductID,
                Product =product,
                Color="",
                Size="",
                Count = 1,
            };

            var reviews = product.Reviews ?? new List<Review>();
            ViewBag.AverageRating = CalculateAverageRating(reviews);
            ViewBag.Colors=product.Color;
            ViewBag.Sizes=product.Size;

            return View("Details", shopincartvm);
        }


        [Authorize]
        [HttpPost]
        public IActionResult AddReview(int id, int rate, string comment)
        {
            var product = unitOfWork.Product.GetByID(p => p.Id == id, icludeWord: "Reviews");
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        
            var existingReview = product.Reviews.FirstOrDefault(r => r.UserId == userId);
            if (existingReview != null)
            {
                TempData["ErrorMessage"] = "You have already reviewed this product.";
                return RedirectToAction("Details", new { ProductID = id });
            }

            var review = new Review
            {
                ProductId = id,
                UserId = userId,
                Rate = rate,
                Comment = comment
            };

            product.Reviews.Add(review);
            unitOfWork.complete();

            TempData["SuccessMessage"] = "Your review has been added successfully.";
            return RedirectToAction("Details", new { ProductID = id });
        }


        [HttpGet]
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }



        [Authorize]
        [HttpPost]
        public IActionResult AddUsermeesage([FromForm] string name, string email, string phone, string message)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var newmessage = new UserMessage
                {

                    UserId=userId,
                    Name = name,
                    Email=email,
                    Phone = phone,
                    Message = message
                };
                unitOfWork.UserMessages.add(newmessage);
                unitOfWork.complete();

                TempData["SuccessMessage"] = "Your message has been submitted successfully!";
                return RedirectToAction("contact"); 

            }
            else
            {
                // Populate ViewBag with the entered data to re-display the form
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.Phone = phone;
                ViewBag.Message = message;

                // Add error messages to ViewData for display in the view
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ViewData["Error"] = modelError.ErrorMessage;
                }

                return View("contact");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeRole()
        {
            string userId = GetUserId();
            var result = await _roleService.ChangeRoleToMarketer(userId);

            switch (result)
            {
                case 1:
                    return Json(new { success = true, message = "Role changed successfully to Marketer!" });
                case 2:
                    return Json(new { success = false, message = "The user is already a Marketer." });
                case 0:
                default:
                    return Json(new { success = false, message = "Failed to change role. Please try again." });
            }
        }



        [HttpPost]
        [Authorize]
        public IActionResult Gift([FromForm] string productName)
        {
                short ProductClassification = 1;
                if (string.IsNullOrEmpty(productName))
                {
                    return Json(new { success = false, message = productName });
                }

                var product = unitOfWork.Product.GetAll(p => p.CanBeGift && p.Name == productName).FirstOrDefault();
                AddToCart(product.Id,1,"","" ,1);
                if (product == null)
                {
                    ReturnPoints();
                    return Json(new { success = false, message = "No eligible products found in this category. Please try again." });
                }

                ViewBag.SelectedProduct=product;
                ViewBag.ProductClassification=1;
            return View("Gift");

        }

        [HttpPost]
        [Authorize]
        public IActionResult DeductPoints()
        {
            string userId = GetUserId();
            var user = unitOfWork.ApplicationUser.GetByID(u => u.Id == userId);

            if (user.Points < 10)
            {
                return Json(new { success = false, message = "Insufficient points." });
            }

            user.Points -= 10;
            unitOfWork.ApplicationUser.update(user);
            unitOfWork.complete();

            return Json(new { success = true, remainingPoints = user.Points });
        }

        public void ReturnPoints()
        {
            string userId = GetUserId();
            var user = unitOfWork.ApplicationUser.GetByID(u => u.Id == userId);
            user.Points += 10;
            unitOfWork.ApplicationUser.update(user);
            unitOfWork.complete();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ApplyCoupon(double totalprice, string couponCode)
        {
            var coupon = unitOfWork.Coupon.GetByCouponCode(couponCode);

            if (coupon == null)
            {
                return Json(new { success = false, message = "Invalid coupon code." });
            }

            if (!coupon.IsActive || coupon.ExpirationDate < DateTime.Now|| coupon.TotalUsagesCount <= 0)
            {
                return Json(new { success = false, message = "Coupon has expired." });
            }

            if (totalprice == null)
            {
                return Json(new { success = false, message = "Not valid now please try in other time" });
            }

            // Validate purchase amount constraints
            if (totalprice < coupon.MinPurchaseAmount)
            {
                return Json(new { success = false, message = "Coupon conditions not met for this order." });
            }

            // Apply discount
            double discount = totalprice * ((double)coupon.DiscountAmount / 100);

            totalprice-=discount;

            if (totalprice < 0)
            {
                totalprice = 0;
            }
            HttpContext.Session.SetString("CouponCode", couponCode);
            unitOfWork.complete();
            return Json(new { success = true, message = "Coupon applied successfully!", newTotal = totalprice });
        }


        //[HttpPost]
        //[Authorize]
        //public IActionResult ApplyCoupon(int orderId, string couponCode)
        //{ 
        //    var order = unitOfWork.OrderHeader.GetByID(x=>x.orderId); ;
        //    var coupon = unitOfWork.Coupon.GetByCouponCode(couponCode);
        //    if (coupon == null)
        //    {
        //        return Json(new { success = false, message = "Invalid coupon code." });
        //    }
        //    if (!coupon.IsActive || coupon.ExpirationDate < DateTime.Now|| coupon.TotalUsagesCount <= 0)
        //    {
        //        return Json(new { success = false, message = "Coupon has expired." });
        //    }

        //    var order = shoppingCartVM.OrderHeader;
        //    CalculateTotalPrice(shoppingCartVM);
        //    // Validate purchase amount constraints
        //    if (order.TotalPrice < coupon.MinPurchaseAmount)
        //    {
        //        return Json(new { success = false, message = "Coupon conditions not met for this order." });
        //    }

        //    // Apply discount
        //    double discount = order.TotalPrice * ((double)coupon.DiscountAmount / 100);

        //    order.TotalPrice-=discount;
        //    order.CouponID = coupon.Id;
        //    order.Coupon = coupon;
        //    if (order.TotalPrice < 0)
        //    {
        //        order.TotalPrice = 0;
        //    }

        //    unitOfWork.complete();
        //    return Json(new { success = true, message = "Coupon applied successfully!", newTotal = order.TotalPrice });
        //}


        [HttpGet]
        [Authorize]
        public  IActionResult Prize()
        {
            string userId = GetUserId();
            var user=unitOfWork.ApplicationUser.GetByID(u => u.Id== userId);

            var products = unitOfWork.Product.GetAll(p => p.CanBeGift);

            ViewBag.UserPoints=user.Points;
            ViewBag.EnableSpan=false;
            if (user.Points>=10)
            {
                ViewBag.EnableSpan=true;
            }

            return View(products);

        }

        private double CalculateAverageRating(IEnumerable<Review> reviews)
        {
            if (reviews == null || !reviews.Any())
            {
                return 0;
            }

            return reviews.Average(r => r.Rate);
        }

        private double CalculateAverageRating(List<Review> reviews)
        {
            if (reviews == null || reviews.Count == 0)
            {
                return 0;
            }

            return reviews.Average(r => r.Rate);
        }

        
    }

}