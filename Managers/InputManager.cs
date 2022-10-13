using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Managers
{
    enum ActionType
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
    }

    class InputManager
    {
        Camera _camera;

        InputManager()
        {
            _camera = new Camera();
        }

        public void AssignAction(ActionType pAction, Key KeyType)
        {
            // Assign to delegate?
        }

        // public void ClearActions()
    }
}
