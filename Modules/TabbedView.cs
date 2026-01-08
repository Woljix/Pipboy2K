using System.Numerics;
using Pipboy2K.UI;
using Raylib_cs;

namespace Pipboy2K.Modules;

public class TabbedView : UIWidget
{
    private List<Tab> tabs = new List<Tab>();
    // Sub-bound for tabs.
    private Rectangle viewBounds;

    #region GREEN TOP LINE RENDER
    private int _lineOffset = 45;
    private int _lineThickness = 4;
    #endregion

    private int selectedTabIndex = 1;

    public int TabSpacing = 40;

    private float _tabWidth = 0;

    public TabbedView(GameContainer gc) : base(gc)
    {
        tabs.Add(
            new TabBuilder("STATS", gc)
                .AddSubTabByName("STATUS")
                .AddSubTabByName("SPECIAL")
                .AddSubTabByName("PERKS")
                .Build());
        tabs.Add(
            new TabBuilder("ITEM", gc)
                .AddSubTabByName("WEAPONS")
                .AddSubTabByName("APPAREL")
                .AddSubTabByName("AID")
                .AddSubTabByName("MISC")
                .AddSubTabByName("MODS")
                .Build());
        tabs.Add(
            new TabBuilder("DATA", gc)
                .AddSubTabByName("QUICKLOAD")
                .AddSubTabByName("JOURNAL")
                .AddSubTabByName("MAP")
                .Build());

        tabs.Add(
            new TabBuilder("RADIO", gc)
                .AddSubTabByName("STATIONS")
                .Build());

        tabs.Add(
            new TabBuilder("CONFIG", gc)
                .AddSubTabByName("TEST1")
                .Build());

        _cacheTabNames();

        // Precalculate width of text, and use that to center tabs.
        _tabWidth = CalculateWidthOfTabs(_cachedTabNames, GC.RobotoBFont, 38);

        Console.WriteLine($"Tab Width: {_tabWidth}");
    }

    private float CalculateWidthOfTabs(List<string> tabNames, Font font, int fontSize)
    {
        float _width = 0;

        for (int i = 0; i < tabNames.Count; i++)
        {
            string tabName = tabNames[i];
            Vector2 textSize = Raylib.MeasureTextEx(font, tabName, fontSize, 1.0f);
            float xPos = textSize.X + TabSpacing;

            _width += xPos;
        }

        return _width;
    }

    private RenderTexture2D _renderTexture = Raylib.LoadRenderTexture(720, 720);

    private Dictionary<string, float> _subTabTextWidths = new Dictionary<string, float>();
    private List<string> _cachedTabNames = new List<string>();

