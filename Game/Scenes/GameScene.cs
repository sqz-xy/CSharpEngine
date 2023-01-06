using System;
using System.Drawing;
using System.Linq;
using OpenGL_Game.Engine;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Scenes;
using OpenGL_Game.Game.Components;
using OpenGL_Game.Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using Vector3 = OpenTK.Vector3;

namespace OpenGL_Game.Game.Scenes
{
/// <summary>
/// This is the main type for your game
/// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        private int playerLives = 999;
        private int playerHealth = 999;
        private int droneCount = 999;
        private int powerUpCount = 999;
        
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
            sceneManager.scriptManager.LoadSystems("Scripts/gameSystemList.json", ref sceneManager ,ref camera);
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

            int tempEntityCount = 0;
            foreach (var entity in sceneManager.entityManager.RenderableEntities())
            {
                if (entity.Name.Contains("EnemyCat"))
                    tempEntityCount++;
            }
            droneCount = tempEntityCount;
            
            tempEntityCount = 0;
            foreach (var entity in sceneManager.entityManager.RenderableEntities())
            {
                if (entity.Name.Contains("PowerUp"))
                    tempEntityCount++;
            }
            powerUpCount = tempEntityCount;
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

            // GUI
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            
            // Health label 
            GUI.Label(new Rectangle(20, 0, (int)width, (int)(fontSize * 2f)), $"Health: {playerHealth}", 18, StringAlignment.Near, Color.White, 0);
            
            // Lives label and icons
            GUI.Label(new Rectangle(20, 32, (int)width, (int)(fontSize * 2f)), $"Lives: {playerLives}", 18, StringAlignment.Near, Color.White, 0);
            int livesIconOffset = 0;
            for (int i = 0; i < playerLives; i++)
            {
                GUI.Image("Images/hearticon.bmp", 32, 32, 150 + livesIconOffset, 32, 0);
                livesIconOffset += 32;
            }

            // Drone and power up count label, icons are drawn later with the minimap icons for efficiency
            GUI.Label(new Rectangle(20, 64, (int)width, (int)(fontSize * 2f)), $"Drones: {droneCount}", 18, StringAlignment.Near, Color.White, 0);

            // Speed and damage labels
            var playerSpeed = ComponentHelper.GetComponent<ComponentSpeed>(sceneManager.entityManager.FindRenderableEntity("Player"), ComponentTypes.COMPONENT_SPEED);
            var playerDamage = ComponentHelper.GetComponent<ComponentDamage>(sceneManager.entityManager.FindRenderableEntity("Player"), ComponentTypes.COMPONENT_DAMAGE);
            var playerPosition = ComponentHelper.GetComponent<ComponentPosition>(sceneManager.entityManager.FindRenderableEntity("Player"), ComponentTypes.COMPONENT_POSITION);
            GUI.Label(new Rectangle(20, 128, (int)width, (int)(fontSize * 2f)), $"Speed: {playerSpeed.Speed}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(20, 160, (int)width, (int)(fontSize * 2f)), $"Damage: {playerDamage.Damage}", 18, StringAlignment.Near, Color.White, 0);
            
            // Debugging labels
            GUI.Label(new Rectangle(560, 0, (int)width, (int)(fontSize * 2f)), $"Wall Collision Active: {sceneManager.collisionManager.IsActive}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(560, 32, (int)width, (int)(fontSize * 2f)), $"AI Active: {sceneManager.aiManager.IsActive}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(560, 64, (int)width, (int)(fontSize * 2f)), $"FPS: {Math.Round(1 / e.Time)}", 18, StringAlignment.Near, Color.White, 0);
            GUI.Label(new Rectangle(20, 750, (int)width, (int)(fontSize * 2f)), $"Position: {playerPosition.Position}", 18, StringAlignment.Near, Color.White, 0);
            
            // Minimap logic
            GUI.Image("Images/minimap.bmp", 256, 256, 900, 0, 0);
            var angle = CalculateAngle(sceneManager.entityManager.FindRenderableEntity("Player"));
            
            // Offset for image location and player speed
            GUI.Image("Images/playericon.bmp", 32, 32, (int)(playerPosition.Position.X * 12.5f) + 1000, (int)(playerPosition.Position.Z * 12.5f) + 100, 0, (int)MathHelper.RadiansToDegrees(angle));

            // Draw drones and powerups, powerups don't have a direction so no angle is needed
            int enemyIconOffset = 0;
            foreach (var entity in sceneManager.entityManager.RenderableEntities())
            {
                if (entity.Name.Contains("FishPowerUp"))
                {
                    var powerUpPosition = ComponentHelper.GetComponent<ComponentPosition>(sceneManager.entityManager.FindRenderableEntity(entity.Name), ComponentTypes.COMPONENT_POSITION);
                    GUI.Image("Images/fishicon.bmp", 32, 32, (int)(powerUpPosition.Position.X * 12.5f) + 1010, (int)(powerUpPosition.Position.Z * 12.5f) + 110, 0);
                }
                
                if (entity.Name.Contains("EnemyCat"))
                {
                    var dronePosition = ComponentHelper.GetComponent<ComponentPosition>(sceneManager.entityManager.FindRenderableEntity(entity.Name), ComponentTypes.COMPONENT_POSITION);
                    var droneNum = entity.Name.Split(' ')[1];
                    
                    angle = CalculateAngle(entity);
                    GUI.Image($"Images/droneicon{droneNum}.bmp", 32, 32, (int)(dronePosition.Position.X * 12.5f) + 1010, (int)(dronePosition.Position.Z * 12.5f) + 110, 0, (int)MathHelper.RadiansToDegrees(angle));
                    GUI.Image($"Images/droneicon{droneNum}.bmp", 32, 32, 150 + enemyIconOffset, 64, 0);
                    enemyIconOffset += 32;
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
