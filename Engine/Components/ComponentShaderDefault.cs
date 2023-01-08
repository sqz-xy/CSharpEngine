using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.OBJLoader;
using OpenGL_Game.Game.Scenes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Game.Components
{
    public class ComponentShaderDefault : ComponentShader
    {
        private int uniform_stex;
        private int uniform_modelviewproj;
        private int uniform_mmodel;
        private int uniform_diffuse;

        public ComponentShaderDefault() : base("Shaders/vs.glsl", "Shaders/fs.glsl")
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