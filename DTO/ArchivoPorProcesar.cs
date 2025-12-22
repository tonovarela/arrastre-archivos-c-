

namespace arrastre_archivos.DTO;

public class ArchivoPorProcesar
{


    public string RutaArchivo { get; set; }
    public TipoArchivo TipoArchivo { get; set; } 
    public string Destino { get; set; }




    public string toString()
    {
        return $"TipoArchivo: {TipoArchivo},Origen: {RutaArchivo}, Destino: {Destino}";
    }

}
public enum TipoArchivo
{
    EC,
    OC,
    SC
}


