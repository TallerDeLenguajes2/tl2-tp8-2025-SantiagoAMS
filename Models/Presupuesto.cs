namespace models;

public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public string NombreDestinatario { get; set; }
    public DateOnly FechaCreacion { get; set; }
    public List<PresupuestoDetalle> Detalle { get; set; }


    public Presupuesto(string nombre, DateOnly fecha)
    {
        this.IdPresupuesto = -1;
        this.NombreDestinatario = nombre;
        this.FechaCreacion = fecha;
        this.Detalle = [];
    }

    public Presupuesto(int id, string nombre, DateOnly fecha)
    {
        this.IdPresupuesto = id;
        this.NombreDestinatario = nombre;
        this.FechaCreacion = fecha;
        this.Detalle = [];
    }

    public Presupuesto(int id, string nombre, string fecha)
    {
        this.IdPresupuesto = id;
        this.NombreDestinatario = nombre;
        this.Detalle = [];
        this.FechaCreacion = DateOnly.Parse(fecha);
    }

    public Presupuesto(int id, string nombre, DateOnly fecha, List<PresupuestoDetalle> detalle)
    {
        this.IdPresupuesto = id;
        this.NombreDestinatario = nombre;
        this.FechaCreacion = fecha;
        this.Detalle = detalle;
    }


    public Presupuesto()
    {


    }

    public double MontoPresupuesto()
    {
        double ret = 0;
        foreach (var p in Detalle)
        {
            ret += p.Subtotal();
        }
        return ret;
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }

    public int CantidadProductos()
    {
        return this.Detalle.Sum(p => p.Cantidad);
    }


    
}