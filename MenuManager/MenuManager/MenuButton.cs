using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SpriteHandler;

namespace MenuHandler
{
    /// <summary>
    /// A special StateSprite made specifically to act as a button.
    /// </summary>
    public class MenuButton : StateSprite
    {
        // Fields
        private double z;
        private double zVel;
        public double Z 
        {
            get
            {
                return z;
            }

            set
            {
                if (value > 2)
                {
                    z = 2;
                    ZVel = 0;
                }
                else if (value < 0.9)
                {
                    z = 0.9;
                    ZVel = 0;
                }
                else
                {
                    z = value;
                }
            }
        }
        public double ZVel
        {
            get
            {
                return zVel;
            }

            set
            {
                if (value > 0.03)
                {
                    zVel = 0.03;
                }
                else if (value < -0.03)
                {
                    zVel = -0.03;
                }
                else
                {
                    zVel = value;
                }
            }
        }
        public String Name { get; private set; }
        public bool MouseClicked { get; set; }
        public bool PadClicked { get; set; }
        public bool Focus { get; set; }

        // Buttons will normally only have 3 states, Neutral, Hovered over, and Pressed.
        public enum ButtonStates { Neutral, Hover, Pressed };
        public int neutralState = 0;

        // The parent form and menu.
        private Form form;
        private MenuAbstract menu;


        // Constructors and Methods
        /// <summary>
        /// The basic constructor for a button.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <param name="states">A list of images to use as states.</param>
        /// <param name="startName">The button's name, which will determine its function in the parent menu.</param>
        /// <param name="parentMenu">The button's parent menu.</param>
        public MenuButton(int x, int y, List<Image> states, String startName, MenuAbstract parentMenu) : base( x, y, states )
        {
            Name = startName;
            menu = parentMenu;
            form = menu.Manager;
            form.MouseDown += MouseDown;
            form.MouseUp += MouseUp;
            Z = 1.00;
            ZVel = 0.00;
        }

        /// <summary>
        /// A special override of the Sprite's Intersects() method.
        /// This Intersects() method takes no parameters and assumes you want to check if the mouse is over the button.
        /// </summary>
        /// <returns>Returns true if the mouse is over the button.</returns>
        public bool Intersects()
        {
            if (!menu.Joystick)
            {
                if (Intersects(form.PointToClient(Cursor.Position)))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Changes the button's base state, which will normally only be used when joystick mode is on in the parent menu.
        /// </summary>
        /// <param name="state">The new base state.</param>
        public void ChangeBaseState(int state)
        {
            neutralState = state;
            ChangeState(neutralState);
        }

        /// <summary>
        /// Called when the left mouse button is pressed down.
        /// </summary>
        /// <param name="sender">s</param>
        /// <param name="m"></param>
        public void MouseDown(object sender, MouseEventArgs m)
        {
            MouseClicked = true;
        }

        /// <summary>
        /// Called when the left mouse button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public void MouseUp(object sender, MouseEventArgs m)
        {
            MouseClicked = false;
        }

        /// <summary>
        /// The update method for Buttons. Does standard button based checks.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        public override void Update(Graphics graphics)
        {
            // Check if the mouse is over the button.
            if (Intersects() || Focus)
            {
                // Check if the gamepad or mouse are clicked.
                if (MouseClicked || PadClicked && menu.padMan[0].A)
                {
                    ZVel -= 0.02;
                }
                else
                {
                    ChangeState((int)ButtonStates.Hover);
                    if ((Z < 1.06 && Z > 1.04) && (ZVel > -0.01 && ZVel < 0.01))
                    {
                        Z = 1.05;
                        ZVel = 0;
                    }
                    else if (Z < 1.05)
                    {
                        ZVel += 0.01;
                    }
                    else if (Z > 1.05 && ZVel > -0.01)
                    {
                        ZVel -= 0.01;
                    }
                }
            }
            else
            {
                ChangeState(neutralState);
                if ((Z < 1.01 && Z > 0.99) && (ZVel > -0.02 && ZVel < 0.01))
                {
                    Z = 1.00;
                    ZVel = 0;
                }
                else if (Z < 1.00)
                {
                    ZVel += 0.01;
                }
                else if (Z > 1.00 && ZVel > -0.01)
                {
                    ZVel -= 0.01;
                }
            }
            position = new Point(x, y);
            Z += ZVel;
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            int newX = x + (int)((width * 2) - ((width * 2) * Z));
            int newY = y + (int)((height * 2) - ((height * 2) * Z));
            graphics.FillRectangle(brush, newX + (int)(100 * (Z-.9)), newY + (int)(100 * (Z-.9)), (float)((width * 2) * Z), (float)((height * 2) * Z));
            graphics.DrawImage(image, newX, newY, (int)((width * 2) * Z), (int)((height * 2) * Z));
        }
    }
}
