using OpenGL_Game.Managers;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentAudio : IComponent
    {
        int audioSource;

        public ComponentAudio(string pAudioName)
        {
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, ResourceManager.LoadAudio(pAudioName)); // attach the buffer to a source
            AL.Source(audioSource, ALSourceb.Looping, true); // source loops infinitely
        }

        public int AudioSource
        {
            get { return audioSource; }
        }
        
        public void PlayAudio()
        {
            // Play
        }

        public void StopAudio()
        {
            // Stop
        }
        
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}