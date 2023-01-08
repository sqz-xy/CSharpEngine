using OpenTK;

namespace OpenGL_Game.Engine.Components
{
    public class ComponentPosition : IComponent
    {
        // Position of an entity
        Vector3 position;

        public ComponentPosition(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
        }

        public ComponentPosition(Vector3 pos)
        {
            position = pos;
        }
        
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_POSITION; }
        }
    }
}
