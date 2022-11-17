using OpenTK;

namespace OpenGL_Game.Components
{
    public abstract class ComponentAudio : IComponent
    {
        public abstract ComponentTypes ComponentType { get; }
        public abstract void PlayAudio();
        public abstract void StopAudio();
        public abstract void UpdateAudioPosition(Vector3 pPosition);
    }
}