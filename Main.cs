using System;
using System.Drawing;
using OpenGL_Game.Engine.Managers;

namespace OpenGL_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainEntry
    {
        public static SceneManager game = new SceneManager();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
           // Capped to fix bug where sound wasnt playing in the game scene, possibly due to too much stuff happening, not sure???
            using (game = new SceneManager())
                game.Run(60);
        }
    }
#endif
}
