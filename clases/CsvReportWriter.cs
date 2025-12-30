
using System.Globalization;
using System.Text;
using arrastre_archivos.DTO;


namespace arrastre_archivos.clases;

public class CsvReportWriter
{


     private readonly string _outputDirectory;

    public CsvReportWriter(string outputDirectory)
    {
        if (string.IsNullOrWhiteSpace(outputDirectory))
            throw new ArgumentException("El directorio de salida no puede estar vacío.", nameof(outputDirectory));

        _outputDirectory = outputDirectory;
        Directory.CreateDirectory(_outputDirectory);
    }

    /// <summary>
    /// Genera un CSV con el detalle de archivos procesados por cada entrada.
    /// </summary>
    public string Write(string entradaDeCompra, IEnumerable<ArchivoPorProcesar> archivosProcesados)
    {
        if (entradaDeCompra is null) throw new ArgumentNullException(nameof(entradaDeCompra));
        if (archivosProcesados is null) throw new ArgumentNullException(nameof(archivosProcesados));

        var safeEntrada = MakeSafeFileName(entradaDeCompra);
        var fileName = $"reporte_{safeEntrada}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        var fullPath = Path.Combine(_outputDirectory, fileName);

        var sb = new StringBuilder();
        sb.AppendLine("EntradaCompra,ID,TipoArchivo,RutaArchivo,Destino,ExisteRutaArchivo");

        foreach (var a in archivosProcesados)
        {
            var existe = a.ExisteRutaArchivo();
            sb.AppendLine(string.Join(",",
                Csv(entradaDeCompra),
                Csv(a.ID.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
                Csv(a.TipoArchivo.ToString()),
                Csv(a.RutaArchivo ?? string.Empty),
                Csv(a.Destino ?? string.Empty),
                Csv(existe ? "1" : "0")
            ));
        }

        File.WriteAllText(fullPath, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        return fullPath;
    }

    private static string Csv(string value)
    {
        value ??= string.Empty;
        var mustQuote = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
        if (value.Contains('"')) value = value.Replace("\"", "\"\"");
        return mustQuote ? $"\"{value}\"" : value;
    }

    private static string MakeSafeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
        return string.IsNullOrWhiteSpace(cleaned) ? "entrada" : cleaned;
    }

}
