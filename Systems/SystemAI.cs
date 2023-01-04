using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenGL_Game.Systems
{
    public class SystemAI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI) | ComponentTypes.COMPONENT_DIRECTION | ComponentTypes.COMPONENT_VELOCITY;
        private bool _initialized = false;
        
        public string Name
        {
            get { return "SystemAI"; }
        }

        // Start is position of the drone
        // Destination is player
        
        public void OnAction(List<Entity> pEntity)
        {
            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var ai = ComponentHelper.GetComponent<ComponentAI>(entity, ComponentTypes.COMPONENT_AI);
                    var velocity = ComponentHelper.GetComponent<ComponentVelocity>(entity, ComponentTypes.COMPONENT_VELOCITY);
                    var position = ComponentHelper.GetComponent<ComponentPosition>(entity, ComponentTypes.COMPONENT_POSITION);
                    var direction = ComponentHelper.GetComponent<ComponentDirection>(entity, ComponentTypes.COMPONENT_DIRECTION);
                    Move(ref ai, ref velocity, ref position, ref direction);
                }
        }
        
        private void Move(ref ComponentAI pAI, ref ComponentVelocity pVelocity, ref ComponentPosition pPosition, ref ComponentDirection pDirection)
        {
            // If the ai isn't active, don't move
            if (!pAI.IsActive)
            {
                pVelocity.Velocity = Vector3.Zero;
                pAI.IsMoving = false;
                return;
            }
            
            // If the entity is already moving
            if (pAI.IsMoving)
            {
                // Check distance travelled 
                pAI.DistanceTravelled = pPosition.Position - pAI.StartPos;
                
                // If we have gone far enough, we no longer need to move
                if (pAI.DistanceTravelled.X >= pAI.DistanceToMove.X && pAI.DistanceTravelled.Y >= pAI.DistanceToMove.Y && pAI.DistanceTravelled.Z >= pAI.DistanceToMove.Z)
                    pAI.IsMoving = false;
            }
            
            // If the entity isn't moving
            if (!pAI.IsMoving)
            {
                // Set start position
                pAI.StartPos = pPosition.Position;

                // Figure out how far we need to move to get to next location
                pAI.DistanceToMove = pAI.Positions[pAI.LocationIndex] - pPosition.Position;
                pDirection.Direction = (pAI.Positions[pAI.LocationIndex] - pPosition.Position).Normalized();
                pVelocity.Velocity = pDirection.Direction;
                
                // Set the index of the next location, if you have got to the end of the list go back to the start
                if (pAI.LocationIndex == pAI.Positions.Count - 1)
                    pAI.LocationIndex = 0;
                else
                    pAI.LocationIndex++;
                
                // We are now moving
                pAI.IsMoving = true;
            }
        }
    }
}