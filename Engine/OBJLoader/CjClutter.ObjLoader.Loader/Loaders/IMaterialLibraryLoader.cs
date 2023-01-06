using System.IO;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public interface IMaterialLibraryLoader
    {
        void Load(Stream lineStream);
    }
}