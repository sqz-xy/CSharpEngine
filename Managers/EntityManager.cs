using System.Collections.Generic;
using OpenGL_Game.Objects;
using System.Diagnostics;

namespace OpenGL_Game.Managers
{
    public class EntityManager
    {
        List<Entity> renderableEntityList;
        List<Entity> NonRenderableEntityList;

        public EntityManager()
        {
            renderableEntityList = new List<Entity>();
            NonRenderableEntityList = new List<Entity>();
        }

        public void AddEntity(Entity entity, bool pIsRenderable)
        {
            Entity result;
            if (pIsRenderable)
            {
                result = FindRenderableEntity(entity.Name);
                Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
                renderableEntityList.Add(entity);
                return;
            }
            result = FindNonRenderableEntity(entity.Name);
            Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
            NonRenderableEntityList.Add(entity);

        }

        public Entity FindRenderableEntity(string name)
        {
            return renderableEntityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        private Entity FindNonRenderableEntity(string name)
        {
            return NonRenderableEntityList.Find(delegate (Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public List<Entity> RenderableEntities()
        {
            return renderableEntityList;
        }

        public List<Entity> NonRenderableEntities()
        {
            return NonRenderableEntityList;
        }
    }
}
