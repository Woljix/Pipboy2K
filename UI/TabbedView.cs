using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI;

public class TabbedView : UIWidget
{
    private List<Tab> tabs = new List<Tab>();
    private List<string> _cachedTabNames = new List<string>();

    // Sub-bound for tabs.
    private Rectangle viewBounds;

    #region GREEN TOP LINE RENDER
    private int _lineOffset = 90;
    private int _lineThickness = 4;
    #endregion

    private int selectedTabIndex = 1;

    public int TabSpacing = 40;

    // This is the
    private float _tabWidth = 0;

    public TabbedView(ref Rectangle bounds) : base(ref bounds)
    {
        // TEMP
        //Bounds = new Rectangle(0, 0, 720, 720);

        tabs.Add(new Tab("STAT", ref bounds));
        tabs.Add(new Tab("ITEM", ref bounds));
        tabs.Add(new Tab("DATA", ref bounds));
        tabs.Add(new Tab("RADIO", ref bounds));

        _cacheTabNames();

        // Precalculate width of text, and use that to center tabs.

        float _width = 0;

        for (int i = 0; i < _cachedTabNames.Count; i++)
        {
            string tabName = _cachedTabNames[i];
            Vector2 textSize = Raylib.MeasureTextEx(Settings.RobotoBFont, tabName, 38, 1.0f);
            float xPos = textSize.X + TabSpacing;

            _width += xPos;


           // Raylib.DrawTextEx(Settings.RobotoBFont, tabName, new Vector2(xPos, Bounds.Y + 30), 46, 1.0f, Color.Green);
        }

        _tabWidth = _width;

        Console.WriteLine($"Tab Width: {_tabWidth}");
    }

    // public void AddTab(Tab tab)
    // {
    //     tabs.Add(tab);
    //     _cacheTabNames();
    // }

    private RenderTexture2D _renderTexture = Raylib.LoadRenderTexture(720, 720);


    public override void Render()
    {
        // Beware of this!!!
        Bounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        float _thiccnessOffset = MathF.Round(_lineThickness / 2.0f);

        Vector2 centerOfBounds = new Vector2(Bounds.X + (Bounds.Width / 2), Bounds.Y + (Bounds.Height / 2));

        //Raylib.BeginTextureMode(_renderTexture);

        // Draw Black background and green line.
        Raylib.DrawRectangleRec(Bounds, Color.Black);

        // //Raylib.DrawLineEx(
        //     new Vector2(Bounds.X, Bounds.Y + _lineOffset),
        //     new Vector2(Bounds.Width, Bounds.Y + _lineOffset),
        //     3,
        //     Color.Green);

            //Raylib.DrawPolyLinesEx(Bounds.Center, 4, 10.0f, 0.0f, 2, Color.Green);

        // Precalculate width of text, and use that to center tabs.

        int _currentOffset = (int)(centerOfBounds.X - (_tabWidth / 2));

        for (int i = 0; i < _cachedTabNames.Count; i++)
        {
            string tabName = _cachedTabNames[i];
            Vector2 textSize = Raylib.MeasureTextEx(Settings.RobotoBFont, tabName, 38, 1.0f);
            float xPos = Bounds.X + _currentOffset;

            Raylib.DrawCircle((int)xPos, 0, 5, Color.Red);

            _currentOffset += (int)(textSize.X + TabSpacing);

            // If selected, draw the green line so that is snakes under tabs that are not selected, then around and over the selected the tab, then down again and continues under.
            if (i == selectedTabIndex)
            {
                // Left line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 2), Bounds.Y + _lineOffset),
                    new Vector2(xPos - (_lineThickness * 2), Bounds.Y + _lineOffset - textSize.Y - 18),
                    _lineThickness,
                    Color.Green);

                // Right line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 2), Bounds.Y + _lineOffset - 18 - textSize.Y),
                    new Vector2(xPos + textSize.X + (_lineThickness * 2), Bounds.Y + _lineOffset + _thiccnessOffset),
                    _lineThickness,
                    Color.Green);

                // Top line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 2) - _thiccnessOffset, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    new Vector2(xPos + textSize.X + (_lineThickness * 2) + _thiccnessOffset, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    _lineThickness,
                    Color.Green);

                // First line segment to start of tab.
                Raylib.DrawLineEx(
                    new Vector2(Bounds.X - _lineThickness, Bounds.Y + _lineOffset),
                    new Vector2(xPos - _lineThickness - _thiccnessOffset, Bounds.Y + _lineOffset),
                    _lineThickness,
                    Color.Green);

