using arrastre_archivos.entities;

namespace arrastre_archivos.clases;

public class MetricsFileNamer
{
    public string ConstruirNombreEC(string entradaCompra, PartidaMetrics partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"EC-{entradaCompra}");
    }

    public string ConstruirNombreOC(string ordenCompra, PartidaMetrics partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"OC-{ordenCompra}");
    }

    public string ConstruirNombreSC(string solicitudCompra, PartidaMetrics partida)
    {
        return Path.Combine(
            partida.Ejercicio.ToString(),
            partida.Periodo,
            partida.numProveedor,
            $"RQ-{solicitudCompra}");
    }
}
