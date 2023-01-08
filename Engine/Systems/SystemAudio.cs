using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenTK;

namespace OpenGL_Game.Engine.Systems
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
                // Update the positional audio
                audioComponent.UpdateAudioPosition(pPosition);
            }
        }
        
        public string Name { get; }
    }
}