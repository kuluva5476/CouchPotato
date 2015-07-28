using SlimDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace slimdx_gamepad
{
    struct DPad
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
    };

    struct Thumbstick
    {
        public bool Click;
        public double X;
        public double Y;
    };

    struct Thumbsticks
    {
        public Thumbstick Left;
        public Thumbstick Right;
    };

    class Gamepad
    {
        private Controller _controller;

        public bool A;
        public bool B;
        public bool X;
        public bool Y;
        public bool Start;
        public bool Back;
        public DPad DPad;
        public bool LeftBumper;
        public bool RightBumper;
        public double LeftTrigger;
        public double RightTrigger;
        public Thumbsticks Thumbsticks;

        public Gamepad(Controller controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Load the current state
        /// Places values in more JS friendly format
        /// </summary>
        public void LoadState()
        {
            var state = _controller.GetState().Gamepad;
            A = ((state.Buttons & GamepadButtonFlags.A) == GamepadButtonFlags.A);
            B = ((state.Buttons & GamepadButtonFlags.B) == GamepadButtonFlags.B);
            X = ((state.Buttons & GamepadButtonFlags.X) == GamepadButtonFlags.X);
            Y = ((state.Buttons & GamepadButtonFlags.Y) == GamepadButtonFlags.Y);
            Start = ((state.Buttons & GamepadButtonFlags.Start) == GamepadButtonFlags.Start);
            Back = ((state.Buttons & GamepadButtonFlags.Back) == GamepadButtonFlags.Back);
            DPad.Up = ((state.Buttons & GamepadButtonFlags.DPadUp) == GamepadButtonFlags.DPadUp);
            DPad.Down = ((state.Buttons & GamepadButtonFlags.DPadDown) == GamepadButtonFlags.DPadDown);
            DPad.Left = ((state.Buttons & GamepadButtonFlags.DPadLeft) == GamepadButtonFlags.DPadLeft);
            DPad.Right = ((state.Buttons & GamepadButtonFlags.DPadRight) == GamepadButtonFlags.DPadRight);
            LeftBumper = ((state.Buttons & GamepadButtonFlags.LeftShoulder) == GamepadButtonFlags.LeftShoulder);
            RightBumper = ((state.Buttons & GamepadButtonFlags.RightShoulder) == GamepadButtonFlags.RightShoulder);
            if (state.LeftTrigger > SlimDX.XInput.Gamepad.GamepadTriggerThreshold)
            {
                LeftTrigger = state.LeftTrigger / 255.0;
            }
            else
            {
                LeftTrigger = 0;
            }
            if (state.RightTrigger > SlimDX.XInput.Gamepad.GamepadTriggerThreshold)
            {
                RightTrigger = state.RightTrigger / 255.0;
            }
            else
            {
                RightTrigger = 0;
            }

            Thumbsticks.Left.Click = ((state.Buttons & GamepadButtonFlags.LeftThumb) == GamepadButtonFlags.LeftThumb);
            Thumbsticks.Right.Click = ((state.Buttons & GamepadButtonFlags.RightThumb) == GamepadButtonFlags.RightThumb);

            Vector2 leftStick = new Vector2(state.LeftThumbX, state.LeftThumbY).Normalize(SlimDX.XInput.Gamepad.GamepadLeftThumbDeadZone);
            Vector2 rightStick = new Vector2(state.RightThumbX, state.RightThumbY).Normalize(SlimDX.XInput.Gamepad.GamepadRightThumbDeadZone);
            Thumbsticks.Left.X = leftStick.X;
            Thumbsticks.Left.Y = leftStick.Y;

            Thumbsticks.Right.X = rightStick.X;
            Thumbsticks.Right.Y = rightStick.Y;
        }
    }
}
