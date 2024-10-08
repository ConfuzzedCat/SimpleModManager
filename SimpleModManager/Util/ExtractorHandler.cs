using System.Diagnostics;
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
            extractDir = '\'' + extractDir + '\'';
            filePath = '\'' + filePath + '\'';
            var bin = Path.Combine(Directory.GetCurrentDirectory() /*AppDomain.CurrentDomain.BaseDirectory*/, "7zipBin", "Linux", "7zzs");
            var psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{bin} x {filePath} -o{extractDir}\""
            };
            var proc = new Process() { StartInfo = psi };
            proc.Start();
            proc.WaitForExit();
            return;
        }

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
        var process = new Process();
        var startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "cmd.exe",
            Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg" // TODO: whoops :P
        };
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }

    private static void ExtractOther(string filePath, string extractDir)
    {
        if (!Directory.Exists(extractDir))
        {
            Directory.CreateDirectory(extractDir);
        }
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