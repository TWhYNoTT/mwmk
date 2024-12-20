using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reposatories
{
    public interface Ifavourite_productRepo : IGenaricRepo<favourite_product>
    {
        void update(favourite_product favourite_Product);
        public void AddProductToFavourite(int productId, string userId);
        public void RemoveProductFromFavourite(int productId, string userId);
        public List<int> GetFavoriteProductIds(string userId);
        public bool IsProductInUserFavorites(int productId, string userId);
        public favourite_product GetFavoriteProduct(int productId, string userId);
    }
}