                // End line segment after tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 2), Bounds.Y + _lineOffset),
                    new Vector2(Bounds.Width, Bounds.Y + _lineOffset),
                    _lineThickness,
                    Color.Green);

                    /*
                    Vector2 endOfStartLine = new Vector2(xPos - 3, Bounds.Y + _lineOffset);

                Raylib.DrawLineEx(
                    new Vector2(Bounds.X, Bounds.Y + _lineOffset),
                    endOfStartLine,
                    3,
                    Color.Green);

                Raylib.DrawLineEx(
                    new Vector2(xPos - 5, Bounds.Y + _lineOffset + 2),
                    new Vector2(xPos - 5, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    3,
                    Color.Blue);

                Raylib.DrawLineEx(
                    new Vector2(xPos - 8, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    new Vector2(xPos + textSize.X + 8, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    3,
                    Color.Red);

                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + 6, Bounds.Y + _lineOffset - 18 - textSize.Y),
                    new Vector2(xPos + textSize.X + 6, Bounds.Y + _lineOffset + 2),
                    3,
                    Color.White);

                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + 6, Bounds.Y + _lineOffset),
                    new Vector2(Bounds.Width, Bounds.Y + _lineOffset),
                    3,
                    Color.Green);

                    */
            }

            Raylib.DrawTextEx(Settings.RobotoBFont, tabName, new Vector2(xPos, Bounds.Y + textSize.Y + (textSize.Y / 3)), 38, 1.0f, Color.Green);
        }

        //var lol = Raylib.MeasureTextEx(Settings.RobotoBFont, "STAT", 46, 1.0f) + Raylib.MeasureTextEx(Settings.RobotoBFont, "INVENTORY", 46, 1.0f);

        //Raylib.DrawTextEx(Settings.RobotoBFont, "STAT", new Vector2(5, 30), 46, 1.0f, Color.Green);
        //Raylib.DrawLine((int)Bounds.X, (int)Bounds.Y + _offset, (int)Bounds.Width, (int)Bounds.Y + _offset, Color.Green );
/*
        Raylib.DrawText("Tabbed View", (int)Bounds.X + 10, (int)Bounds.Y + 10, 20, Color.White);

        // Draw tab names as buttons.
        int tabButtonX = (int)Bounds.X + 10;
        int tabButtonY = (int)Bounds.Y + 40;


        for (int i = 0; i < _cachedTabNames.Count; i++)
        {
            string tabName = _cachedTabNames[i];
            int tabNameWidth = (int)Raylib.MeasureTextEx(Program.DisplayFont, tabName, 20, 1.0f).X + 20;


            Raylib_cs.Rectangle tabButtonRect = new Raylib_cs.Rectangle(tabButtonX, tabButtonY, tabNameWidth, 30);

            Color buttonColor = (i == selectedTabIndex) ? Color.LightGray : Color.Gray;


            Raylib.DrawRectangleRec(tabButtonRect, buttonColor);
            Raylib.DrawRectangleLinesEx(tabButtonRect, 2, Color.Black);
           // Raylib.DrawText(tabName, (int)tabButtonRect.X + 10, (int)tabButtonRect.Y + 5, 20, Color.Black);
            Raylib.DrawTextEx(Program.DisplayFont, tabName, new System.Numerics.Vector2(tabButtonRect.X + 10, tabButtonRect.Y + 5), 20, 1.0f, Color.Black);

            tabButtonX += tabNameWidth + 10;
        }

        */



        for (int i = 0; i < tabs.Count; i++)
        {

        }

        if (tabs.Count == 0)
            return;

        if (tabs[selectedTabIndex] == null)
            return;

        tabs[selectedTabIndex].Render();

        //Raylib.EndTextureMode();

        // Raylib.DrawTexturePro(_renderTexture.Texture,
        //     new Rectangle(0, 0, _renderTexture.Texture.Width, -_renderTexture.Texture.Height),
        //     new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height),
        //     new Vector2(0, 0),
        //     0.0f,
        //     Color.White);
    }

    public void MoveNextTab()
    {
        selectedTabIndex++;
        if (selectedTabIndex >= tabs.Count)
            selectedTabIndex = 0;
    }

    private void _cacheTabNames()
    {
        _cachedTabNames.Clear();

        foreach (Tab tab in tabs)
        {
            _cachedTabNames.Add(tab.Name.ToUpper());
        }
    }
}

public class Tab : UIWidget
{
    public string Name = "Tab";

    public Tab(string name, ref Rectangle bounds) : base(ref bounds)
    {
        Name = name;
    }
}