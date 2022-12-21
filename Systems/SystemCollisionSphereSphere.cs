using System;
using System.Collections.Generic;

using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemCollisionSphereSphere : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        private CollisionManager _collisionManager;

        public SystemCollisionSphereSphere(CollisionManager pCollisionManager)
        {
            _collisionManager = pCollisionManager;
        }

        public void Cleanup(Entity pEntity)
        {

        }

        public string Name
        {
            get { return "SystemCollisionSphere"; }
        }

        public void OnAction(List<Entity> pEntity)
        {

            foreach (var firstEntity in pEntity)
            {
                if ((firstEntity.Mask & MASK) == MASK)
                {
                    foreach (var secondEntity in pEntity)
                    {
                        if ((secondEntity.Mask & MASK) == MASK)
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
            
            ComponentPosition entity1Pos, entity2Pos;
            ComponentCollisionSphere entity1Coll, entity2Coll;

            ExtractComponents(pEntity1, out entity1Pos, out entity1Coll);
            ExtractComponents(pEntity2, out entity2Pos, out entity2Coll);

          
            if ((entity1Pos.Position - entity2Pos.Position).Length < entity1Coll.CollisionField + entity2Coll.CollisionField)
            {
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_SPHERE);
            }
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

    }
}
