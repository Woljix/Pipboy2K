using System;
using Raylib_cs;

using Pipboy2K.UI;
using Pipboy2K.Modules;

namespace Pipboy2K
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameContainer GC = new GameContainer();

            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

            Raylib.InitWindow(720, 720, "Pip-boy 2000 | In-dev");
            Raylib.SetTargetFPS(60);

            // Must be run before drawing inorder for the fonts to be prepared.
            Settings.Initialize();

            UIManager ui = new UIManager(GC);

            // Top Menu
            TabbedView tabbedView = new TabbedView(GC);

            ui.AddWidget(tabbedView);

            ui.Initialize();

            while (!Raylib.WindowShouldClose())
            {
                GC.Bounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

                if (Raylib.IsKeyPressed(KeyboardKey.Tab))
                {
                    tabbedView.MoveNextTab();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.LeftControl))
                {
                    tabbedView.MoveNextSubTab();
                }

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                ui.Process();

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
        public Rectangle Bounds;

        public GameContainer()
        {
            Bounds = new Rectangle();
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