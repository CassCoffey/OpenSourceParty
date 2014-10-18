using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Windows;
using SpriteHandler;
using MenuHandler;
using GamepadHandler;
using FileHandler;
using System.Diagnostics;
using System.IO;

namespace GameAbstracts
{
    /// <summary>
    /// A class that contains all of the essential methods for a menu class to be built off of.
    /// </summary>
    public abstract class GameAbstract : GameState
    {
        // Fields
        protected List<GameObject> gameObjects;
        protected FileManager fileMan;
        protected GameState returnState;
        public GamepadManager padMan;
        public GameManager Manager { get; set; }
        public TimeSpan Elapsed { get; private set; }


        // Properties
        public List<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }
        }
        public FileManager FileMan
        {
            get
            {
                return fileMan;
            }
            protected set
            {
                fileMan = value;
            }
        }

        // Constructors and Methods
        public void Run(GameManager iManager, GamepadManager iPadMan, FileManager iFileMan, GameState iReturnState)
        {
            Manager = iManager;
            Manager.CurState = this;
            padMan = iPadMan;
            fileMan = iFileMan;
            returnState = iReturnState;
            Init();
        }

        public GameAbstract() { }   

        /// <summary>
        /// Initialization method so that common code between the constructors is in one place, and thread-safe.
        /// </summary>
        public virtual void Init()
        {
            gameObjects = new List<GameObject>();
            for (int i = 0; i < padMan.Devices.Count; i++)
            {
                if (padMan.Devices[i] != null)
                {
                    AssignGamepadDelegates(padMan.Devices[i], i);
                }
            }
            Manager.Invalidate();
        }

        public override void Restart()
        {
            for (int i = 0; i < padMan.Devices.Count; i++)
            {
                if (padMan.Devices[i] != null)
                {
                    AssignGamepadDelegates(padMan.Devices[i], i);
                }
            }
            Manager.Invalidate();
        }

        public abstract void AssignGamepadDelegates(GamepadState gamepad, int index);

        public abstract void DestroyGamepadDelegates(GamepadState gamepad, int index);

        /// <summary>
        /// Remove any outstanding menu pieces. Used when switching menus.
        /// </summary>
        public virtual void Destroy()
        {
            for (int i = 0; i < padMan.Devices.Count; i++)
            {
                if (padMan.Devices[i] != null)
                {
                    DestroyGamepadDelegates(padMan.Devices[i], i);
                }
            }
            Manager.CurState = returnState;
            returnState.Restart();
        }

        /// <summary>
        /// Updates this menu and all of it's controls.
        /// </summary>
        /// <param name="elapsedTime">Milliseconds since last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            padMan.Update();
            Elapsed = elapsedTime;
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Draws all menu controls.
        /// </summary>
        public override void Draw(Graphics graphics, List<Rectangle> clipRectangles)
        {
            Graphics = graphics;
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (Rectangle rect in clipRectangles)
                {
                    if (rect.IntersectsWith(gameObject.BoundingRect))
                    {
                        gameObject.Draw(Graphics);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all of the menu objects.
        /// </summary>
        public override void DrawAll()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.AutoInvalidate();
            }
        }
    }
}