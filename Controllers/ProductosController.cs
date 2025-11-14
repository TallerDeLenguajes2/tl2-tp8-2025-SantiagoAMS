using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using models;
using interfaces;
using viewmodel;
using services;
using repositorios;

namespace TP8.Controllers;

public class ProductosController : Controller
{
    private IProductoRepository _repoProducto;
    private IAuthenticationService _auth;

    public ProductosController(IProductoRepository repo, IAuthenticationService auth)
    {
        _repoProducto = repo;
        _auth = auth;
    }


    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions(); 
            if (securityCheck != null) return securityCheck; 
        List<Producto> productos = _repoProducto.GetAll();
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
        _repoProducto.Add(newProd);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var p = _repoProducto.Get(id);
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
        _repoProducto.Update(pr.IdProducto, pr);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var p = _repoProducto.Get(id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Delete(Producto p)
    {           
        _repoProducto.Delete(p.IdProducto);
        return RedirectToAction("Index");
    }

    /////////////////////////////////////////////////////////////
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

}