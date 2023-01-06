using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.OBJLoader;
using OpenGL_Game.Game.Scenes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Game.Components
{
    // Pass in light data
    public class ComponentShaderNoLights : ComponentShader
    {
        public int uniform_stex;
        public int uniform_modelviewproj;
        public int uniform_mmodel;
        public int uniform_diffuse;

        public ComponentShaderNoLights(string pVertexPath, string pFragmentPath) : base(pVertexPath, pFragmentPath)
        {
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_modelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");
        }

        public override void ApplyShader(Matrix4 pModel, Geometry pGeometry)
        {
            GL.UseProgram(pgmID);
            
            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            
            GL.UniformMatrix4(uniform_mmodel, false, ref pModel);
            var modelViewProjection = pModel * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_modelviewproj, false, ref modelViewProjection);
            
            pGeometry.Render(uniform_diffuse);
            
            GL.UseProgram(0);
        }
    }
    
}