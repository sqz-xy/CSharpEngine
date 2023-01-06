using OpenTK.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Scenes;
using OpenTK;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using System.Security.AccessControl;

namespace OpenGL_Game.Managers
{
    // Reset binds when changing scene
    public class GameInputManager : InputManager
    {
        public static string bulletName = "Bullet Source";
        private int bulletIndex = 0;

        private Stopwatch _shootCooldown;
        private Stopwatch _toggleAICooldown;
        private Stopwatch _toggleCollisionCooldown;
        
        public Dictionary<string, Key> _keyBinds;
        public Dictionary<string, MouseButton> _mouseBinds;

        public bool _isAiActive = true;

        public GameInputManager(EntityManager pEntityManager, SceneManager pSceneManager) : base(pEntityManager, pSceneManager)
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>(); ;

            _toggleAICooldown = new Stopwatch();
            _toggleCollisionCooldown = new Stopwatch();
            
            _shootCooldown = new Stopwatch();
        }

        public override void ReadInput(Entity pEntity)
        {
            var keyState = Keyboard.GetState();
            ResetCooldowns();
            
            foreach (var kvp in _keyBinds)
            {
                if (!keyState.IsKeyDown(kvp.Value)) continue;
                
                if (pEntity == null)
                    HandleSceneInput(kvp.Key);
                else
                    HandleEntityInput(kvp.Key, pEntity);
            }

            var mouseState = Mouse.GetState();

            foreach (var kvp in _mouseBinds)
            {
                if (!mouseState.IsButtonDown(kvp.Value)) continue;
                
                if (pEntity == null)
                    HandleSceneInput(kvp.Key);
                else
                    HandleEntityInput(kvp.Key, pEntity);
            }
        }

        public override void HandleEntityInput(string pAction, Entity pEntity)
        {
            var playerPosComponent = ComponentHelper.GetComponent<ComponentPosition>(pEntity, ComponentTypes.COMPONENT_POSITION);
            var playerDirComponent = ComponentHelper.GetComponent<ComponentDirection>(pEntity, ComponentTypes.COMPONENT_DIRECTION);
            var playerSpeedComponent = ComponentHelper.GetComponent<ComponentSpeed>(pEntity, ComponentTypes.COMPONENT_SPEED);

            // camera dependant actions
            switch (pAction)
            {
                case "MOVE_FORWARD":
                    playerPosComponent.Position += ((playerDirComponent.Direction * 4) * GameScene.dt) * playerSpeedComponent.Speed;
                    break;
                case "MOVE_BACKWARD":
                    playerPosComponent.Position += -((playerDirComponent.Direction * 4) * GameScene.dt) * playerSpeedComponent.Speed;
                    break;
                case "MOVE_LEFT":
                    playerDirComponent.Direction = Matrix3.CreateRotationY(-4f * GameScene.dt) * playerDirComponent.Direction;
                    break;
                case "MOVE_RIGHT":
                    playerDirComponent.Direction = Matrix3.CreateRotationY(4f * GameScene.dt) * playerDirComponent.Direction;
                    break;
                case "SHOOT":
                    Shoot(pEntity, 10.0f);
                    break;
            }
        }

        public override void HandleSceneInput(string pAction)
        {
            // Non camera dependant actions, can be called without a system
            switch (pAction)
            {
                case "START_GAME":
                    _sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
                    break;
                case "CLOSE_GAME":
                    _sceneManager.Close();
                    break;
                case "TOGGLE_COLLISION":
                    ToggleCollision(_sceneManager.collisionManager);
                    break;
                case "TOGGLE_AI":
                    ToggleAI(_entityManager);
                    break;
            }
        }

        private void ToggleCollision(CollisionManager pCollisionManager)
        {
            if (_toggleCollisionCooldown.ElapsedMilliseconds == 0)
            {
                var gameCollisionManager = (GameCollisionManager) pCollisionManager;
                gameCollisionManager._wallCollision = !gameCollisionManager._wallCollision;
                _toggleCollisionCooldown.Start();
            }
        }
        
