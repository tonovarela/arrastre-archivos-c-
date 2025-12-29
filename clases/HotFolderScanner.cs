namespace arrastre_archivos.clases;

public class HotFolderScanner
{
    private readonly string _hotFolderPath;

    public HotFolderScanner(string hotFolderPath)
    {
        if (string.IsNullOrWhiteSpace(hotFolderPath))
            throw new ArgumentException("HotFolderPath no puede ser vacío.", nameof(hotFolderPath));

        if (!Directory.Exists(hotFolderPath))
            throw new DirectoryNotFoundException($"No existe el HotFolder: {hotFolderPath}");

        _hotFolderPath = hotFolderPath;
    }

    public List<string> ObtenerArchivos()
    {
        return Directory.GetFiles(_hotFolderPath, "*.pdf").ToList();
    }
}
