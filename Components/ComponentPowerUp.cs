using OpenGL_Game.Managers;
using OpenGL_Game.OBJLoader;

namespace OpenGL_Game.Components
{
    class ComponentPowerUp : IComponent
    {
        string powerUpType;

        public ComponentPowerUp(string pPowerUpType)
        {
            this.powerUpType = pPowerUpType;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POWER_UP; }
        }

        public string PowerUpType()
        {
            return powerUpType;
        }
    }
}
