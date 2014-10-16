﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using SlimDX;
using SlimDX.Windows;
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
        private bool joystickMoved = false;


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
        public MenuAbstract(String name, GameManager iManager, GamepadManager iPadMan, FileManager iFileMan, Graphics iGraphics)
        {
            Manager = iManager;
            Manager.CurState = this;
            graphics = iGraphics;
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
            menuObjects = new List<MenuObject>();
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate += new GamepadState.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate += new GamepadState.GamepadDelegate(GamepadClick);
            }
            Manager.MouseMove += new MouseEventHandler(JoystickModeOff);
            Manager.MouseUp += new MouseEventHandler(CheckClick);
            Manager.Invalidate();
        }

        /// <summary>
        /// Remove any outstanding menu pieces. Used when switching menus.
        /// </summary>
        public void Destroy()
        {
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate -= new GamepadState.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate -= new GamepadState.GamepadDelegate(GamepadClick);
            }
            Manager.MouseMove -= JoystickModeOff;
            Manager.MouseUp -= CheckClick;
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
                // Check if the joystick is moved.
                if ((j.thumbstick.y >= 0.2 || j.thumbstick.y <= -0.2 || j.thumbstick.x >= 0.2 || j.thumbstick.x <= -0.2) && !joystickMoved)
                {
                    MenuObject focusObject = menuObjects[JoystickIndex];
                    Vector2 origin = new Vector2(focusObject.x + focusObject.width, focusObject.y + focusObject.height);
                    Vector2 offset = new Vector2(j.thumbstick.x * 10000, -j.thumbstick.y * 10000);
                    offset += origin;

                    MenuObject tempButton = null;
                    int tempInt = JoystickIndex;
                    
                    for (int i = 0; i < menuObjects.Count; i++)
                    {
                        if (menuObjects[i].Intersects(origin, offset) && i != joystickIndex)
                        {
                            if (tempButton != null && menuObjects[JoystickIndex].Distance(tempButton.position) > menuObjects[JoystickIndex].Distance(menuObjects[i].position))
                            {
                                tempButton = menuObjects[i];
                                tempInt = i;
                            }
                            else if (tempButton == null)
                            {
                                tempButton = menuObjects[i];
                                tempInt = i;
                            }
                        }
                    }

                    menuObjects[JoystickIndex].Focus = false;
                    menuObjects[JoystickIndex].PadClicked = false;
                    JoystickIndex = tempInt;
                    menuObjects[JoystickIndex].Focus = true;
                    joystickMoved = true;
                }
                else if (j.thumbstick.y == 0 && j.thumbstick.x == 0)
                {
                    joystickMoved = false;
                }
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
            if (joystick && padMan[0].A)
            {
                menuObjects[joystickIndex].PadClicked = true;
                ButtonClicked(menuObjects[joystickIndex]);
            }
            JoystickMode();
        }

        /// <summary>
        /// Enables joystick mode and hides the mouse.
        /// </summary>
        private void JoystickMode()
        {
            Cursor.Hide();
            joystick = true;
            menuObjects[joystickIndex].Focus = true;
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
            Manager.Invalidate();   // Tell the GameManager to update graphics.
        }

        /// <summary>
        /// Draws all menu controls.
        /// </summary>
        public override void Draw(Graphics dGraphics)
        {
            Graphics = dGraphics;
            foreach (MenuObject menuObject in menuObjects)
            {
                menuObject.Update(dGraphics, Elapsed.TotalMilliseconds/16.67);
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
                foreach (MenuObject button in menuObjects)
                {
                    if (button.Intersects())
                    {
                        ButtonClicked(button);
                    }
                }
            }
        }

        /// <summary>
        /// This should be overrided with a switch statement that runs off of the button's name.
        /// </summary>
        /// <param name="button">The button that was pressed.</param>
        public abstract void ButtonClicked(MenuObject button);
    }
}
