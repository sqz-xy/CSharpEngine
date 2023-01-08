using System.Collections.Generic;
using System.Linq;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Systems;

namespace OpenGL_Game.Engine.Managers
{
    public class SystemManager
    {
        List<ISystem> _renderableSystemList = new List<ISystem>();
        List<ISystem> _nonRenderableSystemList = new List<ISystem>();
        public SystemManager()
        {
        }

        /// <summary>
        /// Actions all renderable systems
        /// </summary>
        /// <param name="pEntityManager">The entity manager</param>
        public void ActionRenderableSystems(EntityManager pEntityManager)
        {
            var entityList = pEntityManager.RenderableEntities();
            foreach (var system in _renderableSystemList)
                system.OnAction(entityList);
        }

        /// <summary>
        /// Actions all non renderable systems
        /// </summary>
        /// <param name="pEntityManager">The entity manager</param>
        public void ActionNonRenderableSystems(EntityManager pEntityManager)
        {
            var entityList = (List<Entity>)pEntityManager.NonRenderableEntities().Concat(pEntityManager.RenderableEntities()).ToList();
            foreach (var system in _nonRenderableSystemList)
                system.OnAction(entityList);
        }

        /// <summary>
        /// Adds a system to either the renderable or non renderable list
        /// </summary>
        /// <param name="pSystem">The system to add</param>
        /// <param name="pIsRenderable">Is the system renderable</param>
        public void AddSystem(ISystem pSystem, bool pIsRenderable)
        {
            //ISystem result = FindSystem(system.Name);
            //Debug.Assert(result != null, "System '" + system.Name + "' already exists");

            if (pIsRenderable)
            {
                _renderableSystemList.Add(pSystem);
                return;
            }
    
            _nonRenderableSystemList.Add(pSystem);
        }
        
        /// <summary>
        /// Finds a renderable system by name
        /// </summary>
        /// <param name="pName">Name of the system</param>
        /// <returns>The system or null</returns>
        private ISystem FindRenderableSystem(string pName)
        {
            return _renderableSystemList.Find(delegate(ISystem pSystem)
            {
                return pSystem.Name == pName;
            }
            );
        }

        /// <summary>
        /// Finds a non renderable system by name
        /// </summary>
        /// <param name="pName">Name of the system</param>
        /// <returns>The system or null</returns>
        private ISystem FindNonRenderableSystem(string pName)
        {
            return _nonRenderableSystemList.Find(delegate (ISystem pSystem)
            {
                return pSystem.Name == pName;
            }
            );
        }
    }
}
