using OpenTK;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    public abstract class Scene : IScene
    {
        protected SceneManager sceneManager;
        protected EntityManager entityManager;

        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public abstract void Render(FrameEventArgs e);

        public abstract void Update(FrameEventArgs e);

        public abstract void Close();
    }
}
