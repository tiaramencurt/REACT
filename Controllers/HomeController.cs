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
        return View("Principal");
    }
    public IActionResult CrearViaje ( int IdUsuario)
    {
       bool viajeCreado =  BD.CrearViaje(IdUsuario);
       ViewBag.viajeCreado = viajeCreado;
       return View ("Principal");
    }
   
}
