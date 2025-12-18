using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;

class Program
{

    static void Main(string[] args)
    {            
        OrdenDAO ordenDAO = new OrdenDAO();
        ordenDAO.leerOrdenes();        
        ProcesarArchivo("99985.htm", 2024, 6, 12345);
    }

    static void ProcesarArchivo(string nombreArchivo, int ejercicio, int periodo, int proveedor)
    {
        Conf conf = Conf.getInstance();        
        string basePath = conf.SourcePath;
        string destinoPath = conf.DestinationPath;

        string archivoPath = Path.Combine(basePath, nombreArchivo);
        string pathComplementario = $"{destinoPath}\\{ejercicio}\\{periodo}\\{proveedor}\\";
        FileOrder fileOrder = new FileOrder();
        bool existe = fileOrder.Exits(archivoPath);
        Console.WriteLine($"El archivo {nombreArchivo} existe: {existe}");
        if (existe)
        {
            string destinoArchivoPath = Path.Combine(pathComplementario, "99985.htm");
            bool seCopio = fileOrder.Copy(archivoPath, destinoArchivoPath);
            if (seCopio)
                Console.WriteLine($"Archivo copiado a: {destinoArchivoPath}");
        }
    }



}
