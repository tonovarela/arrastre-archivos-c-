
namespace arrastre_archivos.clases;

public class FileOrder
{
    
    

    public void Copy(string sourcePath, string destinationPath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);            
        }
        if (File.Exists(destinationPath))
        {
            File.Delete(destinationPath);            
        }
        
        File.Copy(sourcePath, destinationPath);
        
    }

}
