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
using System.Drawing;
using MinigameLibrary;
using FileHandler;

namespace TestGame1
{
    class PongPaddle : GameObject
    {
        private Rectangle pongRect;

        public int PlayerNum { get; private set; }

        public PongPaddle(int x, int y, int iPlayer, GameWindow window) : base(x, y, Image.FromFile(FileManager.NamedFile("pong", FileManager.MinigameDir + "/TestGame1/Images", "*.jpg")), "Pong Paddle " + iPlayer, window)
        {
            PlayerNum = iPlayer;
            pongRect.Height = image.Height;
            pongRect.Width = image.Width;
            pongRect.X = x;
            pongRect.Y = y;
        }

        public override void Draw(System.Drawing.Graphics graphics)
        {
            graphics.DrawImage(image, pongRect);
        }

        public override void Update(double time)
        {
            position = new Point((int)x, (int)y);
            pongRect.Location = position;
        }
    }
}
