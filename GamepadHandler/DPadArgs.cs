using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamepadHandler
{
    public class DPadArgs : EventArgs
    {
        public GamepadState.DPadState dPad;

        public DPadArgs(GamepadState.DPadState state)
        {
            dPad = state;
        }
    }
}