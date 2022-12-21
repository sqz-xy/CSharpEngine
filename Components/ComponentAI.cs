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

        public float _cost;

        public AICostTypes _costType;

        public ComponentAI(AICostTypes pCostType)
        {
            _costType = 0;
            _costType = pCostType;
        }

        public float Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public AICostTypes CostType
        {
            get { return _costType; }
        }
    }
}