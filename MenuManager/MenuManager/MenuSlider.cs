using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Windows;
using SlimDX.Direct2D;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace MenuHandler
{
    class MenuSlider : MenuObject
    {
        // Fields
        private Point basePos;
        private int length;
        private bool mouseLock = false;
        private double slideCool = 0.00;
        private Rectangle slideRect;

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

        public override void GamepadInput(GamepadHandler.JoystickArgs j)
        {
            if (PadClicked && menu.padMan[0].A)
            {
                return;
            }
            // Check if the joystick is moved.
            else if ((j.thumbstick.y >= 0.2 || j.thumbstick.y <= -0.2 || j.thumbstick.x >= 0.2 || j.thumbstick.x <= -0.2) && !menu.JoystickMoved)
            {
                SlimDX.Vector2 origin = new SlimDX.Vector2(BasePos.X + width + (length/2), BasePos.Y + height);
                SlimDX.Vector2 offset = new SlimDX.Vector2(j.thumbstick.x * 10000, -j.thumbstick.y * 10000);
                offset += origin;

                MenuObject tempButton = null;
                int tempInt = menu.JoystickIndex;

                for (int i = 0; i < menu.MenuObjects.Count; i++)
                {
                    if (menu.MenuObjects[i].Intersects(origin, offset) && i != menu.JoystickIndex)
                    {
                        if (tempButton != null && menu.MenuObjects[menu.JoystickIndex].Distance(tempButton.position) > menu.MenuObjects[menu.JoystickIndex].Distance(menu.MenuObjects[i].position))
                        {
                            tempButton = menu.MenuObjects[i];
                            tempInt = i;
                        }
                        else if (tempButton == null)
                        {
                            tempButton = menu.MenuObjects[i];
                            tempInt = i;
                        }
                    }
                }

                menu.MenuObjects[menu.JoystickIndex].Focus = false;
                menu.MenuObjects[menu.JoystickIndex].PadClicked = false;
                menu.JoystickIndex = tempInt;
                menu.MenuObjects[menu.JoystickIndex].Focus = true;
                menu.JoystickMoved = true;
            }
            else if (j.thumbstick.y == 0 && j.thumbstick.x == 0)
            {
                menu.JoystickMoved = false;
            }
        }

        /// <summary>
        /// Overriden intersection check, so that selecting the Slider is easier with a gamepad.
        /// </summary>
        /// <param name="origin">Beginning of ray.</param>
        /// <param name="offset">End of ray.</param>
        /// <returns>Whether or not the ray intersects the slider.</returns>
        public override bool Intersects(SlimDX.Vector2 origin, SlimDX.Vector2 offset)
        {
            SlimDX.Vector2 topLeft = new SlimDX.Vector2(basePos.X - width, basePos.Y);
            SlimDX.Vector2 topRight = new SlimDX.Vector2(basePos.X + (width * 2) + length, basePos.Y);
            SlimDX.Vector2 bottomLeft = new SlimDX.Vector2(basePos.X - width, basePos.Y + (height * 2));
            SlimDX.Vector2 bottomRight = new SlimDX.Vector2(basePos.X + (width * 2) + length, basePos.Y + (height * 2));
            if (LineIntersects(origin, offset, topLeft, topRight) || LineIntersects(origin, offset, topLeft, bottomLeft) || LineIntersects(origin, offset, bottomLeft, bottomRight) || LineIntersects(origin, offset, topRight, bottomRight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(double time)
        {
            // Check if it is being slid.
            if (MouseClicked && mouseLock)
            {
                Slide(menu.Manager.PointToClient(Cursor.Position).X - width);
                Hover = true;
                releaseSoundBool = false;
                ZVel -= 1 * time;
                if (!pressSoundBool)   // Prevent Sound Spam
                {
                    manager.PlaySound(PressSound);
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
                    Hover = true;
                    releaseSoundBool = false;
                    ZVel -= (0.2 * time);
                    if (!pressSoundBool)   // Prevent Sound Spam
                    {
                        manager.PlaySound(PressSound);
                        pressSoundBool = true;
                    }
                    if (slideCool >= 0.50 && !MouseClicked)
                    {
                        if (menu.padMan[0].LeftStick.x >= 0.1 && menu.padMan[0].LeftStick.x > menu.padMan[0].LeftStick.y && menu.padMan[0].LeftStick.x > -menu.padMan[0].LeftStick.y)
                        {
                            Slide(x + ((menu.padMan[0].LeftStick.x/2) * time));
                        }
                        else if (menu.padMan[0].LeftStick.x <= -0.1 && menu.padMan[0].LeftStick.x < menu.padMan[0].LeftStick.y && menu.padMan[0].LeftStick.x < -menu.padMan[0].LeftStick.y)
                        {
                            Slide(x + ((menu.padMan[0].LeftStick.x/2) * time));
                        }
                        slideCool = 0;
                    }
                }
                else
                {
                    mouseLock = false;
                    Hover = true;
                    pressSoundBool = false;
                    if ((Z % 105 < 2) && (ZVel > -2 && ZVel < 2))
                    {
                        Z = 105;
                        ZVel = 0;
                    }
                    else if (Z < 105 && ZVel < 3)
                    {
                        ZVel += 0.1 * time;
                    }
                    else if (Z > 105 && ZVel > -1)
                    {
                        ZVel -= 0.1 * time;
                    }
                    if (!releaseSoundBool)   // Prevent Sound Spam
                    {
                        manager.PlaySound(ReleaseSound);
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
                if ((Z % 100 < 2) && (ZVel > -2 && ZVel < 2))
                {
                    Z = 100;
                    ZVel = 0;
                }
                else if (Z < 100 && ZVel < 3)
                {
                    ZVel += 0.1 * time;
                }
                else if (Z > 100 && ZVel > -1)
                {
                    ZVel -= 0.1 * time;
                }
            }
            Invalidate();
            position = new Point((int)x, (int)y);
            // Lots of code for calculating Z position. May need some future optimization.
            Z += ZVel;
            double newX = x + ((image.Width * 2) - ((image.Width * 2) * (Z / 100)));
            double newY = y + ((image.Height * 2) - ((image.Height * 2) * (Z / 100)));
            height = (int)(image.Height * (Z / 100));
            width = (int)(image.Width * (Z / 100));
            ButtonRect = new Rectangle((int)basePos.X - (image.Width + width), (int)newY, length + ((image.Width + width)*2), (height * 2));
            ShadowRect = new Rectangle((int)newX + (int)((Z - 90)), (int)newY + (int)((Z - 90)), (width * 2), (height * 2));
            slideRect = new Rectangle((int)newX, (int)newY, (width * 2), (height * 2));
            InvalidateRect = new Rectangle((int)basePos.X - (image.Width + width), (int)newY, length + ((image.Width + width) * 2) + (int)((Z - 90)), (height * 2) + (int)((Z - 90)));
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
