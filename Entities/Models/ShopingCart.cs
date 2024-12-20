using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ShopingCart
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]

        public Product Product { get; set; }
        [Range(1,100,ErrorMessage ="Product Count in between 1:100 ")]
        public int Count { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        [DefaultValue(0)]
        public short? ProductClassification { get; set; }//0->product with price //1->giftProduct
        [ForeignKey("applicationUserId")]
        [ValidateNever]
        public ApplicationUser aspplicationUser { get; set; }
        public string applicationUserId { get; set; }
    }
}
