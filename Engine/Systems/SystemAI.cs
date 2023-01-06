using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Managers;
using OpenTK;

namespace OpenGL_Game.Systems
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