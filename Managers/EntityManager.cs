using System.Collections.Generic;
using OpenGL_Game.Objects;
using System.Diagnostics;
using System.Linq;

namespace OpenGL_Game.Managers
{
    public class EntityManager
    {
        List<Entity> renderableEntityList;
        List<Entity> nonRenderableEntityList;

        public EntityManager()
        {
            renderableEntityList = new List<Entity>();
            nonRenderableEntityList = new List<Entity>();
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
            nonRenderableEntityList.Add(entity);

        }

        public Entity FindRenderableEntity(string name)
        {
            return renderableEntityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public Entity FindNonRenderableEntity(string name)
        {
            return nonRenderableEntityList.Find(delegate (Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public void DeleteRenderableEntity(string name)
        {
            Entity entityToDelete = renderableEntityList.Where(i => i.Name == name).FirstOrDefault();
            renderableEntityList.Remove(entityToDelete);
        }

        public void DeleteNonRenderableEntity(string name)
        {
            Entity entityToDelete = nonRenderableEntityList.Where(i => i.Name == name).FirstOrDefault();
            renderableEntityList.Remove(entityToDelete);
        }

        public List<Entity> RenderableEntities()
        {
            return renderableEntityList;
        }

        public List<Entity> NonRenderableEntities()
        {
            return nonRenderableEntityList;
        }
    }
}
