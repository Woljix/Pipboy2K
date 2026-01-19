using System;
using Raylib_cs;
using Lua;

using Pipboy2K.UI;
using Pipboy2K.Modules;
using Pipboy2K.Engine.Core;
using System.Numerics;
using Pipboy2K.UI.Widgets;
using Pipboy2K.UI.Layout;

namespace Pipboy2K
{
    internal class Program
    {
        /*  TODO:
        Render loop should look something like this:

        if (resizeEvent)
        {
            // Handle resize?
            renderNeeded = true;
        }

        if (renderNeeded)
        {
            renderNeeded = false;
            Raylib.DrawWhatever(renderTexture);

        }

        Raylib.Draw(renderTexture);

        This will not work for everything, but some elements are very simple and only needs to be redrawn on resize or state change.
        */

        private static float TIME = 0.0f;

        [STAThread]
        public static void Main(string[] args)
        {
            Settings settings = Settings.Load();
            UIManager ui = new UIManager();

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

            #if DEBUG
                string _buildTag = "DEBUG";
            #else
                string _buildTag = "RELEASE";
            #endif

            Raylib.InitWindow(settings.WindowWidth, settings.WindowHeight, $"Pip-boy 2000 MK VI | In-dev ({_buildTag})");
            Raylib.SetTargetFPS(settings.TargetFPS);

            // TODO: Turn this into an actual event.
            void _onResize()
            {
                GS.ScreenBounds = new (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            // GC has to be initialized after Raylib.InitWindow, because it loads fonts that require a valid Raylib context.
            GS.Init(settings, ui);

            // Top Menu
            TabbedView tabbedView = ui.Instantiate<TabbedView>();

            tabbedView.Layout = LayoutStyleBuilder.Begin()
                .PositionAnchorPoint(
                    new (0.0f, 0.0f), new (0.0f, 0.0f))
                .Build();

            // tabbedView.Layout = new LayoutStyle()
            // {
            //     UsePositionType = LayoutStyle.PositionType.AnchorPoint,
            //     AnchorPoint = new (0.0f, 0.0f),
            //     PivotPoint = new (0.0f, 0.0f)
            // };
            tabbedView.transform.Position = new Vector2(300, 300);
            tabbedView.renderer.Prepare();
            //tabbedView.Position = new Vector2(0, 0);

            Status statusModule = ui.Instantiate<Status>();
            statusModule.Layout = new LayoutStyle()
            {
                UsePositionType = LayoutStyle.PositionType.AnchorPoint,
                AnchorPoint = new (0, 1f),
                PivotPoint = new (0, 1f)
            };
            statusModule.renderer.Prepare();

            //Console.WriteLine(GS.Bounds.ToString());
            //statusModule.Position = new Vector2(0, 0);
            //ui.AddWidget(statusModule);

            Texture2D boy = Raylib.LoadTexture("resources/sprites/vaultboy_thumbsup_anim.png");
            Raylib.GenTextureMipmaps(ref boy);

            UISprite vaultBoy = new UISprite(boy, 169, 240);
            vaultBoy.Layout = LayoutStyleBuilder.Begin()
                .PositionAnchorPoint(
                    new (0.5f, 0.5f),new (0.5f, 0.5f))
                .Build();
            vaultBoy.FrameStep = 0.12f;

            vaultBoy.renderer.Prepare();

            ui.AddWidget(vaultBoy);

            LuaState lua = LuaState.Create();

            //lua.Registry.

            UICanvas canvas = new UICanvas();
            canvas.transform.Position = new Vector2(100, 200);
            //canvas.UseAnchorPoint = true;
            //canvas.AnchorPoint = new Vector2(0.5f, 0.5f);
            //canvas.PivotPoint = new Vector2(0.5f, 0.5f);

            canvas.transform.AddChild(new UIText("Hello World!"));

            var _textElement = new UIText("Hiii");
            _textElement.transform.Position = new Vector2(-50, -50);
            canvas.transform.AddChild(_textElement);



            canvas.renderer.Prepare();

            ui.AddWidget(canvas);

            //UITerminal term = new UITerminal();
            //term.ClearGrid();
            //term.WriteText(0,0, "Hello World", Color.Black, Color.White);
            //term.Prepare();

            //ui.AddWidget(term);

            int boyIndex = 0;

            // CanvasSprite entity = new CanvasSprite(delegate(CanvasSprite ent)
            // {
            //     Vector2 _startPos = new Vector2(0, 0);

            //     ent.DrawLineEx(_startPos + new Vector2(0, 100), _startPos + new System.Numerics.Vector2(100, 0), 4, Color.Red);
            //     Raylib.DrawRectangleRec(ent.SpriteRect, Color.Gold);
            //     //Raylib.DrawText(ent.Rect.ToString(), 200, 200, 16, Color.Red);
            //     //Console.WriteLine(ent.Bounds.ToString());
            // });

             _onResize();

             ui.Invalidate();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsWindowResized())
                {
                    _onResize();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F3))
                    GS.DebugMode = !GS.DebugMode;

                if (Raylib.IsKeyPressed(KeyboardKey.F4))
                {
                    // Scuffed and dangerous, but fuck.. it works well for what it is ._.
                    void Dive(List<UIWidget> _widgets, int depth)
                    {
                        foreach (var widget in _widgets)
                        {
                            Console.WriteLine($"{new String('#', depth + 1)} Name: '{widget.GetType().ToString()}' Pos: '{widget.transform.Position}', GetSpritePos: {widget.renderer.GetSpritePosition} BB: '{widget.renderer.GetRenderTransform.BoundingBox}', RenderTransform: {widget.renderer.GetRenderTransform.ToString()}");
                            if (widget.transform.Children != null)
                            {
                                Dive(widget.transform.Children, depth + 1);
                            }
                        }
                    }

                    int depth = 0;

                    Console.WriteLine("##### DEBUG DUMP #####");

                    Dive(ui.Widgets, depth);

                    Console.WriteLine("##### END #####");
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F10))
                {
                    ui.Invalidate();
                }

