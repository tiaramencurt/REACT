using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Dapper;

namespace REACT.Models;
public class Viaje
{
    [JsonProperty]
    public int Id {get; set;}
    [JsonProperty]
    public int IdUsuario {get; private set;}
    [JsonProperty]
    public bool Estado {get; set;}
    [JsonProperty]
    public double Latitud {get; set;}
    [JsonProperty]
    public double Longitud {get; set;}
     public Viaje() { } 
    public Viaje(int idusuario, bool estado, double latitud, double longitud)
    {
        this.IdUsuario = idusuario;
        this.Estado = estado;
        this.Latitud = latitud;
        this.Longitud = longitud;
    }
     public double CalcularDistancia(Viaje viajeEmergencia)
        {
            const double R = 6371.0;
            double dLat = (viajeEmergencia.Latitud - this.Latitud) * Math.PI / 180.0;
            double dLon = (viajeEmergencia.Longitud - this.Longitud) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(this.Latitud * Math.PI / 180.0) *
                       Math.Cos(viajeEmergencia.Latitud * Math.PI / 180.0) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; 
        }
}