using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
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
                form.BackgroundImage = Image.FromFile(background);   // Set the background image.
                form.BackgroundImageLayout = ImageLayout.Stretch;
            }
            MakeButton(10, 10, "button1", "Random Game");
            MakeButton(10, 150, "button2", "List Games");
            MakeButton(10, 300, "button3", "Exit");
            MakeButton(300, 10, "button4", "Options");
            form.Width = 640;
            form.Height = 480;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.ShowDialog();
        }

        public MainMenu(String name, Form iForm, GamepadManager iPadMan, FileManager iFileMan, Graphics iGraphics) : base(name, iForm, iPadMan, iFileMan, iGraphics)
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
                    OptionsMenu optionsMenu = new OptionsMenu("Open Source Party Options", form, padMan, fileMan, graphics);
                    Destroy();
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