    public override void Render()
    {
        if (GC.headerStatus == GameContainer.HeaderStatus.Hidden)
            return;

        float _thiccnessOffset = MathF.Round(_lineThickness / 2.0f);

        Vector2 centerOfBounds = new Vector2(Bounds.X + (Bounds.Width / 2), Bounds.Y + (Bounds.Height / 2));

        int _currentOffset = (int)(centerOfBounds.X - (_tabWidth / 2));

        for (int i = 0; i < _cachedTabNames.Count; i++)
        {
            string tabName = _cachedTabNames[i];
            Vector2 textSize = Raylib.MeasureTextEx(GC.RobotoBFont, tabName, 38, 1.0f);
            float xPos = Bounds.X + _currentOffset;

            //Raylib.DrawCircle((int)xPos, 0, 5, Color.Red);

            _currentOffset += (int)(textSize.X + TabSpacing);

            // If selected, draw the green line so that is snakes under tabs that are not selected, then around and over the selected the tab, then down again and continues under.
            if (i == selectedTabIndex)
            {
                // Left line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 3), Bounds.Y + _lineOffset + _thiccnessOffset),
                    new Vector2(xPos - (_lineThickness * 3), Bounds.Y + _lineOffset - (textSize.Y - _lineThickness * 2)),
                    _lineThickness,
                    Color.Green);

                // Right line of the tab
                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), Bounds.Y + _lineOffset + _thiccnessOffset),
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), Bounds.Y + _lineOffset - (textSize.Y - _lineThickness * 2)),
                    _lineThickness,
                    Color.Green);

                float _lineY = Bounds.Y + _lineOffset - (textSize.Y - _lineThickness * 2.5f);

                // Top-left line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 3) + _thiccnessOffset, _lineY),
                    new Vector2(xPos - _lineThickness, _lineY),
                    _lineThickness,
                    Color.Green);

                // Top-right line of the tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3) - _thiccnessOffset, _lineY),
                    new Vector2(xPos + textSize.X + _lineThickness, _lineY),
                    _lineThickness,
                    Color.Green);

                // First line segment to start of tab.
                Raylib.DrawLineEx(
                    new Vector2(Bounds.X - _lineThickness, Bounds.Y + _lineOffset),
                    new Vector2(xPos - (_lineThickness * 3) + _thiccnessOffset, Bounds.Y + _lineOffset),
                    _lineThickness,
                    Color.Green);

                // End line segment after tab.
                Raylib.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), Bounds.Y + _lineOffset),
                    new Vector2(Bounds.Width, Bounds.Y + _lineOffset),
                    _lineThickness,
                    Color.Green);

                // Draw sub tabs

                #region SubTab Rendering (Left Aligned) (Fixed to the left most tab)

                // float _subTabOffset = (int)(Bounds.X + 20);

                // for (int j = 0; j < tabs[i].cachedSubTabNames.Count; j++)
                // {
                //     string subTabName = tabs[i].cachedSubTabNames[j];
                //     Vector2 subTabTextSize = Raylib.MeasureTextEx(Settings.RobotoRFont, subTabName, 38, 1.0f);
                //     float subTabXPos = Bounds.X + _subTabOffset;

                //     _subTabOffset += (int)(subTabTextSize.X + TabSpacing);

                //     Color _color = (j == tabs[i].currentTabIndex) ? Color.Green : Color.DarkGreen;

                //     Raylib.DrawTextEx(Settings.RobotoRFont, subTabName, new Vector2(subTabXPos, Bounds.Y + _lineOffset + 10), 38, 1.0f, _color);
                // }
                #endregion

                #region SubTab Rendering (Center)

                float _subTabWidth = 0;

                if (_subTabTextWidths.ContainsKey(tabName))
                {
                    _subTabWidth = _subTabTextWidths[tabName];
                }
                else
                {
                    _subTabWidth = CalculateWidthOfTabs(tabs[i].cachedSubTabNames, GC.RobotoRFont, 38);
                    _subTabTextWidths[tabName] = _subTabWidth;

                    _subTabWidth = _subTabTextWidths[tabName];
                }

                float _subTabOffset = (int)(centerOfBounds.X - (_subTabWidth / 2));

                for (int j = 0; j < tabs[i].cachedSubTabNames.Count; j++)
                {
                    string subTabName = tabs[i].cachedSubTabNames[j];
                    Vector2 subTabTextSize = Raylib.MeasureTextEx(GC.RobotoRFont, subTabName, 38, 1.0f);
                    float subTabXPos = Bounds.X + _subTabOffset;

                    _subTabOffset += (int)(subTabTextSize.X + TabSpacing);

                    Color _color = (j == tabs[i].currentTabIndex) ? Color.Green : Color.DarkGreen;

                    Raylib.DrawTextEx(GC.RobotoRFont, subTabName, new Vector2(subTabXPos, Bounds.Y + _lineOffset + _thiccnessOffset), 38, 1.0f, _color);
                }


                #endregion
            }

            Raylib.DrawTextEx(GC.RobotoBFont, tabName, new Vector2(xPos, Bounds.Y + _lineOffset - textSize.Y - _thiccnessOffset), 38, 1.0f, Color.Green);
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

    public SubTab GetCurrentSubTab()
    {
        Tab currentTab = tabs[selectedTabIndex];

        SubTab currentSubTab = currentTab.subTabs[currentTab.currentTabIndex];

        return currentSubTab;
    }

    public void MoveNextTab()
    {
        if (tabs[selectedTabIndex] != null)
            tabs[selectedTabIndex].currentTabIndex = 0;

        selectedTabIndex++;
        if (selectedTabIndex >= tabs.Count)
            selectedTabIndex = 0;
    }

    public void MovePrevTab()
    {
        if (tabs[selectedTabIndex] != null)
            tabs[selectedTabIndex].currentTabIndex = 0;

        selectedTabIndex--;
        if (selectedTabIndex < 0)
            selectedTabIndex = tabs.Count - 1;
    }

    public void MoveNextSubTab()
    {
        Tab currentTab = tabs[selectedTabIndex];
        currentTab.currentTabIndex++;
        if (currentTab.currentTabIndex >= currentTab.subTabs.Count)
            currentTab.currentTabIndex = 0;
    }

    public void MovePrevSubTab()
    {
        Tab currentTab = tabs[selectedTabIndex];
        currentTab.currentTabIndex--;
        if (currentTab.currentTabIndex < 0)
            currentTab.currentTabIndex = currentTab.subTabs.Count - 1;
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

public class Tab : SubTab
{
    public List<SubTab> subTabs;

    public int currentTabIndex = 0;

    public List<string> cachedSubTabNames = new List<string>();
    public int textWidth = 0;

    public Tab(string name, GameContainer gc) : base(name, gc)
    {
        subTabs = new List<SubTab>();
    }

    public Tab(string name, List<SubTab> subTabs, GameContainer gc) : base(name, gc)
    {
        this.subTabs = subTabs;
    }

    public void CacheSubTabNames()
    {
        cachedSubTabNames.Clear();

        foreach (SubTab subTab in subTabs)
        {
            cachedSubTabNames.Add(subTab.Name.ToUpper());
        }
    }
}

public class TabBuilder
{
    private Tab _tab;

    public TabBuilder(string name, GameContainer gc)
    {
        _tab = new Tab(name, gc);
    }

    public TabBuilder AddSubTabByName(string name)
    {
        _tab.subTabs.Add(new SubTab(name, _tab.GC));
        return this;
    }

    public Tab Build()
    {
        _tab.CacheSubTabNames();
        return _tab;
    }
}

public class SubTab : UIWidget
{
    public string Name = "Empty";

    public SubTab(string name, GameContainer gc) : base(gc)
    {
        this.Name = name;
    }
}