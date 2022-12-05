using System.Windows.Forms;
using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Components
{
    class ComponentHealth : IComponent
    {
        int health;

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