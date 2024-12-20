using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Components;
using DataAccessLayer.Data;
using Entities.Models;

public class RoleService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    public RoleService(UserManager<IdentityUser> userManager,ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<int> ChangeRoleToMarketer(string userId)
    {
        //0 ->> false , 1->>true ,2->>aready Marketer 3->>error 
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return 3;

        if (await _userManager.IsInRoleAsync(user, "Customer"))
        {
            // Add the "Marketer" role
            var addResult = await _userManager.AddToRoleAsync(user, "Marketer");
            if (!addResult.Succeeded) return 3;

            // Create a new marketer object and assign it to the user
            var marketer = new Marketer
            {
                UserId = userId,
                TotalProfit = 0,
                IsPaymentRequested = false,
                ProofPaymentImage = "/Images/nomoney.png", // You can set a default or leave it null
                WalletNumber = string.Empty // Default value or null
            };

            // Save the marketer to the database
            _context.Marketers.Add(marketer);
            await _context.SaveChangesAsync();

            return 1;
        }
        else if (await _userManager.IsInRoleAsync(user, "Marketer"))
        {
            return 2;
        }
            return 3;
    }


    //public async Task<bool> ChangeRoleToMarketer(string userId)
    //{
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null) return false;

    //    // Remove the Customer role if it exists
    //    if (await _userManager.IsInRoleAsync(user, "Customer"))
    //    {
    //        var removeResult = await _userManager.RemoveFromRoleAsync(user, "Customer");
    //        if (!removeResult.Succeeded)
    //            return false;
    //        // Add the Marketer role
    //        var addResult = await _userManager.AddToRoleAsync(user, "Marketer");
    //        /////////create obj 
    //        return addResult.Succeeded;

    //    }
    //    //else if (await _userManager.IsInRoleAsync(user, "Marketer"))
    //    //{ you are aready marketer
    //    //}
    //    else return true;
    //}
}


