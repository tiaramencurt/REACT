using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Dapper;

namespace REACT.Models;
public class Usuario
{
    [JsonProperty]
    public int Id {get; private set;}
    [JsonProperty]
    public string Username {get; private set;}
    [JsonProperty]
    public string Contraseña {get; private set;}
    [JsonProperty]
    public string Mail {get; private set;}
    [JsonProperty]
    public int Tipo {get; private set;}
    
    public Usuario(string username, string contraseña, string mail, int tipo)
    {
        this.Username = username;
        this.Contraseña = contraseña;
        this.Mail = mail;
        this.Tipo = tipo;
    }
   
}