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
        
        public Dictionary<string, Key> _keyBinds;
        public Dictionary<string, MouseButton> _mouseBinds;
        
        private Vector3 _previousPos;

        private SceneManager _sceneManager;
        private EntityManager _entityManager;

        public GameInputManager(EntityManager pEntityManager, SceneManager pSceneManager)
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();
            _sceneManager = pSceneManager;
            _entityManager = pEntityManager;
            
            _shootCooldown = new Stopwatch();
        }

        public override void ReadInput(Entity pEntity)
        {
            KeyboardState keyState = Keyboard.GetState();
            ResetCooldowns();
            
            foreach (KeyValuePair<string, Key> kvp in _keyBinds)
            {
                if (keyState.IsKeyDown(kvp.Value))
                {
                    if (pEntity == null)
                        HandleSceneInput(kvp.Key);
                    else
                        HandleEntityInput(kvp.Key, pEntity);
                }
            }

            MouseState mouseState = Mouse.GetState();

            foreach (KeyValuePair<string, MouseButton> kvp in _mouseBinds)
            {
                if (mouseState.IsButtonDown(kvp.Value))
                {
                    if (pEntity == null)
                        HandleSceneInput(kvp.Key);
                    else
                        HandleEntityInput(kvp.Key, pEntity);
                }
            }
        }

        public override void HandleEntityInput(string pAction, Entity pEntity)
        {
            ExtractComponents(pEntity, out var playerPosComponent, out var playerDirComponent);

            // camera dependant actions
            switch (pAction)
            {
                case "MOVE_FORWARD":
                    _previousPos = playerPosComponent.Position;
                    playerPosComponent.Position += (playerDirComponent.Direction * 4) * GameScene.dt;
                    break;
                case "MOVE_BACKWARD":
                    _previousPos = playerPosComponent.Position;
                    playerPosComponent.Position += -(playerDirComponent.Direction * 4) * GameScene.dt;
                    break;
                case "MOVE_LEFT":
                    playerDirComponent.Direction = Matrix3.CreateRotationY(-0.03f) * playerDirComponent.Direction;
                    break;
                case "MOVE_RIGHT":
                    playerDirComponent.Direction = Matrix3.CreateRotationY(0.03f) * playerDirComponent.Direction;
                    break;
                case "SHOOT":
                    Shoot(pEntity, 40.0f);
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
                case "GAME_OVER":
                    _sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
                    break;
                case "CLOSE_GAME":
                    _sceneManager.Close();
                    break;
            }
        }

        public void Shoot(Entity pEntity, float pSpeed)
        {
            if (_shootCooldown.ElapsedMilliseconds == 0)
            {
                ExtractComponents(pEntity, out var playerPosComponent, out var playerDirComponent);
                var playerPos = playerPosComponent.Position;
                var playerDir = playerDirComponent.Direction;

                // Make a copy of the saved bullet
                Entity storedBullet = _entityManager.FindRenderableEntity(bulletName);
                Entity newBullet = new Entity($"Bullet{bulletIndex}");

                foreach (var c in storedBullet.Components)
                {
                    if (c.GetType() == typeof(ComponentHealth))
                        continue;
                    
                    IComponent componentCopy = c;
                    newBullet.AddComponent(componentCopy);
                }
                
                IComponent healthComponent = pEntity.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                });
                
                ComponentHealth health = (ComponentHealth) healthComponent;
                
                // Spawn bullet in front of player with camera direction as velocity
                newBullet.AddComponent(new ComponentPosition(playerPos + playerDir * 3));
                newBullet.AddComponent(new ComponentVelocity(playerDir * pSpeed));
                newBullet.AddComponent(new ComponentHealth(health.Health));
                
                IComponent audioComponent = newBullet.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio = (ComponentAudio) audioComponent;
                audio.PlayAudio();

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
        }
        
        private void ExtractComponents(Entity pEntity, out ComponentPosition playerPos, out ComponentDirection playerDir)
        {
            List<IComponent> components = pEntity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            playerPos = (ComponentPosition)positionComponent;

            IComponent directionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
            });
            playerDir = (ComponentDirection)directionComponent;
        }
    }
}
