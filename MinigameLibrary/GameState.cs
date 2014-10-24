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
using System.IO;
using System.Reflection;
using System.Drawing;
using FileHandler;

namespace MinigameLibrary
{
    /// <summary>
    /// Describes a game state, like a menu or minigame.
    /// </summary>
    public abstract class GameState
    {
        // Constructors and Methods
        /// <summary>
        /// Called when restarting the state.
        /// </summary>
        public abstract void Restart();
        /// <summary>
        /// Called when the gamestate is updated by the GameManger.
        /// </summary>
        /// <param name="elapsedTime">Milliseconds since last update.</param>
        public abstract void Update(TimeSpan elapsedTime);
        public abstract void Destroy();
        /// <summary>
        /// Called when the GameManager draws.
        /// </summary>
        /// <param name="graphics">The GameManager will pass this as a parameter when drawing.</param>
        public abstract void Draw(Graphics graphics, List<Rectangle> clipRectangles);

        /// <summary>
        /// Draws all the controls.
        /// </summary>
        public abstract void DrawAll();
    }

    public class GameLoadHelper
    {
        public static void LoadRandomGame(GameWindow window)
        {
            var DLL = Assembly.LoadFile(Path.GetFullPath(FileManager.RandomFile(FileManager.MinigameDir, "*.dll")));   // Load that file.

            foreach (Type type in DLL.GetExportedTypes())   // For every 'type' found in the .dll...
            {
                if (type.IsClass)
                {
                    dynamic c = Activator.CreateInstance(type);   // Create an instance of that type...
                    (c as GameAbstract).Run(window);
                    window.AddState(c as GameAbstract);
                }
            }
        }
    }
}
