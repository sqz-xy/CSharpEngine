using System;
using System.Drawing;
using System.Linq;
using OpenGL_Game.Engine;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Scenes;
using OpenGL_Game.Game.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Game.Scenes
{
    class MainMenuScene : Scene
    {
        
        public Camera camera;
        
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.entityManager = new EntityManager();
            sceneManager.systemManager = new SystemManager();
            sceneManager.inputManager = new GameInputManager(sceneManager.entityManager, base.SceneManager);
            // Set the title of the window
            sceneManager.Title = "Felinephobia!";
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
            SceneManager.scriptManager.LoadEntities("Scripts/mainMenuEntityList.json", ref SceneManager.entityManager);
        }

        private void CreateSystems()
        {
            SceneManager.scriptManager.LoadSystems("Scripts/mainMenuSystemList.json", ref SceneManager, ref camera);
        }

        public override void Update(FrameEventArgs e)
        {
            SceneManager.systemManager.ActionNonRenderableSystems(SceneManager.entityManager);
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, SceneManager.Width, SceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, SceneManager.Width, 0, SceneManager.Height, -1, 1);

            SceneManager.systemManager.ActionRenderableSystems(SceneManager.entityManager);
            
            Gui.ClearColour = Color.CornflowerBlue;

            //Display the Title
            float width = SceneManager.Width, height = SceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            Gui.Image("Images/mainmenu.bmp", width, height, 0);
            Gui.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Felinephobia!", (int)fontSize, StringAlignment.Center, Color.MidnightBlue, 0);
            Gui.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), $"FPS: {Math.Round(1 / e.Time)}", 18, StringAlignment.Near, Color.White, 0);
            Gui.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) (((int)(fontSize * 2f)) + height * 1.5f)), "Press Space to Play!", (int)fontSize / 2, StringAlignment.Center, Color.MidnightBlue, 0);
            Gui.Image("Images/droneicon2.bmp", 30, 30, 900, 130, 0);
            Gui.RenderLayer(0);
        }
        
        public override void Close()
        {
            SceneManager.inputManager.ClearBinds();

            foreach (var entity in SceneManager.entityManager.RenderableEntities().Concat(SceneManager.entityManager.NonRenderableEntities()))
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