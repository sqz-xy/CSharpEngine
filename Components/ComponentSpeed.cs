namespace OpenGL_Game.Components
{
    public class ComponentSpeed
    {
        float speed;

        public ComponentSpeed(int pSpeed)
        {
            this.speed = pSpeed;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SPEED; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
    }
}