using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using models;
namespace viewmodel;
public class LoginViewModel
{
    [Display(Name = "Nombre de usuario.")]
    public string Username{get;set;}

    [Display(Name = "Contraseña.")]
    public string Pass{get;set;}

    public string ErrorMessage{get;set;}

    public LoginViewModel()
    {
        
    }
}