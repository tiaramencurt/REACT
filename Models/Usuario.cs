using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TP07.Models;
public class Usuario
{
    [JsonProperty]
    public int Id {get; private set;}
    [JsonProperty]
    public string Username {get; private set;}
    [JsonProperty]
    public string Password {get; private set;}
    [JsonProperty]
    public string Mail {get; private set;}
    [JsonProperty]
    public string Tipo {get; private set;}
    [JsonProperty]
    public bool Estado {get; private set;}
    [JsonProperty]
    public DateTime? UltimoLogin { get; private set; }
    public Usuario()
    {
    }
    public Usuario(string username, string password, string mail, string tipo)
    {
        this.Username = username;
        this.Password = password;
        this.Mail = mail;
        this.Tipo = tipo;
        this.Estado = false;
        this.UltimoLogin = null;
    }
}