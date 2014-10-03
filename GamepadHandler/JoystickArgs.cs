using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamepadHandler
{
    public class JoystickArgs : EventArgs
    {
        public GamepadState.ThumbstickState thumbstick;

        public JoystickArgs(GamepadState.ThumbstickState state)
        {
            thumbstick = state;
        }
    }
}
