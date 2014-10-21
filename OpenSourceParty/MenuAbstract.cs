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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using SpriteHandler;
using GamepadHandler;
using FileHandler;
using System.Diagnostics;
using System.IO;

namespace MenuHandler
{
    /// <summary>
    /// A class that contains all of the essential methods for a menu class to be built off of.
    /// </summary>
    public abstract class MenuAbstract : GameState
    {
        // Fields
        protected List<MenuObject> menuObjects;
        protected FileManager fileMan;
        public GameManager Manager { get; set; }
        public TimeSpan Elapsed { get; private set; }

        // Joystick related fields
        protected bool joystick;
        public GamepadManager padMan;
        private int joystickIndex = 0;
        public bool JoystickMoved { get; set; }


        // Properties
        public int JoystickIndex   // Keeps track of which button the joystick has selected.
        {
            get
            {
                return joystickIndex;
            }
            set
            {
                if (value >= 0 && value < menuObjects.Count)
                {
                    joystickIndex = value;
                }
                else if (value < 0)
                {
                    joystickIndex = menuObjects.Count - 1;
                }
                else if (value >= menuObjects.Count)
                {
                    joystickIndex = 0;
                }
            }
        }
        public List<MenuObject> MenuObjects
        {
            get
            {
                return menuObjects;
            }
        }
        public bool Joystick
        {
            get
            {
                return joystick;
            }
        }
        public FileManager FileMan
        {
            get
            {
                return fileMan;
            }
            protected set
            {
                fileMan = value;
            }
        }
        

        // Constructors and Methods
        public MenuAbstract(String name)
        {
            Manager = new GameManager(this);
            Manager.Text = name;
            menuObjects = new List<MenuObject>();
            padMan = new GamepadManager();
            padMan.Init();
            fileMan = new FileManager();   // Instantiate a new file manager.
            Init();
        }

        /// <summary>
        /// Used when switching to this menu from another pre-existing menu.
        /// </summary>
        /// <param name="name">The name of this menu.</param>
        /// <param name="iManager">The Game Manager to use.</param>
        /// <param name="iPadMan">The gamepad manager to use.</param>
        /// <param name="iFileMan">The file manager to use.</param>
        /// <param name="iGraphics">The Graphics to use.</param>
        public MenuAbstract(String name, GameManager iManager, GamepadManager iPadMan, FileManager iFileMan)
        {
            Manager = iManager;
            Manager.CurState = this;
            Manager.Text = name;
            menuObjects = new List<MenuObject>();
            padMan = iPadMan;
            fileMan = iFileMan;
            Init();
        }

        /// <summary>
        /// Initialization method so that common code between the constructors is in one place, and thread-safe.
        /// </summary>
        public void Init()
        {
            JoystickMoved = false;
            joystickIndex = 0;
            InitButtons();
            menuObjects = new List<MenuObject>();
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate += new GamepadStateHandler.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate += new GamepadStateHandler.GamepadDelegate(GamepadClick);
            }
            Manager.MouseMove += new MouseEventHandler(JoystickModeOff);
            Manager.MouseUp += new MouseEventHandler(CheckClick);
            Manager.Invalidate();
        }

