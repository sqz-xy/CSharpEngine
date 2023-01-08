using OpenGL_Game.Engine.Components;

namespace OpenGL_Game.Game.Components
{
    class ComponentDamage : IComponent
    {
        private int damage;

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