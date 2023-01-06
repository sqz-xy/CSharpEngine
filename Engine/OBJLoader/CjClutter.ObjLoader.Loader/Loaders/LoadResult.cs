using System.Collections.Generic;
using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Data;
using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public class LoadResult  
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<Texture> Textures { get; set; }
        public IList<Normal> Normals { get; set; }
        public IList<Data.Elements.Group> Groups { get; set; }
        public IList<Material> Materials { get; set; }
    }
}