
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
    /// Genera un CSV con el detalle de archivos procesados en un achivo
    /// .
    /// </summary>
    public string Write(IEnumerable<ArchivoPorProcesar> archivosProcesados)
    {        
        if (archivosProcesados is null) throw new ArgumentNullException(nameof(archivosProcesados));        
        var fileName = $"reporte_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        var fullPath = Path.Combine(_outputDirectory, fileName);

        var sb = new StringBuilder();
        sb.AppendLine("EntradaCompra,TipoArchivo,RutaArchivo,Destino,ExisteRutaArchivo");

        foreach (var a in archivosProcesados.OrderBy(a=>a.EntradaCompra).ThenBy(a=>a.TipoArchivo))    
        {
            var existe = a.ExisteRutaArchivo();
            sb.AppendLine(string.Join(",",
                Csv(a.EntradaCompra ?? string.Empty),                
                Csv(a.TipoArchivo.ToString()),
                Csv(a.RutaArchivo ?? string.Empty),
                Csv(existe ?a.Destino ?? string.Empty: string.Empty),
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
