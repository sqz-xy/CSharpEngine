﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.Collections.Generic;
using OpenTK.Audio;

namespace OpenGL_Game.Managers
{
    public class SceneManager : GameWindow
    {
        public Scene scene;
        
        public static int width = 1200, height = 800;
        public static int windowXPos = 200, windowYPos = 80;
        
        public ScriptManager scriptManager;
        public CollisionManager collisionManager;
        public InputManager inputManager;
        public SystemManager systemManager;
        public EntityManager entityManager;

        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public delegate void KeyboardDelegate(KeyboardKeyEventArgs e);
        public KeyboardDelegate keyboardDownDelegate;
#pragma warning disable CS0649 // Field 'SceneManager.keyboardUpDelegate' is never assigned to, and will always have its default value null
        public KeyboardDelegate keyboardUpDelegate;
#pragma warning restore CS0649 // Field 'SceneManager.keyboardUpDelegate' is never assigned to, and will always have its default value null

        public delegate void MouseDelegate(MouseButtonEventArgs e);
        public MouseDelegate mouseDelegate;

        AudioContext audioContext;


        public SceneManager() : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 16))
        {
            this.X = windowXPos;
            this.Y = windowYPos;
            
            scriptManager = new GameScriptManager();
            collisionManager = new GameCollisionManager();
            
            audioContext = new AudioContext();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            //Load the GUI
            GUI.SetUpGUI(width, height);

            ChangeScene(SceneTypes.SCENE_MAIN_MENU);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
            collisionManager.ProcessCollisions();
            
            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Load the GUI
            GUI.SetUpGUI(Width, Height);
        }

        public void ChangeScene(SceneTypes pSceneType)
        {
            // I PUT THIS HERE GUHHHH
            if (scene != null)
                scene.Close();

            try
            {
                switch (pSceneType)
                {
                    case SceneTypes.SCENE_MAIN_MENU:
                        scene = new MainMenuScene(this);
                        break;
                    case SceneTypes.SCENE_GAME:
                        scene = new GameScene(this);
                        break;
                    case SceneTypes.SCENE_GAME_OVER:
                        scene = new GameOverScene(this);
                        break;
                    case SceneTypes.SCENE_GAME_WIN:
                        scene = new GameWinScene(this);
                        break;
                    default:
                        scene = new MainMenuScene(this);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                scene = new MainMenuScene(this);
            }
            
        }

        public static void ChangeScene(SceneTypes pSceneType, SceneManager pSceneManager)
        {
            if (pSceneManager.scene != null)
                pSceneManager.scene.Close();

            switch (pSceneType)
            {
                case SceneTypes.SCENE_MAIN_MENU:
                    pSceneManager.scene = new MainMenuScene(pSceneManager);
                    break;
                case SceneTypes.SCENE_GAME:
                    pSceneManager.scene = new GameScene(pSceneManager);
                    break;
                case SceneTypes.SCENE_GAME_OVER:
                    pSceneManager.scene = new GameOverScene(pSceneManager);
                    break;
                case SceneTypes.SCENE_GAME_WIN:
                    pSceneManager.scene = new GameWinScene(pSceneManager);
                    break;
                default:
                    pSceneManager.scene = new MainMenuScene(pSceneManager);
                    break;
            }
        }
    }
}

