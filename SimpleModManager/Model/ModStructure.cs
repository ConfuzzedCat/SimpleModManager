namespace SimpleModManager.Model;

public class ModStructure
{
    public string FileExtension { get; private set; }
    public string ModPath { get; private set; }
    public GameModSettings Game { get; private set; }

    public ModStructure(string fileExtension, string modPath, GameModSettings game)
    {
        FileExtension = fileExtension;
        ModPath = modPath;
        Game = game;
    }
}