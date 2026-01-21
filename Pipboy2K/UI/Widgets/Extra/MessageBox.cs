using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI.Widgets.Extra;

public class MessageBox : UIWidget
{
    public enum MsgboxState
    {
        FadeIn,
        Normal,
        FadeOut
    }

    private float _seconds = 2.0f;

    private float _frameCounter = 0.0f;

    private MsgboxState _state = MsgboxState.FadeIn;

    private readonly Color refColor = Color.Yellow;

    private Color color;
    private string text;

    private MessageBox(string text)
    {
        this.text = text;

        color = Raylib.Fade(refColor, 0.75f);
    }

    public override void Update()
    {
        _frameCounter += Raylib.GetFrameTime();

        if (_frameCounter >= _seconds)
        {
            GS.UI.RemoveWidget(this);
        }
    }

    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        //e.DrawTextEx();
        e.DrawTextEx(Raylib.GetFontDefault(), text, Vector2.Zero, 16f, 1f, color);

    }

    public static void Show(string text, LayoutStyle? layout = null)
    {
        var _msg = new MessageBox(text);

        if (layout.HasValue)
            _msg.Layout = layout.Value;
        else
             _msg.Layout = LayoutStyleBuilder.Begin()
                .PositionAnchorPoint(new (GS.GetRandom.Next(0, 1000) / 1000.0f, GS.GetRandom.Next(0, 1000) / 1000.0f), new (0.5f, 0.5f))
            .Build();

        GS.UI.AddWidget(_msg);
    }
}