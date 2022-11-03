using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Game.Components
{
    class ComponentRenderable : IComponent
    {
        private bool isRenderable;

        public ComponentRenderable(bool pIsRenderable)
        {
            isRenderable = pIsRenderable;
        }

        public bool IsRenderable
        {
            get { return isRenderable; }
            set { isRenderable = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_RENDERABLE; }
        }
    }
}
