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

        foreach (string entradaDeCompra in scanner.ObtenerArchivosTxt())
        {
            try
            {

                var archivosProcesados = processor.Procesar(entradaDeCompra);
                foreach (var archivo in archivosProcesados)
                {
                    Console.WriteLine(archivo.toString());
                    fileOrder.Copy(archivo.RutaArchivo, archivo.Destino);
                    ordenDAO.registrarArchivoAnexo(archivo.Destino.Replace("Volumes","192.168.2.217"));
                }
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
