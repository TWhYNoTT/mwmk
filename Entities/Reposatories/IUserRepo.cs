using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reposatories
{
    public interface IUserRepo 
    {
        public Task<bool> TransferRoleAsync(string userId);
        public bool ChangeUserRole(string userId);
    }
}
