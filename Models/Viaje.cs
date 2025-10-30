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
    public int Id {get; private set;}
    [JsonProperty]
    public int IdUsuario {get; private set;}
    [JsonProperty]
    public bool Estado {get; private set;}
    [JsonProperty]
    public double Latitud {get; private set;}
    [JsonProperty]
    public double Longitud {get; private set;}
    
    public Viaje(int idusuario, bool estado, double latitud, double longitud)
    {
        this.IdUsuario = idusuario;
        this.Estado = estado;
        this.Latitud = latitud;
        this.Longitud = longitud;
    }
}