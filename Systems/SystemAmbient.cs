using System.Collections.Generic;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
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
                    if (audioComponents.Count > 1)
                        PlayAmbientSound(audioComponents[1]);
                }
        }
        
        private void PlayAmbientSound(ComponentAudio pComponentAudio)
        {
            pComponentAudio.PlayAudio();
        }
        
        public void Cleanup(Entity pEntity)
        {
            
        }
        
        public string Name { get; }
    }
}