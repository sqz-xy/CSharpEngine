using OpenGL_Game.Engine.Managers;
using OpenTK;

namespace OpenGL_Game.Engine.Scenes
{
    public abstract class Scene : IScene
    {
        // Reference to scene manager
        protected SceneManager SceneManager;
        
        public Scene(SceneManager pSceneManager)
        {
            this.SceneManager = pSceneManager;
        }

        /// <summary>
        /// Scene render method
        /// </summary>
        /// <param name="pE"></param>
        public abstract void Render(FrameEventArgs pE);

        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="pE"></param>
        public abstract void Update(FrameEventArgs pE);

        /// <summary>
        /// Scene close method
        /// </summary>
        public abstract void Close();
    }
}
