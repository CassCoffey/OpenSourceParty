/*  Open Source Party, a platform for making and playing party minigames with your friends
    Copyright (C) 2014  Sean Coffey

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
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
            InitButtons();
            Manager.Width = 640;
            Manager.Height = 480;
            Manager.FormBorderStyle = FormBorderStyle.FixedSingle;
            Manager.MaximizeBox = false;
            MessagePump.Run(Manager, Manager.UpdateMenu);
        }

        public MainMenu(String name, GameManager iManager, GamepadManager iPadMan, FileManager iFileMan) : base(name, iManager, iPadMan, iFileMan)
        {
            InitButtons();
            String background = fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension);
            if (background != null)
            {
                Manager.BackgroundImage = Image.FromFile(background);   // Set the background image.
            }
        }

        /// <summary>
        /// Creates all of the buttons for the menu.
        /// </summary>
        public override void InitButtons()
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
        public override void ButtonClicked(MenuObject button)
        {
            switch (button.Name)
            {
                case "Random Game":
                    fileMan.RandomGame(Manager, this, padMan);
                    break;
                case "List Games":
                    Console.WriteLine("Games:");
                    fileMan.printFileList(fileMan.MinigameDir, fileMan.GameExtension);
                    Console.WriteLine("Backgrounds:");
                    fileMan.printFileList(fileMan.BackgroundDir, fileMan.ImageExtension);
                    break;
                case "Options":
                    Destroy();
                    OptionsMenu optionsMenu = new OptionsMenu("Open Source Party Options", Manager, padMan, fileMan);
                    break;
                case "Exit":
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }
    }
}