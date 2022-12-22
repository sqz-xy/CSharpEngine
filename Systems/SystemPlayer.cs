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
    public class SystemPlayer : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_CONTROLLABLE | ComponentTypes.COMPONENT_DIRECTION);
        private Camera _camera;

        public SystemPlayer(Camera pCamera)
        {
            _camera = pCamera;
        }

        public void Cleanup(Entity pEntity)
        {

        }

        public string Name
        {
            get { return "SystemPlayer"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;
                    
                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    IComponent directionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                    });
                    ComponentDirection direction = (ComponentDirection)directionComponent;
                    
                    MovePlayer(ref position, ref direction);
                }
        }

        // Camera at player pos
        public void MovePlayer(ref ComponentPosition pPos, ref ComponentDirection pDir)
        {
            _camera.cameraPosition = pPos.Position;
            _camera.cameraPosition.Y += 0.5f;
            _camera.cameraDirection = pDir.Direction;
            _camera.UpdateView();
        }
    }
}
