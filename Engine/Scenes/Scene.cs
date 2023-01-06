using OpenGL_Game.Engine.Managers;
using OpenTK;

namespace OpenGL_Game.Engine.Scenes
{
    public abstract class Scene : IScene
    {
        protected SceneManager sceneManager;
        
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public abstract void Render(FrameEventArgs e);

        public abstract void Update(FrameEventArgs e);

        public abstract void Close();
    }
}
