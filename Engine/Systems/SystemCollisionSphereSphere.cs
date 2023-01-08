using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Systems
{
    public class SystemCollisionSphereSphere : ISystem
    {
        // Reference to collision manager
        private CollisionManager _collisionManager;
        
        // System mask
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        
        public SystemCollisionSphereSphere(CollisionManager pCollisionManager)
        {
            _collisionManager = pCollisionManager;
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
            
            var entity1Pos = ComponentHelper.GetComponent<ComponentPosition>(pEntity1, ComponentTypes.COMPONENT_POSITION);
            var entity1Coll = ComponentHelper.GetComponent<ComponentCollisionSphere>(pEntity1, ComponentTypes.COMPONENT_COLLISION_SPHERE);
            var entity2Pos = ComponentHelper.GetComponent<ComponentPosition>(pEntity2, ComponentTypes.COMPONENT_POSITION);
            var entity2Coll = ComponentHelper.GetComponent<ComponentCollisionSphere>(pEntity2, ComponentTypes.COMPONENT_COLLISION_SPHERE);

            // Check for sphere intersection
            if ((entity1Pos.Position - entity2Pos.Position).Length < entity1Coll.CollisionField + entity2Coll.CollisionField)
            {
                _collisionManager.RegisterCollision(pEntity1, pEntity2, COLLISIONTYPE.SPHERE_SPHERE);
            }
        }
    }
}
