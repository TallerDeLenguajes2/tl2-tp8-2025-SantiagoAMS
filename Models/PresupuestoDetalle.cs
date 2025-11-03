namespace models;
public class PresupuestoDetalle
{
    public int Id { get; private set; }
    public int IdPresupuesto { get; private set; }
    public Producto Producto { get; private set; }
    public int Cantidad { get; private set; }

    public PresupuestoDetalle()
    {
        
    }
    public PresupuestoDetalle(int id, Producto pro, int idPresupuesto, int cantidad)
    {
        this.Id = id;
        this.IdPresupuesto = idPresupuesto;
        this.Producto = pro;
        this.Cantidad = cantidad;
    }

    public PresupuestoDetalle(Producto p, int cant)
    {
        this.Producto = p;
        this.Cantidad = cant;
    }

    public double Subtotal(){
        return this.Producto.Precio * this.Cantidad;
    }
    
    public double SubtotalConIva(){
        return Subtotal() * 1.21;
    }

}
