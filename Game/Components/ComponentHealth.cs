using OpenGL_Game.Engine.Components;

namespace OpenGL_Game.Game.Components
{
    class ComponentHealth : IComponent
    {
        private int health;

        public ComponentHealth(int pHealth)
        {
            this.health = pHealth;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_HEALTH; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}