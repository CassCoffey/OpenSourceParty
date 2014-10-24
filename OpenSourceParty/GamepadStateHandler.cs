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
using OpenTK;
using OpenTK.Input;
using System;

namespace GamepadHandler
{
    /// <summary>
    /// Based on the GamepadState Class By Renaud Bedard.
    /// Modifications by Sean Coffey
    /// </summary>
    public class GamepadStateHandler
    {
        // Fields
        // Public delegates for use with the GamepadState class.
        public GamepadDelegate aDelagate;
        public GamepadDelegate bDelagate;
        public GamepadDelegate xDelagate;
        public GamepadDelegate yDelagate;
        public GamepadDelegate rBumpDelagate;
        public GamepadDelegate lBumpDelagate;
        public GamepadDelegate startDelagate;
        public GamepadDelegate selectDelagate;
        public GamepadDelegate rJoyClickDelagate;
        public GamepadDelegate lJoyClickDelagate;
        public GamepadDelegate rTriggerDelagate;
        public GamepadDelegate lTriggerDelagate;
        public DPadDelegate dPadDelegate;
        public JoystickDelegate rJoystickDelegate;
        public JoystickDelegate lJoystickDelegate;

        // Delegates
        public delegate void GamepadDelegate(object sender, EventArgs e);
        public delegate void JoystickDelegate(object sender, JoystickArgs jArgs);
        public delegate void DPadDelegate(object sender, DPadArgs dArgs);

        // Management of states
        public int GamepadNum { get; private set; }
        private GamePadState state;
        private GamePadState lastState;

        public DPadState DPad { get; private set; }
        public ThumbstickState LeftStick { get; private set; }
        public ThumbstickState RightStick { get; private set; }

        public bool A { get; private set; }
        public bool B { get; private set; }
        public bool X { get; private set; }
        public bool Y { get; private set; }

        public bool Start { get; private set; }
        public bool Select { get; private set; }

        public bool RightShoulder { get; private set; }
        public bool LeftShoulder { get; private set; }

        public float RightTrigger { get; private set; }
        public float LeftTrigger { get; private set; }

        // Bools for checking if a button is held down.
        private DPadState DPadPrev { get; set; }
        private ThumbstickState LeftStickPrev { get; set; }
        private ThumbstickState RightStickPrev { get; set; }

        private bool APrev { get; set; }
        private bool BPrev { get; set; }
        private bool XPrev { get; set; }
        private bool YPrev { get; set; }

        private bool StartPrev { get; set; }
        private bool SelectPrev { get; set; }

        private bool RightShoulderPrev { get; set; }
        private bool LeftShoulderPrev { get; set; }

        private float RightTriggerPrev { get; set; }
        private float LeftTriggerPrev { get; set; }

        public bool Connected
        {
            get { return GamePad.GetState(GamepadNum).IsConnected; }
        }


        // Constructors and Methods
        public GamepadStateHandler(int iGamepadNum)
        {
            GamepadNum = iGamepadNum;
        }

        /// <summary>
        /// Vibrate the controller.
        /// </summary>
        /// <param name="leftMotor">Left motor strength.</param>
        /// <param name="rightMotor">Right motor strength.</param>
        public void Vibrate(float leftMotor, float rightMotor)
        {
            GamePad.SetVibration(GamepadNum, leftMotor, rightMotor);
        }

