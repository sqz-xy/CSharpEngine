using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class GameWinScene : Scene
    {
        public GameWinScene(SceneManager sceneManager) : base(sceneManager)
        {
            sceneManager.inputManager = new GameInputManager(sceneManager.entityManager, base.sceneManager);
            
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
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.CornflowerBlue;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Image("Images/gamewin.bmp", width, height, 0);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "The Cats are dead,", (int)fontSize, StringAlignment.Center, Color.MidnightBlue, 0);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) ((int)(fontSize * 2f) + fontSize * 2.5f)), "You Win!", (int)fontSize, StringAlignment.Center, Color.MidnightBlue, 0);
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int) (((int)(fontSize * 2f)) + height * 1.5f)), "Press Space to Play again!", (int)fontSize / 2, StringAlignment.Center, Color.MidnightBlue, 0);
            
            GUI.RenderLayer(0);
        }
        
        public override void Close()
        {
            sceneManager.inputManager.ClearBinds();
            
            ResourceManager.RemoveAllAssets();
        }
    }
}