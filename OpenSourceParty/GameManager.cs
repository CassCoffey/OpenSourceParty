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
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MenuHandler
{
    /// <summary>
    /// A class for managing the various menus and gamestates.
    /// </summary>
    public class GameManager : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
        }

        public GameState CurState { get; set; }   // Keeps track of the current game state.
        Stopwatch GameTime { get; set; }
        TimeSpan LastUpdate { get; set; }
        public List<System.Drawing.Rectangle> InvalidateRectangles { get; set; }

        double fpsSeconds = 0;
        int fpsLoops = 0;

        public GameManager(MenuAbstract iMenu)
        {
            CurState = iMenu;
            InvalidateRectangles = new List<System.Drawing.Rectangle>(2);
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer | System.Windows.Forms.ControlStyles.UserPaint | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            GameTime = new Stopwatch();
            GameTime.Start();
            LastUpdate = new TimeSpan(0);
        }

        /// <summary>
        /// Handles any updates necessary. Will run constantly.
        /// </summary>
        public void UpdateMenu(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                TimeSpan total = GameTime.Elapsed;
                TimeSpan elapsed = total - LastUpdate;
                CurState.Update(elapsed);
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
        }

        /// <summary>
        /// Overrides the OnPaintBackground method, to support backgrounds.
        /// </summary>
        /// <param name="e">Paint Args</param>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //if (BackgroundImage != null)
            //{
            //    CurState.Graphics = e.Graphics;
            //    e.Graphics.DrawImage(BackgroundImage, DisplayRectangle);
            //}
        }

        /// <summary>
        /// Plays a sound. MediaPlayer sucks and I am currently looking for an alternative.
        /// </summary>
        /// <param name="location">The file path to the sound.</param>
        public void PlaySound(String location)
        {

        }

        public void OnSoundEnd(object sender, EventArgs e)
        {

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
            CurState.Draw(e.Graphics, InvalidateRectangles);
            InvalidateRectangles.Clear();
        }
    }
}
