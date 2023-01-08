﻿using System;
using OpenGL_Game.Engine.OBJLoader;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ResourceManager = OpenGL_Game.Engine.Managers.ResourceManager; // Temp Fix

namespace OpenGL_Game.Engine.Components
{
    public abstract class ComponentShader : IComponent
    {
        // Program ID
        public int pgmID;
        
        // Filepath names
        public string vertexPath, fragmentPath;

        public ComponentShader(string pVertexPath, string pFragmentPath)
        {
            pgmID = GL.CreateProgram();
            vertexPath = pVertexPath;
            fragmentPath = pFragmentPath;
            
            GL.AttachShader(pgmID, ResourceManager.LoadShader(pVertexPath, ShaderType.VertexShader));
            GL.AttachShader(pgmID, ResourceManager.LoadShader(pFragmentPath, ShaderType.FragmentShader));
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));
        }

        public abstract void ApplyShader(Matrix4 pModel, Geometry pGeometry);
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }
        
    }
}