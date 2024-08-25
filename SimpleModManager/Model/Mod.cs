using SimpleModManager.Model.VirtualFileSystem;

namespace SimpleModManager.Model;

public class Mod
{
    public Mod(int id, string gameId, string name, bool installed, string filePath, string version,
        DirectoryNode modFiles)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        Installed = installed;
        FilePath = filePath;
        Version = version;
        ModFiles = modFiles;
    }

    public required int Id { get; set; }
    public required string GameId { get; set; }
    public required string Name { get; set; }
    public bool Installed { get; set; }
    public string FilePath { get; set; }
    public string Version { get; set; }
    public DirectoryNode ModFiles { get; set; }
}