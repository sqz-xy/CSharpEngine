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
        COMPONENT_COLLISION_SPHERE = 1 << 6,
        COMPONENT_DAMAGE= 1 << 7,
        COMPONENT_HEALTH = 1 << 8,
        COMPONENT_COLLISION_AABB = 1 << 9,
        COMPONENT_RENDERABLE = 1 << 10

    }

    public interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
