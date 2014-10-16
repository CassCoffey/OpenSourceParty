using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using SlimDX;
using SlimDX.Windows;
using System.Diagnostics;
using System.Windows.Media;

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

        public GameManager(MenuAbstract iMenu)
        {
            CurState = iMenu;
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
        /// Plays a sound.
        /// </summary>
        /// <param name="location">The file path to the sound.</param>
        /// <param name="loop">Whether or not to loop the sound. (NOT FUNCTIONAL YET)</param>
        public void PlaySound(String location, bool loop)
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(location));
            player.Play();
        }

        /// <summary>
        /// Overrides the RenderForm's default OnPaint() function to also draw the current GameState.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            CurState.Draw(e.Graphics);
        }
    }
}
