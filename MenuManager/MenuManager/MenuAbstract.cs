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
    public abstract class MenuAbstract
    {
        // Fields
        protected List<MenuButton> buttons;
        protected Form form;
        protected Graphics graphics;

        protected bool joystick;

        public List<MenuButton> Buttons
        {
            get
            {
                return buttons;
            }
        }

        public Form Form
        {
            get
            {
                return form;
            }
        }

        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
        }

        public bool Joystick
        {
            get
            {
                return joystick;
            }
        }
        
        // Constructors and Methods
        public MenuAbstract(String name)
        {
            form = new Form();
            graphics = form.CreateGraphics();
            buttons = new List<MenuButton>();
            form.MouseUp += new MouseEventHandler(CheckClick);
        }

        public abstract void CheckClick(Object sender, EventArgs e);

        public abstract void ButtonClicked(MenuButton button);
    }
}
