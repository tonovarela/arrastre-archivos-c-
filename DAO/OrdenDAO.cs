namespace arrastre_archivos.DAO;

using arrastre_archivos.entities;
using Microsoft.Data.SqlClient;

public class OrdenDAO : DAO
{
    public List<RutaMetrics> obtenerRutas(string ordenCompra)
    {
        List<RutaMetrics> rutas = new List<RutaMetrics>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT  
	                            EC=E.Sequencial,
	                            SC=NumSolicitacao,
	                            OC=B.NroPedido, 
	                            Ejercicio=Year(E.DataEntrada),
	                            Periodo =   right(replicate('0', 2) + CAST(month(e.dataEntrada) as VARCHAR(2)), 2),
	                            numProveedor=E.CliFor_Codigo,
	                            nombreProveedor = E.CliFor_RazaoSocial, 
	                            Fecha=convert(date,E.DataEntrada),
	                            p.RFC,
	                            ruta  = CONCAT('\\192.168.2.217\intelisis\CFD\ContabilidadElectronica\XML_PROV\LITO\FACTURAS_PROVEEDORES\',Year(E.DataEntrada), '\\', right(replicate('0', 2) + CAST(month(e.dataEntrada) as VARCHAR(2)), 2), '\\',E.CliFor_Codigo,'\\') 
                                FROM metricsweb.dbo.ESTNotasFiscaisEntrada E
	                            INNER JOIN metricsweb.dbo.ESTItemNotaEntrada D ON E.Objid = D.Objid_NotasFiscaisEntrada                     
	                            INNER JOIN metricsweb.dbo.ESTItemNFlxItemPedCompra A on D.Objid=Objid_ItemNotaFiscal
	                            INNER JOIN metricsweb.dbo.COMItemPedidoCompra B on A.ObjId_ItemPedidoCompra=B.Objid
	                            INNER JOIN metricsweb.dbo.COMSolicitacaoItemPedidoCompra C on C.ObjId_ItemPedidoCompra = B.ObjId
	                            INNER JOIN metricsweb.dbo.COMSolicitacaoCompra F on F.ObjId = C.ObjId_SolicitacaoCompra
	                            LEFT JOIN lito.dbo.prov p on convert(varchar,p.proveedor)  = convert(varchar,E.CliFor_Codigo)
                                WHERE    
	                            E.CFOP NOT IN ('1.104')  
	                            and (E.Situacao = 1) 
	                            and e.Sequencial = @ordenCompra";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ordenCompra", ordenCompra);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RutaMetrics ruta = new RutaMetrics
                            {
                                EC = reader["EC"].ToString(),
                                SC = reader["SC"].ToString(),
                                OC = reader["OC"].ToString(),
                                Ejercicio = Convert.ToInt32(reader["Ejercicio"]),
                                Periodo = reader["Periodo"].ToString(),
                                numProveedor = reader["numProveedor"].ToString(),
                                nombreProveedor = reader["nombreProveedor"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                RFC = reader["RFC"].ToString(),
                                ruta = reader["ruta"].ToString()
                            };
                            rutas.Add(ruta);                            
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer órdenes: {ex.Message}");
        }
        return rutas;
    }

}
