using System;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentOpenALAudio : ComponentAudio
    {
        int _audioSource;
        private bool playingAudio = false;

        public ComponentOpenALAudio(string pAudioName, bool pIsLooping)
        {
            _audioSource = AL.GenSource();
            AL.Source(_audioSource, ALSourcei.Buffer, ResourceManager.LoadAudio(pAudioName)); // attach the buffer to a source
            AL.Source(_audioSource, ALSourceb.Looping, pIsLooping); // source loops infinitely
        }

        public int AudioSource
        {
            get { return _audioSource; }
        }
        
        public override void PlayAudio()
        {
            // This will need to be moved
            AL.SourcePlay(_audioSource); // play the audio source 
            playingAudio = true;
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
        
        public override ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}