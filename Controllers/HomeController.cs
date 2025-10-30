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
        return View("Principal");
    }
    public IActionResult CrearViaje ( int IdUsuario)
    {
       int idViaje =  BD.CrearViaje(IdUsuario);
       ViewBag.idViaje = idViaje;
       return View ("Principal");
    }

    public IActionResult FinalizarViaje ( int IdViaje)
    {
       BD.FinalizarViaje(IdViaje);
       ViewBag.idViaje = null;
       return View ("Principal");
    }
    public IActionResult GuardarUbicacion (double Latitud, double Longitud)
    {
        BD.ActualizarUbicacion(Latitud, Longitud);
        return View ("Principal");
    }
   
}
