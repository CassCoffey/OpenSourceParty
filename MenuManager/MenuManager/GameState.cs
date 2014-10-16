using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MenuHandler
{
    /// <summary>
    /// Describes a game state, like a menu or minigame.
    /// </summary>
    public abstract class GameState
    {
        // Fields
        protected Graphics graphics;   // All gamestates must store a local Graphics variable.


        // Properties
        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
            set
            {
                graphics = value;
            }
        }

        
        // Constructors and Methods
        /// <summary>
        /// Called when the gamestate is updated by the GameManger.
        /// </summary>
        /// <param name="elapsedTime">Milliseconds since last update.</param>
        public abstract void Update(TimeSpan elapsedTime);
        /// <summary>
        /// Called when the GameManager draws.
        /// </summary>
        /// <param name="graphics">The GameManager will pass this as a parameter when drawing.</param>
        public abstract void Draw(Graphics graphics);

    }
}
