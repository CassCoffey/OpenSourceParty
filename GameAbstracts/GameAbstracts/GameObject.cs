using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteHandler;
using System.Drawing;
using MenuHandler;

namespace GameAbstracts
{
    public abstract class GameObject : Sprite
    {
        // Fields
        public Rectangle BoundingRect { get; set; }
        public Rectangle InvalidateRect { get; set; }
        protected Rectangle invalidateRectPrev;
        protected bool needsUpdate = true;

        // Basic gameObject fields.
        public String Name { get; protected set; }

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
        public GameObject(int x, int y, Image startImage, String startName, MenuAbstract parentMenu) : base( x, y, startImage)
        {
            Name = startName;
            menu = parentMenu;
            manager = menu.Manager;
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
