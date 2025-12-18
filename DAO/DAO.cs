
namespace arrastre_archivos.DAO;

using Microsoft.Data.SqlClient;
using DotNetEnv;
public class DAO
{    
     public static string connectionString =String.Empty;

     public DAO()
        {
            Env.Load(); // Carga las variables del archivo .env
        connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("La variable de entorno 'SQL_CONNECTION_STRING' no está definida.");                    
        }

    public bool ValidarConexion()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
            return false;
        }
    }


}
