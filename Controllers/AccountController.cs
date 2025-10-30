using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REACT.Models;
using BCrypt.Net;

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
        return View("Login");
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
                return View("Login");
            }else if(BCrypt.Net.BCrypt.Verify(Contraseña, usuario.Password)){
                HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
                BD.ActualizarFechaLogin(usuario.Id);
                return RedirectToAction("MostrarTareas", "Home", new { Eliminadas = false });
            }else{
                ViewBag.mailExiste = true;
                ViewBag.contraseñaCoincide = false;
                return View("Login");
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
        return View("Registro");
    }
    [HttpPost]
    public IActionResult Registrarse(string Usuario, string Contraseña1, string Contraseña2, string Nombre, string Apellido, IFormFile Foto)
    {
        if (Usuario == null || Contraseña1 == null || Contraseña2 == null || Nombre == null || Apellido == null)
        {
            return RedirectToAction("Registrarse");
        }else
        {
            if (Contraseña1 != Contraseña2)
            {
                ViewBag.mailExiste = false;
                ViewBag.contraseñaCoincide = false;
                return View("Registro");
            }
            string carpeta = null;
            string rutaDestino = null;
            string nombreFoto = null;
            if (Foto != null && Foto.Length > 0)
            {
                carpeta = Path.Combine(_env.WebRootPath, "imagenes");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }
                nombreFoto = Guid.NewGuid().ToString() + Path.GetExtension(Foto.FileName);
                rutaDestino = Path.Combine(carpeta, nombreFoto);
                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    Foto.CopyTo(stream);
                }
            }
            else
            {
                carpeta = Path.Combine(_env.WebRootPath, "imagenes");
                if (!Directory. Exists(carpeta)){
                    Directory.CreateDirectory (carpeta);
                }
                rutaDestino = Path.Combine (carpeta, "default.png");
            }
                string hash = BCrypt.Net.BCrypt.HashPassword(Contraseña1);
                Usuario nuevoUsuario = new Usuario(Usuario, hash, Nombre, Apellido );
                bool registro = BD.Registrarse(nuevoUsuario);
                if (!registro)
                {
                    ViewBag.contraseñaCoincide = true;
                    ViewBag.mailExiste = true;
                    return View("Registro");
                }
                return RedirectToAction("Login"); 
        }
    }
    /*public IActionResult Registrarse(string Usuario, string Contraseña1, string Contraseña2, string Nombre, string Apellido, string Foto)
    {
        if (Contraseña1 != Contraseña2)
        {
            ViewBag.mailExiste = false;
            ViewBag.contraseñaCoincide = false;
            return View("Registro");
        }
        Usuario nuevoUsuario = new Usuario(Usuario, Contraseña1, Nombre, Apellido, Foto);
        bool registro = BD.Registrarse(nuevoUsuario);
        if (!registro)
        {
            ViewBag.contraseñaCoincide = true;
            ViewBag.mailExiste = true;
            return View("Registro");
        }
        return RedirectToAction("Login"); 
    }*/
}