        /// <summary>
        /// Update the Gamepad's State and send events for all buttons.
        /// </summary>
        public void Update()
        {
            // If not connected, nothing to update
            if (!Connected) return;

            // If same packet, nothing to update
            state = GamePad.GetState(GamepadNum);

            if (state.Equals(lastState))
            {
                return;
            }
            lastState = state;

            // Shoulders
            LeftShoulder = (state.Buttons.LeftShoulder == ButtonState.Pressed);
            if (lBumpDelagate != null && LeftShoulder != LeftShoulderPrev)
            {
                lBumpDelagate(this, EventArgs.Empty);
                LeftShoulderPrev = LeftShoulder;
            }
            RightShoulder = (state.Buttons.RightShoulder == ButtonState.Pressed);
            if (rBumpDelagate != null && RightShoulder != RightShoulderPrev)
            {
                rBumpDelagate(this, EventArgs.Empty);
                RightShoulderPrev = true;
            }

            // Triggers
            LeftTrigger = state.Triggers.Left / (float)byte.MaxValue;
            if (lTriggerDelagate != null && LeftTrigger != LeftTriggerPrev)
            {
                lTriggerDelagate(this, EventArgs.Empty);
                LeftTriggerPrev = LeftTrigger;
            }
            RightTrigger = state.Triggers.Right / (float)byte.MaxValue;
            if (rTriggerDelagate != null && RightTrigger != RightTriggerPrev)
            {
                rTriggerDelagate(this, EventArgs.Empty);
                RightTriggerPrev = RightTrigger;
            }

            // Buttons
            Start = state.Buttons.Start == ButtonState.Pressed;
            if (startDelagate != null && Start != StartPrev)
            {
                startDelagate(this, EventArgs.Empty);
                StartPrev = Start;
            }

            Select = state.Buttons.Back == ButtonState.Pressed;
            if (selectDelagate != null && Select != SelectPrev)
            {
                selectDelagate(this, EventArgs.Empty);
                SelectPrev = Select;
            }

            A = state.Buttons.A == ButtonState.Pressed;
            if (aDelagate != null && A != APrev)
            {
                aDelagate(this, EventArgs.Empty);
                APrev = A;
            }

            B = state.Buttons.B == ButtonState.Pressed;
            if (bDelagate != null && B != BPrev)
            {
                bDelagate(this, EventArgs.Empty);
                BPrev = B;
            }

            X = state.Buttons.X == ButtonState.Pressed;
            if (xDelagate != null && X != XPrev)
            {
                xDelagate(this, EventArgs.Empty);
                XPrev = X;
            }

            Y = state.Buttons.Y == ButtonState.Pressed;
            if (yDelagate != null && Y != YPrev)
            {
                yDelagate(this, EventArgs.Empty);
                YPrev = Y;
            }

            // D-Pad
            DPad = new DPadState(state.DPad.IsUp, state.DPad.IsDown, state.DPad.IsLeft, state.DPad.IsRight);
            if (dPadDelegate != null && !DPad.Equals(DPadPrev))
            {
                DPadArgs dArgs = new DPadArgs(DPad);
                dPadDelegate(this, dArgs);
                DPadPrev = DPad;
            }

            // Thumbsticks
            LeftStick = new ThumbstickState(state.ThumbSticks.Left.Yx, state.Buttons.LeftStick == ButtonState.Pressed);
            if (lJoyClickDelagate != null)
            {
                lJoyClickDelagate(this, EventArgs.Empty);
            }
            if (lJoystickDelegate != null && !LeftStick.Equals(LeftStickPrev))
            {
                JoystickArgs jArgs = new JoystickArgs(LeftStick);
                lJoystickDelegate(this, jArgs);
                LeftStickPrev = LeftStick;
            }

            RightStick = new ThumbstickState(state.ThumbSticks.Right.Yx, state.Buttons.RightStick == ButtonState.Pressed);
            if (rJoyClickDelagate != null)
            {
                rJoyClickDelagate(this, EventArgs.Empty);
            }
            if (rJoystickDelegate != null && !RightStick.Equals(RightStickPrev))
            {
                JoystickArgs jArgs = new JoystickArgs(RightStick);
                rJoystickDelegate(this, jArgs);
                RightStickPrev = RightStick;
            }
        }

        /// <summary>
        /// Combines the x and y values into a normalized Vector2.
        /// </summary>
        /// <param name="rawX">The raw x value.</param>
        /// <param name="rawY">The raw y value.</param>
        /// <param name="threshold">The dead zone.</param>
        /// <returns>A normalized Vector2</returns>
        static Vector2 Normalize(short rawX, short rawY, short threshold)
        {
            var value = new Vector2(rawX, rawY);
            var magnitude = value.Length;
            var direction = value / (magnitude == 0 ? 1 : magnitude);

            var normalizedMagnitude = 0.0f;
            if (magnitude - threshold > 0)
                normalizedMagnitude = Math.Min((magnitude - threshold) / (short.MaxValue - threshold), 1);

            return direction * normalizedMagnitude;
        }

        /// <summary>
        /// A struct that organizes all DPad info.
        /// </summary>
        public struct DPadState
        {
            public readonly bool Up, Down, Left, Right;

            public DPadState(bool up, bool down, bool left, bool right)
            {
                Up = up; Down = down; Left = left; Right = right;
            }

            public override string ToString()
            {
                return ("Up: " + Up + ", Down: " + Down + ", Left: " + Left + ", Right: " + Right);
            }
        }

        /// <summary>
        /// A struct that organizes all Thumbstick info.
        /// </summary>
        public struct ThumbstickState
        {
            public readonly Vector2 Position;
            public readonly bool Clicked;
            public readonly float x;
            public readonly float y;

            public ThumbstickState(Vector2 position, bool clicked)
            {
                Clicked = clicked;
                Position = position;
                x = position.Y;
                y = position.X;
            }
            public override string ToString()
            {
                return ("Position: " + Position + ", Clicked: " + Clicked);
            }
        }
    }

    public static class MathHelper
    {
        /// <summary>
        /// Constrains the value between 0 and 1.
        /// </summary>
        /// <param name="value">The value to constrain.</param>
        /// <returns>A value between 0 and 1.</returns>
        public static float Saturate(float value)
        {
            return value < 0 ? 0 : value > 1 ? 1 : value;
        }
    }
}