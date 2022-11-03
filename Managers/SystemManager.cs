using System.Collections.Generic;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    class SystemManager
    {
        List<ISystem> renderableSystemList = new List<ISystem>();
        List<ISystem> nonRenderableSystemList = new List<ISystem>();
        public SystemManager()
        {
        }

        public void ActionRenderableSystems(EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.Entities();
            foreach(ISystem system in renderableSystemList)
            {
                foreach(Entity entity in entityList)
                {
                    system.OnAction(entity);
                }
            }
        }

        public void ActionNonRenderableSystems(EntityManager entityManager)
        {
            List<Entity> entityList = entityManager.Entities();
            foreach (ISystem system in nonRenderableSystemList)
            {
                foreach (Entity entity in entityList)
                {
                    system.OnAction(entity);
                }
            }
        }

        public void AddSystem(ISystem system, bool pIsRenderable)
        {
            //ISystem result = FindSystem(system.Name);
            //Debug.Assert(result != null, "System '" + system.Name + "' already exists");

            if (pIsRenderable)
                renderableSystemList.Add(system);
         
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
