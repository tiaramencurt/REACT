using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REACT.Models;

namespace REACT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult CrearViaje ( int IdUsuario)
    {
       int idViaje =  BD.CrearViaje(IdUsuario);
        Usuario usuario = BD.TraerUsuarioPorId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        ViewBag.usuario = usuario;
       ViewBag.idViaje = idViaje;
       if(usuario.Tipo == true)
       {
        return View ("Servicios");
       }else
       {
        return View ("Particulares");
       }
    }

    public IActionResult FinalizarViaje ( int IdViaje)
    {
       BD.FinalizarViaje(IdViaje);
        Usuario usuario = BD.TraerUsuarioPorId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        ViewBag.usuario = usuario;
       ViewBag.idViaje = null;
       return View ("Servicios");
    }
    public IActionResult GuardarUbicacion (double Latitud, double Longitud, int IdViaje)
    {
        BD.GuardarUbicacion(Latitud, Longitud, IdViaje);
        return View ("Servicios");
    }
[HttpPost]
public ActionResult CompararUbicacion(double Latitud, double Longitud, int IdViaje)
{
    Viaje viajeParticular = new Viaje();
    viajeParticular.Id = IdViaje;
    viajeParticular.Latitud = Latitud;
    viajeParticular.Longitud = Longitud;

    List<Viaje> viajesEmergencia = BD.ObtenerViajesActivos();

    double distanciaMinima = double.MaxValue;

    foreach (Viaje viajeEmergencia in viajesEmergencia)
    {
        double distanciaKm = viajeParticular.CalcularDistancia(viajeEmergencia);
        double distanciaMetros = distanciaKm * 1000;

        if (distanciaMetros < distanciaMinima)
        {
            distanciaMinima = distanciaMetros;
        }
    }


    string color = "green"; 
    if (distanciaMinima <= 100)
    {
        color = "red"; 
    }
    else if (distanciaMinima <= 300)
    {
        color = "yellow"; 
    }

    TempData["Distancia"] = (int)Math.Round(distanciaMinima);
    TempData["Color"] = color;

    return RedirectToAction("Principal");
}
    public IActionResult Home()
    {
            Usuario usuario = BD.TraerUsuarioPorId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
            ViewBag.usuario = usuario;
            if(usuario.Tipo == true)
            {
             return View("Particulares");
            }
            else 
            {
                 Viaje viaje = BD.ObtenerUltimoViaje(int.Parse(HttpContext.Session.GetString("IdUsuario")));
                if(viaje == null)
                {
                    ViewBag.estadoUltimoViaje = false;
                    ViewBag.idViaje = null;
                }
                else
                {
                    ViewBag.estadoUltimoViaje = viaje.Estado;
                    ViewBag.idViaje = viaje.Id;
                }
                return View("Servicios");
            }
    }

}
  