                // if (Raylib.IsKeyPressed(KeyboardKey.Space))
                //     entity.SpritePosition += new Vector2(10, 0);

                if (Raylib.IsKeyPressed(KeyboardKey.D))
                {
                    tabbedView.MoveNextTab();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.A))
                {
                    tabbedView.MovePrevTab();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.W))
                {
                    tabbedView.MoveNextSubTab();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.S))
                {
                    tabbedView.MovePrevSubTab();
                }

                vaultBoy.Tick();

                // if (Raylib.IsKeyPressed(KeyboardKey.Up))
                // {
                //     boyIndex++;

                //     if (boyIndex > 7)
                //     {
                //         boyIndex = 0;
                //     }

                //     vaultBoy.SetFrame(boyIndex);
                // }

                // if (Raylib.IsKeyPressed(KeyboardKey.Down))
                // {
                //     boyIndex--;

                //     if (boyIndex < 0)
                //     {
                //         boyIndex = 7;
                //     }

                //     vaultBoy.SetFrame(boyIndex);
                // }

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    //term.WriteText(50, 20, "Hello", Color.DarkBrown, Color.Blue);
                    //term.Write("Hello World!");
                    //tabbedView.Position = Raylib.GetMousePosition();
                    //statusModule.Position = Raylib.GetMousePosition();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    //tabbedView.transform.Position += new Vector2(5, 5);
                    Console.WriteLine(tabbedView.Layout.UsePositionType.ToString() + ", " + tabbedView.Layout.AnchorPoint);
                }

                Raylib.BeginDrawing();

                // Draw Black background
                Raylib.DrawRectangleRec(new Rectangle(0,0, GS.ScreenBounds), Color.Black);

                ui.Tick();



                //Rectangle boyRect = new Rectangle(169 * boyIndex, 0, 169, 240);



                //Raylib.DrawTextureRec(boy, boyRect, new System.Numerics.Vector2(200, 200), Color.White);
                //Raylib.DrawTextureNPatch()

                Raylib.DrawText("Status: " + statusModule.transform.Position.ToString(), 200, 300, 18, Color.White);
                Raylib.DrawText("Tabbed:" + tabbedView.transform.Position.ToString(), 200, 350, 18, Color.White);
                Raylib.DrawText("Mouse: " + Raylib.GetMousePosition().ToString(), 200, 400, 18, Color.RayWhite);
                //Raylib.DrawText("Time: " + TIME.ToString(), 200, 450, 18, Color.RayWhite);
                //Raylib.DrawText("FPS: " + Raylib.GetFrameTime(), 200, 500, 18, Color.RayWhite);

                //TIME += Raylib.GetFrameTime();

                var _indexes = tabbedView.GetIndexes;

                Raylib.DrawText($"({_indexes.Item1}, {_indexes.Item2})", 200, 250, 18, Color.RayWhite);




                //entity.AttemptDraw();

                //Raylib.DrawRectangle(0, 0, 720, 250, Color.Black);


                //Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Red);

                //Raylib.DrawFPS(0, 0);

                // Draws scanlines to the screen with better gradient.
                for (int i = 0; i < Raylib.GetScreenHeight(); i += 3)
                {
                    Raylib.DrawLine(0, i, Raylib.GetScreenWidth(), i, new Color(0, 0, 0, 30));
                    Raylib.DrawLine(0, i - 2, Raylib.GetScreenWidth(), i, new Color(0, 0, 0, 25));
                }

                Raylib.EndDrawing();

            }



            //UIWidget widget = new UIWidget();
            // widget.Update();

            Raylib.CloseWindow();
        }
    }

    // Probably temporary, as i want to test something.
    // GS = GameState
    public static class GS
    {
        public enum HeaderStatus
        {
            Hidden,
            Visible,
        }

        public enum FooterStatus
        {
            Hidden,
            STATUS,
            ITEM,
            DATA
        }

        public static Settings Settings;
        public static UIManager UI;

        public static void Init(Settings settings, UIManager ui)
        {
            Settings = settings;
            UI = ui;

            ScreenBounds = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        }

        public static readonly Font RobotoBFont;
        public static readonly Font RobotoRFont;


        public static readonly Color COLORGREEN = new Color(0, 238, 0);

        public static bool DebugMode = false;

        private static Vector2 _screenBounds = Vector2.Zero;
        public static Vector2 ScreenBounds
        {
            get
            {
                if (_screenBounds == Vector2.Zero)
                {
                    _screenBounds = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
                }

                return _screenBounds;
            }
            set
            {
                _screenBounds = value;
            }
        }

        public static HeaderStatus headerStatus = HeaderStatus.Visible;
        public static FooterStatus footerStatus = FooterStatus.STATUS;

        static GS()
        {
            static Font _loadAndFilterFont(string path)
            {
                var _font = Raylib.LoadFontEx(path, 100, null, 0);

                Raylib.GenTextureMipmaps(ref _font.Texture);
                Raylib.SetTextureFilter(_font.Texture, TextureFilter.Bilinear);

                return _font;
            }
            //Bounds = new Rectangle();

            RobotoBFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Bold.ttf");
            RobotoRFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Regular.ttf");
        }
    }
}