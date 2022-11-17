using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemCollisionCameraSphere : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        private CollisionManager _collisionManager;
        private Camera _camera; 

        public SystemCollisionCameraSphere(CollisionManager pCollisionManager, Camera pCamera)
        {
            _collisionManager = pCollisionManager;
            _camera = pCamera;
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

        // Pass by ref so the values within the entity change
        private void CheckCollision(Entity pEntity, ComponentPosition pComponentPosition, ComponentCollisionSphere pComponentCollisionSphere)
        {
            if ((pComponentPosition.Position - _camera.cameraPosition).Length <
                pComponentCollisionSphere.CollisionField + _camera.Radius)
            {
                _collisionManager.CollisionBetweenCamera(pEntity, COLLISIONTYPE.SPHERE_SPHERE);
            }
        }
    }
}
