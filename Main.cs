using System;
using OpenGL_Game.Managers;

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
           
            using (game = new SceneManager())
                game.Run();
        }
    }
#endif
}
