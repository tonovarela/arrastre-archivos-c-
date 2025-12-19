using System;

namespace arrastre_archivos.entities;

#nullable enable

public class RutaMetrics
{


    public required string EC { get; set; }
    public required string SC { get; set; }

    public required string OC { get; set; }
    public required int Ejercicio { get; set; }
    public required string Periodo { get; set; }
    public required string numProveedor { get; set; }
    public required string nombreProveedor { get; set; }
    public required DateTime Fecha { get; set; }
    public required string RFC { get; set; }
    public required string ruta { get; set; }

  public override string ToString()
    {
        return $"EC: {EC}, SC: {SC}, OC: {OC}, Ejercicio: {Ejercicio}, Periodo: {Periodo}, numProveedor: {numProveedor}, RFC: {RFC}";
    }
    
    

}





