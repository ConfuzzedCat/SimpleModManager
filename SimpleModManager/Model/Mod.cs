using System.Reflection.Metadata.Ecma335;
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

    internal void Install(string currentGamePath)
    {
        var currentGameModSettings = ModManager.CurrentGame.ModSettings;
        var clientIo = ModManager.ClientIo;
        var overwriteAll = false;
        foreach (var modNode in ModFiles.GetAllChildren())
        {
            if (modNode.IsFile())
            {
                var p = Path.Combine(currentGamePath, modNode.RelativePath);
                if (File.Exists(p))
                {
                    if (SettingsManager.Settings.RememberOverwriteChoose && !overwriteAll)
                    {
                         overwriteAll = ModManager.ClientIo.ReadBool($"{Name}: Overwrite(s) found, do you want to overwrite all?", false);
                        
                    }
                    var overwrite = true;
                    if (!overwriteAll)
                    {
                        overwrite = ModManager.ClientIo.ReadBool($"{Name}: {modNode.Name} Already exists in game folder, want to overwrite it?", true);
                    }
                    if (overwrite)
                    {
                        var backupPath = ModFiles.AbsolutePath + "_backup" + modNode.RelativePath.Remove(modNode.RelativePath.LastIndexOf(Path.DirectorySeparatorChar))[1..];
                        Directory.CreateDirectory(backupPath);
                        File.Move(p, ModFiles.AbsolutePath + "_backup" + modNode.RelativePath[1..], false);
                    }
                    else
                    {
                        continue;
                    }
                }
                File.Copy(modNode.AbsolutePath, p, false);
            }

            if (modNode.IsDir())
            {
                var p = Path.Combine(currentGamePath, modNode.RelativePath);
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

        try
        {
            // TODO: Dumb way of doing this. Instead keep track of previous mods and find file conflicts that way and restore it to that. 
            Directory.Delete(ModFiles.AbsolutePath + "_backup", false);
        }
        catch (IOException)
        {
            // The directory is not empty
        }
    }

    //TODO: remember wtf i wanted to do with this....
    internal void WriteToDisk()
    {
        var file = Path.Combine(ModFiles.AbsolutePath, ".smm");
        var content = JsonSerializer.Serialize(this);
        File.WriteAllText(file, content);
    }

    public void Delete()
    {
        try
        {
            Directory.Delete(ModFiles.AbsolutePath, true);
            Directory.Delete(ModFiles.AbsolutePath + "_backup", true);
        }
        catch (DirectoryNotFoundException)
        {
            // duplicated mod already deleted all files or no backup folder found
        }
    }
}