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
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GamepadHandler;

namespace MinigameLibrary
{
    /// <summary>
    /// A class for managing the various menus and gamestates.
    /// </summary>
    public class GameWindow : Form
    {
        // Properties
        public GameState CurState { get; set; }   // Keeps track of the current game state.
        private Stopwatch GameTime { get; set; }
        private Timer Timer { get; set; }
        private TimeSpan LastUpdate { get; set; }
        public List<System.Drawing.Rectangle> InvalidateRectangles { get; set; }

        public GamepadManager PadMan { get; set; }

        public Stack<GameState> stateStack;

        private double fpsSeconds = 0;
        private int fpsLoops = 0;

        public bool linux = false;

        /// <summary>
        /// Checks if the OS is linux.
        /// </summary>
        public static bool IsLinux
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                PlatformID pid = os.Platform;
                return (pid == PlatformID.Unix) || (pid == PlatformID.MacOSX);
            }
        }


        // Constructors and Methods
        public GameWindow()
        {
            stateStack = new Stack<GameState>(4);
            PadMan = new GamepadManager();
            PadMan.Init();
            InvalidateRectangles = new List<System.Drawing.Rectangle>(2);
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer | System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            GameTime = new Stopwatch();
            GameTime.Start();
            LastUpdate = new TimeSpan(0);
            Timer = new Timer();
            Timer.Interval = 15;
            Timer.Tick += UpdateState;
            Timer.Start();
            if (IsLinux)
            {
                linux = true;
            }
        }

        /// <summary>
        /// Adds a state to the stack. If there is already a state there, it disables that one.
        /// </summary>
        /// <param name="state">The state to add.</param>
        public void AddState(GameState state)
        {
            if (stateStack.Count == 0)
            {
                stateStack.Push(state);
                CurState = stateStack.Peek();
            }
            else
            {
                CurState.Destroy();
                stateStack.Push(state);
                CurState = stateStack.Peek();
            }
        }

        /// <summary>
        /// Returns to the previous state in the stack.
        /// </summary>
        public void BackState()
        {
            stateStack.Pop().Destroy();
            CurState = stateStack.Peek();
            CurState.Restart();
        }

        /// <summary>
        /// Handles any updates necessary. Will run constantly.
        /// This should be hooked on to Application.Idle.
        /// </summary>
        public void UpdateState(object sender, EventArgs e)
        {
            TimeSpan total = GameTime.Elapsed;
            TimeSpan elapsed = total - LastUpdate;
            if (CurState != null)
            {
                CurState.Update(elapsed);
            }
            LastUpdate = total;
            fpsSeconds += elapsed.TotalSeconds;
            fpsLoops++;
            if (fpsSeconds >= 1.00)
            {
                Text = "FPS - " + fpsLoops;
                fpsLoops = 0;
                fpsSeconds = 0;
            }
        }

        /// <summary>
        /// Refreshes all menuObjects when the form size is changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            if (CurState != null)
            {
                CurState.DrawAll();
            }
        }

        /// <summary>
        /// Overrides the RenderForm's default OnPaint() function to also draw the current GameState.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            if (CurState != null)
            {
                CurState.Draw(e.Graphics, InvalidateRectangles);
            }
            InvalidateRectangles.Clear();
        }
    }
}