using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using models;
using viewmodel;

namespace TP8.Controllers;

public class ProductosController : Controller
{
    private ProductoRepository _repoProducto;
    public ProductosController()
    {
        _repoProducto = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = _repoProducto.Listar();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel pvm)
    {
        if (!ModelState.IsValid){
            return View(pvm);
        }
        var newProd = new Producto
        {
            Descripcion = pvm.Descripcion,
            Precio = (double)pvm.Precio
        };
        _repoProducto.Crear(newProd);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var p = _repoProducto.Obtener(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel pvm)
    {
        if (pvm == null || id != pvm.IdProducto)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(pvm);
        }
        var pr = new Producto
        {
            IdProducto = pvm.IdProducto,
            Descripcion = pvm.Descripcion,
            Precio = (double)pvm.Precio
        };
        _repoProducto.Modificar(pr.IdProducto, pr);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var p = _repoProducto.Obtener(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Delete(Producto p)
    {           
        _repoProducto.Borrar(p.IdProducto);
        return RedirectToAction("Index");
    }

}