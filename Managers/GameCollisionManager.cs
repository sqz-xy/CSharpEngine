using System;
using OpenGL_Game.Components;

using OpenTK;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        /*  Removed camera collision
         *  Add sphere to camera 
         *  https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
         */

        public override void ProcessCollisions()
        {
            foreach (var collision in _collisionManifold)
            {
                IComponent entity1Audioomponent = collision.entity1.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio1 = (ComponentAudio)entity1Audioomponent;


                IComponent entity2Audioomponent = collision.entity2.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio2 = (ComponentAudio)entity1Audioomponent;

                audio1.PlayAudio();
                //audio2.PlayAudio();

            }
            
            ClearManifold();
        }
    }
}