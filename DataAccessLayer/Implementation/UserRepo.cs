using DataAccessLayer.Data;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Reposatories;
using System.Data;

namespace DataAccessLayer.Implementation
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<bool> TransferRoleAsync(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user != null&&await _userManager.IsInRoleAsync(user, "Customer"))
            //{
            //    await _userManager.RemoveFromRoleAsync(user, "Customer");
            //    await _userManager.AddToRoleAsync(user, "Marketer");
            //    return true;
            //}
            return false;
        }
        public bool ChangeUserRole(string userId)
        {
            string newRoleName = "Marketer";
            // Ensure the role exists
            var role = _context.Roles.SingleOrDefault(r => r.Name == newRoleName);
            // Fetch the user's current role mapping
            var userRoleMapping = _context.UserRoles.SingleOrDefault(ur => ur.UserId == userId);
            if (userRoleMapping != null)
            {
                // If the role is already assigned, do nothing
                if (userRoleMapping.RoleId == "Customer")
                {
                    return false;
                }

                // Remove the current role
                _context.UserRoles.Remove(userRoleMapping);

                // Assign the new role
                var newUserRole = new IdentityUserRole<string>
                {
                    UserId = userId,
                    RoleId = role.Id
                };
                _context.UserRoles.Add(newUserRole);

                // Save changes
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
