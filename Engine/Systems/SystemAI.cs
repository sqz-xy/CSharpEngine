using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Systems
{
    public class SystemAI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI) | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_VELOCITY;
        private bool _initialized = false;
        private AIManager _aiManager;

        public SystemAI(AIManager pAIManager)
        {
            _aiManager = pAIManager;
        }
        
        public string Name
        {
            get { return "SystemAI"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    _aiManager.Move(entity);
                }
        }
    }
}