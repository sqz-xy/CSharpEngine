using System;
using System.Collections.Generic;

using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemCollisionSphereAABB : ISystem
    {
        const ComponentTypes SPHEREMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        const ComponentTypes AABBMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_AABB);
        
        private CollisionManager _collisionManager;

        public SystemCollisionSphereAABB(CollisionManager pCollisionManager)
        {
            _collisionManager = pCollisionManager;
        }

        public void Cleanup(Entity pEntity)
        {

        }

        public string Name
        {
            get { return "SystemCollisionSphereAABB"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var firstEntity in pEntity)
            {
                if ((firstEntity.Mask & SPHEREMASK) == SPHEREMASK)
                {
                    foreach (var secondEntity in pEntity)
                    {
                        if ((secondEntity.Mask & AABBMASK) == AABBMASK)
                        {
                            // Check if an inverse collision is also added
                            CheckCollision(firstEntity, secondEntity);
                        }
                    }
                }
            }
        }
        

        // Pass by ref so the values within the entity change
        private void CheckCollision(Entity pEntity1, Entity pEntity2)
        {
            if (pEntity1 == pEntity2)
                return;
            
            ComponentPosition spherePos, AABBPos;
            ComponentCollisionSphere sphereCol;
            ComponentCollisionAABB AABBCol;
            
            ExtractComponents(pEntity1, out spherePos, out sphereCol);
            ExtractComponents(pEntity2, out AABBPos, out AABBCol);

            var xDistance = Math.Abs(spherePos.Position.X - AABBPos.Position.X);
            var yDistance = Math.Abs(spherePos.Position.Y - AABBPos.Position.Y);
            var zDistance = Math.Abs(spherePos.Position.Z - AABBPos.Position.Z);
            
            if (xDistance >= (AABBCol.Width + sphereCol.CollisionField) || yDistance >= (AABBCol.Height + sphereCol.CollisionField) || zDistance >= (AABBCol.Depth + sphereCol.CollisionField))
                return;
                
            if ((xDistance < AABBCol.Width) || (yDistance < AABBCol.Height) || (zDistance < AABBCol.Depth))
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_AABB);

            float cornerDistance = ((xDistance - AABBCol.Width) * (xDistance - AABBCol.Width)) +
                                   ((yDistance - AABBCol.Height) * (yDistance - AABBCol.Height)) +
                                   ((yDistance - AABBCol.Depth) * (yDistance - AABBCol.Depth));
            
            if (cornerDistance < (sphereCol.CollisionField * sphereCol.CollisionField))
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_AABB);
        }
        
        private void ExtractComponents(Entity pEntity, out ComponentPosition pComponentPosition, out ComponentCollisionSphere pComponentCollision)
        {
            List<IComponent> components = pEntity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            pComponentPosition = (ComponentPosition)positionComponent;

            IComponent collisionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;
            });
            pComponentCollision = (ComponentCollisionSphere)collisionComponent;
        }
        
        private void ExtractComponents(Entity pEntity, out ComponentPosition pComponentPosition, out ComponentCollisionAABB pComponentCollision)
        {

            List<IComponent> components = pEntity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            pComponentPosition = (ComponentPosition)positionComponent;

            IComponent collisionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_AABB;
            });
            pComponentCollision = (ComponentCollisionAABB)collisionComponent;
        }
    }
}
