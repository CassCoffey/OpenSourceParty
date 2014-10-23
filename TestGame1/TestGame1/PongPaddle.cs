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
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using GameAbstracts;
using FileHandler;

namespace TestGame1
{
    class PongPaddle : GameObject
    {
        public int PlayerNum { get; private set; }

        public PongPaddle(int x, int y, int iPlayer, Form form) : base(x, y, Image.FromFile(FileManager.NamedFile("pong", ".\\MiniGames\\TestGame1\\Images", "*.jpg")), "Pong Paddle " + iPlayer, form)
        {
            PlayerNum = iPlayer;
        }

        public override void Draw(System.Drawing.Graphics graphics)
        {
            graphics.DrawImage(image, position);
        }

        public override void Update(double time)
        {
            position = new Point((int)x, (int)y);
        }
    }
}
