using OpenGL_Game.Objects;

namespace OpenGL_Game.Managers
{
    public abstract class InputManager
    {
        protected SceneManager _sceneManager;
        protected EntityManager _entityManager;
        
        public InputManager(EntityManager pEntityManager, SceneManager pSceneManager)
        {
            _sceneManager = pSceneManager;
            _entityManager = pEntityManager;
        }
        
        /// <summary>
        /// Reads input from the user and handles response if the response is bound, should be called during an update loop
        /// </summary>
        /// <param name="pSceneManager">The scene manager</param>
        /// <param name="pCamera">The camera</param>
        public abstract void ReadInput(Entity pEntity);

        /// <summary>
        /// Handles user input by checking the bound action, controls entities
        /// </summary>
        /// <param name="pAction">The action of the keybind</param>
        /// <param name="pSceneManager">the scene manager</param>
        /// <param name="pCamera">the camera</param>
        public abstract void HandleEntityInput(string pAction, Entity entity);
        
        /// <summary>
        /// Handles user input by checking the bound action, controls the scene
        /// </summary>
        /// <param name="pAction"></param>
        public abstract void HandleSceneInput(string pAction);

        /// <summary>
        /// Should be called on scene initialization, initialization logic for binds, i.e. assigning default binds
        /// </summary>
        public abstract void InitializeBinds();

        /// <summary>
        /// Clears all saved binds
        /// </summary>
        public abstract void ClearBinds();
    }
}