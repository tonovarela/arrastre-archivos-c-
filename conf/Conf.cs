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

    private Conf()
    {
        Env.Load(); // Carga las variables del archivo .env
        SourcePath = Env.GetString("PATH_ORIGEN") ?? throw new InvalidOperationException("La variable de entorno 'PATH_ORIGEN' no está definida.");
        DestinationPath = Env.GetString("PATH_DESTINO") ?? throw new InvalidOperationException("La variable de entorno 'PATH_DESTINO' no está definida.");
        ConnectionString = Env.GetString("SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("La variable de entorno 'SQL_CONNECTION_STRING' no está definida.");
        Console.WriteLine($"Inicializando Conf:");
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
