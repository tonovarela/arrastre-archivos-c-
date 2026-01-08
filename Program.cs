
using System.Net;
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
        List<string> entradasMover = new List<string>();
        foreach (string entradaDeCompra in archivos)
        {
            var archivosProcesados = processor.Procesar(entradaDeCompra);
            var idArchivo = archivosProcesados[0].ID;
            //ordenDAO.EliminarAnexoMov(idArchivo.ToString());
            List<ArchivoPorProcesar> archivosPorRegistrar = archivosProcesados.Where(a => a.ExisteRutaArchivo()).ToList();
            List<ArchivoPorProcesar> archivosNoEncontrados = archivosProcesados.Where(a => !a.ExisteRutaArchivo()).ToList();            
            todos.AddRange(archivosPorRegistrar);
            todos.AddRange(archivosNoEncontrados);
            if (archivosNoEncontrados.Count >0)
            {
                entradasMover.Add(idArchivo.ToString());
            }

            foreach (ArchivoPorProcesar archivo in archivosPorRegistrar)
            {
                fileOrder.Copy(archivo.RutaArchivo, archivo.Destino);
                string destino = archivo.Destino.Replace("Volumes", "192.168.2.217");
                //ordenDAO.registrarArchivoAnexo(destino, archivo.ID, archivo.TipoArchivo.ToString());       
            }
                   
        }
        if (todos.Count >0){
        csvWriter.Write(todos);
        List<string> archivosMover = todos
                                             .Where(x => x.TipoArchivo == TipoArchivo.EC)
                                             .Where(x => entradasMover.Contains(x.ID.ToString()))
                                             .Select(x => x.RutaArchivo)
                                             .ToList(); 
        List<string> archivosEliminar = todos
                                         .Where(x => x.TipoArchivo == TipoArchivo.EC)
                                         .Where(x => !entradasMover.Contains(x.ID.ToString()))
                                         .Select(x => x.RutaArchivo)
                                         .ToList();                                                                                          
        fileOrder.MoveFiles(archivosMover,  Path.Combine(conf.HotFolderPath, "no_procesados"));
        fileOrder.DeleteFiles(archivosEliminar);
        }
        
    }

}
