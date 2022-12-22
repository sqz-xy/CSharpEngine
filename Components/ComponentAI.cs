namespace OpenGL_Game.Components
{
    public enum AICostTypes
    {
        G_X,
        H_X
    }
    public class ComponentAI : IComponent
    {
        public ComponentTypes ComponentType { get; }

        private float _cost;
        private bool _isActive;
        private AICostTypes _costType;

        public ComponentAI(AICostTypes pCostType, bool pIsActive)
        {
            _cost = 0;
            _costType = pCostType;
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

        public AICostTypes CostType
        {
            get { return _costType; }
        }
    }
}