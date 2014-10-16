using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MenuHandler
{
    class MenuSlider : MenuObject
    {
        // Fields
        private Point basePos;
        private int length;
        private bool mouseLock = false;

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
        public MenuSlider(int x, int y, int iLength, Image Slide, String name, MenuAbstract menu, String pressSound, String releaseSound) : base(x, y, Slide, name, menu, pressSound, releaseSound)
        {
            basePos = new Point(x + width, y);
            position = new Point(x, y);
            length = iLength;
        }

        /// <summary>
        /// Slides the slider to the specified location, does error checking to make sure it stays on the base.
        /// </summary>
        /// <param name="offset">The X position to slide to.</param>
        public void Slide(int offset)
        {
            if (offset < basePos.X - width)
            {
                x = basePos.X - width;
            }
            else if (offset > basePos.X + length - width)
            {
                x = basePos.X + length - width;
            }
            else
            {
                x = offset;
            }
        }

        public override void Update(System.Drawing.Graphics graphics, double time)
        {
            // Check if it is being slid.
            if (MouseClicked && mouseLock)
            {
                Slide(menu.Manager.PointToClient(Cursor.Position).X - width);
                Hover = true;
                releaseSoundBool = false;
                ZVel -= 2 * time;
                if (!pressSoundBool)   // Prevent Sound Spam
                {
                    manager.PlaySound(PressSound, false);
                    pressSoundBool = true;
                }
            }
            // Check if the mouse is over the slider.
            else if (Intersects() || Focus)
            {
                // Check if the gamepad or mouse are clicked.
                if (MouseClicked || PadClicked && menu.padMan[0].A)
                {
                    mouseLock = true;
                    Slide(menu.Manager.PointToClient(Cursor.Position).X - width);
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
                    mouseLock = false;
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
                mouseLock = false;
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
            Pen pen = new Pen(Color.Black, 8);
            Pen smallPen = new Pen(Color.Black, 4);
            graphics.DrawLine(pen, basePos.X, basePos.Y + height, basePos.X + length, basePos.Y + height);
            graphics.DrawLine(smallPen, basePos.X, basePos.Y, basePos.X, basePos.Y + (height*2));
            graphics.DrawLine(smallPen, basePos.X + length, basePos.Y, basePos.X + length, basePos.Y + (height*2));
            // Lots of code for calculating Z position. May need some future optimization.
            Z += ZVel;
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            int newX = x + ((int)((width * 2) - ((width * 2)) * (Z / 100)));
            int newY = y + ((int)((height * 2) - ((height * 2)) * (Z / 100)));
            graphics.FillRectangle(brush, newX + (int)((Z - 90)), newY + (int)((Z - 90)), (float)((width * 2) * (Z / 100)), (float)((height * 2) * (Z / 100)));
            graphics.DrawImage(image, newX, newY, (int)((width * 2) * (Z / 100)), (int)((height * 2) * (Z / 100)));
            if (Hover)   // Dynamically darkens slider, no need for more than one button image!
            {
                graphics.FillRectangle(brush, newX, newY, (int)((width * 2) * (Z / 100)), (int)((height * 2) * (Z / 100)));
            }
        }
    }
}
