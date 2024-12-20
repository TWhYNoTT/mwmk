using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Marketer
    {
        [Key]
        public int Id { get; set; }
        public double TotalProfit { get; set; }
        [DefaultValue(false)]
        public bool IsPaymentRequested { get; set; }
        public string ProofPaymentImage { get; set; }
        public string WalletNumber { get; set; }


        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
