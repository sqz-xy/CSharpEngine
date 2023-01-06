using OpenTK;

namespace OpenGL_Game.Engine.Components
{
    public class ComponentDirection : IComponent
    {
        Vector3 _direction;

        public ComponentDirection(float x, float y, float z)
        {
            _direction = new Vector3(x, y, z);
        }

        public ComponentDirection(Vector3 pDirection)
        {
            _direction = pDirection;
        }
       
        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_DIRECTION; }
        }
    }
}
