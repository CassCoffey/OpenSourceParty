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
using System.Drawing;
using MinigameLibrary;
using FileHandler;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace TestGame1
{
    class PongPaddle : GameObject
    {
        public int PlayerNum { get; private set; }
        public bool joyMove = false;
        private World world;
        private Body body;

        public PongPaddle(int x, int y, int iPlayer, GameWindow window, bool turn, World iWorld) : base(x, y, Image.FromFile(FileManager.NamedFile("pong", FileManager.MinigameDir + "/TestGame1/Images", "*.jpg")), "Pong Paddle " + iPlayer, window)
        {
            world = iWorld;
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y), 1);
            body.BodyType = BodyType.Static;
            PlayerNum = iPlayer;
            if (turn)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                body.Rotation = 90;
            }
            BoundingRect = new Rectangle(x, y, image.Width, image.Height);
        }

        public override void Draw(System.Drawing.Graphics graphics)
        {
            graphics.DrawImage(image, BoundingRect);
        }

        public override void Update(double time)
        {
            if (joyMove)
            {
                if (PlayerNum == 1 || PlayerNum == 2)
                {
                    yVel = -window.PadMan[PlayerNum - 1].LeftStick.y;
                }
                else
                {
                    xVel = window.PadMan[PlayerNum - 1].LeftStick.x;
                }
            }
            else
            {
                xVel = 0;
                yVel = 0;
            }
            x += xVel * time;
            y += yVel * time;
            if (PlayerNum == 1 || PlayerNum == 2)
            {
                if (y + image.Height > window.Height - 70)
                {
                    y = window.Height - (image.Height + 70);
                }
                if (y < 30)
                {
                    y = 30;
                }
            }
            else
            {
                if (x < 30)
                {
                    x = 30;
                }
                if (x + image.Width > window.Width - 50)
                {
                    x = window.Width - (image.Width + 50);
                }
            }
            
            position = new Point((int)x, (int)y);
            body.Position = new Microsoft.Xna.Framework.Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y));
            BoundingRect = new Rectangle(position, image.Size);
            InvalidateRect = BoundingRect;
            if (InvalidateRect == invalidateRectPrev)
            {
                needsUpdate = false;
            }
            else
            {
                needsUpdate = true;
            }
            Invalidate();
            invalidateRectPrev = InvalidateRect;
        }
    }
}
