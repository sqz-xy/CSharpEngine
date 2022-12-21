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
        public static string bulletName = "Bullet";
        private int bulletIndex = 0;

        private Stopwatch _shootCooldown;
        
        public Dictionary<string, Key> _keyBinds;
        public Dictionary<string, MouseButton> _mouseBinds;

        private bool _spectating = false;

        private Vector3 _playerCameraPos;
        private Vector3 _playerCameraTarget;

        public GameInputManager()
        {
            _keyBinds = new Dictionary<string, Key>();
            _mouseBinds = new Dictionary<string, MouseButton>();

            _shootCooldown = new Stopwatch();
        }

        public override void ReadInput(SceneManager pSceneManager, Camera pCamera, EntityManager pEntityManager)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyValuePair<string, Key> kvp in _keyBinds)
            {
                if (keyState.IsKeyDown(kvp.Value))
                {
                    HandleInput(kvp.Key, pSceneManager, pCamera, pEntityManager);
                }
            }

            MouseState mouseState = Mouse.GetState();

            foreach (KeyValuePair<string, MouseButton> kvp in _mouseBinds)
            {
                if (mouseState.IsButtonDown(kvp.Value))
                {
                    HandleInput(kvp.Key, pSceneManager, pCamera, pEntityManager);
                }
            }
        }

        public override void HandleInput(string pAction, SceneManager pSceneManager, Camera pCamera, EntityManager pEntityManager)
        {
            ResetCooldowns();
           
            // Non camera dependant actions
            switch (pAction)
            {
                case "START_GAME":
                    pSceneManager.ChangeScene(SceneTypes.SCENE_GAME);
                    break;
                case "GAME_OVER":
                    pSceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
                    break;
                case "CLOSE_GAME":
                    pSceneManager.Close();
                    break;
            }

            if (pCamera == null)
                return;

            // camera dependant actions
            switch (pAction)
            {
                case "MOVE_FORWARD":
                    pCamera.previousPos = pCamera.cameraPosition;
                    pCamera.MoveForward(0.1f);
                    break;
                case "MOVE_BACKWARD":
                    pCamera.previousPos = pCamera.cameraPosition;
                    pCamera.MoveForward(-0.1f);
                    break;
                case "MOVE_LEFT":
                    pCamera.RotateY(-0.03f);
                    break;
                case "MOVE_RIGHT":
                    pCamera.RotateY(0.03f);
                    break;
                case "SPECTATE":
                    Spectate(pCamera);
                    pCamera.UpdateView();
                    break;
                case "LEAVE_SPECTATE":
                    LeaveSpectate(pCamera);
                    pCamera.UpdateView();
                    break;
                case "SHOOT":
                    if (!_spectating)
                        Shoot(pEntityManager, pCamera, 40.0f);
                    break;
            }
        }

        private void LeaveSpectate(Camera pCamera)
        {
            if (_spectating)
            {
                pCamera.cameraPosition = _playerCameraPos;
                pCamera.targetPosition = _playerCameraTarget;
                _spectating = false;
            }
        }

        private void Spectate(Camera pCamera)
        {
            if (!_spectating)
            {
                _playerCameraPos = pCamera.cameraPosition;
                _playerCameraTarget = pCamera.targetPosition;
                pCamera.cameraPosition = new Vector3(0, 4, 7);
                pCamera.targetPosition = new Vector3(0, 0, 0);
                _spectating = true;
            }
        }
       

        public void Shoot(EntityManager pEntityManager, Camera pCamera, float pSpeed)
        {
            if (_shootCooldown.ElapsedMilliseconds == 0)
            {
                // Make a copy of the saved bullet
                Entity storedBullet = pEntityManager.FindRenderableEntity(bulletName);
                Entity newBullet = new Entity($"{bulletName}{bulletIndex}");

                foreach (var c in storedBullet.Components)        
                    newBullet.AddComponent(c);
            
                // Spawn bullet in front of player with camera direction as velocity
                newBullet.AddComponent(new ComponentPosition(pCamera.cameraPosition + pCamera.cameraDirection * 3));
                newBullet.AddComponent(new ComponentVelocity(pCamera.cameraDirection * pSpeed));
                
                IComponent audioComponent = newBullet.Components.Find(delegate(IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio = (ComponentAudio) audioComponent;
                audio.PlayAudio();

                pEntityManager.AddEntity(newBullet, true);
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
    }
}
