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

namespace OpenSourceParty
{
    class MainMenu : MenuAbstract
    {
        // Fields
        private FileManager fileMan;
        private GamepadManager padMan;

        private int joystickIndex = 0;

        private System.Timers.Timer joysticktimer = new System.Timers.Timer(2000);

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
                else if (value > buttons.Count)
                {
                    joystickIndex = 0;
                }
            }
        }

        // Constructors and Methods
        public MainMenu(String name) : base(name)
        {
            fileMan = new FileManager();   // Instantiate a new file manager.
            padMan = new GamepadManager();
            String background = fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension);
            if (background != null)
            {
                form.BackgroundImage = Image.FromFile(background);   // Set the background image.
                form.BackgroundImageLayout = ImageLayout.Stretch;
            }
            padMan.Init();
            if (padMan[0] != null)
            {
                padMan[0].lJoystickDelegate = new GamepadState.JoystickDelegate(ThumbstickManage);
                padMan[0].aDelagate = new GamepadState.GamepadDelegate(GamepadClick);
            }
            joysticktimer.Elapsed += OnTimerEnd;
            joysticktimer.Enabled = true;
            MakeButton(10, 10, "button1", "Random Game");
            MakeButton(10, 150, "button2", "List Games");
            MakeButton(10, 300, "button3", "Exit");
            form.Width = 640;
            form.Height = 480;
            form.MouseMove += JoystickModeOff;
            Application.Idle += Draw;
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
        /// Manages thumbstick input.
        /// </summary>
        /// <param name="sender">The gamepad that sent the delegate.</param>
        /// <param name="j">The joystick arguments.</param>
        private void ThumbstickManage(object sender, JoystickArgs j)
        {
            if (joystick)
            {
                joysticktimer.Stop();
                joysticktimer.Start();
                if (j.thumbstick.y > 0)
                {
                    buttons[JoystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Neutral);
                    ++JoystickIndex;
                    buttons[JoystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Hover);
                }
                else if (j.thumbstick.y < 0)
                {
                    buttons[JoystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Neutral);
                    --JoystickIndex;
                    buttons[JoystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Hover);
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
            if (joystick)
            {
                joysticktimer.Stop();
                joysticktimer.Start();
                buttons[joystickIndex].ChangeState((int)MenuButton.ButtonStates.Pressed);
                ButtonClicked(buttons[joystickIndex]);
            }
            JoystickMode();
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

        /// <summary>
        /// Enables joystick mode and hides the mouse.
        /// </summary>
        private void JoystickMode()
        {
            Cursor.Hide();
            joystick = true;
            joysticktimer.Stop();
            joysticktimer.Start();
            buttons[joystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Hover);
        }

        private void JoystickModeOff(Object source, EventArgs e)
        {
            Cursor.Show();
            joystick = false;
            buttons[joystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Neutral);
        }

        /// <summary>
        /// Shows the mouse and disables joystick mode.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimerEnd(Object source, ElapsedEventArgs e)
        {
            Cursor.Show();
            joystick = false;
            buttons[joystickIndex].ChangeBaseState((int)MenuButton.ButtonStates.Neutral);
        }

        /// <summary>
        /// Draws all buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw(object sender, EventArgs e)
        {
            Graphics temp = form.CreateGraphics();
            foreach (MenuButton button in buttons)
            {
                button.Update(temp);
            }
        }
    }
}