        /// <summary>
        /// Called when restarting the menu. Usually after a game finishes.
        /// Should be very similar to init.
        /// </summary>
        public override void Restart()
        {
            InitButtons();
            JoystickMoved = false;
            joystick = false;
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate += new GamepadStateHandler.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate += new GamepadStateHandler.GamepadDelegate(GamepadClick);
            }
            Manager.MouseMove += new MouseEventHandler(JoystickModeOff);
            Manager.MouseUp += new MouseEventHandler(CheckClick);
            Manager.Invalidate();
            DrawAll();
        }

        /// <summary>
        /// Remove any outstanding menu pieces. Used when switching menus.
        /// </summary>
        public void Destroy()
        {
            JoystickModeOff(this, EventArgs.Empty);
            menuObjects.Clear();
            joystickIndex = 0;
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate -= new GamepadStateHandler.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate -= new GamepadStateHandler.GamepadDelegate(GamepadClick);
            }
            Manager.MouseMove -= JoystickModeOff;
            Manager.MouseUp -= CheckClick;
            foreach (MenuObject menuObject in menuObjects)
            {
                menuObject.Dispose();
            }
        }

        /// <summary>
        /// Manages thumbstick input.
        /// </summary>
        /// <param name="sender">The gamepad that sent the delegate.</param>
        /// <param name="j">The joystick arguments.</param>
        private void ThumbstickManage(object sender, JoystickArgs j)
        {
            if (joystick)
            {
                MenuObject focusObject = menuObjects[JoystickIndex];
                focusObject.GamepadInput(j);
            }
            JoystickMode();
        }

        /// <summary>
        /// Handles joystick button presses.
        /// </summary>
        /// <param name="sender">The gamepad that sent the delegate.</param>
        /// <param name="j"></param>
        private void GamepadClick(object sender, EventArgs j)
        {
            if (joystick)
            {
                menuObjects[joystickIndex].PadClicked = true;
                if (menuObjects[joystickIndex].PadClicked == true && !padMan[0].A)
                {
                    ButtonClicked(menuObjects[joystickIndex]);
                }
            }
            JoystickMode();
        }

        /// <summary>
        /// Enables joystick mode and hides the mouse.
        /// </summary>
        private void JoystickMode()
        {
            //Cursor.Hide();
            joystick = true;
            if (menuObjects.Count > 0 && menuObjects.Count >= joystickIndex)
            {
                menuObjects[joystickIndex].Focus = true;
            }
        }

        /// <summary>
        /// Disables joystick mode and shows the mouse.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void JoystickModeOff(Object source, EventArgs e)
        {
            Cursor.Show();
            joystick = false;
            menuObjects[joystickIndex].Focus = false;
        }

        /// <summary>
        /// Updates this menu and all of it's controls.
        /// </summary>
        /// <param name="elapsedTime">Milliseconds since last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            padMan.Update();
            Elapsed = elapsedTime;
            foreach (MenuObject menuObject in menuObjects)
            {
                menuObject.Update(Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Draws all menu controls.
        /// </summary>
        public override void Draw(Graphics graphics, List<Rectangle> clipRectangles)
        {
            Graphics = graphics;
            List<MenuObject> drawObjects = new List<MenuObject>();
            foreach (MenuObject menuObject in menuObjects)
            {
                foreach (Rectangle rect in clipRectangles)
                {
                    if (rect.IntersectsWith(menuObject.ButtonRect) || rect.IntersectsWith(menuObject.ShadowRect))
                    {
                        if (!drawObjects.Contains(menuObject))
                        {
                            drawObjects.Add(menuObject);
                            continue;
                        }
                    }
                }
            }
            if (drawObjects.Count > 0)
            {
                foreach (MenuObject drawObject in drawObjects)
                {
                    drawObject.Draw(Graphics);
                }
            }
        }

        /// <summary>
        /// Draws all of the menu objects.
        /// </summary>
        public override void DrawAll()
        {
            foreach (MenuObject menuObject in menuObjects)
            {
                menuObject.AutoInvalidate();
            }
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
            Image image = Image.FromFile(fileMan.NamedFile(file, fileMan.ImageDir + "\\Buttons", fileMan.ImageExtension));
            MenuButton button = new MenuButton(x, y, image, name, this, Path.GetFullPath(fileMan.NamedFile("button_press", ".\\Sounds", "*.wav")), Path.GetFullPath(fileMan.NamedFile("button_release", ".\\Sounds", "*.wav")));
            menuObjects.Add(button);
        }

        /// <summary>
        /// Creates a slider object and adds it to the list.
        /// </summary>
        /// <param name="x">The slider's x coordinate.</param>
        /// <param name="y">The slider's y coordinate.</param>
        /// <param name="file">The slider's file name.</param>
        /// <param name="name">The slider's name.</param>
        public void MakeSlider(int x, int y, int length, String file, String name)
        {
            Image image = Image.FromFile(fileMan.NamedFile(file, fileMan.ImageDir + "\\Buttons", fileMan.ImageExtension));
            MenuSlider slider = new MenuSlider(x, y, length, image, name, this, Path.GetFullPath(fileMan.NamedFile("button_press", ".\\Sounds", "*.wav")), Path.GetFullPath(fileMan.NamedFile("button_release", ".\\Sounds", "*.wav")));
            menuObjects.Add(slider);
        }

        /// <summary>
        /// Runs when the mouse is clicked, checks to see if it was clicked on a button.
        /// </summary>
        /// <param name="sender">The mouse sending the event.</param>
        /// <param name="e">Mouse Event Args.</param>
        public void CheckClick(Object sender, EventArgs e)
        {
            if (!joystick)
            {
                if (menuObjects.Count > 0)
                {
                    for (int i = 0; i < menuObjects.Count; i++ )
                    {
                        if (menuObjects[i].Intersects())
                        {
                            ButtonClicked(menuObjects[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates all of the buttons for the menu.
        /// </summary>
        public abstract void InitButtons();

        /// <summary>
        /// This should be overrided with a switch statement that runs off of the button's name.
        /// </summary>
        /// <param name="button">The button that was pressed.</param>
        public abstract void ButtonClicked(MenuObject button);
    }
}
