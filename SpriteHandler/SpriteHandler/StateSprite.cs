using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpriteHandler
{
    public class StateSprite : Sprite
    {
        // Fields
        protected List<Image> states = new List<Image>();
        protected int stateIndex = 0;

        // Constructors and Methods
        public StateSprite(int x, int y, List<Image> startStates) : base(x, y, startStates[0])
        {
            states = startStates;
        }

        public void ChangeState(int index)
        {
            if (index >= 0 && index < states.Count)
            {
                stateIndex = index;
                image = states[stateIndex];
            }
        }

        public override void Update(Graphics graphics)
        {
            base.Update(graphics);
        }
    }
}
