
namespace arrastre_archivos.clases;

public class FileOrder
{
    
    public bool Exits(string path)
    {
        return File.Exists(path);
    }

    public void Copy(string sourcePath, string destinationPath)
    {
        if(File.Exists(destinationPath))
        {
            Console.WriteLine("El archivo ya existe en el destino.");
            return;
        }
        File.Copy(sourcePath, destinationPath);
    }

}
