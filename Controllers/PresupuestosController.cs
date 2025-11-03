using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using models;

namespace TP8.Controllers;

public class PresupuestosController : Controller
{
    private PresupuestosRepository _repoPresu;
    public PresupuestosController()
    {
        _repoPresu = new PresupuestosRepository();
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
    public IActionResult Create(Presupuesto p)
    {
        var now = DateTime.Now;
        p.FechaCreacion = new DateOnly(now.Year, now.Month, now.Day);
        _repoPresu.Crear(p);
        return RedirectToAction("Index");
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
        return RedirectToAction("Index");
    }




}