using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Managers
{
    public abstract class AIManager
    {
        // Quick toggle boolean to turn off AI for everything
        public bool IsActive { get; set; }

        public AIManager()
        {
            IsActive = true;
        }

        /// <summary>
        /// Entities passed into this method will be moved based on the concrete AI Implementation
        /// </summary>
        /// <param name="pEntity">The Entity to move</param>
        public abstract void Move(Entity pEntity);
    }
}