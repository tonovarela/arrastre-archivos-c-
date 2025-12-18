
namespace arrastre_archivos.DAO;

using Microsoft.Data.SqlClient;
using DotNetEnv;
using arrastre_archivos.conf;

public class DAO
{    
     public static string connectionString =String.Empty;

     public DAO()
        {
        Conf conf = Conf.getInstance();
        connectionString = conf.ConnectionString ;
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
