using System;

namespace OpenGL_Game.Components
{
    public enum EntityFlags
    {
        Player,
        Enemy,
    }
    
    public class ComponentEntityFlag : IComponent
    {
        private EntityFlags _flag;

        public ComponentEntityFlag(string pEntityFlag)
        {
            Enum.TryParse(pEntityFlag, out EntityFlags flag);
            _flag = flag;
        }
        
        public EntityFlags Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_ENTITY_FLAG; }
        }
    }
}