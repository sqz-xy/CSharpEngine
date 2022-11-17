using OpenGL_Game.Objects;

using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    interface ISystem
    {
        void OnAction(List<Entity> pEntity);

        void Cleanup(Entity pEntity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
