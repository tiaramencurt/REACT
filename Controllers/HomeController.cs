using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REACT.Models;
using System.Globalization;

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

      
        public IActionResult CrearViaje()
        {
             string idStr = HttpContext.Session.GetString("IdUsuario");
    if (string.IsNullOrEmpty(idStr))
        return RedirectToAction("Login", "Account");

    int id = int.Parse(idStr);
    BD.CrearViaje(id);
    return RedirectToAction("Inicio");
        }

        public IActionResult FinalizarViaje(int IdViaje)
        {
            BD.FinalizarViaje(IdViaje);
            return RedirectToAction("Inicio");
        }

          [HttpPost]
     [HttpPost]
public IActionResult GuardarUbicacion(string Latitud, string Longitud, int IdViaje)
{
    double lat = double.Parse(Latitud.Replace(',', '.'), CultureInfo.InvariantCulture);
    double lon = double.Parse(Longitud.Replace(',', '.'), CultureInfo.InvariantCulture);
    BD.GuardarUbicacion(lat, lon, IdViaje);
    return RedirectToAction("Inicio");
}

[HttpPost]
public IActionResult CompararUbicacion(string Latitud, string Longitud, int IdViaje = 0)
{
    double lat = double.Parse(Latitud.Replace(',', '.'), CultureInfo.InvariantCulture);
    double lon = double.Parse(Longitud.Replace(',', '.'), CultureInfo.InvariantCulture);
    if (IdViaje > 0) BD.GuardarUbicacion(lat, lon, IdViaje);

    Viaje viajeParticular = new Viaje { Id = IdViaje, Latitud = lat, Longitud = lon };

    List<Viaje> viajesEmergencia = BD.ObtenerViajesActivos() ?? new List<Viaje>();
    List<Viaje> viajesValidos = new List<Viaje>();
    foreach (Viaje v in viajesEmergencia)
        if (v.Latitud != 0 && v.Longitud != 0) viajesValidos.Add(v);

    if (viajesValidos.Count == 0)
    {
        TempData["Distancia"] = 0;
        TempData["Color"] = "green";
        return RedirectToAction("Inicio");
    }

    double minMetros = double.MaxValue;
    foreach (Viaje v in viajesValidos)
    {
        if (v.Id == IdViaje) continue;
        double km = viajeParticular.CalcularDistancia(v);
        double m = km * 1000.0;
        if (m < minMetros) minMetros = m;
    }

        string color;

        if (minMetros <= 100)
        {
            color = "red";
        }
        else if (minMetros <= 300)
        {
            color = "yellow";
        }
        else
        {
            color = "green";
        }    
    TempData["Distancia"] = (int)Math.Round(minMetros);
    TempData["Color"] = color;
    return RedirectToAction("Inicio");
}

       public IActionResult Inicio()
        {
            string idStr = HttpContext.Session.GetString("IdUsuario");
            if (string.IsNullOrEmpty(idStr))
                return RedirectToAction("Login", "Account");

            Usuario usuario = BD.TraerUsuarioPorId(int.Parse(idStr));
            ViewBag.usuario = usuario;

            Viaje viaje = BD.ObtenerUltimoViaje(usuario.Id);
            if (viaje == null)
            {
                ViewBag.estadoUltimoViaje = false;
                ViewBag.idViaje = null;
            }
            else
            {
                ViewBag.estadoUltimoViaje = viaje.Estado;
                ViewBag.idViaje = viaje.Id;
            }

            
            if (usuario.Tipo == true) 
                return View("Servicios");
            else                       
                return View("Particulares");
        }
}
    
