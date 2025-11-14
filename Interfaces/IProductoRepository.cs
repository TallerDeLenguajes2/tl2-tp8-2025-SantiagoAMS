using models;
namespace interfaces;
public interface IProductoRepository
{
    List<Producto> GetAll();
    Producto Get(int id);
    Producto Get(string descripcion, bool equalDescripcion = false);
    int Add(Producto p);
    void Update(int id, Producto p);
    void Delete(int id);
}