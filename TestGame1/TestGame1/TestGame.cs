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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using GameAbstracts;

namespace TestGame1
{
    public class TestGame : GameAbstract
    {
        public override void Init()
        {
            base.Init();
            String background = fileMan.RandomFile(fileMan.BackgroundDir, fileMan.ImageExtension);
            if (background != null)
            {
                Manager.BackgroundImage = Image.FromFile(background);   // Set the background image.
            }
        }

        public override void AssignGamepadDelegates(GamepadHandler.GamepadState gamepad, int index)
        {
            gamepad.aDelagate += EndGame;
        }

        public override void DestroyGamepadDelegates(GamepadHandler.GamepadState gamepad, int index)
        {
            gamepad.aDelagate -= EndGame;
        }

        public override void AssignMouseDelegates()
        {
            Manager.MouseUp += new MouseEventHandler(MouseUp);
        }

        public override void DestroyMouseDelegates()
        {
            Manager.MouseUp -= MouseUp;
        }

        public void MouseUp(Object sender, EventArgs e)
        {
            EndGame(sender, e);
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
