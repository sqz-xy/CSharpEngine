using OpenTK;

namespace OpenGL_Game.Engine.Scenes
{
    /// <summary>
    /// Enum describing the types of scene
    /// </summary>
    public enum SceneTypes
    {
        SceneNone,
        SceneMainMenu,
        SceneGame,
        SceneGameOver,
        SceneGameWin
    }
    interface IScene
    {
        /// <summary>
        /// Scene render method
        /// </summary>
        /// <param name="pE"></param>
        void Render(FrameEventArgs pE);
        
        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="pE"></param>
        void Update(FrameEventArgs pE);
        
        /// <summary>
        /// Scene close method
        /// </summary>
        void Close();
    }
}
