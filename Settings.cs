using Raylib_cs;

namespace Pipboy2K;

public class Settings
{
    private static Settings _singleton;

    public static Font RobotoBFont;
    public static Font RobotoRFont;

    public static void Initialize()
    {
        static Font _loadAndFilterFont(string path)
        {
            var _font = Raylib.LoadFontEx(path, 100, null, 0);

            Raylib.GenTextureMipmaps(ref _font.Texture);
            Raylib.SetTextureFilter(_font.Texture, TextureFilter.Bilinear);

            return _font;
        }

        RobotoBFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Bold.ttf");
        RobotoRFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Regular.ttf");
    }

    // private Settings()
    // {
    //     RobotoBFont = Raylib.LoadFontEx("resources/fonts/RobotoCondensed-Bold.ttf", 100, null, 0);
    //     Raylib.GenTextureMipmaps(ref RobotoBFont.Texture);
    //     Raylib.SetTextureFilter(RobotoBFont.Texture, TextureFilter.Bilinear);
    // }

    // public static Settings Active
    // {
    //     get
    //     {
    //         if (_singleton == null)
    //         {
    //             _singleton = new Settings();
    //         }

    //         return _singleton;
    //     }
    //     set
    //     {
    //         _singleton = value;
    //     }
    // }
}