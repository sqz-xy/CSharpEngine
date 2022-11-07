using OpenGL_Game.Managers;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentAudio : IComponent
    {
        int audio;
        int audioSource;

        public ComponentAudio(string pAudioName)
        {
            audio = ResourceManager.LoadAudio(pAudioName);
            audioSource = ResourceManager.GenerateAudioSource(audio);
        }

        public int Audio
        {
            get { return audio; }
        }
        
        public int AudioSource
        {
            get { return audioSource; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}