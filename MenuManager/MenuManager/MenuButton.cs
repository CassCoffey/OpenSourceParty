using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SpriteHandler;

namespace MenuHandler
{
    public class MenuButton : StateSprite
    {
        // Fields
        public String Name { get; private set; }
        public enum ButtonStates { Neutral, Hover, Pressed };

        public int neutralState = 0;

        private Form form;

        private MenuAbstract menu;

        // Constructors and Methods
        public MenuButton(int x, int y, List<Image> states, String startName, MenuAbstract parentMenu) : base( x, y, states )
        {
            Name = startName;
            menu = parentMenu;
            form = menu.Form;
        }

        public bool Intersects()
        {
            if (!menu.Joystick)
            {
                if (Intersects(form.PointToClient(Cursor.Position)))
                {
                    return true;
                }
                return false;
            }
            else return false;
        }

        public void ChangeBaseState(int state)
        {
            neutralState = state;
            ChangeState(neutralState);
        }

        public override void Update(Graphics graphics)
        {
            if (Intersects())
            {
                ChangeState((int)ButtonStates.Hover);
            }
            else
            {
                ChangeState(neutralState);
            }
            position = new Point(x, y);
            graphics.DrawImage(image, x, y, width*2, height*2);
        }
    }
}
