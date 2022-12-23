using OpenGL_Game.Objects;

namespace OpenGL_Game.Components
{
    public static class ComponentHelper
    {
        public static T GetComponent<T>(Entity pEntity, ComponentTypes pComponentType)
        {
            IComponent componentToGet = pEntity.Components.Find(delegate(IComponent component)
            {
                return component.ComponentType == pComponentType;
            });
            return (T) componentToGet;
        }
    }
}