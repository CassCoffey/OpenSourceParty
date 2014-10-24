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
using System.Drawing;
using System.Windows.Forms;
using FileHandler;
using MinigameLibrary;

namespace OpenSourceParty
{
    class MainMenu : MenuAbstract
    {
        // Constructors and Methods
        public MainMenu(String name, GameWindow window) : base(name, window)
        {
            InitButtons();
            Window.Width = 640;
            Window.Height = 480;
            Window.FormBorderStyle = FormBorderStyle.FixedSingle;
            Window.MaximizeBox = false;
            Application.Idle += new EventHandler(Window.UpdateMenu);
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
            MakeButton(10, 10, "button1", "Random Game", new Action(() =>
            {
                GameLoadHelper.LoadRandomGame(Window);
            }));
            MakeButton(10, 150, "button2", "List Games", new Action(() =>
            {
                Console.WriteLine("Games:");
                FileManager.printFileList(FileManager.MinigameDir, FileManager.GameExtension);
                Console.WriteLine("Backgrounds:");
                FileManager.printFileList(FileManager.BackgroundDir, FileManager.ImageExtension);
            }));
            MakeButton(10, 300, "button3", "Exit", new Action(() =>
            {
                Application.Exit();
            }));
            MakeButton(300, 10, "button4", "Options", new Action(() =>
            {
                Window.AddState(new OptionsMenu("Open Source Party Options", Window));
            }));
        }
    }
}