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
        public static ModFileSettingStructure Default()
        {
            return new ModFileSettingStructure(
                "",
                [],
                "Let the the mod manager guess.",
                ""
                );
        }
        
        
        public ModFileSettingStructure(string modPath, string[] fileExtensions, string modType, string rootFolder)
        {
            ModPath = modPath;
            FileExtensions = fileExtensions;
            ModType = modType;
            RootFolder = rootFolder;
        }

        public string ModPath { get; set; }
        public string[] FileExtensions { get; set; }
        public string ModType  { get; set; }
        public string RootFolder { get; set; }
    }
}