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
using SlimDX.Direct2D;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
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

        public override void GamepadInput(GamepadHandler.JoystickArgs j)
        {
                // Check if the joystick is moved.
                if ((j.thumbstick.y >= 0.2 || j.thumbstick.y <= -0.2 || j.thumbstick.x >= 0.2 || j.thumbstick.x <= -0.2) && !menu.JoystickMoved)
                {
                    SlimDX.Vector2 origin = new SlimDX.Vector2((float)x + width, (float)y + height);
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
                    ZVel -= 0.2 * time;
                    if (!pressSoundBool)   // Prevent Sound Spam
                    {
                        manager.PlaySound(PressSound);
                        pressSoundBool = true;
                    }
                }
                else
                {
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
            position = new Point((int)x, (int)y);
            // Lots of code for calculating Z position. May need some future optimization.
            Z += ZVel;
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            double newX = x + ((width * 2) - ((width * 2) * (Z / 100)));
            double newY = y + ((height * 2) - ((height * 2) * (Z / 100)));
            height = (int)(image.Height * (Z / 100));
            width = (int)(image.Width * (Z / 100));
            ButtonRect = new Rectangle((int)newX, (int)newY, (width * 2), (height * 2));
            ShadowRect = new Rectangle((int)newX + (int)((Z - 90)), (int)newY + (int)((Z - 90)), (width * 2), (height * 2));
            graphics.FillRectangle(brush, ShadowRect);
            graphics.DrawImage(image, ButtonRect);
            if (Hover)   // Dynamically darkens button, no need for more than one button image!
            {
                graphics.FillRectangle(brush, (int)newX, (int)newY, (width * 2), (height * 2));
            }
        }
    }
}