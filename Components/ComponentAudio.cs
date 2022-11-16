using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentAudio : IComponent
    {
        int _audioSource;
        private bool playingAudio = false;

        public ComponentAudio(string pAudioName)
        {
            _audioSource = AL.GenSource();
            AL.Source(_audioSource, ALSourcei.Buffer, ResourceManager.LoadAudio(pAudioName)); // attach the buffer to a source
            AL.Source(_audioSource, ALSourceb.Looping, true); // source loops infinitely
        }

        public int AudioSource
        {
            get { return _audioSource; }
        }
        
        public void PlayAudio()
        {
            // This will need to be moved
            if (!playingAudio)
            {
                AL.SourcePlay(_audioSource); // play the audio source 
                playingAudio = true;
            }
        }

        public void StopAudio()
        {
            AL.SourceStop(_audioSource); 
            AL.DeleteSource(_audioSource);  
        }

        public void UpdateAudioPosition(Vector3 pPosition)
        {
            AL.Source(_audioSource, ALSource3f.Position, ref pPosition);
        }
        
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}