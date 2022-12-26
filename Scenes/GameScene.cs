using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Scenes
{
/// <summary>
/// This is the main type for your game
/// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        private int playerHealth = 30;
        private static int playerLives = 3;
        private int droneCount = 9999;

        //EntityManager entityManager;
        SystemManager systemManager;
        private GameInputManager inputManager;

        // Made static because there should only be one
        public Camera camera;
        public Camera specCamera;

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            inputManager = new GameInputManager(entityManager, base.sceneManager);

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            //sceneManager.keyboardDownDelegate += Keyboard_KeyDown;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            //.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(-5.0f, 0.5f, 3.0f), new Vector3(0, 0.5f, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            //camera = new Camera(new Vector3(0, 4, 7), new Vector3(0, 0, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            
            CreateEntities();
            CreateSystems();
            
            sceneManager.scriptManager.LoadControls("Scripts/GameControls.json", ref inputManager);
            inputManager.InitializeBinds();

            // TODO: Add your initialization logic here
        }

        private void CreateEntities()
        {
            sceneManager.scriptManager.LoadEntities("Scripts/gameSceneScript.json", ref entityManager);
        }

        private void CreateSystems() 
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem, true);

            ISystem physicsSystem;
            physicsSystem = new SystemPhysics();
            systemManager.AddSystem(physicsSystem, false);

            ISystem audioSystem;
            audioSystem = new SystemAudio();
            systemManager.AddSystem(audioSystem, false);

            ISystem collisionSphereSystem;
            collisionSphereSystem = new SystemCollisionSphereSphere(sceneManager.collisionManager);
            systemManager.AddSystem(collisionSphereSystem, false);
            
            ISystem healthSystem;
            healthSystem = new SystemHealth(entityManager);
            systemManager.AddSystem(healthSystem, false);

            ISystem collisionAABBSystem;
            collisionAABBSystem = new SystemCollisionSphereAABB(sceneManager.collisionManager);
            systemManager.AddSystem(collisionAABBSystem, false);

            ISystem systemInput;
            systemInput = new SystemInput(inputManager, camera);
            systemManager.AddSystem(systemInput, false);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            //System.Console.WriteLine("fps=" + (int)(1.0/dt));

            // NEW for Audio
            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);

            // Action ALL Non renderable systems
            systemManager.ActionNonRenderableSystems(entityManager);
            sceneManager.collisionManager.ProcessCollisions(camera);
            inputManager.ReadInput(null);

            ComponentHealth health = ComponentHelper.GetComponent<ComponentHealth>(entityManager.FindRenderableEntity("Player"), ComponentTypes.COMPONENT_HEALTH);
            playerHealth = health.Health;

            if (playerHealth <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
                playerLives--;
            }
            
            if (playerLives <= 0)
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            
            if (droneCount <= 0)
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_WIN);

            int tempDroneCount = 0;
            foreach (var entity in entityManager.RenderableEntities())
            {
                if (entity.Name.Contains("EnemyCat"))
                    tempDroneCount++;
            }
            droneCount = tempDroneCount;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL renderable systems
            systemManager.ActionRenderableSystems(entityManager);

            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), $"Health: {playerHealth}", 18, StringAlignment.Near, Color.White);
            GUI.Label(new Rectangle(150, 0, (int)width, (int)(fontSize * 2f)), $"Lives: {playerLives}", 18, StringAlignment.Near, Color.White);
            GUI.Label(new Rectangle(300, 0, (int)width, (int)(fontSize * 2f)), $"Drone Count: {droneCount}", 18, StringAlignment.Near, Color.White);
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            systemManager.CleanupSystems(entityManager);
            inputManager.ClearBinds();
            ResourceManager.RemoveAllAssets();
        }
    }
}
