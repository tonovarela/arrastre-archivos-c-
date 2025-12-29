

namespace arrastre_archivos.entities;

public class Partida
{
  public  required int ID { get; set; }
  
  public required string Tipo { get; set; }
  public required int Ejercicio { get; set; }
  public required string Periodo { get; set; }
  public required string numProveedor { get; set; }
  public required string Folio { get; set; }
  public required string RFC { get; set; }
  

  public override string ToString()
  {
    return $"ID: {ID}, Tipo: {Tipo}, Ejercicio: {Ejercicio}, Periodo: {Periodo}, numProveedor: {numProveedor}, RFC: {RFC}";
  }

}
