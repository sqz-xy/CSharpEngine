namespace OpenGL_Game.Components
{
    public class ComponentAI : IComponent
    {
        public ComponentTypes ComponentType { get; }

        public float _gCost;

        public ComponentAI()
        {
            _gCost = 0;
        }

        public float gCost
        {
            get { return _gCost; }
            set { _gCost = value; }
        }
    }
}