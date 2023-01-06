using System.IO;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public interface IObjLoader
    {
        LoadResult Load(Stream lineStream);

        void SetPath(string path);
    }
}