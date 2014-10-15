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
    class OptionsMenu : MenuAbstract
    {
        // Constructors and Methods
        public OptionsMenu(String name, GameManager iManager, GamepadManager iPadMan, FileManager iFileMan, Graphics iGraphics) : base(name, iManager, iPadMan, iFileMan, iGraphics)
        {
            MakeButton(10, 10, "button6", "Cool Option");
            MakeButton(10, 300, "button5", "Main Menu");
        }

        /// <summary>
        /// Code to run when buttons are clicked.
        /// </summary>
        /// <param name="button">The button that was clicked.</param>
        public override void ButtonClicked(MenuButton button)
        {
            switch (button.Name)
            {
                case "Cool Option":
                    if (Manager.Width == 640)
                    {
                        Manager.Width = 1024;
                        Manager.Height = 768;
                    }
                    else if (Manager.Width == 1024)
                    {
                        Manager.Width = 640;
                        Manager.Height = 480;
                    }
                    break;
                case "Main Menu":
                    Destroy();
                    MainMenu mainMenu = new MainMenu("Open Source Party", Manager, padMan, fileMan, graphics);
                    break;
                default:
                    break;
            }
        }
    }
}