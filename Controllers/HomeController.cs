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

       
        public IActionResult GuardarUbicacion(double Latitud, double Longitud, int IdViaje)
        {
            BD.GuardarUbicacion(Latitud, Longitud, IdViaje);
            return RedirectToAction("Inicio");
        }

        
        [HttpPost]
        public IActionResult CompararUbicacion(double Latitud, double Longitud, int IdViaje = 0)
        {
            Viaje viajeParticular = new Viaje
            {
                Id = IdViaje,
                Latitud = Latitud,
                Longitud = Longitud
            };

            List<Viaje> viajesEmergencia = BD.ObtenerViajesActivos() ?? new List<Viaje>();

            double distanciaMinimaMetros = viajesEmergencia
                .Where(v => v.Id != IdViaje) 
                .Select(v => viajeParticular.CalcularDistancia(v) * 1000.0) 
                .DefaultIfEmpty(double.MaxValue)
                .Min();

            string color = distanciaMinimaMetros <= 100 ? "red"
                         : distanciaMinimaMetros <= 300 ? "yellow"
                         : "green";

            TempData["Distancia"] = (int)Math.Round(distanciaMinimaMetros);
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
