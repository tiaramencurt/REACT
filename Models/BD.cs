using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using BCrypt.Net;
namespace TP07.Models;

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
            string query = "INSERT INTO Usuarios (Username, Password, Nombre, Apellido, Foto) VALUES (@PUsername, @PPassword, @PNombre, @PApellido, @PFoto)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { PUsername = usuario.Username, PPassword = usuario.Password, PNombre = usuario.Nombre, PApellido = usuario.Apellido, PFoto = usuario.Foto  });
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
}