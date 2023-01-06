using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Systems;

namespace OpenGL_Game.Game.Systems
{
    public class SystemAmbient : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var audioComponents = ComponentHelper.GetComponents<ComponentAudio>(entity);
                    foreach (var audioComponent in audioComponents)
                    {
                        if (audioComponent.IsLooping)
                            PlayAmbientSound(audioComponent);
                    }
                }
        }
        
        private void PlayAmbientSound(ComponentAudio pComponentAudio)
        {
            pComponentAudio.PlayAudio();
        }
        
        public string Name { get; }
    }
}