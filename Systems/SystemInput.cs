using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    public class SystemInput : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_CONTROLLABLE);
        private InputManager _inputManager;

        public SystemInput(InputManager pInputManager)
        {
            _inputManager = pInputManager;
        }
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent controlComponent = components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_CONTROLLABLE;
                    });
                    ComponentControllable control = (ComponentControllable)controlComponent;

                    if (control.IsControllable)
                        Control(entity);
                }
        }

        public void Control(Entity pEntity)
        {
            _inputManager.ReadInput(pEntity);
        }

        public void Cleanup(Entity pEntity)
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; }
    }
}