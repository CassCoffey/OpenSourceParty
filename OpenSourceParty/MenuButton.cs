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
using OpenTK;

namespace MenuHandler
{
    /// <summary>
    /// A special StateSprite made specifically to act as a button.
    /// </summary>
    public class MenuButton : MenuObject
    {
        Vector2 Origin { get; set; }
        Vector2 Offset { get; set; }
        Action action;
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
        public MenuButton(int x, int y, Image startImage, String startName, GameWindow window, String pressSoundLocation, String releaseSoundLocation, Action iAction) : base( x, y, startImage, startName, window, pressSoundLocation, releaseSoundLocation) 
        {
            action = iAction;
        }

        public void ClickButton()
        {
            action.Invoke();
        }

        /// <summary>
        /// The update method for Buttons. Does standard button based checks.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        /// <param name="time">The time elapsed since last update.</param>
        public override void Update(double time)
        {
            // Check if the mouse is over the button.
            if (Intersects() || Focus)
            {
                // Check if the gamepad or mouse are clicked.
                if (MouseClicked || PadClicked && window.PadMan[0].A)
                {
                    Hover = true;
                    releaseSoundBool = false;
                    ZVel -= 0.2 * time;
                    if (!pressSoundBool)   // Prevent Sound Spam
                    {
                        window.PlaySound(PressSound);
                        pressSoundBool = true;
                    }
                }
                else
                {
                    Hover = true;
                    pressSoundBool = false;
                    if ((Z % 1050 < 2) && (ZVel > -2 && ZVel < 2))
                    {
                        Z = 1050;
                        ZVel = 0;
                    }
                    else if (Z < 1050 && ZVel < 3)
                    {
                        ZVel += 0.1 * time;
                    }
                    else if (Z > 1050 && ZVel > -1)
                    {
                        ZVel -= 0.1 * time;
                    }
                    if (!releaseSoundBool)   // Prevent Sound Spam
                    {
                        window.PlaySound(ReleaseSound);
                        releaseSoundBool = true;
                    }
                }
            }
            else
            {
                Hover = false;
                pressSoundBool = false;
                releaseSoundBool = false;
                if ((Z % 1000 < 2) && (ZVel > -2 && ZVel < 2))
                {
                    Z = 1000;
                    ZVel = 0;
                }
                else if (Z < 1000 && ZVel < 3)
                {
                    ZVel += 0.1 * time;
                }
                else if (Z > 1000 && ZVel > -1)
                {
                    ZVel -= 0.1 * time;
                }
            }
            Invalidate();
            position = new Point((int)x, (int)y);
            // Lots of code for calculating Z position. May need some future optimization.
            Z += ZVel;
            double newX = x + ((width * 2) - ((width * 2) * (Z / 1000)));
            double newY = y + ((height * 2) - ((height * 2) * (Z / 1000)));
            height = (int)(image.Height * (Z / 1000));
            width = (int)(image.Width * (Z / 1000));
            ButtonRect = new Rectangle((int)newX, (int)newY, (width * 2), (height * 2));
            ShadowRect = new Rectangle((int)newX + (int)((Z/10) - 90), (int)newY + (int)((Z/10) - 90), (width * 2), (height * 2));
            InvalidateRect = new Rectangle((int)newX, (int)newY, (width * 2) + (int)((Z/10) - 90), (height * 2) + (int)((Z/10) - 90));
            if (InvalidateRect == invalidateRectPrev)
            {
                needsUpdate = false;
            }
            else
            {
                needsUpdate = true;
            }
            invalidateRectPrev = InvalidateRect;
        }

        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        public override void Draw(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            graphics.FillRectangle(brush, ShadowRect);
            graphics.DrawImage(image, ButtonRect);
            if (Hover)   // Dynamically darkens button, no need for more than one button image!
            {
                graphics.FillRectangle(brush, ButtonRect);
            }
        }
    }
}