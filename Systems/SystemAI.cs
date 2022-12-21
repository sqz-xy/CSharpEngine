using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Systems
{
    public class SystemAI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI);
        
        public void Cleanup(Entity pEntity)
        {

        }

        public string Name
        {
            get { return "SystemAI"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if (((entity.Mask & MASK) == MASK))
                {
                    List<IComponent> components = entity.Components;
                    Entity playerEntity = null;
                    
                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    IComponent aiComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_AI;
                    });
                    ComponentAI ai = (ComponentAI) aiComponent;
                }
        }

        // Camera at player pos
        public void MoveEntity(ref ComponentPosition pPos, List<Entity> pNodes, Entity pPlayer)
        {
            if (pPlayer == null)
                return;
        }
    }
}