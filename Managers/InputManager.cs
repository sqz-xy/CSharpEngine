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

        InputManager(ref Camera pCamera)
        {
            _camera = pCamera;
        }

        public void HandleInput()
        {
            // Load custom control script using script manager?
            // If not use default controls
        }
    }
}
