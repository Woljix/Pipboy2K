using Pipboy2K.UI.Widgets;

namespace Pipboy2K.UI.Layout;

//Testing something..
public class UICanvas : UIWidget
{
    UIText text;

    public UICanvas()
    {
        text = new UIText("Yoo");
    }

    public override void OnInvalidate()
    {

    }

    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        text.PaintFamily();
    }
}