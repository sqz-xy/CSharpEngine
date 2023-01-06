using System.Collections.Generic;
using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore
{
    public interface IDataStore 
    {
        IList<Vertex> Vertices { get; }
        IList<Texture> Textures { get; }
        IList<Normal> Normals { get; }
        IList<Material> Materials { get; }
        IList<Elements.Group> Groups { get; }
    }
}