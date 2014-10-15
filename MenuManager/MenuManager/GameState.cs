using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MenuHandler
{
    public abstract class GameState
    {
        protected Graphics graphics;
        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
            set
            {
                graphics = value;
            }
        }

        public virtual void Update(TimeSpan elapsedTime)
        {

        }
        public virtual void Draw(Graphics graphics)
        {

        }
    }
}
