namespace models;
public class PresupuestoDetalle
{
    public Producto Producto { get; private set; }
    public int Cantidad { get; private set; }

    public PresupuestoDetalle()
    {
        
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
