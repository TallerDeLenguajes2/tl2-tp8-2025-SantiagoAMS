using Microsoft.AspNetCore.Mvc.Rendering;

namespace viewmodel;

public class AgregarProductoViewModel
{
    // required, mayor a 0
    public int Cantidad;
    public SelectList ListaProducto;
}