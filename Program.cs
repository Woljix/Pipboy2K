using System;
using Raylib_cs;

using Pipboy2K.UI;
using Pipboy2K.Modules;

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
            Settings.Instance.Load();

            //ConfigFlags flags = ConfigFlags.ResizableWindow;

            //flags |= ConfigFlags.FullscreenMode;

            //flags = flags + ConfigFlags.FullscreenMode;


            Raylib.SetConfigFlags(Settings.Instance.Flags);

            Raylib.InitWindow(Settings.Instance.WindowWidth, Settings.Instance.WindowHeight, "Pip-boy 2000 MK VI | In-dev");
            Raylib.SetTargetFPS(Settings.Instance.TargetFPS);
            // GC has to be initialized after Raylib.InitWindow, because it loads fonts that require a valid Raylib context.
            GameContainer GC = new GameContainer();

            UIManager ui = new UIManager(GC);

            // Top Menu
            TabbedView tabbedView = new TabbedView(GC);
            ui.AddWidget(tabbedView);

            Status statusModule = new Status(GC);
            ui.AddWidget(statusModule);

            ui.Initialize();

            // TODO: Turn this into an actual event.
            void _onResize()
            {
                GC.Bounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            _onResize();

            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsWindowResized())
                {
                    _onResize();
                }

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

                Raylib.BeginDrawing();

                // Draw Black background
                Raylib.DrawRectangleRec(GC.Bounds, Color.Black);

                ui.Render();

                // Draws scanlines to the screen with better gradient.
                for (int i = 0; i < Raylib.GetScreenHeight(); i += 3)
                {
                    Raylib.DrawLine(0, i, Raylib.GetScreenWidth(), i, new Color(0, 0, 0, 30));
                    //Raylib.DrawLine(0, i - 2, Raylib.GetScreenWidth(), i, new Color(0, 0, 0, 25));
                }

                //Raylib.DrawRectangle(0, 0, 720, 250, Color.Black);


                //Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Red);

                //Raylib.DrawFPS(0, 0);

                Raylib.EndDrawing();
            }



            //UIWidget widget = new UIWidget();
            // widget.Update();

            Raylib.CloseWindow();
        }
    }

    // Probably temporary, as i want to test something.
    public class GameContainer
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

        public Font RobotoBFont;
        public Font RobotoRFont;

        public Rectangle Bounds;

        public HeaderStatus headerStatus = HeaderStatus.Visible;
        public FooterStatus footerStatus = FooterStatus.STATUS;

        public GameContainer()
        {
            static Font _loadAndFilterFont(string path)
            {
                var _font = Raylib.LoadFontEx(path, 100, null, 0);

                Raylib.GenTextureMipmaps(ref _font.Texture);
                Raylib.SetTextureFilter(_font.Texture, TextureFilter.Bilinear);

                return _font;
            }
            Bounds = new Rectangle();


            RobotoBFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Bold.ttf");
            RobotoRFont = _loadAndFilterFont("resources/fonts/RobotoCondensed-Regular.ttf");
        }
    }



    // public class TopMenu : UIWidget
    // {
    //     public TopMenu(ref Rectangle bounds) : base(bounds)
    //     {

    //     }

    //     public override void Update()
    //     {
    //         Raylib.DrawRectangle(0, 0, 720, 50, Color.DarkGray);
    //     }
    // }
}