namespace OpenGL_Game.Components
{
    public class ComponentCollisionSphere : IComponent
    {
        
        float _collisionField;

        public ComponentCollisionSphere(float pCollisionField)
        {
            _collisionField = pCollisionField;
        }
        

        public float CollisionField
        {
            get { return _collisionField; }
            set { _collisionField = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SPHERE; }
        }
    }
}