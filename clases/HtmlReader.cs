using System;
using System.Text.RegularExpressions;


namespace arrastre_archivos.clases;

public class HtmlReader
{

    private string path { get; set; }

    public HtmlReader(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"No se encontró el archivo HTML: {path}");
        }
        this.path = path;
    }

    public string obtenerRFC()
    {

        string html = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(html))
        {
            throw new ArgumentException("El HTML está vacío.", nameof(html));
        }
        
        var matches = Regex.Matches(
            html,
            @"RFC\s*:?\s*(?:&nbsp;|\s)*([A-Z&Ñ]{3,4}\s*\d{6}\s*[A-Z0-9]{3})(?![A-Z0-9])",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (matches.Count >= 2)
        {
            string rfcConEspacios = matches[1].Groups[1].Value.ToUpperInvariant();
            return Regex.Replace(rfcConEspacios, @"\s+", "");
        }

        
        matches = Regex.Matches(
            html,
            @"\b([A-Z&Ñ]{3,4}\d{6}[A-Z0-9]{3})\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        if (matches.Count >= 2)
        {
            return matches[1].Groups[1].Value.ToUpperInvariant();
        }

        throw new InvalidOperationException("No se encontró un segundo RFC en el HTML.");


    }




}
