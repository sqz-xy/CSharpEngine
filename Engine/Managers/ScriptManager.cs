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
        /// Reads in a script of systems for a scene
        /// </summary>
        /// <param name="pScriptName">Script file name</param>
        /// <param name="pSceneManager">The scene manager</param>
        /// <param name="pCamera">The camera</param>
        public abstract void LoadSystems(string pScriptName, ref SceneManager pSceneManager, ref Camera pCamera);
        
        /// <summary>
        /// Reads a script to load controls for a scene
        /// </summary>
        /// <param name="pScriptName">Name of the controls script</param>
        /// <param name="pInputManager">The game input manager</param>
        public abstract void LoadControls(string pScriptName, ref InputManager pInputManager);

        /// <summary>
        /// Loads in custom game data
        /// </summary>
        /// <param name="pFileName">name of data file</param>
        /// <param name="pDataName">name of the data store</param>
        /// <param name="pDataValue">the value stored at the data name</param>
        public abstract void LoadData(string pFileName, string pDataName, out string pDataValue);

        /// <summary>
        /// Saves data to a specific file
        /// </summary>
        /// <param name="pFileName">name of the data file</param>
        /// <param name="pDataName">name of the data store</param>
        /// <param name="pDataValue">the value stored at the data name</param>
        public abstract void SaveData(string pFileName, string pDataName, string pDataValue);

    }
}