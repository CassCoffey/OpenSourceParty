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

namespace MenuHandler
{
    public abstract class MenuAbstract
    {
        // Fields
        protected List<MenuButton> buttons;
        protected Form form;
        protected Graphics graphics;

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

        public Form Form
        {
            get
            {
                return form;
            }
        }

        public Graphics Graphics
        {
            get
            {
                return graphics;
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
            form = new Form();
            form.Text = name;
            graphics = form.CreateGraphics();
            buttons = new List<MenuButton>();
            form.MouseUp += new MouseEventHandler(CheckClick);
            padMan = new GamepadManager();
            padMan.Init();
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate += new GamepadState.JoystickDelegate(ThumbstickManage);
                padMan[0].lJoystickDelegate += new GamepadState.JoystickDelegate(Draw);
                padMan[0].aDelagate += new GamepadState.GamepadDelegate(GamepadClick);
                padMan[0].aDelagate += new GamepadState.GamepadDelegate(Draw);
            }
            Application.Idle += Draw;
            form.MouseMove += JoystickModeOff;
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
                    buttons[JoystickIndex].Clicked = false;
                    --JoystickIndex;
                    buttons[JoystickIndex].Focus = true;
                    joystickMoved = true;
                }
                else if (j.thumbstick.y < 0 && j.thumbstick.y < j.thumbstick.x && j.thumbstick.y < -j.thumbstick.x && !joystickMoved)
                {
                    buttons[JoystickIndex].Focus = false;
                    buttons[JoystickIndex].Clicked = false;
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
                ButtonClicked(buttons[joystickIndex]);
                buttons[joystickIndex].Clicked = true;
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

        private void JoystickModeOff(Object source, EventArgs e)
        {
            Cursor.Show();
            joystick = false;
            buttons[joystickIndex].Focus = false;
        }

        /// <summary>
        /// Draws all buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Draw(object sender, EventArgs e)
        {
            Graphics temp = form.CreateGraphics();
            foreach (MenuButton button in buttons)
            {
                button.Update(temp);
            }
        }

        public abstract void CheckClick(Object sender, EventArgs e);

        public abstract void ButtonClicked(MenuButton button);
    }
}
