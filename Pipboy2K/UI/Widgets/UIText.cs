using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI.Widgets;

public class UIText : UIWidget
{
    public string Text = string.Empty;
    public Font Font = GS.RobotoRFont;

    public float FontSize = 28;
    public float Spacing = 1.0f;

    public Color Color = Color.Green;

    public UIText(string Text, Font? Font = null, float FontSize = 28.0f, float Spacing = 1.0f, Color? Color = null)
    {
        this.Text = Text;
        if (Font != null)
            this.Font = Font.Value;

        this.FontSize = FontSize;
        this.Spacing = Spacing;

        if (Color.HasValue)
            this.Color = Color.Value;
    }

    public override void Paint() => DrawTextEx(Font, Text, Position, FontSize, Spacing, Color);
}