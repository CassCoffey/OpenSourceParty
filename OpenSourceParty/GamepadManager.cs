/*  Open Source Party, a platform for making and playing party minigames with your friends
    Copyright (C) 2014  Sean Coffey

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
using System.Collections.Generic;

namespace GamepadHandler
{
    public class GamepadManager
    {
        // Fields
        public List<GamepadStateHandler> Devices { get; private set; }   // List of devices that could be connected.
        private List<GamepadStateHandler> activeDevices = new List<GamepadStateHandler>(4);   // List of devices that are connected.

        // the four default controllers
        private GamepadStateHandler gamepadOne = new GamepadStateHandler(0);
        private GamepadStateHandler gamepadTwo = new GamepadStateHandler(1);
        private GamepadStateHandler gamepadThree = new GamepadStateHandler(2);
        private GamepadStateHandler gamepadFour = new GamepadStateHandler(3);


        // Properties
        public GamepadStateHandler this[int index]
        {
            get
            {
                if (index >= 0 && index < Devices.Count)
                {
                    return Devices[index];
                }
                return null;
            }
            set
            {
                if (index >= 0 && index < Devices.Count)
                {
                    Devices[index] = value;
                }
            }
        }


        // Constructors and Methods
        /// <summary>
        /// Initializes the manager and runs the first device check.
        /// </summary>
        public void Init()
        {
            Devices = new List<GamepadStateHandler>(4);
            Devices.Add(gamepadOne);
            Devices.Add(gamepadTwo);
            Devices.Add(gamepadThree);
            Devices.Add(gamepadFour);
            UpdateDevices();
        }

        /// <summary>
        /// Updates the controller list, and gets input from controllers.
        /// </summary>
        public void Update()
        {
            UpdateDevices();
            foreach (GamepadStateHandler wrapper in Devices)
            {
                wrapper.Update();
            }
        }

        /// <summary>
        /// Updates the device list. Removes disconnected devices and adds in any devices that are connected.
        /// </summary>
        private void UpdateDevices()
        {
            // Check all of the gamepads. If connected, add them to the active devices list.
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


            // Check to see if there are any inactive devices that can be removed from the list.
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