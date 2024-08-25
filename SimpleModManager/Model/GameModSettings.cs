namespace SimpleModManager.Model;

public sealed class GameModSettings
{
    public GameModSettings(string id, string name, string steamId, ModFileSettingStructure[] modStructures)
    {
        Id = id;
        Name = name;
        ModStructures = modStructures;
        SteamId = steamId;
    }

    public string Id { get; private set; }
    public string Name { get; private set; }

    // steam://rungameid/steamid
    public string SteamId { get; private set; }
    public ModFileSettingStructure[] ModStructures { get; private set; }
}