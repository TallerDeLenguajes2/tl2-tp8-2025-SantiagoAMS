using System.ComponentModel.DataAnnotations;

namespace viewmodel;

public class ProductoViewModel
{
    // max 250([StringLe])
    
    public int IdProducto { get; set; }

    [Display(Name="Descripcion del producto")]
    [StringLength(250, ErrorMessage="La descripción no puede superar los 250 caracteres.")
    public string Descripcion { get; set; }

    [Display(Name = "Precio Unitario")]
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage="El precio debe ser un valor positivo.")]
    public decimal Precio { get; set; }
}