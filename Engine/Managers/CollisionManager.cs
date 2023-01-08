using System;
using System.Collections.Generic;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Managers
{
    /// <summary>
    /// This enum specifies the types of collision
    /// </summary>
    public enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        SPHERE_AABB
    }
    
    /// <summary>
    /// This struct holds the details of a collision
    /// </summary>
    public struct Collision
    {
        public Entity entity1;
        public Entity entity2;
        public COLLISIONTYPE collisionType;
    }

    public abstract class CollisionManager
    {
        // List of collisions
        protected List<Collision> _collisionManifold = new List<Collision>();
        
        // Is collision active
        public bool IsActive { get; set; }
        
        protected CollisionManager()
        {
            IsActive = true;
        }

        /// <summary>
        /// Clears the collision manifold
        /// </summary>
        public void ClearManifold() {_collisionManifold.Clear();}
        
        /// <summary>
        /// Registers a collision, typically called by a collision system
        /// </summary>
        /// <param name="pEntity1">Collision entity 1</param>
        /// <param name="pEntity2">Collision entity 2</param>
        /// <param name="pCollisionType">Type of collision</param>
        public void RegisterCollision(Entity pEntity1, Entity pEntity2, COLLISIONTYPE pCollisionType)
        {
            foreach (var coll in _collisionManifold)
                if (coll.entity1 == pEntity1 && coll.entity2 == pEntity2)
                    return;
            
            Collision collision;
            collision.entity1 = pEntity1;
            collision.entity2 = pEntity2;
            collision.collisionType = pCollisionType;
            _collisionManifold.Add(collision);
        }
        
        /// <summary>
        /// A concrete collision manager will implement this method to process all collisions within the manifold
        /// </summary>
        public abstract void ProcessCollisions();
    }
}