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
	public class OrderDetail
	{
		public int Id { get; set; }
		[ForeignKey("OrderHeader")]
		public int OrderId { get; set; }
		[ValidateNever]
		public OrderHeader OrderHeader { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }		
		public int Count { get; set; }
		public double Price { get; set; }
        public List<string>? Size { get; set; }
        public List<string>? Color { get; set; }
        [DefaultValue(0)]
        public short? ProductClassification { get; set; }//0->product with price //1->giftProduct
    }
}
