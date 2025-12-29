using System.Data.Common;
using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;
using arrastre_archivos.DTO;
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
        EntradaCompraProcessor processor = new EntradaCompraProcessor(ordenDAO,namer,rfcValidator,$"{conf.SourcePath}//{conf.SourcePathOC}",$"{conf.SourcePath}//{conf.SourcePathSC}",$"{conf.DestinationPath}");             

        var archivos = scanner.ObtenerArchivos();            
         foreach (string entradaDeCompra in archivos)
        {
            var archivosProcesados = processor.Procesar(entradaDeCompra);
            var idArchivo = archivosProcesados[0].ID;            
            //ordenDAO.EliminarAnexoMov(idArchivo.ToString());                            
            //List<ArchivoPorProcesar> ordenesPorRegistrar = archivosProcesados.Where(a=>a.ExisteRutaArchivo()).ToList();
            //List<ArchivoPorProcesar> ordenesNoEncontradas = archivosProcesados.Where(a=>!a.ExisteRutaArchivo()).ToList();
            Console.WriteLine("--------------------------------------");
            foreach (ArchivoPorProcesar archivo in archivosProcesados)
            {
                 //fileOrder.Copy(archivo.RutaArchivo, archivo.Destino);
                 Console.WriteLine(archivo.toString());
                 //string destino =archivo.Destino.Replace("Volumes","192.168.2.217");                    
                 //ordenDAO.registrarArchivoAnexo(destino, archivo.ID,archivo.TipoArchivo.ToString());
            }
            Console.WriteLine("--------------------------------------");
            
        }   
        
    }

}
