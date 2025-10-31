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
        var presupuestos = _repoPresu.Listar();
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
}