namespace OpenGL_Game.Engine.Components
{
    public abstract class ComponentAI : IComponent
    {
        protected bool _isMoving = false;
        protected bool _isActive = true;
        public ComponentTypes ComponentType { get { return ComponentTypes.COMPONENT_AI; } }
    }
}