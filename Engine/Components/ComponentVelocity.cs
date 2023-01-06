using OpenTK;

namespace OpenGL_Game.Engine.Components
{
    class ComponentVelocity : IComponent
    {
        Vector3 _velocity;

        public ComponentVelocity(float x, float y, float z)
        {
            _velocity = new Vector3(x, y, z);
        }

        public ComponentVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }
       
        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
    }
}
