using arrastre_archivos.DAO;
using arrastre_archivos.DTO;
using arrastre_archivos.entities;
using arrastre_archivos.exceptions;

namespace arrastre_archivos.clases;

public class EntradaCompraProcessor
{
    private readonly OrdenDAO _ordenDAO;    
    private readonly RfcValidator _rfcValidator;
    private readonly string _pathOrigenOC;
    private readonly string _pathOrigenSC;


    private readonly string _pathDestino;
    private readonly MetricsFileNamer _namer;
    public EntradaCompraProcessor(
        OrdenDAO ordenDAO,
      
        MetricsFileNamer namer,
        RfcValidator rfcValidator,
        string pathOrigenOC,
        string pathOrigenSC,
        string pathDestino
        )
    {
        _ordenDAO = ordenDAO;      
        _namer = namer;
        _rfcValidator = rfcValidator;
        _pathOrigenOC = pathOrigenOC;
        _pathOrigenSC = pathOrigenSC;
        _pathDestino = pathDestino;
    }

    // public List<ArchivoPorProcesar> Procesar(string entradaDeCompraPath)
    // {
    //     List<ArchivoPorProcesar> archivos = new List<ArchivoPorProcesar>();
    //     string entradaCompra = Path.GetFileNameWithoutExtension(entradaDeCompraPath.Trim());
    //     List<PartidaMetrics> partidas = _ordenDAO.obtenerInfo(entradaCompra);        
    //     if (partidas.Count == 0)
    //         throw new PartidasNotFoundException("No se encontraron partidas para el archivo de entrada.");

    //     PartidaMetrics cabecera = partidas[0];
    //     string ordenCompra = cabecera.OC;
    //     string rfcMetrics = cabecera.RFC;
    //     string rutaOrdenCompra = Path.Combine(_pathOrigenOC, ordenCompra + ".htm");
    //     if (!_rfcValidator.CoincideRfcProveedor(rutaOrdenCompra, rfcMetrics))        
    //         throw new RFCNotEqualsException("El RFC del archivo no coincide con el RFC de la base de datos.");        

    //     archivos.Add(new ArchivoPorProcesar
    //     {
    //         ID = cabecera.ID,
    //         EntradaCompra = entradaCompra,
    //         RutaArchivo = entradaDeCompraPath,
    //         TipoArchivo = TipoArchivo.EC,
    //         Destino = $"{_pathDestino}//{_namer.ConstruirNombreEC($"{Path.GetFileName(entradaDeCompraPath)}", cabecera)}"
    //     });

    //     archivos.Add(new ArchivoPorProcesar
    //     {
    //         ID = cabecera.ID,
    //         EntradaCompra = entradaCompra,
    //         RutaArchivo = rutaOrdenCompra,
    //         TipoArchivo = TipoArchivo.OC,
    //         Destino = $"{_pathDestino}//{_namer.ConstruirNombreOC($"{ordenCompra}.htm", cabecera)}"
    //     });

    //     foreach (var partida in partidas)
    //     {
    //         string rutaSolicitudCompra = Path.Combine(_pathOrigenSC, partida.SC + ".htm");            
    //             archivos.Add(new ArchivoPorProcesar
    //             {
    //                 ID = partida.ID,
    //                 RutaArchivo = rutaSolicitudCompra,
    //                 EntradaCompra = entradaCompra,
    //                 TipoArchivo = TipoArchivo.SC,
    //                 Destino = $"{_pathDestino}//{_namer.ConstruirNombreSC($"{partida.SC}.htm", partida)}"
    //             });            
    //     }
    //     return archivos;

    // }


    public List<ArchivoPorProcesar> Procesar(string entradaDeCompraPath)
    {
        List<ArchivoPorProcesar> archivos = new List<ArchivoPorProcesar>();
        string entradaCompra = Path.GetFileNameWithoutExtension(entradaDeCompraPath.Trim());
        List<Partida> partidas = _ordenDAO.obtenerPartidasRev(entradaCompra);        
        partidas.ForEach(partida =>
        {
            string identificador = partida.ID.ToString();
            string rutaOrdenCompra = Path.Combine(_pathOrigenOC, identificador + ".htm");
        
            switch (partida.Tipo)
            {
                case "OC":
                    archivos.Add(new ArchivoPorProcesar
                    {
                        ID = partida.ID,
                        EntradaCompra = entradaCompra,
                        RutaArchivo = Path.Combine(_pathOrigenOC, partida.Folio + ".htm"),
                        TipoArchivo = TipoArchivo.OC,
                        Destino = $"{_pathDestino}//{_namer.ConstruirNombreOC($"{partida.Folio}.htm",partida)}"
                    });
                    break;
                case "SC":
                    archivos.Add(new ArchivoPorProcesar
                    {
                        ID = partida.ID,
                        EntradaCompra = entradaCompra,
                        RutaArchivo = Path.Combine(_pathOrigenSC, partida.Folio + ".htm"),
                        TipoArchivo = TipoArchivo.SC,
                        Destino = $"{_pathDestino}//{_namer.ConstruirNombreSC($"{partida.Folio}.htm",partida)}"
                    });
                    break;
                    case "EC":
                    archivos.Add(new ArchivoPorProcesar
                    {
                        ID = partida.ID,
                        EntradaCompra = entradaCompra,
                        RutaArchivo = entradaDeCompraPath,
                        TipoArchivo = TipoArchivo.EC,
                        Destino = $"{_pathDestino}//{_namer.ConstruirNombreEC($"{Path.GetFileName(entradaDeCompraPath)}",partida)}"
                    });
                    break;
            }
            
        });
        
        
        return archivos;

    }
}
