using OpenGL_Game.Components;
using OpenGL_Game.Scenes;

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
        /// Reads a script to load controls for a scene
        /// </summary>
        /// <param name="pScriptName">Name of the controls script</param>
        /// <param name="pInputManager">The game input manager</param>
        public abstract void LoadControls(string pScriptName, ref GameInputManager pInputManager);

        /// <summary>
        /// Reads a file to load any extra scene data
        /// </summary>
        /// <param name="pFileName">Name of data script</param>
        /// <param name="pScene">Scene to add data to</param>
        public abstract void LoadData(string pFileName, Scene pScene);

        /// <summary>
        /// Saves data to a specific file
        /// </summary>
        /// <param name="pFileName">Name of the data script</param>
        /// <param name="pScene">Scene to save data from</param>
        public abstract void SaveData(string pFileName, Scene pScene);

    }
}