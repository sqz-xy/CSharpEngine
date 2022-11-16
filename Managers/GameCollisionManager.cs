using System;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        public override void ProcessCollisions()
        {
            foreach (var collision in _collisionManifold)
            {
                IComponent audioComponent = collision.entity.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                
                if (audioComponent == null)
                    continue;
                
                ComponentAudio audio = ((ComponentAudio) audioComponent);
                audio.PlayAudio();
            }
            
            ClearManifold();
        }
    }
}