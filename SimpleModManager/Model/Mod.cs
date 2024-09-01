using System.Text.Json;
using SimpleModManager.Model.VirtualFileSystem;

namespace SimpleModManager.Model;

public class Mod
{
    public Mod(int id, string gameId, string name, bool installed, string modPath, string version,
        Node modFiles)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        Installed = installed;
        ModPath = modPath;
        Version = version;
        ModFiles = modFiles;
    }

    public int Id { get; set; }
    public string GameId { get; set; }
    public string Name { get; set; }
    public bool Installed { get; set; }
    public string ModPath { get; set; }
    public string Version { get; set; }
    public Node ModFiles { get; set; }

    internal void Install(string currentGameGamePath)
    {
        foreach (var modFile in ModFiles.GetAllChildren())
        {
            if (modFile.IsFile())
            {
                var p = Path.Combine(currentGameGamePath, modFile.RelativePath);
                if (File.Exists(p))
                {
                    var overwrite = ModManager.ClientIo.ReadBool($"{modFile.Name} Already exists in game folder, want to overwrite it?", true);
                    if (overwrite)
                    {
                        var backupPath = ModFiles.AbsolutePath + "_backup" + modFile.RelativePath.Remove(modFile.RelativePath.LastIndexOf(Path.DirectorySeparatorChar))[1..];
                        Directory.CreateDirectory(backupPath);
                        File.Move(p, ModFiles.AbsolutePath + "_backup" + modFile.RelativePath[1..], false);
                    }
                    else
                    {
                        continue;
                    }
                }
                File.Copy(modFile.AbsolutePath, p, false);
            }

            if (modFile.IsDir())
            {
                var p = Path.Combine(currentGameGamePath, modFile.RelativePath);
                Directory.CreateDirectory(p);
            }
        }
    }

    internal void Uninstall(string currentGameGamePath)
    {
        foreach (var modFile in ModFiles.GetAllChildren())
        {
            if (modFile.IsFile())
            {
                var installedFilePath = Path.Combine(currentGameGamePath, modFile.RelativePath);
                var backupFilePath = ModFiles.AbsolutePath + "_backup" + modFile.RelativePath;
                if (File.Exists(backupFilePath))
                {
                    File.Move(backupFilePath, installedFilePath, true);
                }
            
                if (File.Exists(installedFilePath))
                {
                    File.Delete(installedFilePath);
                }
            }

            if (!modFile.IsDir())
            {
                continue;
            }
            var directoryPath = Path.Combine(currentGameGamePath, modFile.RelativePath);
            if (!Directory.Exists(directoryPath))
            {
                continue;
            }
            try
            {
                Directory.Delete(directoryPath, false);
            }
            catch (IOException)
            {
                // The directory is not empty, leaving it as is
            }
        }
    }

    //TODO: remember wtf i wanted to do with this....
    internal void WriteToDisk()
    {
        var file = Path.Combine(ModFiles.AbsolutePath, ".smm");
        var content = JsonSerializer.Serialize(this);
        File.WriteAllText(file, content);
    }
}