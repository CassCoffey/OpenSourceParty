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
using System.Windows.Forms;
using System.Drawing;
using MinigameLibrary;
using GamepadHandler;
using FileHandler;

namespace TestGame1
{
    public class TestGame : GameAbstract
    {
        PongPaddle paddleOne;
        PongPaddle paddleTwo;
        PongPaddle paddleThree;
        PongPaddle paddleFour;

        public override void Init()
        {
            base.Init();
            Console.WriteLine("Is Game");
            String background = FileManager.RandomFile(FileManager.BackgroundDir, FileManager.ImageExtension);
            if (background != null)
            {
                Window.BackgroundImage = Image.FromFile(background);   // Set the background image.
                Window.BackgroundImageLayout = ImageLayout.Stretch;
            }
            Window.BackColor = Color.Black;
            paddleOne = new PongPaddle(Window.Width / 2, Window.Height / 2, 1, Window);
            paddleTwo = new PongPaddle(Window.Width / 2, 10, 2, Window);
            paddleThree = new PongPaddle(Window.Width / 2, 10, 3, Window);
            paddleFour = new PongPaddle(Window.Width / 2, 10, 4, Window);
            GameObjects.Add(paddleOne);
            GameObjects.Add(paddleTwo);
            GameObjects.Add(paddleThree);
            GameObjects.Add(paddleFour);
            Window.Invalidate();
        }

        public override void AssignGamepadDelegates(GamepadStateHandler gamepad, int index)
        {
            gamepad.aDelagate += EndGame;
        }

        public override void DestroyGamepadDelegates(GamepadStateHandler gamepad, int index)
        {
            gamepad.aDelagate -= EndGame;
        }

        public override void AssignMouseDelegates()
        {
            Window.MouseUp += new MouseEventHandler(MouseUp);
        }

        public override void DestroyMouseDelegates()
        {
            Window.MouseUp -= MouseUp;
        }

        public void MouseUp(Object sender, EventArgs e)
        {
            EndGame(sender, e);
        }

        private void EndGame(Object sender, EventArgs e)
        {
            Window.BackState();
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
