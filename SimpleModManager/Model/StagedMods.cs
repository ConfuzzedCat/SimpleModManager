namespace SimpleModManager.Model;

public class StagedMods
{
    public string GameId { get; }
    public List<Mod> InstalledMods { get; }
    public List<Mod> _StagedMods { get; }
}