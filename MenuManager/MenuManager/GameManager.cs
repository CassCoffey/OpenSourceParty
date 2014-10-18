using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;
using System.Diagnostics;
using System.Windows.Forms;

namespace MenuHandler
{
    /// <summary>
    /// A class for managing the various menus and gamestates.
    /// </summary>
    public class GameManager : RenderForm
    {
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
        public void UpdateMenu()
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

        /// <summary>
        /// Overrides the OnPaintBackground method, to support backgrounds.
        /// </summary>
        /// <param name="e">Paint Args</param>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            if (BackgroundImage != null)
            {
                CurState.Graphics = e.Graphics;
                e.Graphics.DrawImage(BackgroundImage, DisplayRectangle);
            }
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
            Console.WriteLine("Size Changed");
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
