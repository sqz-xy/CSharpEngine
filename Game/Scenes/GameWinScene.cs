using System;
using System.Drawing;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Scenes;
using OpenGL_Game.Game.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Game.Scenes
{
    class GameWinScene : Scene
    {
        public GameWinScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.inputManager = new GameInputManager(sceneManager.entityManager, base.SceneManager);
            
            // Set the title of the window
            sceneManager.Title = "You Win!";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            
            sceneManager.scriptManager.LoadControls("Scripts/gameWinControls.json", ref sceneManager.inputManager);
            sceneManager.inputManager.InitializeBinds();
        }

        public override void Update(FrameEventArgs e)
        {
            
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, SceneManager.Width, SceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, SceneManager.Width, 0, SceneManager.Height, -1, 1);

            Gui.ClearColour = Color.CornflowerBlue;

            //Display the Title
            float width = SceneManager.Width, height = SceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            Gui.Image("Images/gamewin.bmp", width, height, 0);
            Gui.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "The Cats are dead,", (int)fontSize, StringAlignment.Center, Color.MidnightBlue, 0);
            Gui.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) ((int)(fontSize * 2f) + fontSize * 2.5f)), "You Win!", (int)fontSize, StringAlignment.Center, Color.MidnightBlue, 0);
            Gui.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) (((int)(fontSize * 2f)) + height * 1.5f)), "Press Space to Play again!", (int)fontSize / 2, StringAlignment.Center, Color.MidnightBlue, 0);
            
            Gui.RenderLayer(0);
        }
        
        public override void Close()
        {
            SceneManager.inputManager.ClearBinds();
            
            ResourceManager.RemoveAllAssets();
        }
    }
}