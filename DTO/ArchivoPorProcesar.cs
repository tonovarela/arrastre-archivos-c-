



namespace arrastre_archivos.DTO;

public class ArchivoPorProcesar
{


    public string RutaArchivo { get; set; }
    public required TipoArchivo TipoArchivo { get; set; } 
    public string Destino { get; set; }
    public int ID { get; set; }    
    public string  EntradaCompra { get; set; }
    

    public bool ExisteRutaArchivo()
    {
        return  RutaArchivo.Length > 0 && File.Exists(RutaArchivo);
    }


    public string toString()
    {
        return $"TipoArchivo: {TipoArchivo},Origen: {RutaArchivo.Replace("//","\\")} ";
    }

}
public enum TipoArchivo
{
    EC ,
    OC,
    SC
}


