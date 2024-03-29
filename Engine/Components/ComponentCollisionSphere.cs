﻿namespace OpenGL_Game.Engine.Components
{
    public class ComponentCollisionSphere : IComponent
    {
        // Radius of the collision sphere
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