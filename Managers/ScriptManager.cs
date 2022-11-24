using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    public abstract class ScriptManager
    {
        /// <summary>
        /// Reads a script to create entities and their components
        /// </summary>
        /// <param name="pScriptName">The name of the script</param>
        /// <param name="pEntityManager">The entity manager</param>
        public abstract void LoadEntities(string pScriptName, ref EntityManager pEntityManager);

        /// <summary>
        /// Converts component strings into component objects
        /// </summary>
        /// <param name="pComponentType">The type of component to create</param>
        /// <param name="pComponentValue">The values for the component</param>
        /// <returns>An IComponent object</returns>
        public abstract IComponent GetComponent(string pComponentType, string pComponentValue);

        /// <summary>
        /// Reads a script to load controls for a scene
        /// </summary>
        /// <param name="pScriptName">Name of the controls script</param>
        /// <param name="pInputManager">The game input manager</param>
        public abstract void LoadControls(string pScriptName, ref GameInputManager pInputManager);

        /// <summary>
        /// Adds the controls to the binding dictionary
        /// </summary>
        /// <param name="pAction">The action</param>
        /// <param name="pBind">The input to bind to</param>
        /// <param name="pInputManager">The game input manager</param>
        public abstract void GetControls(string pAction, string pBind, ref GameInputManager pInputManager);
    }
}