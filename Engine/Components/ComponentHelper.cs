using System.Collections.Generic;
using System.Linq;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Components
{
    public static class ComponentHelper
    {
        /// <summary>
        /// Gets the first component of a specific type, used if only one component of a certain type exists or the first one specifically is needed
        /// </summary>
        /// <param name="pEntity">The entity to extract components from</param>
        /// <param name="pComponentType">The type of component</param>
        /// <typeparam name="T">The type of component</typeparam>
        /// <returns>A component of the specified type</returns>
        public static T GetComponent<T>(Entity pEntity, ComponentTypes pComponentType)
        {
            return (T) pEntity.Components.Find(component => component.ComponentType == pComponentType);
        }

        /// <summary>
        /// Returns all components of a specific type
        /// </summary>
        /// <param name="pEntity">The entity to extract components from</param>
        /// <typeparam name="T">The type of component</typeparam>
        /// <returns>A list of components of the specified type</returns>
        public static List<T> GetComponents<T>(Entity pEntity)
        {
            return pEntity.Components.OfType<T>().ToList();
        }
    }
}