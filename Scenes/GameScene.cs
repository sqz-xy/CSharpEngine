﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
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
        private ScriptManager scriptManager;

        public Camera camera;

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            scriptManager = new ScriptManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;

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

            // TODO: Add your initialization logic here

        }

        private void CreateEntities()
        {
            scriptManager.readJSONScript("Scripts/gameSceneScript.json", entityManager);
            
            /*
            Entity newEntity;

            newEntity = new Entity("Moon");
            newEntity.AddComponent(new ComponentPosition(-2.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Moon/moon.obj"));
            entityManager.AddEntity(newEntity);

            Entity wraithEntity;

            wraithEntity = new Entity("Wraith_Raider_Starship");
            wraithEntity.AddComponent(new ComponentPosition(2.0f, 0.0f, 0.0f));
            wraithEntity.AddComponent(new ComponentGeometry("Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            entityManager.AddEntity(wraithEntity);

            Entity intergalacticEntity;

            intergalacticEntity = new Entity("Intergalactic_Raider_Starship");
            intergalacticEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            intergalacticEntity.AddComponent(new ComponentGeometry("Geometry/Cat/cat.obj"));
            intergalacticEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 1.0f, 1.0f)));
            entityManager.AddEntity(intergalacticEntity);
*/


        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);

            ISystem physicsSystem;
            physicsSystem = new SystemPhysics();
            systemManager.AddSystem(physicsSystem);
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

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

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
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            ResourceManager.RemoveAllAssets();
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    camera.MoveForward(0.1f);
                    break;
                case Key.Down:
                    camera.MoveForward(-0.1f);
                    break;
                case Key.Left:
                    camera.RotateY(-0.01f);
                    break;
                case Key.Right:
                    camera.RotateY(0.01f);
                    break;
                case Key.M:
                    sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
                    break;
            }
        }
    }
}
