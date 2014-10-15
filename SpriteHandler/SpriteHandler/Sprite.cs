using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SlimDX;

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
        /// Checks to see if the sprite intersects with specified ray.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns></returns>
        public bool Intersects(Vector2 origin, Vector2 offset)
        {
            Vector2 topLeft = new Vector2(x,y);
            Vector2 topRight = new Vector2(x + (width*2),y);
            Vector2 bottomLeft = new Vector2(x,y + (height*2));
            Vector2 bottomRight = new Vector2(x + (width*2),y + (height*2));
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
        static bool LineIntersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
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
        /// The default update method for sprites. Updates position and draws the image.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        public virtual void Update(Graphics graphics, double time)
        {
            position = new Point(x, y);
            graphics.DrawImage(image, x, y, width*2, height*2);
        }
    }
}
