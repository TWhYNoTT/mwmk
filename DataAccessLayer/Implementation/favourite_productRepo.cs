using DataAccessLayer.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Reposatories;

namespace DataAccessLayer.Implementation
{
    public class favourite_productRepo : GenaricRepo<favourite_product>, Ifavourite_productRepo
    {

        private readonly ApplicationDbContext _context;
        public favourite_productRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(favourite_product fav)
        { //need to re see it 
            var favproinDB = _context.favourite_products.FirstOrDefault(x => x.Id==fav.Id);
            if (favproinDB != null)
            {
                favproinDB.Products = fav.Products;
                favproinDB.aspplicationUser = fav.aspplicationUser;
            }
        }

        public List<int> GetFavoriteProductIds(string userId)
        {
            var FavProducts = _context.favourite_products
                              .Where(fav => fav.applicationUserId == userId)
                              .Select(fav => fav.ProductId)
                              .ToList();

            return FavProducts ?? new List<int>();
        }


        public bool IsProductInUserFavorites(int productId, string userId)
        {
            // Check if the product is in the user's favorites
            var isFavorite = _context.favourite_products
                .Any(f => f.applicationUserId == userId && f.ProductId == productId);

            return isFavorite;
        }

        public favourite_product GetFavoriteProduct(int productId, string userId)
        {
            return _context.favourite_products
                .FirstOrDefault(f => f.applicationUserId == userId && f.ProductId == productId);
        }


        public void AddProductToFavourite(int productId, string userId)
        {
            // Check if the product is already in the user's favorites
            var existingFavorite = _context.favourite_products
                .FirstOrDefault(f => f.applicationUserId == userId && f.ProductId == productId);

            if (existingFavorite != null)
            {
                // Add the product to the user's favorites if it's not already there
                var favoriteProduct = new favourite_product
                {
                    applicationUserId = userId,
                    ProductId = productId
                };

                // Add the product to the favorites
                _context.favourite_products.Add(favoriteProduct);

                // Save changes to the database
                _context.SaveChanges();
            }
        }
        public void RemoveProductFromFavourite(int productId, string userId)
        {
            // Find the favorite product in the user's favorites
            var existingFavorite = _context.favourite_products
                .FirstOrDefault(f => f.applicationUserId == userId && f.ProductId == productId);

            if (existingFavorite != null)
            {
                // Remove the product from the user's favorites
                _context.favourite_products.Remove(existingFavorite);

                // Save changes to the database
                _context.SaveChanges();
            }
        }

    }
}