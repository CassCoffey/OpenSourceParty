using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SlimDX;
using SlimDX.XInput;

namespace OpenSourceParty
{
    class GamepadManager
    {
        // Fields
        private List<GamepadState> devices = new List<GamepadState>(4);   // List of devices that could be connected.
        private List<GamepadState> activeDevices = new List<GamepadState>(4);   // List of devices that are connected.

        // the four default controllers
        private GamepadState gamepadOne = new GamepadState(UserIndex.One);
        private GamepadState gamepadTwo = new GamepadState(UserIndex.Two);
        private GamepadState gamepadThree = new GamepadState(UserIndex.Three);
        private GamepadState gamepadFour = new GamepadState(UserIndex.Four);
        private Thread updateThread;   // The updateThread that handles controller updates.

        // Properties
        public GamepadState this[int index]
        {
            get
            {
                if (index >= 0 && index < devices.Count)
                {
                    return devices[index];
                }
                return null;
            }
            set
            {
                if (index >= 0 && index < devices.Count)
                {
                    devices[index] = value;
                }
            }
        }

        // Constructors and Methods

        /// <summary>
        /// Initializes the manager and starts the updateThread.
        /// </summary>
        public void Init()
        {
            devices.Add(gamepadOne);
            devices.Add(gamepadTwo);
            devices.Add(gamepadThree);
            devices.Add(gamepadFour);
            UpdateDevices();
            updateThread = new Thread(() => Update());
            updateThread.Start();
        }

        /// <summary>
        /// Should only be run in a seperate thread.
        /// Updates the controller list, and gets input from controllers.
        /// </summary>
        private void Update()
        {
            while (true)
            {
                Thread.Sleep(10);
                UpdateDevices();
                foreach (GamepadState wrapper in devices)
                {
                    wrapper.Update();
                }
            }
        }

        /// <summary>
        /// Updates the device list. Removes disconnected devices and adds in any devices that are connected.
        /// </summary>
        private void UpdateDevices()
        {
            if (gamepadOne.Connected && !activeDevices.Contains(gamepadOne))
            {
                activeDevices.Add(gamepadOne);
            }
            if (gamepadTwo.Connected && !activeDevices.Contains(gamepadTwo))
            {
                activeDevices.Add(gamepadTwo);
            }
            if (gamepadThree.Connected && !activeDevices.Contains(gamepadThree))
            {
                activeDevices.Add(gamepadThree);
            }
            if (gamepadFour.Connected && !activeDevices.Contains(gamepadFour))
            {
                activeDevices.Add(gamepadFour);
            }

            if (activeDevices != null)
            {
                for (int i = 0; i < activeDevices.Count; i++)
                {
                    if (!activeDevices[i].Connected)
                    {
                        activeDevices.RemoveAt(i);
                        --i;
                    }
                }
            }

        }
    }
}
