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
            if (LeftShoulder && lBumpDelagate != null && !LeftShoulderPrev)
            {
                lBumpDelagate(this, EventArgs.Empty);
                LeftShoulderPrev = true;
            }
            else if (!LeftShoulder)
            {
                LeftShoulderPrev = false;
            }
            RightShoulder = (gamepadState.Buttons & GamepadButtonFlags.RightShoulder) != 0;
            if (RightShoulder && rBumpDelagate != null && !RightShoulderPrev)
            {
                rBumpDelagate(this, EventArgs.Empty);
                RightShoulderPrev = true;
            }
            else if (!RightShoulder)
            {
                RightShoulderPrev = false;
            }

            // Triggers
            LeftTrigger = gamepadState.LeftTrigger / (float)byte.MaxValue;
            if (LeftTrigger > 0 && lTriggerDelagate != null && LeftTrigger != LeftTriggerPrev)
            {
                lTriggerDelagate(this, EventArgs.Empty);
                LeftTriggerPrev = LeftTrigger;
            }
            RightTrigger = gamepadState.RightTrigger / (float)byte.MaxValue;
            if (RightTrigger > 0 && rTriggerDelagate != null && RightTrigger != RightTriggerPrev)
            {
                rTriggerDelagate(this, EventArgs.Empty);
                RightTriggerPrev = RightTrigger;
            }

            // Buttons
            Start = (gamepadState.Buttons & GamepadButtonFlags.Start) != 0;
            if (Start && startDelagate != null && !StartPrev)
            {
                startDelagate(this, EventArgs.Empty);
                StartPrev = true;
            }
            else if (!Start)
            {
                StartPrev = false;
            }
            Select = (gamepadState.Buttons & GamepadButtonFlags.Back) != 0;
            if (Select && selectDelagate != null && !SelectPrev)
            {
                selectDelagate(this, EventArgs.Empty);
                SelectPrev = true;
            }
            else if (!Select)
            {
                SelectPrev = false;
            }

            A = (gamepadState.Buttons & GamepadButtonFlags.A) != 0;
            if (A && aDelagate != null && !APrev)
            {
                aDelagate(this, EventArgs.Empty);
                APrev = true;
            }
            else if (!A)
            {
                APrev = false;
            }
            B = (gamepadState.Buttons & GamepadButtonFlags.B) != 0;
            if (B && bDelagate != null && !BPrev)
            {
                bDelagate(this, EventArgs.Empty);
                BPrev = true;
            }
            else if (!B)
            {
                BPrev = false;
            }
            X = (gamepadState.Buttons & GamepadButtonFlags.X) != 0;
            if (X && xDelagate != null && !XPrev)
            {
                xDelagate(this, EventArgs.Empty);
                XPrev = true;
            }
            else if (!X)
            {
                XPrev = false;
            }
            Y = (gamepadState.Buttons & GamepadButtonFlags.Y) != 0;
            if (Y && yDelagate != null && !YPrev)
            {
                yDelagate(this, EventArgs.Empty);
                YPrev = true;
            }
            else if (!Y)
            {
                YPrev = false;
            }

            // D-Pad
            DPad = new DPadState((gamepadState.Buttons & GamepadButtonFlags.DPadUp) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadDown) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadLeft) != 0,
                                 (gamepadState.Buttons & GamepadButtonFlags.DPadRight) != 0);
            if ((DPad.Down || DPad.Left || DPad.Right || DPad.Up) && dPadDelegate != null && !DPad.Equals(DPadPrev))
            {
                DPadArgs dArgs = new DPadArgs(DPad);
                dPadDelegate(this, dArgs);
                DPadPrev = DPad;
            }

            // Thumbsticks
            LeftStick = new ThumbstickState(
                Normalize(gamepadState.LeftThumbX, gamepadState.LeftThumbY, Gamepad.GamepadLeftThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.LeftThumb) != 0);
            if (LeftStick.Clicked && lJoyClickDelagate != null)
            {
                lJoyClickDelagate(this, EventArgs.Empty);
            }
            if ((LeftStick.Position.X > 0 || LeftStick.Position.Y > 0 || LeftStick.Position.X < 0 || LeftStick.Position.Y < 0) && lJoystickDelegate != null && !LeftStick.Equals(LeftStickPrev))
            {
                JoystickArgs jArgs = new JoystickArgs(LeftStick);
                lJoystickDelegate(this, jArgs);
                LeftStickPrev = LeftStick;
            }

            RightStick = new ThumbstickState(
                Normalize(gamepadState.RightThumbX, gamepadState.RightThumbY, Gamepad.GamepadRightThumbDeadZone),
                (gamepadState.Buttons & GamepadButtonFlags.RightThumb) != 0);
            if (RightStick.Clicked && rJoyClickDelagate != null)
            {
                rJoyClickDelagate(this, EventArgs.Empty);
            }
            if ((RightStick.Position.X > 0 || RightStick.Position.Y > 0 || RightStick.Position.X < 0 || RightStick.Position.Y < 0) && rJoystickDelegate != null && !RightStick.Equals(RightStickPrev))
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