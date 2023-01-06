namespace OpenGL_Game.Engine.Components
{
    class ComponentControllable : IComponent
    {
        private bool _isControllable;

        public ComponentControllable(bool pIsControllable)
        {
            IsControllable = pIsControllable;
        }

        public bool IsControllable
        {
            get { return _isControllable; }
            set { _isControllable = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_CONTROLLABLE; }
        }
    }
}
