using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Serilog;
using SimpleModManager.Model;
using SimpleModManager.Util;

namespace SimpleModManager;

public sealed class SettingsManager
{
    private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smm_settings.json");
    private static readonly ILogger Logger;
    
    public static readonly SmmConfig Settings;
    

    static SettingsManager()
    {
        Logger = LoggerHandler.GetLogger<SettingsManager>();
        Settings = LoadSettings();
    }
    
    private static SmmConfig LoadSettings()
    {
        if (!File.Exists(SettingsPath))
        {
            return SaveSettings(CreateDefaultSettings());
        }
        var content = File.ReadAllText(SettingsPath);
        try
        {
            var smmConfig = JsonConvert.DeserializeObject<SmmConfig>(content);
            if (smmConfig is not null)
            {
                return smmConfig;
            }
            Logger.Warning("Deserializing settings file content, returned null. Default settings will be loaded and saved.");
            return SaveSettings(CreateDefaultSettings());
        }
        catch (Exception e)
        {
            Logger.Error(e,"There was an error loading the settings. Default settings will be loaded and saved.");
        }
        return SaveSettings(CreateDefaultSettings());
    }

    private static SmmConfig CreateDefaultSettings()
    {
        var stagingDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Staging", "{game}");
        var archiveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archive", "{game}");
        return new SmmConfig(stagingDir, archiveDir);
    }

    private static SmmConfig SaveSettings(SmmConfig settings)
    {
        try
        {
            var settingsContent = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsPath, settingsContent);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to save settings.");
        }
        Logger.Debug("Settings saved.");
        return settings;
    }

    public static void SaveSettings()
    {
        SaveSettings(Settings);
    }
}