

namespace models;

public class Producto{

    public int IdProducto { get; set; }
    
    public string Descripcion { get; set; }
    
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