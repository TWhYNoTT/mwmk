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
    public class AdminRepo : GenaricRepo<Admin>, IAdminRepo
    {
        private readonly ApplicationDbContext _context;
        public AdminRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Admin admin)
        {
            var productinDB = _context.Admins.FirstOrDefault(x => x.Id==admin.Id);
            if (productinDB != null)
            {
                productinDB.UserName = admin.UserName;
                productinDB.Email = admin.Email;
                productinDB.DefaultMarketerDiscount=admin.DefaultMarketerDiscount;
                //Admin.HomeBackgroundImages=admin.HomeBackgroundImages;
                //Admin.TextInHome = admin.TextInHome;
            }
        }


        public List<string> GetHomeBackgroundImage()
        {
            // Check if the property is null and return default images if so
            return Admin.HomeBackgroundImages ?? new List<string> { "~/Images/home1.jpg", "~/Images/home2.jpg" };
        }


        public List<string> GetTextInHome()
        {
            // Since HomeBackgroundImages is static, you can return it directly without querying the database
            return Admin.HomeBackgroundImages?? new List<string>();
        }

        public bool AppearRateReview()
        {
            return Admin.AppearRateAndReview??true;
        }
    }
 
}
