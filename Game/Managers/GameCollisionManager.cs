using System;
using System.Diagnostics;
using OpenGL_Game.Engine.Components;
using OpenGL_Game.Engine.Managers;
using OpenGL_Game.Engine.Objects;
using OpenGL_Game.Game.Components;
using OpenTK;

namespace OpenGL_Game.Game.Managers
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
        
        public override void ProcessCollisions()
        {
            ResetCooldowns();
            
            foreach (var collision in _collisionManifold)
            {
                if (collision.collisionType == COLLISIONTYPE.SPHERE_SPHERE)
                {
                    DamageCollision(collision.entity1, collision.entity2, "Player", "EnemyCat", _healthCooldown, 10);
                    DamageCollision(collision.entity1, collision.entity2, "EnemyCat", "Bullet", _enemyHealthCooldown, 10);
                
                    PowerUpHealth(collision.entity2, collision.entity1, "FishPowerUp Health", "Player", _powerUpHealthCooldown, 1, 15);
                    PowerUpSpeed(collision.entity2, collision.entity1, "FishPowerUp Speed", "Player", _powerUpSpeedCooldown, 1, 1.25f);
                    PowerUpDamage(collision.entity2, collision.entity1, "FishPowerUp Damage", "Player", _powerUpSpeedCooldown, 1, 20);
                }
                
                if (collision.collisionType == COLLISIONTYPE.SPHERE_AABB)
                {
                    if (collision.entity2.Name.Contains("Wall") && IsActive)
                        WallCollision(collision);
                }
            }        
            ClearManifold();
        }

        private void WallCollision(Collision collision)
        {
            // Destroy bullets if they hit a wall
            if (collision.entity1.Name.Contains("Bullet"))
            {
                var health = ComponentHelper.GetComponent<ComponentHealth>(collision.entity1, ComponentTypes.COMPONENT_HEALTH);
                health.Health -= 1000;
                return;
            }

            // Walls only collide with player after bullet check
            if (!collision.entity1.Name.Contains("Player"))
                return;

            // Collision response for player and wall
            var playerPosition =
                ComponentHelper.GetComponent<ComponentPosition>(collision.entity1, ComponentTypes.COMPONENT_POSITION);
            var playerCollision =
                ComponentHelper.GetComponent<ComponentCollisionSphere>(collision.entity1,
                    ComponentTypes.COMPONENT_COLLISION_SPHERE);

            var wallPosition =
                ComponentHelper.GetComponent<ComponentPosition>(collision.entity2, ComponentTypes.COMPONENT_POSITION);
            var wallCollision =
                ComponentHelper.GetComponent<ComponentCollisionAABB>(collision.entity2,
                    ComponentTypes.COMPONENT_COLLISION_AABB);

            var xDistance = Math.Abs(playerPosition.Position.X - wallPosition.Position.X);
            var zDistance = Math.Abs(playerPosition.Position.Z - wallPosition.Position.Z);

            Vector3 newPlayerPos = playerPosition.Position;

            // Extra multiplier is there because the collision didn't teleport the player far enough away
            if (xDistance < wallCollision.Width)
            {
                if (playerPosition.Position.Z > wallPosition.Position.Z) // Right side
                    newPlayerPos.Z = wallPosition.Position.Z + wallCollision.Depth + (playerCollision.CollisionField * 1.25f);
                else if (playerPosition.Position.Z < wallPosition.Position.Z) // Left side
                    newPlayerPos.Z = wallPosition.Position.Z - wallCollision.Depth - (playerCollision.CollisionField * 1.25f);
            }

            if (zDistance < wallCollision.Depth)
            {
                if (playerPosition.Position.X > wallPosition.Position.X) // Front
                    newPlayerPos.X = wallPosition.Position.X + wallCollision.Width + (playerCollision.CollisionField * 1.25f);
                else if (playerPosition.Position.X < wallPosition.Position.X) // Back
                    newPlayerPos.X = wallPosition.Position.X - wallCollision.Width - (playerCollision.CollisionField * 1.25f);
            }

            playerPosition.Position = newPlayerPos;
        }

        private void PowerUpHealth(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, int pHealth)
        {
            if (!DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage)) return;
            
            var health = ComponentHelper.GetComponent<ComponentHealth>(pEntityToHit, ComponentTypes.COMPONENT_HEALTH);
                
            // Collects twice
            health.Health += pHealth;
            pStopwatch.Start();
        }
        
        private void PowerUpSpeed(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, float pSpeed)
        {
            if (!DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage)) return;
            
            var speed = ComponentHelper.GetComponent<ComponentSpeed>(pEntityToHit, ComponentTypes.COMPONENT_SPEED);
                
            // Collects twice
            speed.Speed *= pSpeed;
            pStopwatch.Start();
        }

        private void PowerUpDamage(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage, int pDamageIncrease)
        {
            if (!DamageCollision(pEntityToAct, pEntityToHit, pEntity1Name, pEntity2Name, pStopwatch, pDamage)) return;
            
            var damage = ComponentHelper.GetComponent<ComponentDamage>(pEntityToHit, ComponentTypes.COMPONENT_DAMAGE);
                
            // Collects twice
            damage.Damage += pDamageIncrease;
            pStopwatch.Start();
        }
        
        
        private bool DamageCollision(Entity pEntityToAct, Entity pEntityToHit, string pEntity1Name, string pEntity2Name, Stopwatch pStopwatch, int pDamage)
        {
            if (!pEntityToAct.Name.Contains(pEntity1Name)) return false;

            if (!pEntityToHit.Name.Contains(pEntity2Name)) return false;
            
            var health = ComponentHelper.GetComponent<ComponentHealth>(pEntityToAct, ComponentTypes.COMPONENT_HEALTH);
            var damage = ComponentHelper.GetComponent<ComponentDamage>(pEntityToHit, ComponentTypes.COMPONENT_DAMAGE);
            var audio = ComponentHelper.GetComponent<ComponentAudio>(pEntityToAct, ComponentTypes.COMPONENT_AUDIO);
                    
            int damageValue;
            if (damage == null)
                damageValue = pDamage;
            else
                damageValue = damage.Damage;
                    
            if (pStopwatch.ElapsedMilliseconds == 0)
            {
                audio.PlayAudio();
                health.Health -= damageValue;
                pStopwatch.Start();
            }
                    
            return true;
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