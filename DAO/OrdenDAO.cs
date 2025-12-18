
namespace arrastre_archivos.DAO;
using Microsoft.Data.SqlClient;

public class OrdenDAO : DAO
{
      public void leerOrdenes()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 10 * FROM Orden"; // Ejemplo de consulta
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["numero_orden"]} ");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer órdenes: {ex.Message}");
        }


    }

}
