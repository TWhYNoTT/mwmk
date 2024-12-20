using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }// should be unique
        public int DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }// year,month,day,hour,minute,second
        public int TotalUsagesCount { get; set; }
        [DefaultValue(0)]
        public int MinPurchaseAmount { get; set; }
        public bool IsActive { get; set; }



    }
}
