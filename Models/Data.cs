using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Dapper;

namespace REACT.Models;
public class Data
{
    [JsonProperty]
    public string Color { get; private set; }
    [JsonProperty]
    public int Distancia { get; private set; }

    public Data(string color, int distancia)
    {
        this.Color = color;
        this.Distancia = distancia;
    
    }
    public Data()
    {
    }

}