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
        private SceneManager _sceneManager;

        public SystemHealth(EntityManager pEntityManager, SceneManager pSceneManager)
        {
            _entityManager = pEntityManager;
            _sceneManager = pSceneManager;
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
                    ComponentHealth health = ComponentHelper.GetComponent<ComponentHealth>(entity, ComponentTypes.COMPONENT_HEALTH);
                    
                    if (entity.Name == "Player")
                    {
                        if (health.Health <= 0)
                            SceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER, _sceneManager);
                        else
                            GameScene.playerHealth = health.Health;
                        continue;
                    }
                    
                    if (health.Health <= 0) 
                            KillEntity(entity);
                }
        }

        private void KillEntity(Entity pEntity)
        {
            if (_entityManager.DeleteRenderableEntity(pEntity.Name))
                return;

            _entityManager.DeleteNonRenderableEntity(pEntity.Name);
        }
    }
}