namespace SimpleModManager.Model;

public sealed class GameModSettings
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    
    // steam://rungameid/steamid
    public string SteamId { get; private set; }
    public ModStructure[] ModStructures { get; private set; }

    public GameModSettings(string id, string name, string steamId, ModStructure[] modStructures)
    {
        Id = id;
        Name = name;
        ModStructures = modStructures;
        SteamId = steamId;
    }
}