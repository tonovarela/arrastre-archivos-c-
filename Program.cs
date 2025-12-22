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
        foreach (string entradaDeCompra in entradasDeCompra)
        {
            
            
          Console.WriteLine($"Entrada de compra: {entradaDeCompra}");
            string entradaCompraArchivoSinExtension = Path.GetFileNameWithoutExtension(entradaDeCompra.Trim());

            List<PartidaMetrics> partidas = ordenDAO.obtenerInfo(entradaCompraArchivoSinExtension);
            if (partidas.Count == 0)
                continue;

            string nuevoNombreArchivoEC = $"{partidas.FirstOrDefault().Ejercicio}/{partidas.FirstOrDefault().Periodo}/{partidas.FirstOrDefault().numProveedor}/EC-{entradaCompraArchivoSinExtension}.htm";    
            Console.WriteLine($"Nuevo nombre de archivo EC: {nuevoNombreArchivoEC}");

            string ordenCompra = partidas.FirstOrDefault().OC;
            string rfcMetrics = partidas.FirstOrDefault().RFC;

            string rutaOrdenCompra = Path.Combine(pathOrigenOC, ordenCompra + ".htm");

            string nuevoNombreArchivoOC = $"{partidas.FirstOrDefault().Ejercicio}/{partidas.FirstOrDefault().Periodo}/{partidas.FirstOrDefault().numProveedor}/OC-{ordenCompra}.htm";
            Console.WriteLine($"Orden de compra: {nuevoNombreArchivoOC}");
            
            
            try
            {
                HtmlReader htmlReader = new HtmlReader(rutaOrdenCompra);
                string rfc = htmlReader.obtenerRFC();
                if (!rfc.Equals(rfcMetrics))
                {
                    //TODO: Mandar correo            
                    continue;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (var partida in partidas)
            {
                string rutaSolicitudCompra = Path.Combine(pathOrigenSC, partida.SC + ".htm");
                
                if (fileOrder.Exits(rutaSolicitudCompra))
                {                    
                    string nuevoNombreArchivoSC = $"{partida.Ejercicio}/{partida.Periodo}/{partida.numProveedor}/RQ-{partida.SC}.htm";
                    
                    Console.WriteLine($"Procesando Requisicion de compra: {rutaSolicitudCompra}");
                    Console.WriteLine($"Nuevo nombre de archivo SC: {nuevoNombreArchivoSC}");

                }
            }
        }

    }

    static List<string> obtenerArchivosHotFolder()
    {
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
