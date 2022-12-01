using System;
using System.Collections.Generic;

using OpenGL_Game.Components;
using OpenGL_Game.Objects;

using OpenTK;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        //TODO: Add a sphere to the player, systemPlayer to update the position, remove camerasphere, add sphere-box collision

        public override void ProcessCollisions()
        {
            foreach (var collision in _collisionManifold)
            {
                if (collision.entity1.Name == "Player" || collision.entity2.Name == "player" && collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
                {
                    IComponent entity1Audioomponent = collision.entity1.Components.Find(delegate (IComponent component)
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
                    audio2.PlayAudio();

                    //SceneManager.ChangeScene(Scenes.SceneTypes.SCENE_GAME_OVER, MainEntry.game);
                }
            }        
            ClearManifold();
        }      
    }
}