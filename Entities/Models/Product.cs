using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        [DefaultValue(0)]
        public double? Discount { get; set; }
     
        public string? SizeAndFit { get; set; }
        public string? Addition_Formation { get; set; }
        public string? Care { get; set; }
        public List<string>? img { get; set; }
        public List<string>? Size { get; set; }
        public List<string>? Color { get; set; }

        public int TotalNumberOfPieces { get; set; }
        public int NumberOfPiecesAvailable { get; set; }


        [DefaultValue(false)]
        public bool CanBeGift { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<Review>? Reviews { get; set; }
        public DateTime DataInserted { get; set; } = DateTime.Now;// mean when this item insereted from admin
        [DefaultValue(0)]
        public int CommissionRate { get; set; }



    }
}
