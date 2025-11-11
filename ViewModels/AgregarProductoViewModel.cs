using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace viewmodel;

public class AgregarProductoViewModel
{
    [Display(Name="ID del presupuesto")]
    public int IdPresupuesto{get;set;}

    [Display(Name="ID del producto")]
    public int IdProducto{get;set;}

    [Display(Name="Cantidad")]
    [Required(ErrorMessage="La cantidad es obligatoria.")]
    [Range(1,int.MaxValue, ErrorMessage="La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }
    public SelectList ListaProducto { get; set; }
}