using System.Reflection;
using SimpleModManager.Util;

namespace SimpleModManager.Model;

public class StagedMods
{
    public StagedMods()
    {
        
    }
    public StagedMods(string gameId, List<Mod> installedMods, List<Mod> stagedMods)
    {
        GameId = gameId;
        InstalledMods = installedMods;
        _StagedMods = stagedMods;
    }

    public string GameId { get; set; }
    public List<Mod> InstalledMods { get; set; }
    public List<Mod> _StagedMods { get; set; }

    public StagedMods(string gameId)
    {
        GameId = gameId;
        InstalledMods = new List<Mod>();
        _StagedMods = new List<Mod>();
    }
    public void AddStagedMod(Mod mod)
    {
        _StagedMods.Add(mod);
    }

    public void InstallMod(Mod mod)
    {
        mod.Installed = true;
        _StagedMods.Remove(mod);
        InstalledMods.Add(mod);
    }
    public void StageMod(Mod mod)
    {
        mod.Installed = false;
        InstalledMods.Remove(mod);
        _StagedMods.Add(mod);
    }

    public List<Mod> GetAllMods()
    {
        var temp = new List<Mod>(InstalledMods);
        temp.AddRange(_StagedMods);
        return SortAlphabetically(temp);
    }

    private List<Mod> SortAlphabetically(List<Mod> temp)
    {
        return temp.OrderBy(x => x.Id).ToList();
    }

    internal void FetchMods()
    {
        for (var index = 0; index < InstalledMods.Count; index++)
        {
            InstalledMods[index].ModFiles = VFSHandler.CreateFromPath(InstalledMods[index].ModFiles.AbsolutePath);
        }
        for (int index = 0; index < _StagedMods.Count; index++)
        {
            _StagedMods[index].ModFiles = VFSHandler.CreateFromPath(_StagedMods[index].ModFiles.AbsolutePath);
        }
    }
}