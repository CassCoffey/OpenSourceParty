using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamepadHandler
{
    public class DPadArgs : EventArgs
    {
        /// <summary>
        /// Event Arguments that contain a thumbstick state.
        /// </summary>
        public GamepadState.DPadState dPad;

        public DPadArgs(GamepadState.DPadState state)
        {
            dPad = state;
        }
    }
}