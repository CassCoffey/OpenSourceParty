using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpriteHandler
{
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
        public Sprite(int startX, int startY, Image startImage)
        {
            x = startX;
            y = startY;
            position = new Point(x, y);
            image = startImage;
            height = image.Height;
            width = image.Width;
        }

        public bool Intersects(Sprite sprite)
        {
            if (x + (width*2) > sprite.x && x < sprite.x + (sprite.width*2) && y + (height*2) > sprite.y && y < sprite.y + (sprite.height*2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

        public virtual void Update(Graphics graphics)
        {
            position = new Point(x, y);
            graphics.DrawImage(image, x, y, width*2, height*2);
        }
    }
}
