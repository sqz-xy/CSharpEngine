namespace OpenGL_Game.Engine.Components
{
    public class ComponentSpeed : IComponent
    {
        // Speed of an entity
        float speed;

        // Speed multiplier
        public ComponentSpeed(float pSpeed)
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