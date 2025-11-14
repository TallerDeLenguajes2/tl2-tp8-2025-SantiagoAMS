using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using services;
using models;
using viewmodel;
using repositorios;
using interfaces;
namespace TP8.Controllers;

public class PresupuestosController : Controller
{
    private IPresupuestoRepository _repoPresu;
    private IProductoRepository _repoProducto;
    private IAuthenticationService _auth;
    public PresupuestosController(IPresupuestoRepository repopresu, IProductoRepository repopro, IAuthenticationService auth)
    {
        _repoPresu = repopresu;
        _repoProducto = repopro;
        _auth = auth;
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
    ////////////////////////////////////////////////////////////////////////
    
    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null)
        {
            return securityCheck;
        }

        var presupuestos = _repoPresu.GetAll(true);
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

        var ret = _repoPresu.Get(id);
        return View(ret);
    }
    ////////////////////////////////////////////////////////////////////////

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
        _repoPresu.Add(newPres);
        return RedirectToAction(nameof(Index));
    }
    //////////////////////////////////////////////////////////////////////////////////////////
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var p = _repoPresu.Get(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto p)
    {
        _repoPresu.Update(p);   
        if (p.Detalle != null)
        {
            foreach (var d in p.Detalle)
            {
                _repoPresu.UpdateDetail(d);
            }
        }

        return RedirectToAction("Index");
    }

    /////////////////////////////////////////////////////////////////////////////
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var p = _repoPresu.Get(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Delete(Presupuesto p)
    {
        _repoPresu.Delete(p.IdPresupuesto);
        return RedirectToAction(nameof(Index));
    }

    ////////////////////////////////////////////////////////////////////////
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
        _repoPresu.AddProducto(model.IdPresupuesto, model.IdProducto, model.Cantidad);
        return RedirectToAction(nameof(Index), new { id = model.IdPresupuesto });
        
    }
    ////////////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////////////
    
    
    private IActionResult CheckAdminPermissions() 
    { 
        if (!_auth.IsAuthenticated()) 
        { 
            return RedirectToAction("Index", "Login"); 
        } 

        if (!_auth.HasAccessLevel("Administrador")) 
        { 
            return RedirectToAction(nameof(AccesoDenegado)); 
        } 
        return null; // Permiso concedido 
    } 

    private IActionResult CheckClientePermissions() 
    { 
        if (!_auth.IsAuthenticated()) 
        { 
            return RedirectToAction("Index", "Login"); 
        } 

        if (!_auth.HasAccessLevel("Cliente")) 
        { 
            return RedirectToAction(nameof(AccesoDenegado)); 
        } 
        return null; // Permiso concedido 
    } 

}