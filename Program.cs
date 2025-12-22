using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;


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
            $"{conf.SourcePath}//{conf.SourcePathSC}");

        foreach (string entradaDeCompra in scanner.ObtenerArchivosTxt())
        {
            var archivosProcesados = processor.Procesar(entradaDeCompra);
            foreach (var archivo in archivosProcesados)
            {
                Console.WriteLine(archivo.toString());             
            }
        }
    }

}
