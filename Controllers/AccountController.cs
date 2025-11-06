using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REACT.Models;
using BCrypt.Net;
using System.Linq;

namespace REACT.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IWebHostEnvironment _env;

    public AccountController(ILogger<AccountController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public IActionResult Login()
    {
        ViewBag.mailExiste = true;
        ViewBag.contraseñaCoincide = true;
        return View("login");
    }
    [HttpPost]
    public IActionResult Login(string Usuario, string Contraseña)
    {
        if (Usuario == null || Contraseña == null)
        {
            return RedirectToAction("Login");
        }else
        {
            Usuario usuario = BD.TraerUsuario(Usuario);
            if (usuario == null)
            {
                ViewBag.mailExiste = false;
                ViewBag.contraseñaCoincide = true;
                return View("login");
            }else if(BCrypt.Net.BCrypt.Verify(Contraseña, usuario.Contraseña)){
                HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
                if(usuario.Tipo == false)
                {
                    return RedirectToAction("Inicio", "Home");
                }else
                {
                    return RedirectToAction("Inicio", "Home");
                }
            }else{
                ViewBag.mailExiste = true;
                ViewBag.contraseñaCoincide = false;
                return View("login");
            }
        }
    }
    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Remove("IdUsuario");
        return RedirectToAction("Login");
    }
    public IActionResult Registrarse()
    {
        ViewBag.contraseñaCoincide = true;
        ViewBag.mailExiste = false;
        return View("registrarse");
    }
    [HttpPost]
    public IActionResult Registrarse(string Usuario, string Contraseña1, string Contraseña2, string Mail, bool Tipo)
    {
        if (Usuario == null || Contraseña1 == null || Contraseña2 == null || Mail == null )
        {
            return RedirectToAction("Registrarse");
        }else
        {
            if (Contraseña1 != Contraseña2)
            {
                ViewBag.mailExiste = false;
                ViewBag.contraseñaCoincide = false;
                return View("registrarse");
            }else
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(Contraseña1);
                
                Usuario nuevoUsuario = new Usuario(Usuario, hash, Mail, Tipo);
                bool registro = BD.Registrarse(nuevoUsuario);
                if (!registro)
                {
                    ViewBag.contraseñaCoincide = true;
                    ViewBag.mailExiste = true;
                    return View("registrarse");
                }
                return RedirectToAction("Login"); 
            }
        }
    }

}