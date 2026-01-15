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

        return InternalCreateFromPath(path);
    }

    private static Node InternalCreateFromPath(string path)
    {
        var directoryInfo = new DirectoryInfo(path);
        var root = new Node(directoryInfo.Name, path, "");
        foreach (var file in directoryInfo.GetFiles())
        {
            if (file.Name == ".smm")
            {
                continue;
            }            
            root.AddChild(new Node(file.Name, file.FullName, root.RelativePath));
        }

        foreach (var directory in directoryInfo.GetDirectories())
        {
            root.AddChild(InternalCreateFromPathWithParent(directory.FullName, root, true));
        }
        return root;
    }

    private static Node InternalCreateFromPathWithParent(string path, Node parent, bool isRootDir = false)
    {
        var directoryInfo = new DirectoryInfo(path);
        var root = new Node(directoryInfo.Name, path, parent.RelativePath);
        foreach (var file in directoryInfo.GetFiles())
        {
            if (file.Name == ".smm")
            {
                continue;
            }
            root.AddChild(new Node(file.Name, file.FullName, root.RelativePath));
        }

        foreach (var directory in directoryInfo.GetDirectories())
        {
            if (isRootDir)
            {
                var relativePath = root.RelativePath.Substring(2);
                if (relativePath == root.Name)
                {
                    var rootFolders = ModManager.CurrentGame.ModSettings.ModStructures.Select(ms => ms.RootFolder);
                    if (rootFolders.Contains(root.Name) == false)
                    {
                        var keepFolder = ModManager.ClientIo
                            .ReadBool($"Mod has non-standard folder structure, do you want to preserve it? folder name:{root.Name}", false);
                        if (keepFolder == false)
                        {
                            return InternalCreateFromPathWithParent(directory.FullName, parent, isRootDir);
                        }
                    }
                }
            }
            root.AddChild(InternalCreateFromPathWithParent(directory.FullName, root));
        }
        return root;
    }
    
    public static string CalculateMd5Checksum(Node node)
    {
        if (!node.IsFile())
        {
            throw new IOException("Node isn't a file");
        }
        using var stream = new FileInfo(node.AbsolutePath).OpenRead();
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).ToLower();
    }
}