using System;
using System.Collections.Generic;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    public enum COLLISIONTYPE
    {
        SPHERE_SPHERE
    }

    // Add second entity later
    public struct Collision
    {
        public Entity entity;
        public COLLISIONTYPE collisionType;
    }
    
    public abstract class CollisionManager
    {
        protected List<Collision> _collisionManifold = new List<Collision>();
        
        public void ClearManifold() {_collisionManifold.Clear();}

        public void CollisionBetweenCamera(Entity pEntity, COLLISIONTYPE pCollisionType)
        {
            foreach (var coll in _collisionManifold)
                if (coll.entity == pEntity) 
                    return;

            Console.WriteLine("Collision");
            
            Collision collision;
            collision.entity = pEntity;
            collision.collisionType = pCollisionType;
            _collisionManifold.Add(collision);
        }

        public void CollisionBetweenSpheres(Entity pEntity, COLLISIONTYPE pCollisionType)
        {
            foreach (var coll in _collisionManifold)
                if (coll.entity == pEntity)
                    return;

            Console.WriteLine("Collision");

            Collision collision;
            collision.entity = pEntity;
            collision.collisionType = pCollisionType;
            _collisionManifold.Add(collision);
        }

        public abstract void ProcessCollisions();

    }
}