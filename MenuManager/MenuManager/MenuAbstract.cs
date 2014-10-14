using System;
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

namespace MenuHandler
{
    /// <summary>
    /// A class that contains all of the essential methods for a menu class to be built off of.
    /// </summary>
    public abstract class MenuAbstract
    {
        // Fields
        protected List<MenuButton> buttons;
        protected Graphics graphics;
        protected FileManager fileMan;
        public GameManager Manager { get; set; }

        // Joystick related fields
        protected bool joystick;
        public GamepadManager padMan;
        private int joystickIndex = 0;
        private bool joystickMoved = false;


        // Properties
        public int JoystickIndex
        {
            get
            {
                return joystickIndex;
            }
            set
            {
                if (value >= 0 && value < buttons.Count)
                {
                    joystickIndex = value;
                }
                else if (value < 0)
                {
                    joystickIndex = buttons.Count - 1;
                }
                else if (value >= buttons.Count)
                {
                    joystickIndex = 0;
                }
            }
        }
        public List<MenuButton> Buttons
        {
            get
            {
                return buttons;
            }
        }
        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
            set
            {
                graphics = value;
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
            buttons = new List<MenuButton>();
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
            Manager.CurMenu = this;
            graphics = iGraphics;
            Manager.Text = name;
            buttons = new List<MenuButton>();
            padMan = iPadMan;
            fileMan = iFileMan;
            Init();
        }

        /// <summary>
        /// Initialization method so that common code between the constructors is in one place, and thread-safe.
        /// </summary>
        public void Init()
        {
            buttons = new List<MenuButton>();
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
                if (j.thumbstick.y > 0 && j.thumbstick.y > j.thumbstick.x && j.thumbstick.y > -j.thumbstick.x && !joystickMoved)
                {
                    buttons[JoystickIndex].Focus = false;
                    buttons[JoystickIndex].PadClicked = false;
                    --JoystickIndex;
                    buttons[JoystickIndex].Focus = true;
                    joystickMoved = true;
                }
                else if (j.thumbstick.y < 0 && j.thumbstick.y < j.thumbstick.x && j.thumbstick.y < -j.thumbstick.x && !joystickMoved)
                {
                    buttons[JoystickIndex].Focus = false;
                    buttons[JoystickIndex].PadClicked = false;
                    ++JoystickIndex;
                    buttons[JoystickIndex].Focus = true;
                    joystickMoved = true;
                }
                else if (j.thumbstick.y == 0)
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
                buttons[joystickIndex].PadClicked = true;
                ButtonClicked(buttons[joystickIndex]);
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
            buttons[joystickIndex].Focus = true;
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
            buttons[joystickIndex].Focus = false;
        }

        public void Update()
        {
            padMan.Update();
            Draw();
        }

        /// <summary>
        /// Draws all menu controls.
        /// </summary>
        protected void Draw()
        {
            graphics = Manager.CreateGraphics();
            foreach (MenuButton button in buttons)
            {
                button.Update(graphics);
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
        public void CheckClick(Object sender, EventArgs e)
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
        /// This should be overrided with a switch statement that runs off of the button's name.
        /// </summary>
        /// <param name="button">The button that was pressed.</param>
        public abstract void ButtonClicked(MenuButton button);
    }
}
