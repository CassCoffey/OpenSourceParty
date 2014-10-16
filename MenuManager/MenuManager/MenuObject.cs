using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MenuHandler
{
    /// <summary>
    /// Describes an object that can be displayed on a menu.
    /// </summary>
    public interface MenuObject
    {
        bool Intersects();

        void MouseDown(object sender, MouseEventArgs m);
        void MouseUp(object sender, MouseEventArgs m);

        void Update(Graphics graphics, double time);
    }
}
