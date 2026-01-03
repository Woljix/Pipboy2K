using System.Numerics;
using Pipboy2K.UI;
using Raylib_cs;

namespace Pipboy2K.Modules;

// Status Module - Bottom Bar
public class Status : UIWidget
{
    private int statusBarHeight = 45;
    private int statusBarPadding = 2;

    public Status(GameContainer gameContainer) : base(gameContainer)
    {

    }

    public override void Update()
    {

    }

    public override void Render()
    {
        switch (GC.footerStatus)
        {
            case GameContainer.FooterStatus.STATUS:
                RenderStatusBar();
                break;
            case GameContainer.FooterStatus.ITEM:
                // RenderItemBar();
                break;
            case GameContainer.FooterStatus.DATA:
                // RenderDataBar();
                break;
            case GameContainer.FooterStatus.Hidden:
                // Do nothing.
                break;
        }

        //Raylib.DrawRectangle(0, (int)GC.Bounds.Height - statusBarHeight, (int)GC.Bounds.Width, statusBarHeight, Color.DarkGray);

        // This is rougly the ratio of the segments in the status bar on the "STATUS" screen.
    }

    private void RenderStatusBar()
    {
        int firstSegmentWidth = (int)(GC.Bounds.Width * 0.25f);
        int secondSegmentWidth = (int)(GC.Bounds.Width * 0.50f);
        int thirdSegmentWidth = (int)(GC.Bounds.Width * 0.25f);

        #region First Segment
        Vector2 firstPos = new Vector2(Bounds.X, Bounds.Y + Bounds.Height - statusBarHeight);
        Raylib.DrawRectangle((int)firstPos.X, (int)firstPos.Y, firstSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
        Raylib.DrawTextEx(GC.RobotoBFont, "HP 97/100", firstPos + new Vector2(statusBarPadding, statusBarPadding), 36, 1.0f, Color.Green);
        #endregion

        Raylib.DrawRectangle(firstSegmentWidth + statusBarPadding, (int)Bounds.Height - statusBarHeight, secondSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
        Raylib.DrawRectangle(firstSegmentWidth + secondSegmentWidth + statusBarPadding * 2, (int)Bounds.Height - statusBarHeight, thirdSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
    }
}