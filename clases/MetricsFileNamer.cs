using arrastre_archivos.entities;

namespace arrastre_archivos.clases;

public class MetricsFileNamer
{
    public string ConstruirNombreEC(string entradaCompra, Partida partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"EC-{entradaCompra}");
    }

    public string ConstruirNombreOC(string ordenCompra, Partida partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"OC-{ordenCompra}");
    }

    public string ConstruirNombreSC(string solicitudCompra, Partida partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"RQ-{solicitudCompra}");
    }
}
