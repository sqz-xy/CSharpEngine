using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        private Stopwatch _healthCooldown;
        private Stopwatch _enemyHealthCooldown;
        private Stopwatch _powerUpHealthCooldown;

        public GameCollisionManager()
        {
            _healthCooldown = new Stopwatch();
            _enemyHealthCooldown = new Stopwatch();
            _powerUpHealthCooldown = new Stopwatch();
        }
        
        //TODO: BULLET DAMAGE POWER UP, DISABLE DRONES POWER UP
        
        
        public override void ProcessCollisions(Camera pCamera)
        {
            ResetCooldowns();
            
            foreach (var collision in _collisionManifold)
            {
                //PlayerEnemyCollision(collision);
                //BulletEnemyCollision(collision);

                if (collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
                {
                    DamageCollision(collision.entity1, collision.entity2, "Player", "EnemyCat", _healthCooldown, 10);
                    DamageCollision(collision.entity1, collision.entity2, "EnemyCat", "Bullet", _enemyHealthCooldown, 10);
                
                    PowerUpHealth(collision.entity2, collision.entity1, "FishPowerUpHealth", "Player", _powerUpHealthCooldown, 1, 10);
                }

                if (collision.collisionType == COLLISIONTYPE.SPHERE_AABB)
                {
                    if (collision.entity1.Name.Contains("Bullet"))
                    {
                        IComponent healthComponent = collision.entity1.Components.Find(delegate(IComponent component)
                        {
                            return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                        });
                
                        ComponentHealth health = (ComponentHealth) healthComponent;
                        health.Health -= 10;
                        continue;
                    }

                    ComponentPosition position =
                        ComponentHelper.GetComponent<ComponentPosition>(collision.entity1,
                            ComponentTypes.COMPONENT_POSITION);

                    IComponent directionComponent = collision.entity1.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
                    });
                    ComponentDirection direction = (ComponentDirection) directionComponent;

                    position.Position += direction.Direction * -0.1f;
                }
            }        
            ClearManifold();
        }

        private void PowerUpHealth(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, int pHealth)
        {
            // Doesnt work because of the order of precedence for the collided entities
            
            if (DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage))
            {
                IComponent healthComponent = pEntityToHit.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                });
                
                ComponentHealth health = (ComponentHealth) healthComponent;
                
                // Collects twice
                health.Health += pHealth;
                pStopwatch.Start();
            }
        }
        

        
        
        private bool DamageCollision(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage)
        {
            if (pEntityToAct.Name.Contains(pEntity1Name))
            {
                if (pEntityToHit.Name.Contains(pEntity2Name))
                {
                    IComponent healthComponent = pEntityToAct.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                    });
                    ComponentHealth health = (ComponentHealth) healthComponent;
                    
                    IComponent damageComponent = pEntityToHit.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DAMAGE;
                    });
                    ComponentDamage damage = (ComponentDamage) damageComponent;
                    
                    IComponent audioComponent = pEntityToHit.Components.Find(delegate(IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                    });
                    ComponentAudio audio = (ComponentAudio) audioComponent;
                    
                    int damageValue;
                    if (damage == null)
                        damageValue = pDamage;
                    else
                        damageValue = damage.Damage;

                    if (health.Health <= 0)
                    {
                        Console.WriteLine("Enemy Dead!");
                    }
                    
                    if (pStopwatch.ElapsedMilliseconds == 0)
                    {
                        audio.PlayAudio();
                        health.Health -= damageValue;
                        pStopwatch.Start();
                    }
                    return true;
                }
            }
            return false;
        }

        private void ResetCooldowns()
        {
            if (_healthCooldown.ElapsedMilliseconds >= 3000)
                _healthCooldown.Reset();
            
            if (_enemyHealthCooldown.ElapsedMilliseconds >= 1000)
                _enemyHealthCooldown.Reset();
            
            if (_powerUpHealthCooldown.ElapsedMilliseconds >= 1000)
                _powerUpHealthCooldown.Reset();
        }
    }
}