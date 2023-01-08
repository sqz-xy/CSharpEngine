using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Managers
{
    public class EntityManager
    {
        List<Entity> _renderableEntityList;
        List<Entity> _nonRenderableEntityList;

        public EntityManager()
        {
            _renderableEntityList = new List<Entity>();
            _nonRenderableEntityList = new List<Entity>();
        }

        /// <summary>
        /// Adds an entity to the entity manager list
        /// </summary>
        /// <param name="pEntity">The entity to add</param>
        /// <param name="pIsRenderable">Is the entity renderable</param>
        public void AddEntity(Entity pEntity, bool pIsRenderable)
        {
            Entity result;
            if (pIsRenderable)
            {
                result = FindRenderableEntity(pEntity.Name);
                Debug.Assert(result == null, "Entity '" + pEntity.Name + "' already exists");
                _renderableEntityList.Add(pEntity);
                return;
            }
            result = FindNonRenderableEntity(pEntity.Name);
            Debug.Assert(result == null, "Entity '" + pEntity.Name + "' already exists");
            _nonRenderableEntityList.Add(pEntity);

        }

        /// <summary>
        /// Finds a renderable entity
        /// </summary>
        /// <param name="pName">Name of the entity</param>
        /// <returns>The entity with the passed in name or null</returns>
        public Entity FindRenderableEntity(string pName)
        {
            return _renderableEntityList.Find(delegate(Entity pE)
            {
                return pE.Name == pName;
            }
            );
        }

        /// <summary>
        /// Finds a non renderable entity
        /// </summary>
        /// <param name="pName">Name of the entity</param>
        /// <returns>The entity with the passed in name or null</returns>
        public Entity FindNonRenderableEntity(string pName)
        {
            return _nonRenderableEntityList.Find(delegate (Entity pE)
            {
                return pE.Name == pName;
            }
            );
        }

        /// <summary>
        /// Deletes a renderable entity
        /// </summary>
        /// <param name="pName">Name of the entity</param>
        /// <returns>A boolean value to signify if the entity has been deleted</returns>
        public bool DeleteRenderableEntity(string pName)
        {
            var entityToDelete = _renderableEntityList.FirstOrDefault(pI => pI.Name == pName);
            _renderableEntityList.Remove(entityToDelete);
            
            if (entityToDelete == null)
                return false;
            
            _renderableEntityList.Remove(entityToDelete);
            return true;
        }

        /// <summary>
        /// Deletes a non renderable entity
        /// </summary>
        /// <param name="pName">Name of the entity</param>
        /// <returns>A boolean value to signify if the entity has been deleted</returns>
        public bool DeleteNonRenderableEntity(string pName)
        {
            var entityToDelete = _nonRenderableEntityList.FirstOrDefault(pI => pI.Name == pName);

            if (entityToDelete == null)
                return false;
            
            _renderableEntityList.Remove(entityToDelete);
            return true;
        }
        
        public List<Entity> RenderableEntities()
        {
            return _renderableEntityList;
        }

        public List<Entity> NonRenderableEntities()
        {
            return _nonRenderableEntityList;
        }
    }
}
