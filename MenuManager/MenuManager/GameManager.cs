using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Windows;

namespace MenuHandler
{
    /// <summary>
    /// A class for managing the various menus and gamestates.
    /// </summary>
    public class GameManager : RenderForm
    {
        public MenuAbstract CurMenu { get; set; }   // Keeps track of the current menu. (will keep track of gamestates soon when implemented)

        public GameManager(MenuAbstract iMenu)
        {
            CurMenu = iMenu;
        }

        /// <summary>
        /// Handles any updates necessary. Will run constantly.
        /// </summary>
        public void UpdateMenu()
        {
            CurMenu.Update();
        }

        /// <summary>
        /// Overrides the OnPaintBackground method, to support backgrounds.
        /// </summary>
        /// <param name="e">Paint Args</param>
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            if (BackgroundImage != null)
            {
                CurMenu.Graphics = e.Graphics;
                e.Graphics.DrawImage(BackgroundImage, DisplayRectangle);
            }
        }
    }
}
