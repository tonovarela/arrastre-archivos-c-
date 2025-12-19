using System;
using DotNetEnv;
namespace arrastre_archivos.conf;

public class Conf
{
    private static Conf? _instance;
    private static readonly object _lock = new();

    public string SourcePath { get; set; }
    public string DestinationPath { get; set; }
    public string ConnectionString { get; set; }
    public string HotFolderPath { get; set; }

    public string SourcePathSC { get; set; }
    public string SourcePathOC { get; set; }

    private Conf()
    {
        Env.Load(); // Carga las variables del archivo .env        
        SourcePathSC = Env.GetString("PATH_ORIGEN_SC") ?? throw new InvalidOperationException("La variable de entorno 'PATH_ORIGEN_SC' no está definida.");
        SourcePathOC = Env.GetString("PATH_ORIGEN_OC") ?? throw new InvalidOperationException("La variable de entorno 'PATH_ORIGEN_OC' no está definida.");
        DestinationPath = Env.GetString("PATH_DESTINO") ?? throw new InvalidOperationException("La variable de entorno 'PATH_DESTINO' no está definida.");
        ConnectionString = Env.GetString("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("La variable de entorno 'SQL_CONNECTION_STRING' no está definida.");
        HotFolderPath = Env.GetString("HOT_FOLDER_PATH") ?? throw new InvalidOperationException("La variable de entorno 'HOT_FOLDER_PATH' no está definida.");        
    }

    public static Conf getInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Conf();
                }
            }
        }
        return _instance;
    }
}
