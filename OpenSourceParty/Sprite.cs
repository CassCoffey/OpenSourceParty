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
using OpenTK;

namespace SpriteHandler
{
    /// <summary>
    /// The most basic representation of an image. A sprite contains an image and its screen coordinates.
    /// </summary>
    public class Sprite
    {
        // Fields
        public double x;
        public double y;
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
            position = new Point((int)x, (int)y);
            image = startImage;
            height = image.Height;
            width = image.Width;
        }

        /// <summary>
        /// Checks to see if the sprite intersects with another sprite.
        /// </summary>
        /// <param name="sprite">The other sprite to check.</param>
        /// <returns></returns>
        public virtual bool Intersects(Sprite sprite)
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
        public virtual bool Intersects(int otherX, int otherY)
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
        public virtual bool Intersects(Point point)
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
        /// Checks to see if the sprite intersects with specified ray.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>True if it intersects with the ray.</returns>
        public virtual bool Intersects(Vector2 origin, Vector2 offset)
        {
            Vector2 topLeft = new Vector2((float)x, (float)y);
            Vector2 topRight = new Vector2((float)x + (width * 2), (float)y);
            Vector2 bottomLeft = new Vector2((float)x, (float)y + (height * 2));
            Vector2 bottomRight = new Vector2((float)x + (width * 2), (float)y + (height * 2));
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
        /// Nice Line intersection algorithm by Callum Rogers
        /// (http://stackoverflow.com/questions/3746274/line-intersection-with-aabb-rectangle)
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        public static bool LineIntersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            intersection = a1 + t * b;

            return true;
        }

        /// <summary>
        /// Distance calculation between this sprite and another point.
        /// </summary>
        /// <param name="point2">The point to check distance to.</param>
        /// <returns>The distance between this sprite and the specified point.</returns>
        public double Distance(Point point2)
        {
            double xOne = x;
            double xTwo = point2.X;
            double yOne = y;
            double yTwo = point2.Y;
            return Math.Sqrt(Math.Pow((xTwo - xOne), 2) + Math.Pow((yTwo - yOne), 2));
        }

        /// <summary>
        /// Nice Line intersection algorithm by Callum Rogers
        /// (http://stackoverflow.com/questions/3746274/line-intersection-with-aabb-rectangle)
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="intersection"></param>
        /// <returns></returns>
        public static bool LineIntersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            float bDotDPerp = b.X * d.Y - b.Y * d.X;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }

        /// <summary>
        /// The default update method for sprites. Updates position.
        /// </summary>
        /// <param name="time">The milliseconds since the last update.</param>
        public virtual void Update(double time)
        {
            position = new Point((int)x, (int)y);
        }

        /// <summary>
        /// The default draw method for sprites. Draws the sprite.
        /// </summary>
        /// <param name="graphics">The graphics object to use.</param>
        public virtual void Draw(Graphics graphics)
        {
            graphics.DrawImage(image, (int)x, (int)y, width * 2, height * 2);
        }
    }
}
