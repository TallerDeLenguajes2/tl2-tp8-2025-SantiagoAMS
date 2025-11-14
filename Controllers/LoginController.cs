using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using models;
using viewmodel;
using repositorios;
namespace TP8.Controllers;

public class LoginController : Controller
{
    private IAuthenticationService _auth;

    public LoginController(IAuthenticationService auth)
    {
        _auth = auth;
    }

    [HttpGet]
    public IActionResult Index()
    {

        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrEmpty(model.Username))
        {
            model.ErrorMessage = "Ingresar usuario.";
            return View("Index",model);
        }
        if (string.IsNullOrEmpty(model.Pass))
        {
            model.ErrorMessage = "Ingresar contraseña.";
            return View("Index",model);
        }

        if (!_auth.Login(model.Username,model.Pass))
        {
            model.ErrorMessage = "Credenciales inválidas.";
            return View("Index",model);
        }

        return RedirectToAction("Index","Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _auth.Logout();
        return RedirectToAction("Index");
    }

}