using System.Windows.Forms;
using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Components
{
    class ComponentDamage : IComponent
    {
        int damage;

        public ComponentDamage(int pDamage)
        {
            this.damage = pDamage;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_DAMAGE; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
    }
}