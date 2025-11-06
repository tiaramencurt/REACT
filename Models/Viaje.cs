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
   public double CalcularDistancia(Viaje otro)
{
    const double R = 6371.0; 
    double lat1 = GradosARadianes(this.Latitud);
    double lon1 = GradosARadianes(this.Longitud);
    double lat2 = GradosARadianes(otro.Latitud);
    double lon2 = GradosARadianes(otro.Longitud);

    double dlat = lat2 - lat1;
    double dlon = lon2 - lon1;

    double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
               Math.Cos(lat1) * Math.Cos(lat2) *
               Math.Sin(dlon / 2) * Math.Sin(dlon / 2);
    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    return R * c; 
}

private static double GradosARadianes(double grados)
{
    return grados * Math.PI / 180.0;
}
}