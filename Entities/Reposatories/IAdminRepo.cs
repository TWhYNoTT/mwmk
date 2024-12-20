using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reposatories
{
    public interface IAdminRepo:IGenaricRepo<Admin>
    {
        void update(Admin admin);

        List<string> GetHomeBackgroundImage();

        List<string> GetTextInHome();

        bool AppearRateReview();
    }
}
