using System.Collections.Generic;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;
using System.Linq;

namespace OpenGL_Game.Managers
{
    class SystemManager
    {
        // Presrcibe entities to systems specifically 
        List<ISystem> renderableSystemList = new List<ISystem>();
        List<ISystem> nonRenderableSystemList = new List<ISystem>();
        public SystemManager()
        {
        }

        public void ActionRenderableSystems(EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.RenderableEntities();
            foreach (ISystem system in renderableSystemList)
                system.OnAction(entityList);
        }

        public void ActionNonRenderableSystems(EntityManager entityManager)
        {
            List<Entity> entityList = (List<Entity>)entityManager.NonRenderableEntities().Concat(entityManager.RenderableEntities()).ToList();
            foreach (ISystem system in nonRenderableSystemList)
                system.OnAction(entityList);
        }

        public void AddSystem(ISystem system, bool pIsRenderable)
        {
            //ISystem result = FindSystem(system.Name);
            //Debug.Assert(result != null, "System '" + system.Name + "' already exists");

            if (pIsRenderable)
            {
                renderableSystemList.Add(system);
                return;
            }
    
            nonRenderableSystemList.Add(system);
        }

        public void CleanupSystems(EntityManager entityManager)
        {
            foreach (var system in nonRenderableSystemList.Concat(renderableSystemList).ToList())
            {
                List<Entity> entityList = (List<Entity>)entityManager.NonRenderableEntities().Concat(entityManager.RenderableEntities()).ToList();
                foreach (Entity entity in entityList)
                {
                    system.Cleanup(entity);
                }
            }
        }

        private ISystem FindRenderableSystem(string name)
        {
            return renderableSystemList.Find(delegate(ISystem system)
            {
                return system.Name == name;
            }
            );
        }

        private ISystem FindNonRenderableSystem(string pName)
        {
            return nonRenderableSystemList.Find(delegate (ISystem system)
            {
                return system.Name == pName;
            }
            );
        }
    }
}
