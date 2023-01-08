using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Scenes;

namespace OpenGL_Game.Engine.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY | ComponentTypes.COMPONENT_SPEED);

        public SystemPhysics()
        {

        }

        public void Cleanup(Entity pEntity)
        {
            
        }

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var position = ComponentHelper.GetComponent<ComponentPosition>(entity, ComponentTypes.COMPONENT_POSITION);
                    var velocity = ComponentHelper.GetComponent<ComponentVelocity>(entity, ComponentTypes.COMPONENT_VELOCITY);
                    var speed = ComponentHelper.GetComponent<ComponentSpeed>(entity, ComponentTypes.COMPONENT_SPEED);
                    
                    Motion(ref position, ref velocity, ref speed);
                }
        }

        // Pass by ref so the values within the entity change
        private void Motion(ref ComponentPosition pPos, ref ComponentVelocity pVel, ref ComponentSpeed pSpeed)
        {
            // Move an entity based on velocity and speed
            pPos.Position += (pVel.Velocity * GameScene.dt) * pSpeed.Speed;
        }
    }
}
