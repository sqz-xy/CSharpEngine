﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

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
        public Entity entity1;
        public Entity entity2;
        public COLLISIONTYPE collisionType;
    }

    public abstract class CollisionManager
    {
        protected List<Collision> _collisionManifold = new List<Collision>();
        
        public void ClearManifold() {_collisionManifold.Clear();}

        public void CollisionBetweenCamera(Entity pEntity, COLLISIONTYPE pCollisionType)
        {/*
            foreach (var coll in _collisionManifold)
                if (coll.entity == pEntity) 
                    return;

            Console.WriteLine("Collision");
            
            Collision collision;
            collision.entity = pEntity;
            collision.collisionType = pCollisionType;
            _collisionManifold.Add(collision); */
        }

        public void CollisionBetweenSpheres(Entity pEntity1, Entity pEntity2, COLLISIONTYPE pCollisionType)
        {
            foreach (var coll in _collisionManifold)
                if (coll.entity1 == pEntity1 && coll.entity2 == pEntity2)
                    return;

            Console.WriteLine("Collision");

            Collision collision;
            collision.entity1 = pEntity1;
            collision.entity2 = pEntity2;
            collision.collisionType = pCollisionType;
            _collisionManifold.Add(collision);
        }

        public abstract void ProcessCollisions();

    }
}