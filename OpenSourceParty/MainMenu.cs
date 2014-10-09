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
        // Fields
        private FileManager fileMan;

        // Constructors and Methods
        public MainMenu(String name) : base(name)
        {
            fileMan = new FileManager();   // Instantiate a new file manager.
            String background = fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension);
            if (background != null)
            {
                form.BackgroundImage = Image.FromFile(background);   // Set the background image.
                form.BackgroundImageLayout = ImageLayout.Stretch;
            }
            MakeButton(10, 10, "button1", "Random Game");
            MakeButton(10, 150, "button2", "List Games");
            MakeButton(10, 300, "button3", "Exit");
            form.Width = 640;
            form.Height = 480;
            form.ShowDialog();
        }

        /// <summary>
        /// Creates a button object and adds it to the list.
        /// </summary>
        /// <param name="x">The button's x coordinate.</param>
        /// <param name="y">The button's y coordinate.</param>
        /// <param name="file">The button's file name.</param>
        /// <param name="name">The button's name.</param>
        public void MakeButton(int x, int y, String file, String name)
        {
            List<Image> tempList = new List<Image>(3);
            tempList.Add(Image.FromFile(fileMan.NamedFile(file + "_neutral", fileMan.ImageDir + "\\Buttons", fileMan.ImageExtension)));
            tempList.Add(Image.FromFile(fileMan.NamedFile(file + "_hover", fileMan.ImageDir + "\\Buttons", fileMan.ImageExtension)));
            tempList.Add(Image.FromFile(fileMan.NamedFile(file + "_clicked", fileMan.ImageDir + "\\Buttons", fileMan.ImageExtension)));
            MenuButton button = new MenuButton(x, y, tempList, name, this);
            buttons.Add(button);
        }

        /// <summary>
        /// Runs when the mouse is clicked, checks to see if it was clicked on a button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void CheckClick(Object sender, EventArgs e)
        {
            if (!joystick)
            {
                foreach (MenuButton button in buttons)
                {
                    if (button.Intersects())
                    {
                        ButtonClicked(button);
                    }
                }
            }
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