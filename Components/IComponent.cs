﻿using System;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    public enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE  = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_SHADER = 1 << 4,
        COMPONENT_AUDIO = 1 << 5,
        COMPONENT_RENDERABLE = 1 << 6

    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
