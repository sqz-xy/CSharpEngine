﻿using OpenTK;

namespace OpenGL_Game.Engine.Components
{
    public abstract class ComponentAudio : IComponent
    {
        // File name
        protected string _audioName;
        
        // Is the audio currently playing
        protected bool _isPlaying;
        
        // Is the audio a looping type
        protected bool _isLooping;

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

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public abstract void PlayAudio();
        public abstract void StopAudio();
        public abstract void UpdateAudioPosition(Vector3 pPosition);
    }
}