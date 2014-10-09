using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.XInput;

namespace GamepadHandler
{
    /// <summary>
    /// Original Class By Renaud Bedard.
    /// Delegates added.
    /// </summary>
    public class GamepadState
    {
        // Fields
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

        // Public delegates for use with the GamepadState class.
        public delegate void GamepadDelegate(object sender, EventArgs e);
        public delegate void JoystickDelegate(object sender, JoystickArgs jArgs);
        public delegate void DPadDelegate(object sender, DPadArgs dArgs);

        private Controller controller;
        private UserIndex userIndex;

        uint lastPacket;

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
            get { return controller.IsConnected; }
        }


        // Constructors and Methods
        public GamepadState(UserIndex index)
        {

            userIndex = index;
            controller = new Controller(userIndex);
        }

        /// <summary>
        /// Vibrate the controller.
        /// </summary>
        /// <param name="leftMotor">Left motor strength.</param>
        /// <param name="rightMotor">Right motor strength.</param>
        public void Vibrate(float leftMotor, float rightMotor)
        {
            controller.SetVibration(new Vibration
            {
                LeftMotorSpeed = (ushort)(MathHelper.Saturate(leftMotor) * ushort.MaxValue),
                RightMotorSpeed = (ushort)(MathHelper.Saturate(rightMotor) * ushort.MaxValue)
            });
        }

        /// <summary>
        /// Update the Gamepad's State and send events for all buttons.
        /// </summary>
        public void Update()
        {
            // If not connected, nothing to update
            if (!Connected) return;

            // If same packet, nothing to update
            State state = controller.GetState();
            if (lastPacket == state.PacketNumber) return;
            lastPacket = state.PacketNumber;

            var gamepadState = state.Gamepad;

            // Shoulders
            LeftShoulder = (gamepadState.Buttons & GamepadButtonFlags.LeftShoulder) != 0;
            if (lBumpDelagate != null && LeftShoulder != LeftShoulderPrev)
            {
                lBumpDelagate(this, EventArgs.Empty);
                LeftShoulderPrev = LeftShoulder;
            }
            RightShoulder = (gamepadState.Buttons & GamepadButtonFlags.RightShoulder) != 0;
            if (rBumpDelagate != null && RightShoulder != RightShoulderPrev)
            {
                rBumpDelagate(this, EventArgs.Empty);
                RightShoulderPrev = true;
            }

            // Triggers
            LeftTrigger = gamepadState.LeftTrigger / (float)byte.MaxValue;
            if (lTriggerDelagate != null && LeftTrigger != LeftTriggerPrev)
            {
                lTriggerDelagate(this, EventArgs.Empty);
                LeftTriggerPrev = LeftTrigger;
            }
            RightTrigger = gamepadState.RightTrigger / (float)byte.MaxValue;
            if (rTriggerDelagate != null && RightTrigger != RightTriggerPrev)
            {
                rTriggerDelagate(this, EventArgs.Empty);
                RightTriggerPrev = RightTrigger;
            }

            // Buttons
            Start = (gamepadState.Buttons & GamepadButtonFlags.Start) != 0;
            if (startDelagate != null && Start != StartPrev)
            {
                startDelagate(this, EventArgs.Empty);
                StartPrev = Start;
            }

            Select = (gamepadState.Buttons & GamepadButtonFlags.Back) != 0;
            if (selectDelagate != null && Select != SelectPrev)
            {
                selectDelagate(this, EventArgs.Empty);
                SelectPrev = Select;
            }

            A = (gamepadState.Buttons & GamepadButtonFlags.A) != 0;
            if (aDelagate != null && A != APrev)
            {
                aDelagate(this, EventArgs.Empty);
                APrev = A;
            }

            B = (gamepadState.Buttons & GamepadButtonFlags.B) != 0;
            if (bDelagate != null && B != BPrev)
            {
                bDelagate(this, EventArgs.Empty);
                BPrev = B;
            }

            X = (gamepadState.Buttons & GamepadButtonFlags.X) != 0;
            if (xDelagate != null && X != XPrev)
            {
                xDelagate(this, EventArgs.Empty);
                XPrev = X;
            }

            Y = (gamepadState.Buttons & GamepadButtonFlags.Y) != 0;
            if (yDelagate != null && Y != YPrev)
            {
                yDelagate(this, EventArgs.Empty);
                YPrev = Y;
            }

            // D-Pad
            DPad = new DPadState((gamepadState.Buttons & GamepadButtonFlags.DPadUp) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadDown) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadLeft) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadRight) != 0);
            if (dPadDelegate != null && !DPad.Equals(DPadPrev))
            {
                DPadArgs dArgs = new DPadArgs(DPad);
                dPadDelegate(this, dArgs);
                DPadPrev = DPad;
            }

            // Thumbsticks
            LeftStick = new ThumbstickState(
                Normalize(gamepadState.LeftThumbX, gamepadState.LeftThumbY, Gamepad.GamepadLeftThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.LeftThumb) != 0);
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

            RightStick = new ThumbstickState(
                Normalize(gamepadState.RightThumbX, gamepadState.RightThumbY, Gamepad.GamepadRightThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.RightThumb) != 0);
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
            var magnitude = value.Length();
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
                x = position.X;
                y = position.Y;
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