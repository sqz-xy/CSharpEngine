using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.OBJLoader;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Systems
{
    public class SystemAudio : ISystem
    {
        
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);
        private bool playingAudio = false;
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent audioComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                int audioSource = ((ComponentAudio) audioComponent).AudioSource;

                PlayAudio(audioSource, position);
            }
        }

        public void Cleanup(Entity pEntity)
        {
            StopAudio(pEntity);
        }

        // May have issues with multiple sources
        private void PlayAudio(int pAudioSource, Vector3 pPosition)
        {
            AL.Source(pAudioSource, ALSource3f.Position, ref pPosition);

            // This will need to be moved
            if (!playingAudio)
            {
                AL.SourcePlay(pAudioSource); // play the audio source 
                playingAudio = true;
            }
        }

        public void StopAudio(Entity pEntity)
        {
            if ((pEntity.Mask & MASK) == MASK)
            {
                List<IComponent> components = pEntity.Components;
                
                IComponent audioComponent = components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                int audioSource = ((ComponentAudio) audioComponent).AudioSource;
                
                AL.SourceStop(audioSource); 
                AL.DeleteSource(audioSource);   
            }
        }

        public string Name { get; }
    }
}