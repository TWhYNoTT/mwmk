using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double TotalPrice { get; set; }
        public string? OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
  

		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }



        // Marketer relationship
        public int? MarketerId { get; set; } // Foreign key to Marketer

        [ForeignKey("MarketerId")]
        [ValidateNever]
        public Marketer? Marketer { get; set; } // Navigation property




        public DateTime OrderTime { get; set; }

        [ForeignKey("IncomePeriod")]
        public int? IncomePeriodID { get; set; }
        [ValidateNever]
        public IncomePeriod? IncomePeriod { get; set; }

        [ForeignKey("Coupon")]
        public int? CouponID { get; set; }
        [ValidateNever]
        public Coupon? Coupon { get; set; }

    }


}
