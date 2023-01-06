using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Managers
{
    public abstract class AIManager
    {
        public bool IsActive { get; set; }

        public AIManager()
        {
            IsActive = true;
        }

        public abstract void Move(Entity pEntity);
    }
}