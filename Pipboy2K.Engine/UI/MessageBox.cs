using System.Numerics;
using Pipboy2K.Engine.Core;
using Raylib_cs;

namespace Pipboy2K.Engine.UI;

public class MessageBox : UIWidget
{
    public enum MsgboxState
    {
        FadeIn,
        Normal,
        FadeOut
    }

    public static Random RDM = new Random();

    private float _seconds = 2.0f;

    private float _frameCounter = 0.0f;

    private MsgboxState _state = MsgboxState.FadeIn;

    private readonly Color refColor = Color.Yellow;

    private Color color;
    private string text;
    private Font font;

    private MessageBox(string text, Font? font = null)
    {
        this.text = text;

        color = Raylib.Fade(refColor, 0.75f);

        this.font = font.HasValue ? font.Value : Raylib.GetFontDefault();
    }

    public override void Update()
    {
        _frameCounter += Raylib.GetFrameTime();

        if (_frameCounter >= _seconds)
        {
            UI.RemoveWidget(this);
        }
    }

    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        //e.DrawTextEx();
        e.DrawText(font, text, Vector2.Zero, 16f, 1f, color);
    }

    public static void Show(string text, UIManager ui, LayoutStyle? layout = null)
    {
        var _msg = new MessageBox(text);

        if (layout.HasValue)
            _msg.Layout = layout.Value;
        else
             _msg.Layout = LayoutStyleBuilder.Begin()
                .PositionAnchorPoint(new (RDM.Next(0, 1000) / 1000.0f, RDM.Next(0, 1000) / 1000.0f), new (0.5f, 0.5f))
            .Build();

        ui.AddWidget(_msg);

        //UI.AddWidget(_msg);
    }
}