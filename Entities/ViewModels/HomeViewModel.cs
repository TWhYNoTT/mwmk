using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> FeaturedCategories { get; set; }
        public List<Product> TopOffers { get; set; }
        public List<Product> NewArrivals { get; set; }
        public List<Product> PopularProducts { get; set; }
        public List<int> favourite_products { get; set; }
        public List<string> HomeBackgroundImage { get; set; }
        public List<string> TextInHome { get; set; }
        public bool AppearRateAndReview { get; set; }
    }
}
