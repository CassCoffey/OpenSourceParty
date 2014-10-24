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
using OpenTK;

namespace MinigameLibrary
{
    class MenuSlider : MenuObject
    {
        // Fields
        private Point basePos;
        private int length;
        private bool mouseLock = false;
        private double slideCool = 0.00;
        private Rectangle slideRect;


        // Properties
        public Point BasePos
        {
            get
            {
                return basePos;
            }
            set
            {
                basePos = value;
            }
        }


        // Constructors and Methods
        public MenuSlider(int x, int y, int iLength, Image Slide, String name, GameWindow window, String pressSound, String releaseSound) : base(x, y, Slide, name, window, pressSound, releaseSound)
        {
            basePos = new Point(x + width, y);
            position = new Point(x, y);
            length = iLength;
        }

        /// <summary>
        /// Slides the slider to the specified location, does error checking to make sure it stays on the base.
        /// </summary>
        /// <param name="offset">The X position to slide to.</param>
        public void Slide(double offset)
        {
            if (offset < basePos.X - image.Width)
            {
                x = basePos.X - image.Width;
            }
            else if (offset > basePos.X + length - image.Width)
            {
                x = basePos.X + length - image.Width;
            }
            else
            {
                x = offset;
            }
        }

        /// <summary>
        /// Overriden intersection check, so that selecting the Slider is easier with a gamepad.
        /// </summary>
        /// <param name="origin">Beginning of ray.</param>
        /// <param name="offset">End of ray.</param>
        /// <returns>Whether or not the ray intersects the slider.</returns>
        public override bool Intersects(Vector2 origin, Vector2 offset)
        {
            Vector2 topLeft = new Vector2(basePos.X - width, basePos.Y);
            Vector2 topRight = new Vector2(basePos.X + (width * 2) + length, basePos.Y);
            Vector2 bottomLeft = new Vector2(basePos.X - width, basePos.Y + (height * 2));
            Vector2 bottomRight = new Vector2(basePos.X + (width * 2) + length, basePos.Y + (height * 2));
            if (LineIntersects(origin, offset, topLeft, topRight) || LineIntersects(origin, offset, topLeft, bottomLeft) || LineIntersects(origin, offset, bottomLeft, bottomRight) || LineIntersects(origin, offset, topRight, bottomRight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the slider's position and state.
        /// </summary>
        /// <param name="time">The number of milliseconds since the last update.</param>
        public override void Update(double time)
        {
            // Check if it is being slid.
            if (MouseClicked && mouseLock)
            {
                Slide(window.PointToClient(Cursor.Position).X - width);
                Hover = true;
                releaseSoundBool = false;
                ZVel -= 1 * time;
                if (!pressSoundBool)   // Prevent Sound Spam
                {
                    //window.PlaySound(PressSound);
                    pressSoundBool = true;
                }
            }
            // Check if the mouse is over the slider.
            else if (Intersects() || Focus)
            {
                // Check if the gamepad or mouse are clicked.
                if (MouseClicked || PadClicked && window.PadMan[0].A)
                {
                    mouseLock = true;
                    Hover = true;
                    releaseSoundBool = false;
                    ZVel -= (0.2 * time);
                    if (!pressSoundBool)   // Prevent Sound Spam
                    {
                        //window.PlaySound(PressSound);
                        pressSoundBool = true;
                    }
                    if (slideCool >= 0.50 && !MouseClicked)
                    {
                        if (window.PadMan[0].LeftStick.x >= 0.1 && window.PadMan[0].LeftStick.x > window.PadMan[0].LeftStick.y && window.PadMan[0].LeftStick.x > -window.PadMan[0].LeftStick.y)
                        {
                            Slide(x + ((window.PadMan[0].LeftStick.x / 2) * time));
                        }
                        else if (window.PadMan[0].LeftStick.x <= -0.1 && window.PadMan[0].LeftStick.x < window.PadMan[0].LeftStick.y && window.PadMan[0].LeftStick.x < -window.PadMan[0].LeftStick.y)
                        {
                            Slide(x + ((window.PadMan[0].LeftStick.x / 2) * time));
                        }
                        slideCool = 0;
                    }
                }
                else
                {
                    mouseLock = false;
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
                        //window.PlaySound(ReleaseSound);
                        releaseSoundBool = true;
                    }
                }
            }
            else
            {
                mouseLock = false;
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
            double newX = x + ((image.Width * 2) - ((image.Width * 2) * (Z / 1000)));
            double newY = y + ((image.Height * 2) - ((image.Height * 2) * (Z / 1000)));
            height = (int)(image.Height * (Z / 1000));
            width = (int)(image.Width * (Z / 1000));
            ButtonRect = new Rectangle((int)basePos.X - (image.Width + width), (int)newY, length + ((image.Width + width)*2), (height * 2));
            ShadowRect = new Rectangle((int)newX + (int)((Z/10) - 90), (int)newY + (int)((Z/10) - 90), (width * 2), (height * 2));
            slideRect = new Rectangle((int)newX, (int)newY, (width * 2), (height * 2));
            InvalidateRect = new Rectangle((int)basePos.X - (image.Width + width), (int)newY, length + ((image.Width + width) * 2) + (int)((Z/10) - 90), (height * 2) + (int)((Z/10) - 90));
            if (InvalidateRect == invalidateRectPrev && !mouseLock)
            {
                needsUpdate = false;
            }
            else
            {
                needsUpdate = true;
            }
            invalidateRectPrev = InvalidateRect;
            slideCool += time;
        }

        /// <summary>
        /// Draws the slider.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        public override void Draw(Graphics graphics)
        {
            Pen pen = new Pen(Color.Black, 8);
            Pen smallPen = new Pen(Color.Black, 4);
            graphics.DrawLine(pen, basePos.X, basePos.Y + image.Height, basePos.X + length, basePos.Y + image.Height);
            graphics.DrawLine(smallPen, basePos.X, basePos.Y, basePos.X, basePos.Y + (image.Height * 2));
            graphics.DrawLine(smallPen, basePos.X + length, basePos.Y, basePos.X + length, basePos.Y + (image.Height * 2));
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            graphics.FillRectangle(brush, ShadowRect);
            graphics.DrawImage(image, slideRect);
            if (Hover)   // Dynamically darkens slider, no need for more than one button image!
            {
                graphics.FillRectangle(brush, slideRect);
            }
        }
    }
}
