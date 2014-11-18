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
            Window.BackgroundImage = null;
            Window.BackColor = Color.Black;
            paddleOne = new PongPaddle(10, (Window.Height / 2) - 75, 1, Window, true);
            paddleTwo = new PongPaddle(Window.Width - 50, (Window.Height / 2) - 75, 2, Window, true);
            paddleThree = new PongPaddle((Window.Width / 2) - 75, 10, 3, Window, false);
            paddleFour = new PongPaddle((Window.Width / 2) - 75, Window.Height - 75, 4, Window, false);
            GameObjects.Add(paddleOne);
            GameObjects.Add(paddleTwo);
            GameObjects.Add(paddleThree);
            GameObjects.Add(paddleFour);
            DrawAll();
        }

        public override void AssignGamepadDelegates(GamepadStateHandler gamepad, int index)
        {
            gamepad.aDelagate += EndGame;
            gamepad.lJoystickDelegate += MovePong;
        }

        public override void DestroyGamepadDelegates(GamepadStateHandler gamepad, int index)
        {
            gamepad.aDelagate -= EndGame;
            gamepad.lJoystickDelegate -= MovePong;
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

        private void MovePong(object sender, JoystickArgs j)
        {
            GamepadStateHandler gamepad = sender as GamepadStateHandler;
            switch (padMan.Devices.IndexOf(gamepad))
            {
                case (0):
                    if (j.thumbstick.y > 0.1 || j.thumbstick.y < -0.1)
                    {
                        paddleOne.joyMove = true;
                    }
                    else
                    {
                        paddleOne.joyMove = false;
                    }
                    break;
                case (1):
                    if (j.thumbstick.y > 0.1 || j.thumbstick.y < -0.1)
                    {
                        paddleTwo.joyMove = true;
                    }
                    else
                    {
                        paddleTwo.joyMove = false;
                    }
                    break;
                case (2):
                    if (j.thumbstick.x > 0.1 || j.thumbstick.x < -0.1)
                    {
                        paddleThree.joyMove = true;
                    }
                    else
                    {
                        paddleThree.joyMove = false;
                    }
                    break;
                case (3):
                    if (j.thumbstick.x > 0.1 || j.thumbstick.x < -0.1)
                    {
                        paddleFour.joyMove = true;
                    }
                    else
                    {
                        paddleFour.joyMove = false;
                    }
                    break;
            }
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
