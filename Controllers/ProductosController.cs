using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using models;

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

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var p = _repoProducto.Obtener(id);
        return View(p);
    }

}