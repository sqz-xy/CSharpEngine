using OpenGL_Game.Objects;

using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    public interface ISystem
    {
        void OnAction(List<Entity> pEntity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
