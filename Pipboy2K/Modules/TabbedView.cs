using System.Numerics;
using Pipboy2K.UI;
using Raylib_cs;
using static Pipboy2K.GS;

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
    private int selectedSubTabIndex = 0;

    public (int, int) GetIndexes
    {
        get
        {
            return (selectedTabIndex, selectedSubTabIndex);
        }
    }

    public int TabSpacing = 40;

    private float _tabWidth = 0;

    public TabbedView()
    {
        tabs.Add(
            new TabBuilder("STATS")
                .AddSubTabByName("STATUS")
                .AddSubTabByName("SPECIAL")
                .AddSubTabByName("PERKS")
                .Build());
        tabs.Add(
            new TabBuilder("ITEM")
                .AddSubTabByName("WEAPONS")
                .AddSubTabByName("APPAREL")
                .AddSubTabByName("AID")
                .AddSubTabByName("MISC")
                .AddSubTabByName("MODS")
                .Build());
        tabs.Add(
            new TabBuilder("DATA")
                .AddSubTabByName("QUICKLOAD")
                .AddSubTabByName("JOURNAL")
                .AddSubTabByName("MAP")
                .Build());

        tabs.Add(
            new TabBuilder("RADIO")
                .AddSubTabByName("STATIONS")
                .Build());

        // tabs.Add(
        //     new TabBuilder("CONFIG", gc)
        //         .AddSubTabByName("TEST1")
        //         .Build());

        _cacheTabNames();

        // Precalculate width of text, and use that to center tabs.
        _tabWidth = CalculateWidthOfTabs(_cachedTabNames, GS.RobotoBFont, 38);

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

        // Remove the trailing TabSpacing.

        _width -= TabSpacing;

        return _width;
    }

    private RenderTexture2D _renderTexture = Raylib.LoadRenderTexture(720, 720);

    private Dictionary<string, float> _subTabTextWidths = new Dictionary<string, float>();
    private List<string> _cachedTabNames = new List<string>();

    protected override void Draw(WidgetRenderer e, RenderTransform transform)
    {
        if (GS.headerStatus == HeaderStatus.Hidden)
            return;

        float _thiccnessOffset = MathF.Round(_lineThickness / 2.0f);

        Vector2 centerOfBounds = new Vector2((transform.GetWindowSize.X / 2), (transform.GetWindowSize.Y / 2));

        int _currentOffset = (int)(centerOfBounds.X - (_tabWidth / 2));

        for (int i = 0; i < _cachedTabNames.Count; i++)
        {
            string tabName = _cachedTabNames[i];
            Vector2 textSize = Raylib.MeasureTextEx(GS.RobotoBFont, tabName, 38, 1.0f);
            float xPos = _currentOffset;

            //Raylib.DrawCircle((int)xPos, 0, 5, Color.Red);

            _currentOffset += (int)(textSize.X + TabSpacing);

            // If selected, draw the green line so that is snakes under tabs that are not selected, then around and over the selected the tab, then down again and continues under.
            if (i == selectedTabIndex)
            {
                // Left line of the tab.
                e.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 3), _lineOffset + _thiccnessOffset),
                    new Vector2(xPos - (_lineThickness * 3),  _lineOffset - (textSize.Y - _lineThickness * 2)),
                    _lineThickness,
                    Color.Green);

                // Right line of the tab
                e.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), _lineOffset + _thiccnessOffset),
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), _lineOffset - (textSize.Y - _lineThickness * 2)),
                    _lineThickness,
                    Color.Green);

                float _lineY = _lineOffset - (textSize.Y - _lineThickness * 2.5f);

                // Top-left line of the tab.
                e.DrawLineEx(
                    new Vector2(xPos - (_lineThickness * 3) + _thiccnessOffset, _lineY),
                    new Vector2(xPos - _lineThickness, _lineY),
                    _lineThickness,
                    Color.Green);

                // Top-right line of the tab.
                e.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3) - _thiccnessOffset, _lineY),
                    new Vector2(xPos + textSize.X + _lineThickness, _lineY),
                    _lineThickness,
                    Color.Green);

                // First line segment to start of tab.
                e.DrawLineEx(
                    new Vector2(0, _lineOffset),
                    new Vector2(xPos - (_lineThickness * 3) + _thiccnessOffset, _lineOffset),
                    _lineThickness,
                    Color.Green);

                // End line segment after tab.
                e.DrawLineEx(
                    new Vector2(xPos + textSize.X + (_lineThickness * 3), _lineOffset),
                    new Vector2(transform.GetWindowSize.X,  _lineOffset),
                    _lineThickness,
                    Color.Green);

                // Draw sub tabs

                #region SubTab Rendering (Center)

                float _subTabWidth = 0;

                if (_subTabTextWidths.ContainsKey(tabName))
                {
                    _subTabWidth = _subTabTextWidths[tabName];
                }
                else
                {
                    _subTabWidth = CalculateWidthOfTabs(tabs[i].cachedSubTabNames, GS.RobotoRFont, 38);
                    _subTabTextWidths[tabName] = _subTabWidth;

                    _subTabWidth = _subTabTextWidths[tabName];
                }

                float _subTabOffset = (int)(centerOfBounds.X - (_subTabWidth / 2));
               // float _subTabOffset = (ScreenBounds.X - _subTabWidth) / 2;

                for (int j = 0; j < tabs[i].cachedSubTabNames.Count; j++)
                {
                    string subTabName = tabs[i].cachedSubTabNames[j];
                    Vector2 subTabTextSize = Raylib.MeasureTextEx(GS.RobotoRFont, subTabName, 38, 1.0f);
                    float subTabXPos = _subTabOffset;

                    _subTabOffset += (int)(subTabTextSize.X + TabSpacing);

                    Color _color = (j == selectedSubTabIndex) ? Color.Green : Color.DarkGreen;

                    e.DrawTextEx(GS.RobotoRFont, subTabName, new Vector2(subTabXPos, _lineOffset + _thiccnessOffset), 38, 1.0f, _color);
                }

                #endregion
            }

            e.DrawTextEx(GS.RobotoBFont, tabName, new Vector2(xPos, _lineOffset - textSize.Y - _thiccnessOffset), 38, 1.0f, Color.Green);
        }
    }

    public SubTab GetCurrentSubTab()
    {
        Tab currentTab = tabs[selectedTabIndex];

        SubTab currentSubTab = currentTab.subTabs[selectedSubTabIndex];

        return currentSubTab;
    }

    public void MoveNextTab()
    {
        selectedTabIndex = handleTabMovement(selectedTabIndex, 1, tabs.Count - 1);

        selectedSubTabIndex = 0;
        //Invalidate();
    }

    public void MovePrevTab()
    {
        selectedTabIndex = handleTabMovement(selectedTabIndex, -1, tabs.Count - 1);

        selectedSubTabIndex = 0;

        //Invalidate();
    }

    public void MoveNextSubTab()
    {
        selectedSubTabIndex = handleTabMovement(selectedSubTabIndex, 1, tabs[selectedTabIndex].subTabs.Count - 1);

        //Tab currentTab = tabs[selectedTabIndex];

        //if (currentTab.subTabs[(selectedSubTabIndex + 1)] == null)
            //return;

        //Invalidate();
    }

    public void MovePrevSubTab()
    {
        selectedSubTabIndex = handleTabMovement(selectedSubTabIndex, -1, tabs[selectedTabIndex].subTabs.Count - 1);
    }

    private int handleTabMovement(int currentIndex, int dir, int max, int min = 0)
    {
        int _newIndex = currentIndex + dir;

        if (_newIndex > max)
            _newIndex = min;
        else if (_newIndex < min)
            _newIndex = max;

        return _newIndex;
    }

    private void _cacheTabNames()
    {
        _cachedTabNames.Clear();

        foreach (Tab tab in tabs)
        {
            _cachedTabNames.Add(tab.Name.ToUpper());
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public class TabBuilder
{
    private Tab _tab;

    public TabBuilder(string name)
    {
        _tab = new Tab(name);
    }

    public TabBuilder AddSubTabByName(string name)
    {
        _tab.subTabs.Add(new SubTab(name));
        return this;
    }

    public Tab Build()
    {
        _tab.CacheSubTabNames();
        return _tab;
    }
}

public class Tab
{
    public string Name = "DEF";

    public List<SubTab> subTabs;

    //public int currentTabIndex = 0;

    public List<string> cachedSubTabNames = new List<string>();
    public int textWidth = 0;

    public Tab(string name)
    {
        subTabs = new List<SubTab>();
        this.Name = name;
    }

    public Tab(string name, List<SubTab> subTabs)
    {
        this.subTabs = subTabs;
        this.Name = name;
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


public class SubTab
{
    public string Name = "Empty";

    public SubTab(string name)
    {
        this.Name = name;
    }
}