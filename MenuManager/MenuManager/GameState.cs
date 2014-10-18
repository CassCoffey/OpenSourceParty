using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

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
        public abstract void Restart();
        /// <summary>
        /// Called when the gamestate is updated by the GameManger.
        /// </summary>
        /// <param name="elapsedTime">Milliseconds since last update.</param>
        public abstract void Update(TimeSpan elapsedTime);
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
}
