
using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;
using arrastre_archivos.DTO;


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
        EntradaCompraProcessor processor = new EntradaCompraProcessor(ordenDAO, namer, rfcValidator, $"{conf.SourcePath}//{conf.SourcePathOC}", $"{conf.SourcePath}//{conf.SourcePathSC}", $"{conf.DestinationPath}");
        var csvWriter = new CsvReportWriter(Path.Combine(conf.HotFolderPath, "reportes"));

        var archivos = scanner.ObtenerArchivos();
        List<ArchivoPorProcesar> todos = new List<ArchivoPorProcesar>();
        foreach (string entradaDeCompra in archivos)
        {
            var archivosProcesados = processor.Procesar(entradaDeCompra);
            var idArchivo = archivosProcesados[0].ID;

            ordenDAO.EliminarAnexoMov(idArchivo.ToString());    

            List<ArchivoPorProcesar> archivosPorRegistrar = archivosProcesados.Where(a => a.ExisteRutaArchivo()).ToList();
            List<ArchivoPorProcesar> archivosNoEncontrados = archivosProcesados.Where(a => !a.ExisteRutaArchivo()).ToList();
            Console.WriteLine($"Archivos  para la entrada de compra {entradaDeCompra}:");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"Archivos encontrados: {archivosPorRegistrar.Count} de  {archivosProcesados.Count}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (ArchivoPorProcesar archivo in archivosPorRegistrar)
            {
                fileOrder.Copy(archivo.RutaArchivo, archivo.Destino);
                string destino = archivo.Destino.Replace("Volumes", "192.168.2.217");
                ordenDAO.registrarArchivoAnexo(destino, archivo.ID,archivo.TipoArchivo.ToString());            
                Console.WriteLine(archivo.toString());
            }

            if (archivosNoEncontrados.Count > 0)
            {
                Console.ResetColor();
                Console.WriteLine($"Archivos no ubicados: {archivosNoEncontrados.Count}");
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var archivo in archivosNoEncontrados)
                {
                    Console.WriteLine(archivo.toString());
                }
            }
            Console.ResetColor();
            Console.WriteLine("--------------------------------------");
            todos.AddRange(archivosProcesados);
            todos.AddRange(archivosNoEncontrados);


        }

        string path = csvWriter.Write(todos);
                
        Console.WriteLine($"Reporte generado en: {path}");

        string  pathProcessed = Path.Combine(conf.HotFolderPath, "procesados");
        
        List<string> archivosEliminar = todos
                                         .Where(x => x.TipoArchivo == TipoArchivo.EC)
                                         .Select(x => x.RutaArchivo)
                                         .ToList();
        fileOrder.MoveFiles(archivosEliminar, pathProcessed);        
        //fileOrder.EliminarArchivos(archivosEliminar);
    }

}
