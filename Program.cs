class Program
{
    static void Main(string[] args)
    {

        string basePath = "/Volumes/RunningVersion/CS/bin/Scripts/compra/HTML";        
        var archivos = Directory.GetFiles(basePath, "*.htm", SearchOption.TopDirectoryOnly);
        Console.WriteLine($"Se encontraron {archivos.Length} archivos .htm");        
        // string filePath = "archivos/1.htm";
        // FileOrder fileOrder = new FileOrder();        
        // bool existe = fileOrder.Exits(filePath);
        // Console.WriteLine($"El archivo existe: {existe}");
        // fileOrder.Copy("archivos/1.htm", "destino/1.htm");        
    }
}
