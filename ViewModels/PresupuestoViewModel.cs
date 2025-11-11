using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace viewmodel;

public class PresupuestoViewModel
{
    [Display(Name = "Nombre del destinatario.")]
    [Required(ErrorMessage= "El nombre o correo del destinatario es obligatorio.")]
    public string NombreDestinatario {get;set;}

    [Display(Name = "Fecha de Creación.")]
    [Required(ErrorMessage="La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion {get;set;}

}