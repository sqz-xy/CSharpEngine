using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        public SystemPhysics()
        {

        }

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ComponentPosition position = (ComponentPosition)positionComponent;

                IComponent velocityComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });
                ComponentVelocity velocity = (ComponentVelocity)velocityComponent;

                Motion(ref position, ref velocity);
            }
        }

        // Pass by ref so the values within the entity change
        public void Motion(ref ComponentPosition pPos, ref ComponentVelocity pVel)
        {
            pPos.Position = pPos.Position + (pVel.Velocity * GameScene.dt);
            
        }
    }
}
