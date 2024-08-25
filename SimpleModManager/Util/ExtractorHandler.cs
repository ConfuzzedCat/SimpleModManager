using System.Runtime.InteropServices;
using Serilog;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace SimpleModManager.Util;

public sealed class ExtractorHandler
{
    private static readonly ILogger Logger;

    static ExtractorHandler()
    {
        Logger = LoggerHandler.GetLogger<ExtractorHandler>();
    }

    public static void ExtractFromFile(string filePath, string extractDir)
    {
        var fi = new FileInfo(filePath);
        switch (fi.Extension.ToLower())
        {
            case ".7z":
                Extract7z(filePath, extractDir);
                return;
            default:
                ExtractOther(filePath, extractDir);
                break;
        }
    }

    // ReSharper disable once InconsistentNaming
    private static void Extract7z(string filePath, string extractDir)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            
        }
        
        throw new NotImplementedException();
    }

    private static void ExtractOther(string filePath, string extractDir)
    {
        using Stream stream = File.OpenRead(filePath);
        var reader = ReaderFactory.Open(stream);
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory)
            {
                continue;
            }
            Logger.Debug("key: {0}", reader.Entry.Key);
            reader.WriteEntryToDirectory(extractDir, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
        }
    }
}