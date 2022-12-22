using OpenGL_Game.Managers;

namespace OpenGL_Game.Components
{
    class ComponentTexture : IComponent
    {
        int texture;
        private string textureName;

        public ComponentTexture(string pTextureName)
        {
            textureName = pTextureName;
            texture = ResourceManager.LoadTexture(pTextureName);
        }

        public int Texture
        {
            get { return texture; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_TEXTURE; }
        }
    }
}