        private void ToggleAI(EntityManager pEntityManager)
        {
            if (_toggleAICooldown.ElapsedMilliseconds == 0)
            {
                _isAiActive = !_isAiActive;
                foreach (var entity in pEntityManager.RenderableEntities().Concat(pEntityManager.NonRenderableEntities()))
                {
                    var ai = ComponentHelper.GetComponent<ComponentPathFollowAI>(entity, ComponentTypes.COMPONENT_AI);
                    if (ai != null)
                    {
                        ai.IsActive = !ai.IsActive;
                        
                        if (ai.LocationIndex == 0)
                            continue;
                        
                        ai.LocationIndex--;
                    }
                    
                }
                _toggleAICooldown.Start();
            }
        }

        public void Shoot(Entity pEntity, float pSpeed)
        {
            if (_shootCooldown.ElapsedMilliseconds == 0)
            {
                var playerPosComponent = ComponentHelper.GetComponent<ComponentPosition>(pEntity, ComponentTypes.COMPONENT_POSITION);
                var playerDirComponent = ComponentHelper.GetComponent<ComponentDirection>(pEntity, ComponentTypes.COMPONENT_DIRECTION);
                var playerDamageComponent = ComponentHelper.GetComponent<ComponentDamage>(pEntity, ComponentTypes.COMPONENT_DAMAGE);
                
                var playerPos = playerPosComponent.Position;
                var playerDir = playerDirComponent.Direction;
                var playerDamage = playerDamageComponent.Damage;

                // Make a copy of the saved bullet
                var storedBullet = _entityManager.FindRenderableEntity(bulletName);
                var newBullet = new Entity($"Bullet{bulletIndex}");

                var bulletSound = ComponentHelper.GetComponent<ComponentAudio>(storedBullet, ComponentTypes.COMPONENT_AUDIO);

                foreach (var c in storedBullet.Components)
                {
                    if (c.GetType() == typeof(ComponentHealth))
                        continue;
                    
                    var componentCopy = c;
                    newBullet.AddComponent(componentCopy);
                }
                
                var health = ComponentHelper.GetComponent<ComponentHealth>(pEntity, ComponentTypes.COMPONENT_HEALTH);
                
                // Spawn bullet in front of player with camera direction as velocity
                var bulletPos = playerPos + playerDir;
                bulletPos.Y += 0.5f;
                
                newBullet.AddComponent(new ComponentPosition(bulletPos));
                newBullet.AddComponent(new ComponentVelocity(playerDir * pSpeed));
                newBullet.AddComponent(new ComponentHealth(health.Health));
                newBullet.AddComponent(new ComponentDamage(playerDamage));
                
                bulletSound.PlayAudio();
                _entityManager.AddEntity(newBullet, true);
                bulletIndex++;
                _shootCooldown.Start();
            }
        }

        public override void InitializeBinds()
        {
            if (_keyBinds.Count != 0 || _mouseBinds.Count != 0) return;
            
            /* Commented out for testing, default binds
            _keyBinds.Add("MOVE_FORWARD", Key.W);
            _keyBinds.Add("MOVE_BACKWARD", Key.S);
            _keyBinds.Add("MOVE_LEFT", Key.A);
            _keyBinds.Add("MOVE_RIGHT", Key.D);
            _keyBinds.Add("GAME_OVER", Key.M);
                
            _mouseBinds.Add("START_GAME", MouseButton.Left);
            */
        }

        public override void ClearBinds()
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();
        }
        
        public void ResetCooldowns()
        {
            if (_shootCooldown.ElapsedMilliseconds >= 1000)
                _shootCooldown.Reset();
            if (_toggleAICooldown.ElapsedMilliseconds >= 500)
                _toggleAICooldown.Reset();
            if (_toggleCollisionCooldown.ElapsedMilliseconds >= 500)
                _toggleCollisionCooldown.Reset();
        }
        

        
    }
}
