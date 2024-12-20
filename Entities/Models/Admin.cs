using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Admin : IdentityUser//: User
    {
        [Key]
        public int Id { get; set; }
        public double DefaultMarketerDiscount { get; set; }
        public static List<string>? HomeBackgroundImages { get; set; }
        public static List<string>? TextInHome { get; set; }
        [DefaultValue(true)]
        public static bool? AppearRateAndReview { get; set; }
    }
}
