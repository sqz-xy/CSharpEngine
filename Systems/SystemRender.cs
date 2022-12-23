using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | ComponentTypes.COMPONENT_SHADER);

        public SystemRender()
        {

        }
        
        public void Cleanup(Entity pEntity)
        {
            
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var geometry = ComponentHelper.GetComponent<ComponentGeometry>(entity, ComponentTypes.COMPONENT_GEOMETRY);
                    var position = ComponentHelper.GetComponent<ComponentPosition>(entity, ComponentTypes.COMPONENT_POSITION);
                    var shader = ComponentHelper.GetComponent<ComponentShader>(entity, ComponentTypes.COMPONENT_SHADER);
                    
                    Draw(Matrix4.CreateTranslation(position.Position), geometry.Geometry(), shader);
                }
        }

        public void Draw(Matrix4 model, Geometry geometry, ComponentShader pShader)
        {
            pShader.ApplyShader(model, geometry);
        }
    }
}
