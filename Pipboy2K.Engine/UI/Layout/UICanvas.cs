//using Pipboy2K.UI.Widgets;

namespace Pipboy2K.Engine.UI.Layout;

//Testing something..
public class UICanvas : UIWidget
{
    //UIText text;

    public UICanvas()
    {
       
    }

    public override void Initialize()
    {
        //text = new UIText() { Text = "YOOO" };
    }

    public override void OnInvalidate()
    {

    }

    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        //text.PaintFamily();
    }
}