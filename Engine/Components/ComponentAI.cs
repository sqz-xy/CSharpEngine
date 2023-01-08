namespace OpenGL_Game.Engine.Components
{
    public abstract class ComponentAI : IComponent
    {
        // Is the entity being moved, starts off
        protected bool _isMoving = false;
        
        // Allows the AI to be toggled on a per entity basis
        protected bool _isActive = true;
        public ComponentTypes ComponentType { get { return ComponentTypes.COMPONENT_AI; } }
    }
}