using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace Pipboy2K;

public sealed class Settings
{
    // private static Settings? _singleton;

    // public static Settings Instance
    // {
    //     get
    //     {
    //         if (_singleton == null)
    //         {
    //             _singleton = new Settings();
    //         }

    //         return _singleton;
    //     }
    // }

    // Hardcoded in, because no fun allowed >:)
    public static readonly string SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");

    [JsonInclude]
    public int WindowWidth = 720;

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

    /// <summary>
    /// Loads settings from Settings.json.
    /// </summary>
    /// <returns></returns>
    public static Settings Load()
    {
        Settings? _settings;

        if (File.Exists(SettingsFilePath))
        {
            string json = File.ReadAllText(SettingsFilePath);
            _settings = JsonSerializer.Deserialize<Settings>(json, SettingsContext.Default.Settings);
            if (_settings != null)
            {
                Console.WriteLine($"Loaded Settings from {SettingsFilePath}");
            }
        }
        else
        {
            _settings = new Settings();
        }

        Save(_settings);

        return _settings;
    }

    // public static Settings LoadOrCreate()
    // {
    //     Settings _settings = Load();

    //     if (_settings == null)
    //         _settings = new Settings();

    //     // Save to update new files
    //     Save(_settings);

    //     return _settings;
    // }

    public static void Save(Settings settings)
    {
        using (FileStream fs = File.OpenWrite(SettingsFilePath))
        {
            JsonSerializer.Serialize<Settings>(fs, settings, SettingsContext.Default.Settings);
            Console.WriteLine($"Saved Settings to {SettingsFilePath}");
        }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Settings))]
internal partial class SettingsContext : JsonSerializerContext { }