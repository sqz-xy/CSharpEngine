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
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var position = ComponentHelper.GetComponent<ComponentPosition>(entity, ComponentTypes.COMPONENT_POSITION);

                    var audioComponents = ComponentHelper.GetComponents<ComponentAudio>(entity);
                    UpdateSourcePosition(audioComponents, position.Position);
                }
        }
        
        private void UpdateSourcePosition(List<ComponentAudio> pAudioComponents, Vector3 pPosition)
        {
            foreach (var audioComponent in pAudioComponents)
            {
                audioComponent.UpdateAudioPosition(pPosition);
            }
        }
        
        public void Cleanup(Entity pEntity)
        {
            
        }
        
        public string Name { get; }
    }
}