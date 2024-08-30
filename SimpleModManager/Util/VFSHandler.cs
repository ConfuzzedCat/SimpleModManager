using System.Security.Cryptography;
using Serilog;
using SimpleModManager.Model.VirtualFileSystem;

namespace SimpleModManager.Util;

public class VFSHandler
{
    private static readonly ILogger Logger;

    static VFSHandler()
    {
        Logger = LoggerHandler.GetLogger<VFSHandler>();
    }

    public static Node CreateFromPath(string path)
    {
        Logger.Information("Making VFS of {0}", path);

        return _CreateFromPath(path);
    }

    private static Node _CreateFromPath(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var root = new Node(directoryInfo.Name, path);
        foreach (var file in directoryInfo.GetFiles())
        {
            root.AddChild(new Node(file.Name, file.FullName));
        }

        foreach (var directory in directoryInfo.GetDirectories())
        {
            root.AddChild(_CreateFromPath(directory.FullName));
        }

        return root;
    }
    
    public static string CalculateMd5Checksum(Node node)
    {
        if (!node.IsFile())
        {
            throw new IOException("Node isn't a file");
        }
        using var stream = new FileInfo(node.RealPath).OpenRead();
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).ToLower();
    }
}