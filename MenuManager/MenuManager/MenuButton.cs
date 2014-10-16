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
    public class MenuButton : MenuObject
    {
        // Constructors and Methods
        /// <summary>
        /// The basic constructor for a button.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <param name="startImage">The image to use for this button.</param>
        /// <param name="startName">The button's name, which will determine its function in the parent menu.</param>
        /// <param name="parentMenu">The button's parent menu.</param>
        /// <param name="pressSoundLocation">The file path for this button's press sound.</param>
        /// <param name="releaseSoundLocation">The file path for this button's release sound.</param>
        public MenuButton(int x, int y, Image startImage, String startName, MenuAbstract parentMenu, String pressSoundLocation, String releaseSoundLocation) : base( x, y, startImage, startName, parentMenu, pressSoundLocation, releaseSoundLocation) {        }

        /// <summary>
        /// The update method for Buttons. Does standard button based checks.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        /// <param name="time">The time elapsed since last update.</param>
        public override void Update(Graphics graphics, double time)
        {
            // Check if the mouse is over the button.
            if (Intersects() || Focus)
            {
                // Check if the gamepad or mouse are clicked.
                if (MouseClicked || PadClicked && menu.padMan[0].A)
                {
                    Hover = true;
                    releaseSoundBool = false;
                    ZVel -= 2 * time;
                    if (!pressSoundBool)   // Prevent Sound Spam
                    {
                        manager.PlaySound(PressSound, false);
                        pressSoundBool = true;
                    }
                }
                else
                {
                    Hover = true;
                    pressSoundBool = false;
                    if ((Z < 106 && Z > 104) && (ZVel > -1 && ZVel < 1))
                    {
                        Z = 105;
                        ZVel = 0;
                    }
                    else if (Z < 105)
                    {
                        ZVel += 1 * time;
                    }
                    else if (Z > 105 && ZVel > -1)
                    {
                        ZVel -= 1 * time;
                    }
                    if (!releaseSoundBool)   // Prevent Sound Spam
                    {
                        manager.PlaySound(ReleaseSound, false);
                        releaseSoundBool = true;
                    }
                }
            }
            else
            {
                Hover = false;
                pressSoundBool = false;
                releaseSoundBool = false;
                if ((Z < 101 && Z > 99) && (ZVel > -2 && ZVel < 1))
                {
                    Z = 100;
                    ZVel = 0;
                }
                else if (Z < 100)
                {
                    ZVel += 1 * time;
                }
                else if (Z > 100 && ZVel > -1)
                {
                    ZVel -= 1 * time;
                }
            }
            position = new Point(x, y);
            // Lots of code for calculating Z position. May need some future optimization.
            Z += ZVel;
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            int newX = x + ((int)((width * 2) - ((width * 2)) * (Z/100)));
            int newY = y + ((int)((height * 2) - ((height * 2)) * (Z/100)));
            graphics.FillRectangle(brush, newX + (int)((Z-90)), newY + (int)((Z-90)), (float)((width * 2) * (Z/100)), (float)((height * 2) * (Z/100)));
            graphics.DrawImage(image, newX, newY, (int)((width * 2) * (Z/100)), (int)((height * 2) * (Z/100)));
            if (Hover)   // Dynamically darkens button, no need for more than one button image!
            {
                graphics.FillRectangle(brush, newX, newY, (int)((width * 2) * (Z / 100)), (int)((height * 2) * (Z / 100)));
            }
        }
    }
}