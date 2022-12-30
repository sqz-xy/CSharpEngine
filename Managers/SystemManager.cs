using System.Collections.Generic;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;
using System.Linq;

namespace OpenGL_Game.Managers
{
    public class SystemManager
    {
        // Presrcibe entities to systems specifically 
        List<ISystem> renderableSystemList = new List<ISystem>();
        List<ISystem> nonRenderableSystemList = new List<ISystem>();
        public SystemManager()
        {
        }

        public void ActionRenderableSystems(EntityManager entityManager)
        {
            var entityList = entityManager.RenderableEntities();
            foreach (var system in renderableSystemList)
                system.OnAction(entityList);
        }

        public void ActionNonRenderableSystems(EntityManager entityManager)
        {
            var entityList = (List<Entity>)entityManager.NonRenderableEntities().Concat(entityManager.RenderableEntities()).ToList();
            foreach (var system in nonRenderableSystemList)
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
