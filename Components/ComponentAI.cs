namespace OpenGL_Game.Components
{
    public enum AINodeType
    {
        DESTINATION,
        NODE,
        DRONE
    }
    public class ComponentAI : IComponent
    {
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI; }
        }

        private float _cost;
        private bool _isActive;
        private AINodeType _nodeType;

        public ComponentAI(AINodeType pNodeType, bool pIsActive)
        {
            _cost = 0;
            _nodeType = pNodeType;
            _isActive = pIsActive;
        }
        
        public float Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public AINodeType NodeType
        {
            get { return _nodeType; }
        }
    }
}