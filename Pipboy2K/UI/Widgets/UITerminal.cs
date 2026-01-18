using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Color = Raylib_cs.Color;
using Rectangle = Raylib_cs.Rectangle;

namespace Pipboy2K.UI.Widgets;

public struct GridV2
{
    public int X;
    public int Y;

    public GridV2(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static GridV2 Zero
    {
        get
        {
            return new GridV2(0, 0);
        }
    }
}

// Very WIP
// Seems to be a bug somewhere that makes this use alot of RAM. (Well its like 50mb, but seeing how everything else combined also uses about 50mb that makes it a little sus)
public class UITerminal : UIWidget
{
    // Terminal Settings
    const int TileSize = 16;       // Pixel size of one tile (Square for simplicity)
    const int GridWidth = 60;      // Number of tiles horizontally
    const int GridHeight = 40;     // Number of tiles vertically

    // Calculated Screen Size
    const int ScreenWidth = GridWidth * TileSize;
    const int ScreenHeight = GridHeight * TileSize;

    // The Data: Our 2D Grid
    private char[,] _consoleGrid = new char[GridWidth, GridHeight];

    public GridV2 CursorPos {get; private set;} = GridV2.Zero;

    // Font
    private Font _terminalFont = GS.RobotoRFont;

    public override void Paint()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            string _yText = string.Empty;

            for (int x = 0; x < GridWidth; x++)
            {
                char c = _consoleGrid[x, y];

                if (c != ' ')
                {
                    _yText.Append(c);
                }
                else
                {
                    _yText.Append(' ');
                }
            }

            Vector2 pos = new Vector2(0, y * TileSize);

            DrawTextEx(_terminalFont, _yText, pos, TileSize, 1f, Color.Green);
        }

        // for (int x = 0; x < GridWidth; x++)
        // {
        //     for (int y = 0; y < GridHeight; y++)
        //     {
        //         char tile = _consoleGrid[x, y];

        //         Vector2 pos = new Vector2(x * TileSize, y * TileSize);

        //         if (tile != ' ' && tile != 0)
        //         {
        //             //DrawTextEx(_terminalFont, tile.ToString(), pos, TileSize, 1f, Color.Green);
        //         }
        //     }
        // }
    }

    public void SetTile(int x, int y, char glyph)
    {
        if (x >= 0 && x < GridWidth && y >= 0 && y < GridHeight)
        {
            _consoleGrid[x, y] = glyph;
        }
    }

    public void WriteTextCoords(int x, int y, string text, Color fg, Color bg)
    {
        for (int i = 0; i < text.Length; i++)
        {
            SetTile(x + i, y, text[i]);
        }
    }

    public void Write(string text)
    {
        for (int c = 0; c < text.Length; c++)
        {
            if (CursorPos.X >= GridWidth)
                CursorPos = new GridV2(0, CursorPos.Y + 1);

            SetTile((int)CursorPos.X, (int)CursorPos.Y, text[c]);
            CursorPos = new GridV2(CursorPos.X + 1, CursorPos.Y);
        }
    }

    public void DrawBox(int x, int y, int w, int h, Color fg, Color bg)
    {
        for (int i = x; i < x + w; i++)
        {
            for (int j = y; j < y + h; j++)
            {
                // Simple borders
                char c = ' ';
                if (i == x || i == x + w - 1) c = '|';
                if (j == y || j == y + h - 1) c = '-';
                if ((i == x || i == x + w - 1) && (j == y || j == y + h - 1)) c = '+';

                SetTile(i, j, c);
            }
        }
    }

    public void ClearGrid()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                _consoleGrid[x, y] = ' ';
            }
        }
    }
}

public struct TerminalTile
{
    public char Glyph;
    public Color Foreground;
    public Color Background;


    public TerminalTile(char glyph, Color fg, Color bg)
    {
        Glyph = glyph;
        Foreground = fg;
        Background = bg;
    }
}