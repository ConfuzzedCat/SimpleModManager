namespace SimpleModManager.Model;

public sealed class ModFileSettingStructure
{
    public string FileExtension { get; private set; }
    public string ModPath { get; private set; }
    public ModFileSettingStructure(string fileExtension, string modPath)
    {
        FileExtension = fileExtension;
        ModPath = modPath;
    }
}