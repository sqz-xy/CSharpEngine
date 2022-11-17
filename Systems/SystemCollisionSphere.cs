﻿using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemCollisionSphere : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        private CollisionManager _collisionManager;
        private List<Entity> _entityList;

        public SystemCollisionSphere(CollisionManager pCollisionManager)
        {
            _collisionManager = pCollisionManager;
            // List of Entities?
        }
        
        public void Cleanup(Entity pEntity)
        {
            
        }

        public string Name
        {
            get { return "SystemCollisionSphere"; }
        }

        // Take in a list of entities instead for each system
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent positionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    IComponent collisionComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;
                    });
                    ComponentCollisionSphere collision = (ComponentCollisionSphere)collisionComponent;
                
                    CheckCollision(entity, position, collision );
                }
        }

        private void CheckCollision(Entity pEntity, ComponentPosition pComponentPosition, ComponentCollisionSphere pComponentCollisionSphere)
        {
            
        }
    }
}