using System.ComponentModel.DataAnnotations;

namespace AplicacionCRUDAddAccion.Models
{
    
    public class ProductModel
    {
        
        [Required(ErrorMessage = "El campo codigo es obligatorio.")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "El campo categoria es obligatorio.")]
        public int CategoryProductId { get; set; }
        [Required(ErrorMessage = "El campo descripcion es obligatorio.")]
        public string? ProductDescription { get; set; }
        
        public int? Stock { get; set; }
        [Required(ErrorMessage = "El campo precio es obligatorio.")]
        public double Price { get; set; }       
        public bool HaveECDiscount { get; set; }
        public bool IsActive { get; set; }
        public string? CategoryString { get; set; }

    }
}
