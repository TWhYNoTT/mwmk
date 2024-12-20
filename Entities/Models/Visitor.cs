using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Entities.Models
{
    public class Visitor :IdentityUser
    {
        public string IPAddress { get; set; }
        public DateTime? VisitDate { get; set; }
    }
}
