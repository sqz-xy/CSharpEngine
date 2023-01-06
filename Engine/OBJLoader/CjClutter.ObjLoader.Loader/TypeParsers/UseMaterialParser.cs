using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL_Game.Engine.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class UseMaterialParser : TypeParserBase, IUseMaterialParser
    {
        private readonly IElementGroup _elementGroup;

        public UseMaterialParser(IElementGroup elementGroup)
        {
            _elementGroup = elementGroup;
        }

        protected override string Keyword
        {
            get { return "usemtl"; }
        }

        public override void Parse(string line)
        {
            _elementGroup.SetMaterial(line);
        }
    }
}