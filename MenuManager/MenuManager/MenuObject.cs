using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MenuHandler
{
    public interface MenuObject
    {
        bool Intersects();

        void MouseDown(object sender, MouseEventArgs m);
        void MouseUp(object sender, MouseEventArgs m);

        void Update(Graphics graphics, double time);
    }
}
