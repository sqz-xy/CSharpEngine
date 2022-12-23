﻿using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemInput : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_CONTROLLABLE);
        private InputManager _inputManager;
        private Camera _camera;

        public SystemInput(InputManager pInputManager, Camera pCamera)
        {
            _inputManager = pInputManager;
            _camera = pCamera;
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
            _inputManager.ReadInput(pEntity);
            _camera.cameraPosition = pPos.Position;
            _camera.cameraPosition.Y += 0.5f;
            _camera.cameraDirection = pDir.Direction;
            _camera.UpdateView();
        }

        public void Cleanup(Entity pEntity)
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; }
    }
}