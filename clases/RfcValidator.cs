namespace arrastre_archivos.clases;

public class RfcValidator
{
    public bool CoincideRfcProveedor(string rutaOrdenCompraHtml, string rfcEsperado)
    {
        HtmlReader htmlReader = new HtmlReader(rutaOrdenCompraHtml);
        string rfcEnHtml = htmlReader.obtenerRFC();

        return string.Equals(
            (rfcEnHtml ?? string.Empty).Trim(),
            (rfcEsperado ?? string.Empty).Trim(),
            StringComparison.OrdinalIgnoreCase);
    }
}
