using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GameAbstracts;

namespace TestGame1
{
    public class TestGame : GameAbstract
    {
        public override void Init()
        {
            base.Init();
            Manager.Invalidate();
            Console.WriteLine("Is Gam");
        }

        public override void AssignGamepadDelegates(GamepadHandler.GamepadState gamepad, int index)
        {
            gamepad.aDelagate += EndGame;
        }

        public override void DestroyGamepadDelegates(GamepadHandler.GamepadState gamepad, int index)
        {
            gamepad.aDelagate -= EndGame;
        }

        private void EndGame(Object sender, EventArgs e)
        {
            Destroy();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void Draw(System.Drawing.Graphics graphics, List<System.Drawing.Rectangle> clipRectangles)
        {
            base.Draw(graphics, clipRectangles);
        }
    }
}
