using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Scenes;
using OpenTK;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    // Reset binds when changing scene
    public class GameInputManager : InputManager
    {
        public static string bulletName = "Bullet";
        private int bulletIndex = 0;

        public Dictionary<string, Key> _keyBinds;
        public Dictionary<string, MouseButton> _mouseBinds;

        public GameInputManager()
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();
        }

        public override void ReadInput(SceneManager pSceneManager, Camera pCamera, EntityManager pEntityManager)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyValuePair<string, Key> kvp in _keyBinds)
            {
                if (keyState.IsKeyDown(kvp.Value))
                {
                    HandleInput(kvp.Key, pSceneManager, pCamera, pEntityManager);
                }
            }

            MouseState mouseState = Mouse.GetState();

            foreach (KeyValuePair<string, MouseButton> kvp in _mouseBinds)
            {
                if (mouseState.IsButtonDown(kvp.Value))
                {
                    HandleInput(kvp.Key, pSceneManager, pCamera, pEntityManager);
                }
            }
        }

        public override void HandleInput(string pAction, SceneManager pSceneManager, Camera pCamera, EntityManager pEntityManager)
        {
           
            // Non camera dependant actions
            switch (pAction)
            {
                case "START_GAME":
                    pSceneManager.ChangeScene(SceneTypes.SCENE_GAME);
                    break;
                case "GAME_OVER":
                    pSceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
                    break;
                case "CLOSE_GAME":
                    pSceneManager.Close();
                    break;
            }

            if (pCamera == null)
                return;

            // camera dependant actions
            switch (pAction)
            {
                case "MOVE_FORWARD":
                    pCamera.MoveForward(0.1f);
                    break;
                case "MOVE_BACKWARD":
                    pCamera.MoveForward(-0.1f);
                    break;
                case "MOVE_LEFT":
                    pCamera.RotateY(-0.01f);
                    break;
                case "MOVE_RIGHT":
                    pCamera.RotateY(0.01f);
                    break;
                case "SHOOT":
                    Shoot(pEntityManager, pCamera, 6.0f);
                    break;
            }
        }

        public void Shoot(EntityManager pEntityManager, Camera pCamera, float pSpeed)
        {
            // Make a copy of the saved bullet
            Entity storedBullet = pEntityManager.FindRenderableEntity(bulletName);
            Entity newBullet = new Entity($"{bulletName}{bulletIndex}");

            foreach (var c in storedBullet.Components)        
                newBullet.AddComponent(c);
            
            newBullet.AddComponent(new ComponentPosition(pCamera.cameraPosition));
            newBullet.AddComponent(new ComponentVelocity(pCamera.cameraDirection * pSpeed));
            
            pEntityManager.AddEntity(newBullet, true);
            bulletIndex++;
        }

        public override void InitializeBinds()
        {
            if (_keyBinds.Count != 0 || _mouseBinds.Count != 0) return;
            
            /* Commented out for testing, default binds
            _keyBinds.Add("MOVE_FORWARD", Key.W);
            _keyBinds.Add("MOVE_BACKWARD", Key.S);
            _keyBinds.Add("MOVE_LEFT", Key.A);
            _keyBinds.Add("MOVE_RIGHT", Key.D);
            _keyBinds.Add("GAME_OVER", Key.M);
                
            _mouseBinds.Add("START_GAME", MouseButton.Left);
            */
        }

        public override void ClearBinds()
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();
        }
    }
}
