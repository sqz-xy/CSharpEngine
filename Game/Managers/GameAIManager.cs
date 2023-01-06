using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenTK;

namespace OpenGL_Game.Managers
{
    public class GameAIManager : AIManager
    {
        public override void Move(Entity pEntity)
        {
            var ai = ComponentHelper.GetComponent<ComponentPathFollowAI>(pEntity, ComponentTypes.COMPONENT_AI);
            var velocity = ComponentHelper.GetComponent<ComponentVelocity>(pEntity, ComponentTypes.COMPONENT_VELOCITY);
            var position = ComponentHelper.GetComponent<ComponentPosition>(pEntity, ComponentTypes.COMPONENT_POSITION);
            var direction = ComponentHelper.GetComponent<ComponentDirection>(pEntity, ComponentTypes.COMPONENT_DIRECTION);
            
            // If the ai isn't active, don't move
            if (!ai.IsActive)
            {
                velocity.Velocity = Vector3.Zero;
                ai.IsMoving = false;
                return;
            }
            
            // If the entity is already moving
            if (ai.IsMoving)
            {
                // Check distance travelled 
                ai.DistanceTravelled = position.Position - ai.StartPos;
                
                // If we have gone far enough, we no longer need to move
                if (ai.DistanceTravelled.X >= ai.DistanceToMove.X && ai.DistanceTravelled.Y >= ai.DistanceToMove.Y && ai.DistanceTravelled.Z >= ai.DistanceToMove.Z)
                    ai.IsMoving = false;
            }
            
            // If the entity isn't moving
            if (!ai.IsMoving)
            {
                // Set start position
                ai.StartPos = position.Position;

                // Figure out how far we need to move to get to next location
                ai.DistanceToMove = ai.Positions[ai.LocationIndex] - position.Position;
                direction.Direction = (ai.Positions[ai.LocationIndex] - position.Position).Normalized();
                velocity.Velocity = direction.Direction;

                // Set the index of the next location, if you have got to the end of the list go back to the start
                if (ai.LocationIndex == ai.Positions.Count - 1)
                    ai.LocationIndex = 0;
                else
                    ai.LocationIndex++;

                // We are now moving
                ai.IsMoving = true;
            }
        }
    }
}