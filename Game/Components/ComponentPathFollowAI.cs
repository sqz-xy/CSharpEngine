using System;
using System.Collections.Generic;
using OpenTK;

namespace OpenGL_Game.Components
{
    public class ComponentPathFollowAI : ComponentAI
    {
        private List<Vector3> _positions;
        private Vector3 _distanceToMove;
        private Vector3 _distanceTravelled;
        private Vector3 _startPos;
        private int _locationIndex = 0;
        
        public ComponentPathFollowAI(List<Vector3> pPositions)
        {
            _positions = pPositions;
        }
        
        public List<Vector3> Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }
        
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        
        public int LocationIndex
        {
            get { return _locationIndex; }
            set { _locationIndex = value; }
        }
        
        public Vector3 DistanceToMove
        {
            get { return _distanceToMove; }
            set {                 
                float x = (float)Math.Sqrt(value.X * value.X);
                float y = (float)Math.Sqrt(value.Y * value.Y);
                float z = (float)Math.Sqrt(value.Z * value.Z);
                _distanceToMove = new Vector3(x, y, z);  
            }
        }
        
        public Vector3 DistanceTravelled
        {
            get { return _distanceTravelled; }
            set { 
                float x = (float)Math.Sqrt(value.X * value.X);
                float y = (float)Math.Sqrt(value.Y * value.Y);
                float z = (float)Math.Sqrt(value.Z * value.Z);
                _distanceTravelled = new Vector3(x, y, z); 
            }
        }
        
        public Vector3 StartPos
        {
            get { return _startPos; }
            set { _startPos = value; }
        }

        
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AI; }
        }
        
    }
}