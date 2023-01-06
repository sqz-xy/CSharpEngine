using System.IO;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public class MaterialStreamProvider : IMaterialStreamProvider
    {
        public Stream Open(string materialFilePath)
        {
           // return null;
            return File.Open(materialFilePath, FileMode.Open, FileAccess.Read);
        }
    }

    public class MaterialNullStreamProvider : IMaterialStreamProvider
    {
        public Stream Open(string materialFilePath)
        {
            return null;
        }
    }
}