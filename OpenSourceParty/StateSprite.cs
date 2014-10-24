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
using System.Collections.Generic;
using System.Drawing;

namespace SpriteHandler
{
    /// <summary>
    /// A special sprite that can switch between a list of images, or states.
    /// </summary>
    public class StateSprite : Sprite
    {
        // Fields
        protected List<Image> states = new List<Image>();
        protected int stateIndex = 0;


        // Properties
        public Image Image
        {
            get
            {
                return states[stateIndex];
            }
            set
            {
                states[stateIndex] = value;
            }
        }


        // Constructors and Methods
        /// <summary>
        /// The StateSprite's default constructor.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <param name="startStates">A list of images to use for this StateSprite.</param>
        public StateSprite(int x, int y, List<Image> startStates) : base(x, y, startStates[0])
        {
            states = startStates;
        }

        /// <summary>
        /// Sets the StateSprite's state and by extension, it's current image.
        /// </summary>
        /// <param name="index">The index of the state to use.</param>
        public void ChangeState(int index)
        {
            if (index >= 0 && index < states.Count)
            {
                stateIndex = index;
                image = states[stateIndex];
            }
        }

        /// <summary>
        /// The StateSprite's default update method. Calls the Sprite's update method.
        /// </summary>
        /// <param name="graphics">The Graphics Object to use.</param>
        /// <param name="time">Milliseconds since last update.</param>
        public override void Update(double time)
        {
            base.Update(time);
        }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
        }
    }
}
