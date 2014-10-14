using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using SlimDX;
using SlimDX.Windows;
using MenuHandler;
using GamepadHandler;
using FileHandler;

namespace OpenSourceParty
{
    class MainMenu : MenuAbstract
    {
        // Constructors and Methods
        public MainMenu(String name) : base(name)
        {
            String background = fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension);
            if (background != null)
            {
                Manager.BackgroundImage = Image.FromFile(background);   // Set the background image.
            }
            MakeButton(10, 10, "button1", "Random Game");
            MakeButton(10, 150, "button2", "List Games");
            MakeButton(10, 300, "button3", "Exit");
            MakeButton(300, 10, "button4", "Options");
            Manager.Width = 640;
            Manager.Height = 480;
            Manager.FormBorderStyle = FormBorderStyle.FixedSingle;
            Manager.MaximizeBox = false;
            MessagePump.Run(Manager, Manager.UpdateMenu);
        }

        public MainMenu(String name, GameManager iManager, GamepadManager iPadMan, FileManager iFileMan, Graphics iGraphics) : base(name, iManager, iPadMan, iFileMan, iGraphics)
        {
            MakeButton(10, 10, "button1", "Random Game");
            MakeButton(10, 150, "button2", "List Games");
            MakeButton(10, 300, "button3", "Exit");
            MakeButton(300, 10, "button4", "Options");
        }

        /// <summary>
        /// Code to run when buttons are clicked.
        /// </summary>
        /// <param name="button">The button that was clicked.</param>
        public override void ButtonClicked(MenuButton button)
        {
            switch (button.Name)
            {
                case "Random Game":
                    fileMan.RandomGame();
                    break;
                case "List Games":
                    Console.WriteLine("Games:");
                    fileMan.printFileList(fileMan.MinigameDir, fileMan.GameExtension);
                    Console.WriteLine("Backgrounds:");
                    fileMan.printFileList(fileMan.BackgroundDir, fileMan.ImageExtension);
                    break;
                case "Options":
                    Destroy();
                    OptionsMenu optionsMenu = new OptionsMenu("Open Source Party Options", Manager, padMan, fileMan, graphics);
                    break;
                case "Exit":
                    Application.Exit();
                    System.Environment.Exit(1);
                    break;
                default:
                    break;
            }
        }
    }
}