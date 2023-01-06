using OpenTK;

namespace OpenGL_Game.Engine.Components
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

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public abstract void PlayAudio();
        public abstract void StopAudio();
        public abstract void UpdateAudioPosition(Vector3 pPosition);
    }
}