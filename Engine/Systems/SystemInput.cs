using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Systems
{
    public class SystemInput : ISystem
    {
        // Input manager for registering input
        private InputManager _inputManager;
        
        // Camera for coupling with entity
        private Camera _camera;
        
        // System mask
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_CONTROLLABLE);
        
        public SystemInput(InputManager pInputManager, Camera pCamera)
        {
            _inputManager = pInputManager;
            _camera = pCamera;
        }
        
        public string Name
        {
            get { return "SystemInput"; }
        }
        
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var position = ComponentHelper.GetComponent<ComponentPosition>(entity, ComponentTypes.COMPONENT_POSITION);
                    var direction = ComponentHelper.GetComponent<ComponentDirection>(entity, ComponentTypes.COMPONENT_DIRECTION);
                    var control = ComponentHelper.GetComponent<ComponentControllable>(entity, ComponentTypes.COMPONENT_CONTROLLABLE);
                    
                    if (control.IsControllable)
                        Control(entity, ref position, ref direction);
                }
        }

        public void Control(Entity pEntity, ref ComponentPosition pPos, ref ComponentDirection pDir)
        {
            // had system player originally, merged them
            // Apply input to an entity
            _inputManager.ReadInput(pEntity);
            
            // Map the camera to the entity
            _camera.cameraPosition = pPos.Position;
            _camera.cameraPosition.Y += 0.5f;
            _camera.cameraDirection = pDir.Direction;
            _camera.UpdateView();
        }
    }
}