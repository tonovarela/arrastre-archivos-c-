namespace arrastre_archivos.DAO;

using arrastre_archivos.entities;
using Microsoft.Data.SqlClient;

public class OrdenDAO : DAO
{


    public void EliminarAnexoMov(string Id)
    {
        string sql = @"DELETE FROM dbo.AnexoMov WHERE Id = @Id and tipoDocumento is not null";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar anexo mov: {ex.Message}");
        }



    }

    public List<Partida> obtenerPartidasRev(string ordenCompra)
    {
        List<Partida> listaPartidas = new List<Partida>();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                declare @EC int;
Set @EC = @ordenCompra;

	SELECT  
		Tipo = 'EC', 
		Folio = E.Sequencial,
		Ejercicio=Year(E.DataEntrada),
		Periodo =   right(replicate('0', 2) + CAST(month(e.dataEntrada) as VARCHAR(2)), 2),
		numProveedor=E.CliFor_Codigo,
		nombreProveedor = E.CliFor_RazaoSocial, 
		Fecha=convert(date,E.DataEntrada),
		p.RFC,
		X.ID  as ID
	FROM metricsweb.dbo.ESTNotasFiscaisEntrada E
		LEFT JOIN metricsweb.dbo.ESTItemNotaEntrada D ON E.Objid = D.Objid_NotasFiscaisEntrada                     
		LEFT JOIN metricsweb.dbo.ESTItemNFlxItemPedCompra A on D.Objid=Objid_ItemNotaFiscal
		LEFT JOIN metricsweb.dbo.COMItemPedidoCompra B on A.ObjId_ItemPedidoCompra=B.Objid
		LEFT JOIN metricsweb.dbo.COMSolicitacaoItemPedidoCompra C on C.ObjId_ItemPedidoCompra = B.ObjId
		LEFT JOIN metricsweb.dbo.COMSolicitacaoCompra F on F.ObjId = C.ObjId_SolicitacaoCompra
		LEFT JOIN lito.dbo.prov p on convert(varchar,p.proveedor)  = convert(varchar,E.CliFor_Codigo)
		INNER JOIN Lito.dbo.CxP X  ON E.NaturezaOperacao COLLATE Modern_Spanish_CI_AS = X.Mov AND convert(varchar,E.usr_sequencial) = convert(varchar,X.MovID)
	WHERE    
		E.CFOP NOT IN ('1.104') and (E.Situacao = 1)  and e.Sequencial = @EC
UNION
	SELECT  
		Tipo = 'SC', 
		Folio = F.NumSolicitacao,
		Ejercicio=Year(E.DataEntrada),
		Periodo =   right(replicate('0', 2) + CAST(month(e.dataEntrada) as VARCHAR(2)), 2),
		numProveedor=E.CliFor_Codigo,
		nombreProveedor = E.CliFor_RazaoSocial, 
		Fecha=convert(date,E.DataEntrada),
		p.RFC,
		X.ID  as ID
	FROM metricsweb.dbo.ESTNotasFiscaisEntrada E
		LEFT JOIN metricsweb.dbo.ESTItemNotaEntrada D ON E.Objid = D.Objid_NotasFiscaisEntrada                     
		LEFT JOIN metricsweb.dbo.ESTItemNFlxItemPedCompra A on D.Objid=Objid_ItemNotaFiscal
		LEFT JOIN metricsweb.dbo.COMItemPedidoCompra B on A.ObjId_ItemPedidoCompra=B.Objid
		LEFT JOIN metricsweb.dbo.COMSolicitacaoItemPedidoCompra C on C.ObjId_ItemPedidoCompra = B.ObjId
		LEFT JOIN metricsweb.dbo.COMSolicitacaoCompra F on F.ObjId = C.ObjId_SolicitacaoCompra
		LEFT JOIN lito.dbo.prov p on convert(varchar,p.proveedor)  = convert(varchar,E.CliFor_Codigo)
		INNER JOIN Lito.dbo.CxP X  ON E.NaturezaOperacao COLLATE Modern_Spanish_CI_AS = X.Mov AND convert(varchar,E.usr_sequencial) = convert(varchar,X.MovID)
	WHERE    
		E.CFOP NOT IN ('1.104') and (E.Situacao = 1) 
		and  F.NumSolicitacao is not null
		and e.Sequencial = @EC

UNION
	
	SELECT  
		Tipo = 'OC', 
		Folio = B.NroPedido,
		Ejercicio=Year(E.DataEntrada),
		Periodo =   right(replicate('0', 2) + CAST(month(e.dataEntrada) as VARCHAR(2)), 2),
		numProveedor=E.CliFor_Codigo,
		nombreProveedor = E.CliFor_RazaoSocial, 
		Fecha=convert(date,E.DataEntrada),
		p.RFC,
		X.ID  as ID
	FROM metricsweb.dbo.ESTNotasFiscaisEntrada E
		LEFT JOIN metricsweb.dbo.ESTItemNotaEntrada D ON E.Objid = D.Objid_NotasFiscaisEntrada                     
		LEFT JOIN metricsweb.dbo.ESTItemNFlxItemPedCompra A on D.Objid=Objid_ItemNotaFiscal
		LEFT JOIN metricsweb.dbo.COMItemPedidoCompra B on A.ObjId_ItemPedidoCompra=B.Objid
		LEFT JOIN metricsweb.dbo.COMSolicitacaoItemPedidoCompra C on C.ObjId_ItemPedidoCompra = B.ObjId
		LEFT JOIN metricsweb.dbo.COMSolicitacaoCompra F on F.ObjId = C.ObjId_SolicitacaoCompra
		LEFT JOIN lito.dbo.prov p on convert(varchar,p.proveedor)  = convert(varchar,E.CliFor_Codigo)
		INNER JOIN Lito.dbo.CxP X  ON E.NaturezaOperacao COLLATE Modern_Spanish_CI_AS = X.Mov AND convert(varchar,E.usr_sequencial) = convert(varchar,X.MovID)
	WHERE    
		E.CFOP NOT IN ('1.104') and (E.Situacao = 1) and e.Sequencial = @EC;
   ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ordenCompra", ordenCompra);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Partida ruta = new Partida
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Tipo = reader["Tipo"].ToString(),
                                Ejercicio = Convert.ToInt32(reader["Ejercicio"]),
                                Folio = reader["Folio"].ToString(),
                                Periodo = reader["Periodo"].ToString(),
                                numProveedor = reader["numProveedor"].ToString(),
                                RFC = reader["RFC"].ToString()
                            };
                            listaPartidas.Add(ruta);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer órdenes: {ex.Message}");
        }
        return listaPartidas;
    }

  


    public void registrarArchivoAnexo(string rutaArchivo, int idOrden, string tipo)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //DOC
                connection.Open();
                string query = @"begin				
				declare @nombreArchivo varchar(250);
				declare @path varchar(250);
                declare @rama varchar(5);
				declare @id int;
				set @id=@id_;
                set @rama=@rama_;
				set @path=@path_
			        set @nombreArchivo=@nombreArchivo_                                                               
			        declare @orden int;				
		 	        select @orden= iif(max(Orden) is null,0,max(Orden))  from dbo.anexomov			
                                where rama=@rama and ID=@id                                                
                            
				insert into dbo.anexomov (Rama,Nombre,ID,Direccion,Icono,Tipo,Orden,Comentario,FechaEmision,TipoDocumento)
				            values(@rama,@nombreArchivo,@id,@path,66,'Archivo',@orden+1,'HOT FOLDER',GETDATE(),@tipo);
		     	end	";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_", idOrden);
                    command.Parameters.AddWithValue("@path_", rutaArchivo);
                    command.Parameters.AddWithValue("@rama_", "CXP");
                    command.Parameters.AddWithValue("@nombreArchivo_", Path.GetFileName(rutaArchivo));
                    command.Parameters.AddWithValue("@tipo", tipo);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al registrar archivo anexo: {ex.Message}");
        }
    }

}
