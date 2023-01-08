using System.Collections.Generic;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Engine.Systems;
using OpenGL_Game.Game.Components;

namespace OpenGL_Game.Game.Systems
{
    public class SystemHealth : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_HEALTH);
        private EntityManager _entityManager;

        public SystemHealth(EntityManager pEntityManager)
        {
            _entityManager = pEntityManager;
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

                    // If an entity has 0 or below 0 health, remove it
                    if (health.Health <= 0) 
                            KillEntity(entity);
                }
        }

        private void KillEntity(Entity pEntity)
        {
            // If an entity is dead and has a looping ambient sound, stop its ambient audio.
            foreach (var component in pEntity.Components)
                if (component is ComponentOpenALAudio audio && audio.IsLooping)
                    audio.StopAudio();

            if (_entityManager.DeleteRenderableEntity(pEntity.Name)) 
                return;

            _entityManager.DeleteNonRenderableEntity(pEntity.Name);
        }
    }
}