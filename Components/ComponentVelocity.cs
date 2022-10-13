using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentVelocity : IComponent
    {
        Vector3 _velocity;
        Vector3 _oldPosition;
        float _dt;

        public ComponentVelocity(float x, float y, float z, float a, float b, float c, float n)
        {
            _oldPosition = new Vector3(x, y, z);
            _velocity = new Vector3(a, b, c);
            _dt = n;
        }

        public ComponentVelocity(Vector3 pos, Vector3 velocity, float n)
        {
            _oldPosition = pos;
            _velocity = velocity;
            _dt = n;
        }

        public Vector3 OldPosition
        {
            get { return _oldPosition; }
            set { _oldPosition = value; }
        }
        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public float DeltaTime
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_VELOCITY; }
        }
    }
}
