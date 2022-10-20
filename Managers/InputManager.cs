using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Managers
{
    class InputManager
    {
        Camera _camera;
        Dictionary<string, Key> _keyBinds;
        Dictionary<string, MouseButton> _mouseBinds;

        // Create enum for control names, allow it to be extended

        public InputManager(ref Camera pCamera)
        {
            _camera = pCamera;
        }

        public void ReadInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyValuePair<string, Key> kvp in _keyBinds)
            {
               if (keyState.IsKeyDown(kvp.Value))
               {
                    HandleInput(kvp.Key);
               }
            }

            MouseState mouseState = Mouse.GetState();

            foreach (KeyValuePair<string, MouseButton> kvp in _mouseBinds)
            {
                if (mouseState.IsButtonDown(kvp.Value))
                {
                    HandleInput(kvp.Key);
                }
            }
        }

        public void HandleInput(string pAction)
        {
            switch (pAction)
            {
                case "START_GAME":
                    
                    break;
            }
        }

        public void InitializeBinds()
        {
            if (_keyBinds == null)
            {
                _keyBinds = new Dictionary<string, Key>();

                _keyBinds.Add("MOVE_FORWARD", Key.W);
                _keyBinds.Add("MOVE_BACKWARD", Key.S);
                _keyBinds.Add("MOVE_LEFT", Key.A);
                _keyBinds.Add("MOVE_RIGHT", Key.D);
            }

            if (_mouseBinds != null)
            {

                _mouseBinds = new Dictionary<string, MouseButton>();

                _mouseBinds.Add("START_GAME", MouseButton.Left);
            }
        }
    }
}
