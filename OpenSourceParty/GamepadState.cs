using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.XInput;

namespace OpenSourceParty
{
    /// <summary>
    /// Original Class By Renaud Bedard.
    /// Delegates added.
    /// </summary>
    class GamepadState
    {
        // Fields
        public delegate void GamepadDelegate(object sender, EventArgs e);
        public delegate void JoystickDelegate(object sender, JoystickArgs jArgs);
        public delegate void DPadDelegate(object sender, DPadArgs dArgs);
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

        public bool RightShoulder { get; private set; }
        public bool LeftShoulder { get; private set; }

        public bool Start { get; private set; }
        public bool Select { get; private set; }

        public float RightTrigger { get; private set; }
        public float LeftTrigger { get; private set; }

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
            if (LeftShoulder && lBumpDelagate != null)
            {
                lBumpDelagate(this, EventArgs.Empty);
            }
            RightShoulder = (gamepadState.Buttons & GamepadButtonFlags.RightShoulder) != 0;
            if (RightShoulder && rBumpDelagate != null)
            {
                rBumpDelagate(this, EventArgs.Empty);
            }

            // Triggers
            LeftTrigger = gamepadState.LeftTrigger / (float)byte.MaxValue;
            if (LeftTrigger > 0 && lTriggerDelagate != null)
            {
                lTriggerDelagate(this, EventArgs.Empty);
            }
            RightTrigger = gamepadState.RightTrigger / (float)byte.MaxValue;
            if (RightTrigger > 0 && rTriggerDelagate != null)
            {
                rTriggerDelagate(this, EventArgs.Empty);
            }

            // Buttons
            Start = (gamepadState.Buttons & GamepadButtonFlags.Start) != 0;
            if (Start && startDelagate != null)
            {
                startDelagate(this, EventArgs.Empty);
            }
            Select = (gamepadState.Buttons & GamepadButtonFlags.Back) != 0;
            if (Select && selectDelagate != null)
            {
                selectDelagate(this, EventArgs.Empty);
            }

            A = (gamepadState.Buttons & GamepadButtonFlags.A) != 0;
            if (A && aDelagate != null)
            {
                aDelagate(this, EventArgs.Empty);
            }
            B = (gamepadState.Buttons & GamepadButtonFlags.B) != 0;
            if (B && bDelagate != null)
            {
                bDelagate(this, EventArgs.Empty);
            }
            X = (gamepadState.Buttons & GamepadButtonFlags.X) != 0;
            if (X && xDelagate != null)
            {
                xDelagate(this, EventArgs.Empty);
            }
            Y = (gamepadState.Buttons & GamepadButtonFlags.Y) != 0;
            if (Y && yDelagate != null)
            {
                yDelagate(this, EventArgs.Empty);
            }

            // D-Pad
            DPad = new DPadState((gamepadState.Buttons & GamepadButtonFlags.DPadUp) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadDown) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadLeft) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadRight) != 0);
            if ((DPad.Down || DPad.Left || DPad.Right || DPad.Up) && dPadDelegate != null)
            {
                DPadArgs dArgs = new DPadArgs(DPad);
                dPadDelegate(this, dArgs);
            }

            // Thumbsticks
            LeftStick = new ThumbstickState(
                Normalize(gamepadState.LeftThumbX, gamepadState.LeftThumbY, Gamepad.GamepadLeftThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.LeftThumb) != 0);
            if (LeftStick.Clicked && lJoyClickDelagate != null)
            {
                lJoyClickDelagate(this, EventArgs.Empty);
            }
            if ((LeftStick.Position.X >= 1 || LeftStick.Position.Y >=1 || LeftStick.Position.X <= -1 || LeftStick.Position.Y <= -1) && lJoystickDelegate != null)
            {
                JoystickArgs jArgs = new JoystickArgs(LeftStick);
                lJoystickDelegate(this, jArgs);
            }

            RightStick = new ThumbstickState(
                Normalize(gamepadState.RightThumbX, gamepadState.RightThumbY, Gamepad.GamepadRightThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.RightThumb) != 0);
            if (RightStick.Clicked && rJoyClickDelagate != null)
            {
                rJoyClickDelagate(this, EventArgs.Empty);
            }
            if ((RightStick.Position.X >= 1 || RightStick.Position.Y >= 1 || RightStick.Position.X <= -1 || RightStick.Position.Y <= -1) && rJoystickDelegate != null)
            {
                JoystickArgs jArgs = new JoystickArgs(RightStick);
                rJoystickDelegate(this, jArgs);
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

            public DPadState(bool up, bool down, bool left, bool right )
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

            public ThumbstickState(Vector2 position, bool clicked)
            {
                Clicked = clicked;
                Position = position;
            }
            public override string ToString()
            {
                return ("Position: " + Position + ", Clicked: " + Clicked);
            }
        }
    }

    public static class MathHelper
    {
        public static float Saturate(float value)
        {
            return value < 0 ? 0 : value > 1 ? 1 : value;
        }
    }
}
