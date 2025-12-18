using arrastre_archivos.clases;
using arrastre_archivos.DAO;

class Program
{
    
    static void Main(string[] args)
    {

        OrdenDAO ordenDAO = new OrdenDAO();
        ordenDAO.leerOrdenes();        
    }

    static void ProcesarArchivo(string nombreArchivo, int ejercicio, int periodo, int proveedor)
    {
        string basePath = "\\\\192.168.2.218\\RunningVersion\\CS\\bin\\Scripts\\compra\\HTML";
        string destinoPath = "destino";
        string archivoPath = Path.Combine(basePath, nombreArchivo);                
        string pathComplementario = $"{destinoPath}\\{ejercicio}\\{periodo}\\{proveedor}\\";        
        FileOrder fileOrder = new FileOrder();
        bool existe = fileOrder.Exits(archivoPath);
        if (existe)
        {
            string destinoArchivoPath = Path.Combine(pathComplementario, "99985.htm");
            bool seCopio = fileOrder.Copy(archivoPath, destinoArchivoPath);
            if (seCopio)
                Console.WriteLine($"Archivo copiado a: {destinoArchivoPath}");
        }
    }
    


}
