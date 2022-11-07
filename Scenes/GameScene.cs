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
        EntityManager entityManager;
        SystemManager systemManager;
        ScriptManager scriptManager;

        // Made static because there should only be one
        public Camera camera;

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();

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
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 4, 7), new Vector3(0, 0, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);

            CreateEntities();
            CreateSystems();
            
            sceneManager.scriptManager.LoadControls("Scripts/GameControls.json", ref sceneManager.inputManager);
            sceneManager.inputManager.InitializeBinds();

            // TODO: Add your initialization logic here

        }

        private void CreateEntities()
        {
            sceneManager.scriptManager.LoadEntities("Scripts/gameSceneScript.json", ref entityManager);

            Entity newEntity;

            newEntity = new Entity("Moon2");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/moon.obj"));
            newEntity.AddComponent(new ComponentAudio("Audio/buzz.wav"));
            newEntity.AddComponent(new ComponentShaderDefault());
            newEntity.AddComponent(new ComponentVelocity(0.3f, 0f, 0.5f));
            //entityManager.AddEntity(newEntity, true);
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

            sceneManager.inputManager.ReadInput(sceneManager, camera);
            
            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            // TODO: Add your update logic here
            

            
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
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "Score: 000", 18, StringAlignment.Near, Color.White);
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            systemManager.CleanupSystems(entityManager);
            //sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            //ResourceManager.RemoveAllAssets();
        }
    }
}
