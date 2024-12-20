using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities.Models
{
    public class favourite_product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("applicationUserId")]
        [ValidateNever]
        public ApplicationUser? aspplicationUser { get; set; }
        public string applicationUserId { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Products { get; set; }
    }

}
