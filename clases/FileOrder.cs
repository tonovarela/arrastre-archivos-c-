
namespace arrastre_archivos.clases;

public class FileOrder
{
    
    

    public bool Copy(string sourcePath, string destinationPath)
    {
        if (File.Exists(destinationPath))
        {
            Console.WriteLine("El archivo ya existe en el destino.");
            return false;
        }
        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
            Console.WriteLine("Directorio de destino creado.");
        }
        File.Copy(sourcePath, destinationPath);
        return true;    
    }

}
