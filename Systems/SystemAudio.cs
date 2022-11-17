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
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    List<IComponent> components = entity.Components;

                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    Vector3 position = ((ComponentPosition)positionComponent).Position;

                    IComponent audioComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                    });
                    ComponentAudio audio = ((ComponentAudio)audioComponent);

                    UpdateSourcePosition(audio, position);
                }
        }

        public void Cleanup(Entity pEntity)
        {
            
        }
        
        private void UpdateSourcePosition(ComponentAudio pComponentAudio, Vector3 pPosition)
        {
            pComponentAudio.UpdateAudioPosition(pPosition);
            //pComponentAudio.PlayAudio();
        }
        
        public string Name { get; }
    }
}