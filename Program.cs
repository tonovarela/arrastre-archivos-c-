using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;
using arrastre_archivos.exceptions;


class Program
{

    static void Main(string[] args)
    {
        Conf conf = Conf.getInstance();

        OrdenDAO ordenDAO = new OrdenDAO();
        FileOrder fileOrder = new FileOrder();

        HotFolderScanner scanner = new HotFolderScanner(conf.HotFolderPath);
        MetricsFileNamer namer = new MetricsFileNamer();
        RfcValidator rfcValidator = new RfcValidator();

        EntradaCompraProcessor processor = new EntradaCompraProcessor(
            ordenDAO,
            fileOrder,
            namer,
            rfcValidator,
            $"{conf.SourcePath}//{conf.SourcePathOC}",
            $"{conf.SourcePath}//{conf.SourcePathSC}",
            $"{conf.DestinationPath}"
             );             
            var archivos = scanner.ObtenerArchivos();
            
        foreach (string entradaDeCompra in archivos)
        {
            
            try
            {                
                var archivosProcesados = processor.Procesar(entradaDeCompra);
                foreach (var archivo in archivosProcesados)
                {
                    
                    Console.WriteLine(archivo.toString());                    
                    fileOrder.Copy(archivo.RutaArchivo, archivo.Destino);
                    string destino =archivo.Destino.Replace("Volumes","192.168.2.217");                    
                    ordenDAO.registrarArchivoAnexo(destino, archivo.ID,archivo.TipoArchivo.ToString());
                }
            }
            catch (PartidasNotFoundException pnfe)
            {
                Console.WriteLine($"No se procesó el archivo {entradaDeCompra}: {pnfe.Message}");
            }
            catch (RFCNotEqualsException rfcEx)
            {
                Console.WriteLine($"Error de RFC al procesar el archivo {entradaDeCompra}: {rfcEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el archivo {entradaDeCompra}: {ex.Message}");
            }


        }
    }

}
