using Serilog;
using SimpleModManager.Util;

namespace SimpleModManager.Model.VirtualFileSystem;

public class VFSHandler
{
    private static readonly ILogger _logger;

    static VFSHandler()
    {
        _logger = LoggerHandler.GetLogger<VFSHandler>();
    }
    
    public static DirectoryNode CreateFromPath(string path)
    {
        _logger.Information("Making VFS of {0}", path);
        
        return _CreateFromPath(path);
    }
    private static DirectoryNode _CreateFromPath(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var root = new DirectoryNode(directoryInfo.Name);
        foreach (var file in directoryInfo.GetFiles()) root.AddChild(new FileNode(file.Name));
        foreach (var directory in directoryInfo.GetDirectories()) root.AddChild(_CreateFromPath(directory.FullName));
        return root;
    }
    
}