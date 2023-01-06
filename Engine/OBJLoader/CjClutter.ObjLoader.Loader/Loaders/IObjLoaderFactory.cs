namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public interface IObjLoaderFactory
    {
        IObjLoader Create(IMaterialStreamProvider materialStreamProvider);
        IObjLoader Create();
    }
}