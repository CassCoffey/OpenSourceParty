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
using GamepadHandler;
using FileHandler;
using MinigameLibrary;

namespace OpenSourceParty
{
    class OptionsMenu : MenuAbstract
    {
        // Constructors and Methods
        public OptionsMenu(String name, GameWindow window) : base(name, window)
        {
            InitButtons();
        }

        public override void InitBackground()
        {
            String background = FileManager.RandomFile(FileManager.BackgroundDir, FileManager.ImageExtension);
            if (background != null)
            {
                Window.BackgroundImage = Image.FromFile(background);   // Set the background image.
                Window.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        /// <summary>
        /// Creates all of the buttons for the menu.
        /// </summary>
        public override void InitButtons()
        {
            MakeButton(10, 10, "button6", "Cool Option", new Action(() =>
            {
                if (Window.Width == 640)
                {
                    Window.Width = 1024;
                    Window.Height = 768;
                }
                else if (Window.Width == 1024)
                {
                    Window.Width = 640;
                    Window.Height = 480;
                }
            }));
            MakeButton(10, 300, "button5", "Main Menu", new Action(() =>
            {
                Window.BackState();
            }));
            MakeSlider(10, 150, 170, "slider1", "Slider");
            MakeSlider(10, 225, 170, "slider1", "Slider");
        }
    }
}