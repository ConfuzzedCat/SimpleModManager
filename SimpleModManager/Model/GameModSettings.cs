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
    
    public sealed class ModFileSettingStructure
    {
        public ModFileSettingStructure(string modPath, string[] fileExtensions)
        {
            ModPath = modPath;
            FileExtensions = fileExtensions;
        }

        public string ModPath { get; private set; }
        public string[] FileExtensions { get; private set; }
    }
    
    
    
}