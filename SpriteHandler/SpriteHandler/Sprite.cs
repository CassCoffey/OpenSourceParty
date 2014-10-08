using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpriteHandler
{
    /// <summary>
    /// The most basic representation of an image. A sprite contains an image and its screen coordinates.
    /// </summary>
    public class Sprite
    {
        // Fields
        public int x;
        public int y;
        public Point position;
        public int height;
        public int width;
        protected Image image;

        // Constructors and Methods
        /// <summary>
        /// The sprite's default constructor.
        /// </summary>
        /// <param name="startX">The sprite's X position.</param>
        /// <param name="startY">The sprite's Y position.</param>
        /// <param name="startImage">The sprite's image.</param>
        public Sprite(int startX, int startY, Image startImage)
        {
            x = startX;
            y = startY;
            position = new Point(x, y);
            image = startImage;
            height = image.Height;
            width = image.Width;
        }

        /// <summary>
        /// Checks to see if the sprite intersects with another sprite.
        /// </summary>
        /// <param name="sprite">The other sprite to check.</param>
        /// <returns></returns>
        public bool Intersects(Sprite sprite)
        {
            if (x + (width*2) > sprite.x && x < sprite.x + (sprite.width*2) && y + (height*2) > sprite.y && y < sprite.y + (sprite.height*2))   // This is wrong. It does not work yet. It will work soon.
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the sprite intersects with specified X and Y coordinates.
        /// </summary>
        /// <param name="otherX">The X coordinate to check.</param>
        /// <param name="otherY">The Y coordinate to check.</param>
        /// <returns></returns>
        public bool Intersects(int otherX, int otherY)
        {
            if (x + (width*2) > otherX && x < otherX && y + (height*2) > otherY && y < otherY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks to see if the sprite intersects with specified point.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns></returns>
        public bool Intersects(Point point)
        {
            if (x + (width*2) > point.X && x < point.X && y + (height*2) > point.Y && y < point.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The default update method for sprites. Updates position and draws the image.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        public virtual void Update(Graphics graphics)
        {
            position = new Point(x, y);
            graphics.DrawImage(image, x, y, width*2, height*2);
        }
    }
}
