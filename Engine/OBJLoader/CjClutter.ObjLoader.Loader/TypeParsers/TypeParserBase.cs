using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public abstract class TypeParserBase : ITypeParser
    {
        protected abstract string Keyword { get; }

        public bool CanParse(string keyword)
        {
            return keyword.EqualsOrdinalIgnoreCase(Keyword);
        }

        public abstract void Parse(string line);
    }
}