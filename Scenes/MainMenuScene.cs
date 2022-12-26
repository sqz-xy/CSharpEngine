using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Linq;
using OpenGL_Game.Components;
using OpenTK.Input;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Scenes
{
    class MainMenuScene : Scene
    {
        
        public Camera camera;
        
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.entityManager = new EntityManager();
            sceneManager.systemManager = new SystemManager();
            sceneManager.inputManager = new GameInputManager(sceneManager.entityManager, base.sceneManager);
       
            // Set the title of the window
            sceneManager.Title = "Main Menu";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            
            // Set Camera
            camera = new Camera();
            
            CreateEntities();
            CreateSystems();
            
            sceneManager.scriptManager.LoadControls("Scripts/mainMenuControls.json", ref sceneManager.inputManager);
            sceneManager.inputManager.InitializeBinds();
        }
        
        private void CreateEntities()
        {
            sceneManager.scriptManager.LoadEntities("Scripts/mainMenuEntityList.json", ref sceneManager.entityManager);
        }

        private void CreateSystems()
        {
            sceneManager.scriptManager.LoadSystems("Scripts/mainMenuSystemList.json", ref sceneManager.systemManager, ref sceneManager.collisionManager, ref sceneManager.entityManager, ref sceneManager.inputManager, ref camera);
        }

        public override void Update(FrameEventArgs e)
        {
            sceneManager.systemManager.ActionNonRenderableSystems(sceneManager.entityManager);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            sceneManager.systemManager.ActionRenderableSystems(sceneManager.entityManager);
            
            GUI.clearColour = Color.CornflowerBlue;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Image("Images/mainmenu.bmp", width, height);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Felinephobia!", (int)fontSize, StringAlignment.Center, Color.MidnightBlue);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) (((int)(fontSize * 2f)) + height * 1.5f)), "Press Space to Play!", (int)fontSize / 2, StringAlignment.Center, Color.MidnightBlue);
            GUI.Image("Images/droneicon.bmp", 30, 30, 900, 130);
            GUI.Render();
        }
        
        public override void Close()
        {
            sceneManager.systemManager.CleanupSystems(sceneManager.entityManager);
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