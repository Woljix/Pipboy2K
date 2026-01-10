using System;
using Raylib_cs;

using Pipboy2K.UI;
using Pipboy2K.Modules;
using Pipboy2K.Engine.Core;
using System.Numerics;

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

        [STAThread]
        public static void Main(string[] args)
        {
            Settings settings = Settings.Load();
            UIManager ui = new UIManager();

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

            Raylib.InitWindow(settings.WindowWidth, settings.WindowHeight, "Pip-boy 2000 MK VI | In-dev");
            Raylib.SetTargetFPS(settings.TargetFPS);

            // TODO: Turn this into an actual event.
            void _onResize()
            {
                GC.ScreenBounds = new (Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
                ui.HandleResize();
            }

            // GC has to be initialized after Raylib.InitWindow, because it loads fonts that require a valid Raylib context.
            GC.Init(settings, ui);

            // Top Menu
            TabbedView tabbedView = new TabbedView();
            tabbedView.AnchorPoint = new (0, 0);
            tabbedView.PivotPoint = new (0, 0);
            tabbedView.UseAnchorPoint = true;
            tabbedView.Prepare();
            //tabbedView.Position = new Vector2(0, 0);
            ui.AddWidget(tabbedView);

            Status statusModule = new Status();
            statusModule.UseAnchorPoint = true;
            statusModule.AnchorPoint = new (0, 1f);
            statusModule.PivotPoint = new (0, 1f);
            statusModule.Prepare();

            //Console.WriteLine(GC.Bounds.ToString());
            //statusModule.Position = new Vector2(0, 0);
            ui.AddWidget(statusModule);



            Texture2D boy = Raylib.LoadTexture("resources/sprites/vaultboy_thumbsup_anim.png");
            Raylib.GenTextureMipmaps(ref boy);

            UISprite vaultBoy = new UISprite(boy, 169, 240);
            vaultBoy.UseAnchorPoint = true;
            vaultBoy.AnchorPoint = new (0.5f, 0.5f);
            vaultBoy.PivotPoint = new (0.5f, 0.5f);
            vaultBoy.Prepare();


            ui.AddWidget(vaultBoy);

            int boyIndex = 0;

            CanvasSprite entity = new CanvasSprite(delegate(CanvasSprite ent)
            {
                Vector2 _startPos = new Vector2(0, 0);

                ent.DrawLineEx(_startPos + new Vector2(0, 100), _startPos + new System.Numerics.Vector2(100, 0), 4, Color.Red);
                Raylib.DrawRectangleRec(ent.SpriteRect, Color.Gold);
                //Raylib.DrawText(ent.Rect.ToString(), 200, 200, 16, Color.Red);
                //Console.WriteLine(ent.Bounds.ToString());
            });

             _onResize();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsWindowResized())
                {
                    _onResize();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.F3))
                    GC.DebugMode = !GC.DebugMode;

                if (Raylib.IsKeyPressed(KeyboardKey.F4))
                {
                    // Scuffed and dangerous, but fuck.. it works well for what it is ._.
                    void Dive(List<UIWidget> _widgets, int depth)
                    {
                        foreach (var widget in _widgets)
                        {
                            Console.WriteLine($"{new String('#', depth + 1)} Name: '{widget.GetType().ToString()}' Pos: '{widget.Position}', SpritePos: {widget.SpritePosition} Rect: '{widget.Rect}'");
                            if (widget.Children != null)
                            {
                                Dive(widget.Children, depth + 1);
                            }
                        }
                    }

                    int depth = 0;

                    Console.WriteLine("##### DEBUG DUMP #####");

                    Dive(ui.Widgets, depth);

                    Console.WriteLine("##### END #####");
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    entity.SpritePosition += new Vector2(10, 0);

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

                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    boyIndex++;

                    if (boyIndex > 7)
                    {
                        boyIndex = 0;
                    }

                    vaultBoy.SetFrame(boyIndex);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    boyIndex--;

                    if (boyIndex < 0)
                    {
                        boyIndex = 7;
                    }

                    vaultBoy.SetFrame(boyIndex);
                }

                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    //tabbedView.Position = Raylib.GetMousePosition();
                    //statusModule.Position = Raylib.GetMousePosition();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    tabbedView.Position += new Vector2(5, 5);
                }

                Raylib.BeginDrawing();

                // Draw Black background
                Raylib.DrawRectangleRec(new Rectangle(0,0, GC.ScreenBounds), Color.Black);

                ui.Render();



                //Rectangle boyRect = new Rectangle(169 * boyIndex, 0, 169, 240);



                //Raylib.DrawTextureRec(boy, boyRect, new System.Numerics.Vector2(200, 200), Color.White);
                //Raylib.DrawTextureNPatch()

                Raylib.DrawText("Status: " + statusModule.Position.ToString(), 200, 300, 18, Color.White);
                Raylib.DrawText("Tabbed:" + tabbedView.Position.ToString(), 200, 350, 18, Color.White);
                Raylib.DrawText("Mouse: " + Raylib.GetMousePosition().ToString(), 200, 400, 18, Color.RayWhite);




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
    // GC = GameContext
    public static class GC
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

        static GC()
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