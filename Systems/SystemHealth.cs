using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Systems
{
    public class SystemHealth : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_HEALTH);
        private EntityManager _entityManager;

        public SystemHealth(EntityManager pEntityManager)
        {
            _entityManager = pEntityManager;
        }

        public void Cleanup(Entity pEntity)
        {

        }

        public string Name
        {
            get { return "SystemHealth"; }
        }

        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if (((entity.Mask & MASK) == MASK))
                {
                    var health = ComponentHelper.GetComponent<ComponentHealth>(entity, ComponentTypes.COMPONENT_HEALTH);

                    if (health.Health <= 0) 
                            KillEntity(entity);
                }
        }

        private void KillEntity(Entity pEntity)
        {
            // If an entity is dead and has a looping ambient sound, stop its ambient audio.
            foreach (var component in pEntity.Components)
            {
                if (component is ComponentOpenALAudio audio && audio.IsLooping)
                {
                    audio.StopAudio();
                }
            }
            
            if (_entityManager.DeleteRenderableEntity(pEntity.Name))
                return;

            _entityManager.DeleteNonRenderableEntity(pEntity.Name);
        }
    }
}