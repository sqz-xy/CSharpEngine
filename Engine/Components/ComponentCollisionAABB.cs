namespace OpenGL_Game.Engine.Components
{
    public class ComponentCollisionAABB : IComponent
    {
        private float _width;
        private float _height;
        private float _depth;

        public ComponentCollisionAABB(float pWidth, float pHeight, float pDepth)
        {
            _width = pWidth;
            _height = pHeight;
            _depth = pDepth;
        }
        
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }
        
        public float Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_AABB; }
        }
    }
}