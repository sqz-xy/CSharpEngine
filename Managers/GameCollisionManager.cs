using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

using OpenTK;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        private Stopwatch _healthCooldown;
        private Stopwatch _enemyHealthCooldown;

        public GameCollisionManager()
        {
            _healthCooldown = new Stopwatch();
            _enemyHealthCooldown = new Stopwatch();
        }
        
        //TODO: Powerups and death conditions
        
        
        public override void ProcessCollisions()
        {
            ResetCooldowns();
            
            foreach (var collision in _collisionManifold)
            {
                PlayerEnemyCollision(collision);
                BulletEnemyCollision(collision);
            }        
            ClearManifold();
        }

        private void PlayerEnemyCollision(Collision collision)
        {
            if (collision.entity1.Name == "Player" && collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
            {
                if (collision.entity2.Name == "Moon2")
                {
                    IComponent playerHealthComponent = collision.entity1.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                    });
                    ComponentHealth playerHealth = (ComponentHealth) playerHealthComponent;
                    
                    if (playerHealth.Health <= 0)
                    {
                        Console.WriteLine("Player Dead!");
                    }
                    
                    if (_healthCooldown.ElapsedMilliseconds == 0)
                    {
                        playerHealth.Health -= 10;
                        _healthCooldown.Start();
                    }
                    
                    
                }
            }
        }
        
        private void BulletEnemyCollision(Collision collision)
        {
            if (collision.entity1.Name == "Moon2" && collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
            {
                if (collision.entity2.Name.Contains("Bullet"))
                {
                    IComponent playerHealthComponent = collision.entity1.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                    });
                    ComponentHealth enemyHealth = (ComponentHealth) playerHealthComponent;

                    if (enemyHealth.Health <= 0)
                    {
                        Console.WriteLine("Enemy Dead!");
                    }
                    
                    if (_enemyHealthCooldown.ElapsedMilliseconds == 0)
                    {
                        enemyHealth.Health -= 10;
                        _enemyHealthCooldown.Start();
                    }

                }
            }
        }

        public void ResetCooldowns()
        {
            if (_healthCooldown.ElapsedMilliseconds >= 3000)
                _healthCooldown.Reset();
            
            if (_enemyHealthCooldown.ElapsedMilliseconds >= 1000)
                _enemyHealthCooldown.Reset();
        }
    }
}

/*IComponent entity1AudioComponent = collision.entity1.Components.Find(delegate (IComponent component)
{
    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
});
ComponentAudio audio1 = (ComponentAudio)entity1AudioComponent;


IComponent entity2AudioComponent = collision.entity2.Components.Find(delegate (IComponent component)
{
    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
});
ComponentAudio audio2 = (ComponentAudio)entity1AudioComponent;

audio1.PlayAudio();
audio2.PlayAudio();*/

//SceneManager.ChangeScene(Scenes.SceneTypes.SCENE_GAME_OVER, MainEntry.game);