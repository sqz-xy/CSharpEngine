using System;
using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Systems
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
            
            var spherePos = ComponentHelper.GetComponent<ComponentPosition>(pEntity1, ComponentTypes.COMPONENT_POSITION);
            var AABBPos = ComponentHelper.GetComponent<ComponentPosition>(pEntity2, ComponentTypes.COMPONENT_POSITION);
            var sphereCol = ComponentHelper.GetComponent<ComponentCollisionSphere>(pEntity1, ComponentTypes.COMPONENT_COLLISION_SPHERE);
            var AABBCol = ComponentHelper.GetComponent<ComponentCollisionAABB>(pEntity2, ComponentTypes.COMPONENT_COLLISION_AABB);

            var xDistance = Math.Abs(spherePos.Position.X - AABBPos.Position.X);
            var zDistance = Math.Abs(spherePos.Position.Z - AABBPos.Position.Z);
            
            if (xDistance >= (AABBCol.Width + sphereCol.CollisionField) || zDistance >= (AABBCol.Depth + sphereCol.CollisionField))
                return;
            
            if ((xDistance < AABBCol.Width) || (zDistance < AABBCol.Depth))
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_AABB);
            
            _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_AABB);

            var cornerDistance = ((xDistance - AABBCol.Width) * (xDistance - AABBCol.Width)) +
                                 ((zDistance - AABBCol.Height) * (zDistance - AABBCol.Height));

            if (cornerDistance < (sphereCol.CollisionField * sphereCol.CollisionField))
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_AABB);
        }
    }
}
