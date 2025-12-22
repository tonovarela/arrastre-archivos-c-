using arrastre_archivos.DAO;
using arrastre_archivos.DTO;
using arrastre_archivos.entities;

namespace arrastre_archivos.clases;

public class EntradaCompraProcessor
{
    private readonly OrdenDAO _ordenDAO;
    private readonly FileOrder _fileOrder;
    private readonly MetricsFileNamer _namer;
    private readonly RfcValidator _rfcValidator;

    private readonly string _pathOrigenOC;
    private readonly string _pathOrigenSC;

    public EntradaCompraProcessor(
        OrdenDAO ordenDAO,
        FileOrder fileOrder,
        MetricsFileNamer namer,
        RfcValidator rfcValidator,
        string pathOrigenOC,
        string pathOrigenSC)
    {
        _ordenDAO = ordenDAO;
        _fileOrder = fileOrder;
        _namer = namer;
        _rfcValidator = rfcValidator;
        _pathOrigenOC = pathOrigenOC;
        _pathOrigenSC = pathOrigenSC;
    }

    public List<ArchivoPorProcesar> Procesar(string entradaDeCompraPath)
    {
        List<ArchivoPorProcesar> archivos = new List<ArchivoPorProcesar>();
        
        string entradaCompra = Path.GetFileNameWithoutExtension(entradaDeCompraPath.Trim());
        List<PartidaMetrics> partidas = _ordenDAO.obtenerInfo(entradaCompra);
        if (partidas.Count == 0)
            return  archivos;

        PartidaMetrics cabecera = partidas[0];



            
        string ordenCompra = cabecera.OC;
        string rfcMetrics = cabecera.RFC;
        string rutaOrdenCompra = Path.Combine(_pathOrigenOC, ordenCompra + ".htm");

        
        
        try
        {
            if (!_rfcValidator.CoincideRfcProveedor(rutaOrdenCompra, rfcMetrics))
            {
                // TODO: Mandar correo
                Console.WriteLine("RFC no coincide, se omite el procesamiento.");                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);            
            
        }

        archivos.Add(new ArchivoPorProcesar
        {
            RutaArchivo = rutaOrdenCompra,
            TipoArchivo = TipoArchivo.OC,
            Destino = _namer.ConstruirNombreOC(ordenCompra, cabecera)
        });
        archivos.Add(new ArchivoPorProcesar
        {
            RutaArchivo = entradaDeCompraPath,
            TipoArchivo = TipoArchivo.EC,
            Destino = _namer.ConstruirNombreEC(entradaCompra, cabecera)
        });          

        foreach (var partida in partidas)
        {
            string rutaSolicitudCompra = Path.Combine(_pathOrigenSC, partida.SC + ".htm");
            if (_fileOrder.Exits(rutaSolicitudCompra))
            {
                archivos.Add(new ArchivoPorProcesar
                {
                    RutaArchivo = rutaSolicitudCompra,
                    TipoArchivo = TipoArchivo.SC,
                    Destino = _namer.ConstruirNombreSC(partida.SC, partida)
                });                                
            }
        }
        return archivos;
        
    }
}
