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
        Viaje viaje = BD.ObtenerUltimoViaje(1);
        ViewBag.estadoUltimoViaje = viaje.Estado;
        ViewBag.idViaje = viaje.Id;
        return View("Servicios");
    }
    public 

    public IActionResult CrearViaje ( int IdUsuario)
    {
       int idViaje =  BD.CrearViaje(IdUsuario);
       ViewBag.idViaje = idViaje;
       return View ("Servicios");
    }

    public IActionResult FinalizarViaje ( int IdViaje)
    {
       BD.FinalizarViaje(IdViaje);
       ViewBag.idViaje = null;
       return View ("Servicios");
    }
    public IActionResult GuardarUbicacion (double Latitud, double Longitud, int IdViaje)
    {
        BD.GuardarUbicacion(Latitud, Longitud,IdViaje);
        return View ("Servicios");
    }
    public IActionResult CompararUbicacion(double Latitud, double Longitud, int IdViaje)
    {
        
    }
    public IActionResult HomeC()
    {

    }
}
  
