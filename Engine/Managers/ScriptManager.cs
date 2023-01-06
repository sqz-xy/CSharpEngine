namespace OpenGL_Game.Engine.Managers
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
        /// Reads a script to create systems for a scene
        /// </summary>
        /// <param name="pScriptName">Name of the script to read</param>
        /// <param name="pSystemManager">The system manager</param>
        public abstract void LoadSystems(string pScriptName, ref SceneManager pSceneManager, ref Camera pCamera1);
        
        /// <summary>
        /// Reads a script to load controls for a scene
        /// </summary>
        /// <param name="pScriptName">Name of the controls script</param>
        /// <param name="pInputManager">The game input manager</param>
        public abstract void LoadControls(string pScriptName, ref InputManager inputManager);

        /// <summary>
        /// Reads a file to load any extra scene data
        /// </summary>
        /// <param name="pFileName">Name of data script</param>
        /// <param name="pScene">Scene to add data to</param>
        public abstract void LoadData(string pFileName, string pDataName, out string pDataValue);

        /// <summary>
        /// Saves data to a specific file
        /// </summary>
        /// <param name="pFileName">Name of the data script</param>
        /// <param name="pScene">Scene to save data from</param>
        public abstract void SaveData(string pFileName, string pDataName, string pDataValue);

    }
}