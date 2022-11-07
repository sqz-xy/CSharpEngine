using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    interface ISystem
    {
        void OnAction(Entity entity);

        void Cleanup(Entity pEntity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
