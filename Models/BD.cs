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
        string query = "SELECT * FROM Usuarios WHERE Username = @Username";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { Username = username});
            return usuario;
        }
    }
    public static Usuario TraerUsuarioPorId(int IdUsuario)
    {
        string query = "SELECT * FROM Usuarios WHERE Id = @PIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { PIdUsuario = IdUsuario});
            return usuario;
        }
    }
    public static Usuario Login(string username, string password)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE Username = @Username AND Password = @Password";
            Usuario usuario = connection.QueryFirstOrDefault<Usuario>(query, new { Username = username, Password = password });
            return usuario;
        }
    }
    public static bool Registrarse(Usuario usuario)
    {
        if(TraerUsuario(usuario.Username) == null)
        {
            string query = "INSERT INTO Usuarios (Mail, Contraseña, Username, Tipo) VALUES (@PMail, @PContraseña, @PUsername, @PTipo)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
               connection.Execute(query, new { PUsername = usuario.Username, PContraseña = usuario.Contraseña, PMail = usuario.Mail, PTipo = usuario.Tipo  });
            }
            return true;
        }else
        {
            return false;
        }
    }

    public static void ActualizarFechaLogin(int IdUsuario)
    {
        string query = "UPDATE Usuarios SET UltimoLogin = @UltimoLogin WHERE Id = @IdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { UltimoLogin = DateTime.Now, IdUsuario = IdUsuario });
        }
    }
    public static int CrearViaje(int IdUsuario)
    {
        string query = "INSERT INTO Viajes (IdUsuario, Estado, Latitud, Longitud)  OUTPUT INSERTED.Id VALUES (@PIdUsuario, @PEstado, @PLatitud, @PLongitud)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int idViaje =connection.ExecuteScalar<int>(query, new { PIdUsuario = IdUsuario, PEstado = true, PLatitud = 1, PLongitud = 1 });
              return idViaje;
        }
      
    }
    public static void FinalizarViaje(int IdViaje)
    {
        string query = "UPDATE Viajes SET Estado = @PEstado WHERE Id = @PIdViaje";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { PEstado = false, PIdViaje = IdViaje });
        }
    }
    public static Viaje ObtenerUltimoViaje(int IdUsuario)
    {
          string query = "SELECT TOP 1 * FROM Viajes WHERE IdUsuario = @PIdUsuario ORDER BY Id DESC ";
           using (SqlConnection connection = new SqlConnection(_connectionString))
        {
             Viaje viaje = connection.QueryFirstOrDefault<Viaje>(query, new { PIdUsuario = IdUsuario });
             return viaje;
        }
           
            
    }
    public static void GuardarUbicacion (double Latitud, double Longitud, int IdViaje)
    {
        string query = "UPDATE Viajes SET Latitud = @PLatitud, Longitud = @PLongitud WHERE Id = @PIdViaje";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { PLatitud = Latitud, PLongitud = Longitud, PIdViaje = IdViaje });
        }
    }
        public static List<Viaje> ObtenerViajesActivos()
    {
          string query = "SELECT Viajes.Id, Viajes.IdUsuario, Viajes.Estado, Viajes.Latitud, Viajes.Longitud FROM Viajes INNER JOIN Usuarios ON Viajes.IdUsuario = Usuarios.Id WHERE Viajes.Estado = 1 AND Usuarios.Tipo = 1";
           using (SqlConnection connection = new SqlConnection(_connectionString))
        {
             List<Viaje> viajes = connection.Query<Viaje>(query).ToList();
             return viajes;
        }
           
            
    }
    
}

