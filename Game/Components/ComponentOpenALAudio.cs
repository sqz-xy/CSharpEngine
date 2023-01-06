using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Game.Components
{
    public class ComponentOpenALAudio : ComponentAudio
    {
        int _audioSource;
        
        public ComponentOpenALAudio(string pAudioName, bool pIsLooping)
        {
            _audioSource = AL.GenSource();
            _audioName = pAudioName;
            _isLooping = pIsLooping;
            AL.Source(_audioSource, ALSourcei.Buffer, ResourceManager.LoadAudio(pAudioName)); // attach the buffer to a source
            AL.Source(_audioSource, ALSourceb.Looping, pIsLooping); // source loops infinitely
        }

        public int AudioSource
        {
            get { return _audioSource; }
        }

        public override void PlayAudio()
        {
            if (_isLooping && _isPlaying)
                return;

            AL.SourcePlay(_audioSource); // play the audio source 
            
            if (_isLooping)
                _isPlaying = true;
        }

        public override void StopAudio()
        {
            AL.SourceStop(_audioSource); 
            AL.DeleteSource(_audioSource);  
        }

        public override void UpdateAudioPosition(Vector3 pPosition)
        {
            AL.Source(_audioSource, ALSource3f.Position, ref pPosition);
        }
        
    }
}