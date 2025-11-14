using models;
namespace interfaces;
public interface IPresupuestoRepository
{
    int Add(Presupuesto p);
    bool Update(Presupuesto p);
    List<Presupuesto> GetAll(bool getDetails = true);
    List<PresupuestoDetalle> GetDetails(Presupuesto p);
    List<PresupuestoDetalle> GetDetails(int idPresupuesto);
    Presupuesto Get(int idPresupuesto);
    int AddProducto(int idPresupuesto, int idProducto, int cantidad);
    int AddProducto(int idPresupuesto, Producto prod, int cantidad);
    int UpdateDetail(PresupuestoDetalle pd);
    bool Delete(int id);
}