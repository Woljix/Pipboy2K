using System.Numerics;
using Pipboy2K.UI;
using Raylib_cs;
using static Pipboy2K.GC;

namespace Pipboy2K.Modules;

// Status Module - Bottom Bar
public class Status : UIWidget
{
    private int statusBarHeight = 45;
    private int statusBarPadding = 2;

    public Status()
    {
        Prepare();
    }

    public override void Update()
    {

    }

    public override void Paint()
    {
        //Raylib.DrawRectangle(0, (int)GC.Bounds.Height - statusBarHeight, (int)GC.Bounds.Width, statusBarHeight, Color.DarkGray);
        // This is rougly the ratio of the segments in the status bar on the "STATUS" screen.

        switch (GC.footerStatus)
        {
            case FooterStatus.STATUS:
                RenderStatusBar();
                break;
            case FooterStatus.ITEM:
                // RenderItemBar();
                break;
            case FooterStatus.DATA:
                // RenderDataBar();
                break;
            case FooterStatus.Hidden:
                // Do nothing.
                break;
        }
    }

    private void RenderStatusBar()
    {
        /*
        // THIS IS ALL MY CODE, it works but its messy, so i got Gemini to clean it up
        int firstSegmentWidth = (int)(GC.Bounds.Width * 0.25f);
        int secondSegmentWidth = (int)(GC.Bounds.Width * 0.50f);
        int thirdSegmentWidth = (int)(GC.Bounds.Width * 0.25f);

        #region First Segment
        Vector2 firstPos = new Vector2(Bounds.X, Bounds.Y + Bounds.Height - statusBarHeight);
        DrawRectangle((int)firstPos.X, (int)firstPos.Y, firstSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
        DrawTextEx(GC.RobotoBFont, "HP 97/100", firstPos + new Vector2(statusBarPadding, statusBarPadding), 36, 1.0f, Color.Green);
        #endregion

        DrawRectangle(firstSegmentWidth + statusBarPadding, (int)Bounds.Height - statusBarHeight, secondSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
        DrawRectangle(firstSegmentWidth + secondSegmentWidth + statusBarPadding * 2, (int)Bounds.Height - statusBarHeight, thirdSegmentWidth - statusBarPadding, statusBarHeight, Color.DarkGreen);
        */

        int yPos = 0;
        int currentX = 0;
        int pad = statusBarPadding;

        int width25 = (int)(ScreenBounds.X * 0.25f);
        int width50 = (int)(ScreenBounds.X * 0.50f);

        // --- Segment 1 (25%) ---
        DrawRectangle(currentX, yPos, width25 - pad, statusBarHeight, Color.DarkGreen);
        DrawTextEx(GC.RobotoBFont, "HP 97/100", new Vector2(currentX + pad, yPos + pad), 36, 1.0f, Color.Green);

        // Move X pointer to the next segment
        currentX += width25 + pad;

        // --- Segment 2 (50%) ---
        DrawRectangle(currentX, yPos, width50 - pad, statusBarHeight, Color.DarkGreen);

        // Move X pointer
        currentX += width50 + pad;

        // --- Segment 3 (25%) ---
        DrawRectangle(currentX, yPos, width25 - pad, statusBarHeight, Color.DarkGreen);
    }

}