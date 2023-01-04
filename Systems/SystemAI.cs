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
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AI);
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
            // Fix moving on to next node
            if (pAI.IsMoving)
            {
                pAI.DistanceTravelled = pPosition.Position - pAI.StartPos;
                
                if (pAI.DistanceTravelled.X >= pAI.DistanceToMove.X && pAI.DistanceTravelled.Y >= pAI.DistanceToMove.Y && pAI.DistanceTravelled.Z >= pAI.DistanceToMove.Z)
                    pAI.IsMoving = false;
            }
            
            if (!pAI.IsMoving)
            {
                pAI.StartPos = pPosition.Position;
                pAI.DistanceToMove = pAI.Positions[pAI.LocationIndex] - pPosition.Position;
                pDirection.Direction = (pAI.Positions[pAI.LocationIndex] - pPosition.Position).Normalized();
                pVelocity.Velocity = pDirection.Direction;
                
                if (pAI.LocationIndex == pAI.Positions.Count - 1)
                    pAI.LocationIndex = 0;
                else
                    pAI.LocationIndex++;
                pAI.IsMoving = true;
            }
        }
    }
}