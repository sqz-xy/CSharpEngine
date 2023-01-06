using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    public abstract class AIManager
    {
        public bool IsActive { get; }

        public AIManager()
        {
            IsActive = true;
        }

        public abstract void Move(Entity pEntity);
    }
}