using System.ComponentModel.DataAnnotations;

namespace models;

public class Producto{
    [Display(Name = "ID del producto")]
    public int IdProducto { get; set; }
    [Display(Name = "Descripcion del producto")]
    public string Descripcion { get; set; }
    [Display(Name="Precio del producto")]
    public double Precio {get;set;}

    public Producto(int id, string descripcion, double precio) {
        this.IdProducto = id;
        this.Descripcion = descripcion;
        this.Precio = precio;
    }

    public Producto(string descripcion, double precio)
    {
        this.Descripcion = descripcion;
        this.Precio = precio;
    }


    public Producto()
    {

    }
}