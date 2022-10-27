using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Scenes;
using OpenTK;

namespace OpenGL_Game.Managers
{
    // Reset binds when changing scene
    class InputManager
    {
        public Dictionary<string, Key> _keyBinds;
        public Dictionary<string, MouseButton> _mouseBinds;

        public InputManager()
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();
        }

        public void ReadInput(SceneManager pSceneManager, Camera pCamera)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyValuePair<string, Key> kvp in _keyBinds)
            {
               if (keyState.IsKeyDown(kvp.Value))
               {
                    HandleInput(kvp.Key, pSceneManager, pCamera);
               }
            }

            MouseState mouseState = Mouse.GetState();

            foreach (KeyValuePair<string, MouseButton> kvp in _mouseBinds)
            {
                if (mouseState.IsButtonDown(kvp.Value))
                {
                    HandleInput(kvp.Key, pSceneManager, pCamera);
                }
            }
        }

        public virtual void HandleInput(string pAction, SceneManager pSceneManager, Camera pCamera)
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
            }
        }

        public void InitializeBinds()
        {
            if (_keyBinds == null)
            {              
                _keyBinds.Add("MOVE_FORWARD", Key.W);
                _keyBinds.Add("MOVE_BACKWARD", Key.S);
                _keyBinds.Add("MOVE_LEFT", Key.A);
                _keyBinds.Add("MOVE_RIGHT", Key.D);
                _keyBinds.Add("GAME_OVER", Key.M);
                _keyBinds.Add("CLOSE_GAME", Key.Q);
            }

            if (_mouseBinds == null)
            {
                _mouseBinds.Add("START_GAME", MouseButton.Left);
            }
        }
    }
}
