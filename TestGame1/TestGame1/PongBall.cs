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
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace TestGame1
{
    class PongBall : GameObject
    {
        public static Random rand = new Random();
        private World world;
        private Body body;

        public PongBall(int x, int y, GameWindow window, World iWorld) : base(x, y, Image.FromFile(FileManager.NamedFile("ball", FileManager.MinigameDir + "/TestGame1/Images", "*.gif")), "Pong Ball", window)
        {
            BoundingRect = new Rectangle(x, y, image.Width, image.Height);
            world = iWorld;
            body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(width), 1);
            body.Position = new Microsoft.Xna.Framework.Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y));
            body.ApplyForce(new Microsoft.Xna.Framework.Vector2((rand.Next(2) - 1) * 100,(rand.Next(2) - 1) * 100));
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawImage(image, BoundingRect);
        }

        public override void Update(double time)
        {
            position = new Point((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y));
            Console.WriteLine("Position - " + position);
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
