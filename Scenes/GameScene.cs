using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using System.Linq;
using System.Numerics;
using OpenTK.Audio.OpenAL;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace OpenGL_Game.Scenes
{
/// <summary>
/// This is the main type for your game
/// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        public int playerLives = 3;
        public int maxLives = 3;
        public int playerHealth = 30;
        public int droneCount = 3;

        // Made static because there should only be one
        public Camera camera;

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            sceneManager.entityManager = new EntityManager();
            sceneManager.systemManager = new SystemManager();
            sceneManager.inputManager = new GameInputManager(sceneManager.entityManager, base.sceneManager);
            
            // Set the title of the window
            sceneManager.Title = "Game";
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            
            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(-5.0f, 0.5f, 3.0f), new Vector3(-5.0f, 0.5f, 3.0f), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);

            CreateEntities();
            CreateSystems();
            
            sceneManager.scriptManager.LoadControls("Scripts/gameControls.json", ref sceneManager.inputManager);
            sceneManager.scriptManager.LoadData("Scripts/gameData.json",  "Lives", out var livesString);
            playerLives = int.Parse(livesString);
            
            sceneManager.inputManager.InitializeBinds();

            // TODO: Add your initialization logic here
        }

        private void CreateEntities()
        {
            sceneManager.scriptManager.LoadEntities("Scripts/gameEntityList.json", ref sceneManager.entityManager);
        }

        private void CreateSystems()
        {
            sceneManager.scriptManager.LoadSystems("Scripts/gameSystemList.json", ref sceneManager.systemManager, ref sceneManager.collisionManager, ref sceneManager.entityManager, ref sceneManager.inputManager, ref camera);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            
            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);
            
            ComponentHealth health = ComponentHelper.GetComponent<ComponentHealth>(sceneManager.entityManager.FindRenderableEntity("Player"), ComponentTypes.COMPONENT_HEALTH);
            playerHealth = health.Health;

            // Action ALL Non renderable systems
            sceneManager.systemManager.ActionNonRenderableSystems(sceneManager.entityManager);
            
            if (playerHealth <= 0)
            {
                playerLives--;
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
            }
            
            if (playerLives <= 0)
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            
            if (droneCount <= 0)
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_WIN);

            int tempDroneCount = 0;
            foreach (var entity in sceneManager.entityManager.RenderableEntities())
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
            sceneManager.systemManager.ActionRenderableSystems(sceneManager.entityManager);
            var collisionManager = (GameCollisionManager) sceneManager.collisionManager;
            
            // Render all the labels
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(40, 0, (int)width, (int)(fontSize * 2f)), $"Health: {playerHealth}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(220, 0, (int)width, (int)(fontSize * 2f)), $"Lives: {playerLives}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(360, 0, (int)width, (int)(fontSize * 2f)), $"Drone Count: {droneCount}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(560, 0, (int)width, (int)(fontSize * 2f)), $"Wall Collision Active: {collisionManager._wallCollision}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(560, 32, (int)width, (int)(fontSize * 2f)), $"AI Active: N/A", 18, StringAlignment.Near, Color.White, 0);
            GUI.Image("Images/droneicon.bmp", 32, 32, 330, 0, 0);
            GUI.Image("Images/hearticon.bmp", 32, 32, 190, 0, 0);
            GUI.Image("Images/healthicon.bmp", 32, 32, 10, 0, 0);
            GUI.Image("Images/minimap.bmp", 256, 256, 900, 0, 0);
            
            // Minimap logic
            
            var playerEntity = sceneManager.entityManager.FindRenderableEntity("Player");
            var pos = ComponentHelper.GetComponent<ComponentPosition>(playerEntity, ComponentTypes.COMPONENT_POSITION).Position;
            var angle = CalculateAngle(playerEntity);
            
            // Offset for image location and player speed
            GUI.Image("Images/playericon.bmp", 32, 32, (int)(pos.X * 12.5f) + 1000, (int)(pos.Z * 12.5f) + 100, 0, (int)MathHelper.RadiansToDegrees(angle));

            // Draw drones and powerups, powerups don't have a direction so no angle is needed
            foreach (var entity in sceneManager.entityManager.RenderableEntities())
            {
                if (entity.Name.Contains("FishPowerUp"))
                {
                    var powerUpPosition = ComponentHelper.GetComponent<ComponentPosition>(sceneManager.entityManager.FindRenderableEntity(entity.Name), ComponentTypes.COMPONENT_POSITION);
                    pos = powerUpPosition.Position;
                    GUI.Image("Images/fishicon.bmp", 32, 32, (int)(pos.X * 12.5f) + 1010, (int)(pos.Z * 12.5f) + 110, 0);
                }
                
                if (entity.Name.Contains("EnemyCat"))
                {
                    var powerUpPosition = ComponentHelper.GetComponent<ComponentPosition>(sceneManager.entityManager.FindRenderableEntity(entity.Name), ComponentTypes.COMPONENT_POSITION);
                    pos = powerUpPosition.Position;
                    angle = CalculateAngle(entity);
                    GUI.Image("Images/droneicon.bmp", 32, 32, (int)(pos.X * 12.5f) + 1010, (int)(pos.Z * 12.5f) + 110, 0, (int)MathHelper.RadiansToDegrees(angle));
                }
            }
            GUI.RenderLayer(0);
        }

        /// <summary>
        /// Calculates the angle of the player relative to north on the minimap
        /// </summary>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        private float CalculateAngle(Entity pEntity)
        {
            var sourceDir = new Vector3(0.0f, 0, -1f); // North
            var entityDir = ComponentHelper.GetComponent<ComponentDirection>(pEntity, ComponentTypes.COMPONENT_DIRECTION);
            var angle = Vector3.CalculateAngle(sourceDir, entityDir.Direction);
            
            // If the player is looking left, flip the angle value
            if (entityDir.Direction.X < 0)
                angle = -angle;

            return angle;
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.scriptManager.SaveData("Scripts/gameData.json", "Lives", playerLives.ToString());
            sceneManager.inputManager.ClearBinds();

            foreach (var entity in sceneManager.entityManager.RenderableEntities().Concat(sceneManager.entityManager.NonRenderableEntities()))
            {
                var audioComponents = ComponentHelper.GetComponents<ComponentAudio>(entity);
                foreach (var audioComponent in audioComponents)
                {
                    audioComponent.StopAudio();
                }
            }
            
            ResourceManager.RemoveAllAssets();
        }
    }
}
