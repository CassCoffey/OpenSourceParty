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
using System.Windows.Forms;
using System.Drawing;
using SpriteHandler;
using GamepadHandler;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace MenuHandler
{
    /// <summary>
    /// Describes an object that can be displayed on a menu.
    /// </summary>
    public abstract class MenuObject : Sprite
    {
        // Fields
        // Management of button Z height.
        private double z;
        private double zVel;
        public Rectangle ShadowRect { get; set; }
        public Rectangle ButtonRect { get; set; }
        public Rectangle InvalidateRect { get; set; }
        protected Rectangle invalidateRectPrev;
        protected bool needsUpdate = true;
        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                if (value > 1200)
                {
                    z = 1200;
                    ZVel = 0;
                }
                else if (value < 900)
                {
                    z = 900;
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
                zVel = value;
            }
        }

        // Basic button fields.
        public String Name { get; protected set; }
        public bool MouseClicked { get; set; }
        public bool PadClicked { get; set; }
        public bool Focus { get; set; }
        public bool Hover { get; set; }

        // The fields for managing sounds.
        protected bool pressSoundBool = false;
        protected bool releaseSoundBool = false;
        public String PressSound { get; set; }
        public String ReleaseSound { get; set; }

        // The parent form and menu.
        protected GameManager manager;
        protected MenuAbstract menu;


        //Constructors and Methods
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
        public MenuObject(int x, int y, Image startImage, String startName, MenuAbstract parentMenu, String pressSoundLocation, String releaseSoundLocation) : base( x, y, startImage)
        {
            Name = startName;
            menu = parentMenu;
            manager = menu.Manager;
            manager.MouseDown += MouseDown;
            manager.MouseUp += MouseUp;
            Z = 1000.00;
            ZVel = 0.00;
            PressSound = pressSoundLocation;
            ReleaseSound = releaseSoundLocation;
        }

        /// <summary>
        /// A special override of the Sprite's Intersects() method.
        /// This Intersects() method takes no parameters and assumes you want to check if the mouse is over the button.
        /// </summary>
        /// <returns>Returns true if the mouse is over the button.</returns>
        public virtual bool Intersects()
        {
            if (!menu.Joystick)
            {
                if (Intersects(manager.PointToClient(Cursor.Position)))
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
        /// Used to tell the Manager what parts of the screen need to be updated.
        /// </summary>
        public virtual void Invalidate()
        {
            // Tell the GameManager to update graphics.
            if (needsUpdate)
            {
                InvalidateRect = new Rectangle(InvalidateRect.X - 4, InvalidateRect.Y - 4, InvalidateRect.Width + 8, InvalidateRect.Height + 8);
                manager.InvalidateRectangles.Add(InvalidateRect);
                menu.Manager.Invalidate(InvalidateRect);
            }
        }

        /// <summary>
        /// Used to Invalidate a menuObject, even if it does not need an update.
        /// </summary>
        public virtual void AutoInvalidate()
        {
            // Tell the GameManager to update graphics.
                InvalidateRect = new Rectangle(InvalidateRect.X - 4, InvalidateRect.Y - 4, InvalidateRect.Width + 8, InvalidateRect.Height + 8);
                manager.InvalidateRectangles.Add(InvalidateRect);
                menu.Manager.Invalidate(InvalidateRect);
        }

        /// <summary>
        /// Called when the left mouse button is pressed down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public virtual void MouseDown(object sender, MouseEventArgs m)
        {
            MouseClicked = true;
        }

        /// <summary>
        /// Code that determines how the object handles Gamepad Input
        /// </summary>
        /// <param name="j"></param>
        public abstract void GamepadInput(JoystickArgs j);

        /// <summary>
        /// Called when the left mouse button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public virtual void MouseUp(object sender, MouseEventArgs m)
        {
            MouseClicked = false;
        }

        /// <summary>
        /// Called when the object is updated.
        /// </summary>
        /// <param name="time">The time since last update.</param>
        public abstract override void Update(double time);

        /// <summary>
        /// Called when the object is drawn.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        public abstract override void Draw(Graphics graphics);
    }
}
