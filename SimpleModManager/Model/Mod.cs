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
}