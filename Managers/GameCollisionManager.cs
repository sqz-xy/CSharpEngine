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
        private Stopwatch _powerUpSpeedCooldown;

        public GameCollisionManager()
        {
            _healthCooldown = new Stopwatch();
            _enemyHealthCooldown = new Stopwatch();
            _powerUpHealthCooldown = new Stopwatch();
            _powerUpSpeedCooldown = new Stopwatch();
        }
        
        public override void ProcessCollisions(Camera pCamera)
        {
            ResetCooldowns();
            
            foreach (var collision in _collisionManifold)
            {
                if (collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
                {
                    DamageCollision(collision.entity1, collision.entity2, "Player", "EnemyCat", _healthCooldown, 10);
                    DamageCollision(collision.entity1, collision.entity2, "EnemyCat", "Bullet", _enemyHealthCooldown, 10);
                
                    PowerUpHealth(collision.entity2, collision.entity1, "FishPowerUpHealth", "Player", _powerUpHealthCooldown, 1, 5);
                    PowerUpSpeed(collision.entity2, collision.entity1, "FishPowerUpSpeed", "Player", _powerUpSpeedCooldown, 1, 1.2f);
                    PowerUpDamage(collision.entity2, collision.entity1, "FishPowerUpDamage", "Player", _powerUpSpeedCooldown, 1, 20);
                }

                if (collision.collisionType == COLLISIONTYPE.SPHERE_AABB)
                {
                    if (collision.entity1.Name.Contains("Bullet"))
                    {
                        var health = ComponentHelper.GetComponent<ComponentHealth>(collision.entity1, ComponentTypes.COMPONENT_HEALTH);
                        health.Health -= 10;
                        continue;
                    }
                    
                    // Walls only collide with player after bullet check
                    if (!collision.entity1.Name.Contains("Player"))
                        continue;

                    var position = ComponentHelper.GetComponent<ComponentPosition>(collision.entity1, ComponentTypes.COMPONENT_POSITION);
                    var direction = ComponentHelper.GetComponent<ComponentDirection>(collision.entity1, ComponentTypes.COMPONENT_DIRECTION);
                    
                    position.Position += direction.Direction * -0.1f;
                }
            }        
            ClearManifold();
        }

        private void PowerUpHealth(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, int pHealth)
        {
            if (DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage))
            {
                var health = ComponentHelper.GetComponent<ComponentHealth>(pEntityToHit, ComponentTypes.COMPONENT_HEALTH);
                
                // Collects twice
                health.Health += pHealth;
                pStopwatch.Start();
            }
        }
        
        private void PowerUpSpeed(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, float pSpeed)
        {
            if (DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage))
            {
                var speed = ComponentHelper.GetComponent<ComponentSpeed>(pEntityToHit, ComponentTypes.COMPONENT_SPEED);
                
                // Collects twice
                speed.Speed += pSpeed;
                pStopwatch.Start();
            }
        }

        private void PowerUpDamage(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, int pDamageIncrease)
        {
            if (DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage))
            {
                var damage = ComponentHelper.GetComponent<ComponentDamage>(pEntityToHit, ComponentTypes.COMPONENT_DAMAGE);
                
                // Collects twice
                damage.Damage += pDamageIncrease;
                pStopwatch.Start();
            }
        }
        
        
        private bool DamageCollision(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage)
        {
            if (pEntityToAct.Name.Contains(pEntity1Name))
            {
                if (pEntityToHit.Name.Contains(pEntity2Name))
                {
                    var health = ComponentHelper.GetComponent<ComponentHealth>(pEntityToAct, ComponentTypes.COMPONENT_HEALTH);
                    var damage = ComponentHelper.GetComponent<ComponentDamage>(pEntityToHit, ComponentTypes.COMPONENT_DAMAGE);
                    var audio = ComponentHelper.GetComponent<ComponentAudio>(pEntityToHit, ComponentTypes.COMPONENT_AUDIO);
                    
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
            
            if (_powerUpSpeedCooldown.ElapsedMilliseconds >= 1000)
                _powerUpSpeedCooldown.Reset();
        }
    }
}