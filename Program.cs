using arrastre_archivos.clases;
using arrastre_archivos.conf;
using arrastre_archivos.DAO;
using arrastre_archivos.entities;


class Program
{

    static void Main(string[] args)
    {
        OrdenDAO ordenDAO = new OrdenDAO();
        FileOrder fileOrder = new FileOrder();
        Conf conf = Conf.getInstance();
        string pathOrigenOC = conf.SourcePathOC;
        string pathOrigenSC = conf.SourcePathSC;
        
        
        List<string> entradasDeCompra = obtenerArchivosHotFolder();
        foreach (string archivo in entradasDeCompra)
        {
            Console.WriteLine($"Archivo encontrado en HotFolder: {archivo}");
            string entradaCompra = Path.GetFileNameWithoutExtension(archivo.Trim());
            Console.WriteLine($"Nombre del archivo sin extensión: {entradaCompra}");
            List<RutaMetrics> rutas = ordenDAO.obtenerRutas(entradaCompra);


            string ordenCompra = rutas.FirstOrDefault().OC;
            string rfcMetrics = rutas.FirstOrDefault().RFC;
            string rutaOrdenCompra = Path.Combine(pathOrigenOC, ordenCompra + ".htm");
            Console.WriteLine(rutaOrdenCompra);
            try
                {
                HtmlReader htmlReader = new HtmlReader(rutaOrdenCompra);
                string rfc = htmlReader.obtenerRFC();
                if (!rfc.Equals(rfcMetrics))
                {
                    //Todo: Mandar correo            
                    continue;
                }
                
                }                
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }

            foreach (var rutaMetrics in rutas)
            {
                string rutaSolicitudCompra = Path.Combine(pathOrigenSC, rutaMetrics.SC + ".htm");
                if (fileOrder.Exits(rutaSolicitudCompra))
                {
                  Console.WriteLine($"Procesando archivo HTML en ruta: {rutaSolicitudCompra}");                                                    
                }
                
                
            }
            
                
        }
        
        

    }

    static  List<string> obtenerArchivosHotFolder(){
        Conf conf = Conf.getInstance();
        List<string> listaNombresArchivos = new List<string>();
        string hotFolderPath = conf.HotFolderPath;
        string[] archivos = Directory.GetFiles(hotFolderPath, "*.txt");
        foreach (string archivo in archivos)
        {                    
            listaNombresArchivos.Add(archivo);            
        }        
        return listaNombresArchivos;
    }

    

    

    static void ProcesarArchivo(string nombreArchivo, int ejercicio, int periodo, int proveedor)
    {
        Conf conf = Conf.getInstance();        
        string basePath = conf.SourcePath;
        string destinoPath = conf.DestinationPath;
        string archivoPath = Path.Combine(basePath, nombreArchivo);        
        string carpetaDestino = Path.Combine(destinoPath, ejercicio.ToString(), periodo.ToString(), proveedor.ToString());
        FileOrder fileOrder = new FileOrder();
        bool existe = fileOrder.Exits(archivoPath);
        Console.WriteLine($"El archivo {nombreArchivo} existe: {existe}");
        if (existe)
        {
            string destinoArchivoPath = Path.Combine(carpetaDestino, nombreArchivo);
            bool seCopio = fileOrder.Copy(archivoPath, destinoArchivoPath);
            if (seCopio)
                Console.WriteLine($"Archivo copiado a: {destinoArchivoPath}");
        }
    }



}
