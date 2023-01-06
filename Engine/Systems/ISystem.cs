using System.Collections.Generic;
using OpenGL_Game.Engine.Objects;

namespace OpenGL_Game.Engine.Systems
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
