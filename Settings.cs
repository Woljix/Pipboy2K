using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace Pipboy2K;

public sealed class Settings
{
    private static Settings? _singleton;

    public static Settings Instance
    {
        get
        {
            if (_singleton == null)
            {
                _singleton = new Settings();
            }

            return _singleton;
        }
    }

    public static readonly string SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");

    [JsonInclude]
    public int WindowWidth = 1200;

    [JsonInclude]
    public int WindowHeight = 720;

    [JsonInclude]
    public bool VSync = true;

    [JsonInclude]
    public ConfigFlags Flags = ConfigFlags.ResizableWindow;

    [JsonInclude]
    public int TargetFPS = 60;

    [JsonInclude]
    public bool Fullscreen = false;

    public void Load()
    {
        if (File.Exists(SettingsFilePath))
        {
            string json = File.ReadAllText(SettingsFilePath);
            Settings? loadedSettings = JsonSerializer.Deserialize<Settings>(json, SettingsContext.Default.Settings);
            if (loadedSettings != null)
            {
                _singleton = loadedSettings;
                Console.WriteLine($"Loaded Settings from {SettingsFilePath}");
            }
        }


        // Save to either initialize the file, or update it with new values if applicable.
        Save();
    }

    public void Save()
    {
        using (FileStream fs = File.OpenWrite(SettingsFilePath))
        {
            JsonSerializer.Serialize<Settings>(fs, Instance, SettingsContext.Default.Settings);
            Console.WriteLine($"Saved Settings to {SettingsFilePath}");
        }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Settings))]
internal partial class SettingsContext : JsonSerializerContext { }