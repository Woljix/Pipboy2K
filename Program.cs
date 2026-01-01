using System;
using Raylib_cs;

using Pipboy2K.UI;

namespace Pipboy2K
{
    internal class Program
    {
        static Rectangle bounds = new Rectangle(0, 0, 720, 720);

        [STAThread]
        public static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

            Raylib.InitWindow(720, 720, "Pip-boy 2000 | In-dev");
            Raylib.SetTargetFPS(60);

            // Must be run before drawing inorder for the fonts to be prepared.
            Settings.Initialize();

            UIManager ui = new UIManager(ref bounds);

            TabbedView tabbedView = new TabbedView(ref bounds);
            // tabbedView.AddTab(new Tab("Stats"));
            // tabbedView.AddTab(new Tab("Inventory"));
            // tabbedView.AddTab(new Tab("Data"));

            ui.AddWidget(tabbedView);

            ui.Initialize();

            while (!Raylib.WindowShouldClose())
            {
                bounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

                if (Raylib.IsKeyPressed(KeyboardKey.Tab))
                {
                    tabbedView.MoveNextTab();
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

    public class TopMenu : UIWidget
    {
        public TopMenu(ref Rectangle bounds) : base(ref bounds)
        {

        }

        public override void Update()
        {
            Raylib.DrawRectangle(0, 0, 720, 50, Color.DarkGray);
        }
    }
}