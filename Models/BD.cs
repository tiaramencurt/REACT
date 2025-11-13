using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using BCrypt.Net;
namespace REACT.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=REACT; Integrated Security=True; TrustServerCertificate=True;";
    //private static string _connectionString = @"Server=localhost\SQLEXPRESS;Database=REACT;Trusted_Connection=True;Integrated Security=True; TrustServerCertificate=True;";
    public static Usuario TraerUsuario(string username)
    {
        string query = "TraerUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { Username = username}, commandType: CommandType.StoredProcedure);
            return usuario;
        }
    }
    public static Usuario TraerUsuarioPorId(int IdUsuario)
    {
        string query = "TraerUsuarioPorId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { PIdUsuario = IdUsuario}, commandType: CommandType.StoredProcedure);
            return usuario;
        }
    }
    public static Usuario Login(string username, string password)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "Login";
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { Username = username, Password = password }, commandType: CommandType.StoredProcedure);
            return usuario;
        }
    }
    public static bool Registrarse(Usuario usuario)
    {
        if(TraerUsuario(usuario.Username) == null)
        {
            string query = "Registrarse";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
               connection.Execute(query, new { PUsername = usuario.Username, PContraseña = usuario.Contraseña, PMail = usuario.Mail, PTipo = usuario.Tipo  }, commandType: CommandType.StoredProcedure);
            }
            return true;
        }else
        {
            return false;
        }
    }


    public static int CrearViaje(int IdUsuario)
    {
        const string query = "CrearViaje";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int idViaje = connection.ExecuteScalar<int>(query, new { PIdUsuario = IdUsuario, PEstado = true,  PLatitud = 0.0,  PLongitud = 0.0}, commandType: CommandType.StoredProcedure);
            return idViaje;
        }
    }
    public static void FinalizarViaje(int IdViaje)
    {
        string query = "FinalizarViaje";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { PEstado = false, PIdViaje = IdViaje }, commandType: CommandType.StoredProcedure);
        }
    }
    public static Viaje ObtenerUltimoViaje(int IdUsuario)
    {
          string query = "ObtenerUltimoViaje";
           using (SqlConnection connection = new SqlConnection(_connectionString))
        {
             Viaje viaje = connection.QueryFirstOrDefault<Viaje>(query, new { PIdUsuario = IdUsuario }, commandType: CommandType.StoredProcedure);
             return viaje;
        }
           
            
    }
    public static void GuardarUbicacion (double Latitud, double Longitud, int IdViaje)
    {
        string query = "GuardarUbicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { PLatitud = Latitud, PLongitud = Longitud, PIdViaje = IdViaje }, commandType: CommandType.StoredProcedure);
        }
    }
        public static List<Viaje> ObtenerViajesActivos()
    {
          string query = "ObtenerViajesActivos";
           using (SqlConnection connection = new SqlConnection(_connectionString))
        {
             List<Viaje> viajes = connection.Query<Viaje>(query, commandType: CommandType.StoredProcedure).ToList();
             return viajes;
        }
           
            
    }
    
}

