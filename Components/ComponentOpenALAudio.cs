﻿using System;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentOpenALAudio : ComponentAudio
    {
        int _audioSource;
        private string _audioName;
        private bool _isPlaying;
        private bool _isLooping;

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

        public string AudioName
        {
            get { return _audioName; }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        public bool IsLooping
        {
            get { return _isLooping; }
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
        
        public override ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}