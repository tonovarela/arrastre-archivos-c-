
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


    public void MoveFiles(List<string> pathFiles, string pathProcessed)
    {
        if (Directory.Exists(pathProcessed) == false)
        {
            Directory.CreateDirectory(pathProcessed);
        }
        foreach (var archivo in pathFiles)
        {
            string destino = Path.Combine(pathProcessed, Path.GetFileName(archivo));
            if (File.Exists(destino))
            {
                File.Delete(destino);
            }
            File.Move(archivo, destino);
        }
    }
    
    public void DeleteFiles(List<string> pathFiles)
    {
        foreach (var archivo in pathFiles)
        {
            if (File.Exists(archivo))
            {
                File.Delete(archivo);
            }
        }
    }


}
