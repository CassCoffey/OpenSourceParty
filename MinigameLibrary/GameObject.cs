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
using SpriteHandler;
using System.Drawing;

namespace MinigameLibrary
{
    public abstract class GameObject : Sprite
    {
        // Fields
        public Rectangle BoundingRect { get; set; }
        public Rectangle InvalidateRect { get; set; }
        protected Rectangle invalidateRectPrev;
        protected bool needsUpdate = true;

        public double xVel = 0;
        public double yVel = 0;

        // Basic gameObject fields.
        public String Name { get; protected set; }

        // The parent form and menu.
        protected GameWindow window;


        //Constructors and Methods
        // Constructors and Methods
        /// <summary>
        /// The basic constructor for a button.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <param name="startImage">The image to use for this button.</param>
        /// <param name="startName">The button's name, which will determine its function in the parent menu.</param>
        /// <param name="parentGame">The button's parent menu.</param>
        /// <param name="pressSoundLocation">The file path for this button's press sound.</param>
        /// <param name="releaseSoundLocation">The file path for this button's release sound.</param>
        public GameObject(int x, int y, Image startImage, String startName, GameWindow iWindow) : base( x, y, startImage)
        {
            Name = startName;
            window = iWindow;
        }

        /// <summary>
        /// Used to tell the Manager what parts of the screen need to be updated.
        /// </summary>
        public virtual void Invalidate()
        {
            // Tell the GameManager to update graphics.
            if (needsUpdate)
            {
                Rectangle NewInvalidateRect = new Rectangle(invalidateRectPrev.X - 6, invalidateRectPrev.Y - 6, invalidateRectPrev.Width + 12, invalidateRectPrev.Height + 12);
                window.InvalidateRectangles.Add(NewInvalidateRect);
                window.InvalidateRectangles.Add(BoundingRect);
                window.Invalidate(NewInvalidateRect);
            }
        }

        /// <summary>
        /// Used to Invalidate a menuObject, even if it does not need an update.
        /// </summary>
        public virtual void AutoInvalidate()
        {
            // Tell the GameManager to update graphics.
            Rectangle NewInvalidateRect = new Rectangle(invalidateRectPrev.X - 6, invalidateRectPrev.Y - 6, invalidateRectPrev.Width + 12, invalidateRectPrev.Height + 12);
            window.InvalidateRectangles.Add(NewInvalidateRect);
            window.InvalidateRectangles.Add(BoundingRect); 
            window.Invalidate(NewInvalidateRect);
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
