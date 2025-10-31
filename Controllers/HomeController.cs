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
   [HttpPost][HttpPost]
/*public ActionResult CompararUbicacion(double Latitud, double Longitud, int IdViaje)
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

    // 5️⃣ Mandamos datos a la vista
    TempData["Distancia"] = (int)Math.Round(distanciaMinima);
    TempData["Color"] = color;

    return RedirectToAction("Principal");
}*/
    public IActionResult HomeC()
    {
            Usuario usuario = BD.TraerUsuarioPorId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
            ViewBag.usuario = usuario;
            return View("Particulares");
    }
      public IActionResult HomeSE()
    {
        Usuario usuario = BD.TraerUsuarioPorId(int.Parse(HttpContext.Session.GetString("IdUsuario")));
        ViewBag.usuario = usuario;
        Viaje viaje = BD.ObtenerUltimoViaje(1);
        ViewBag.estadoUltimoViaje = viaje.Estado;
        ViewBag.idViaje = viaje.Id;
        return View("Servicios");

    }
}
  
