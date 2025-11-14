using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using viewmodel;

namespace TP8.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository _repoPresu;
    private ProductoRepository _repoProducto;
    public PresupuestosController()
    {
        _repoPresu = new PresupuestosRepository();
        _repoProducto = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var presupuestos = _repoPresu.Listar(true);
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        if (id < 0)
        {
            return View();
        }
        //encabezado del presupuesto
        //productos asociados a ese presupesto

        var ret = _repoPresu.Obtener(id);
        return View(ret);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(PresupuestoViewModel pvm)
    {
        if (!ModelState.IsValid)
        {
            return View(pvm);
        }
        if (pvm.FechaCreacion > DateTime.Now)
        {
            return View(pvm);
        }
        var fc = pvm.FechaCreacion;
        var newPres = new Presupuesto
        {
            NombreDestinatario = pvm.NombreDestinatario,
            FechaCreacion = new DateOnly(fc.Year, fc.Month,fc.Day)
        };
        _repoPresu.Crear(newPres);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var p = _repoPresu.Obtener(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto p)
    {
        _repoPresu.ModificarDatos(p);   
        if (p.Detalle != null)
        {
            foreach (var d in p.Detalle)
            {
                _repoPresu.ModificarDetalle(d);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var p = _repoPresu.Obtener(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Delete(Presupuesto p)
    {
        _repoPresu.Eliminar(p.IdPresupuesto);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult AgregarProducto(int id){
        List<Producto> productos = _repoProducto.GetAll();
        var apvm = new AgregarProductoViewModel
        {
            IdPresupuesto = id,
            ListaProducto = new SelectList(productos,"IdProducto","Descripcion")
        };
        return View(apvm);
    }
    
    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var productos = _repoProducto.GetAll();
            model.ListaProducto = new SelectList(productos, "IdProducto", "Descripcion");
            return View(model);
        }
        _repoPresu.Agregar(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        return RedirectToAction(nameof(Index), new { id = model.IdPresupuesto });
        
    }


}