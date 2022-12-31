using OpenTK;

namespace OpenGL_Game.Components
{
    public abstract class ComponentAudio : IComponent
    {
        protected string _audioName;
        protected bool _isPlaying;
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
        
        
        public abstract ComponentTypes ComponentType { get; }
        public abstract void PlayAudio();
        public abstract void StopAudio();
        public abstract void UpdateAudioPosition(Vector3 pPosition);
    }
}