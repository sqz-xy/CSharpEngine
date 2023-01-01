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
            List<Entity> nodes = new List<Entity>();
            List<Entity> drones = new List<Entity>();
            Entity destination;

            foreach (var entity in pEntity)
                if ((entity.Mask & MASK) == MASK)
                {
                    var ai = ComponentHelper.GetComponent<ComponentAI>(entity, ComponentTypes.COMPONENT_AI);

                    switch (ai.NodeType)
                    {
                        case AINodeType.DESTINATION:
                            destination = entity;
                            break;
                        case AINodeType.DRONE:
                            drones.Add(entity);
                            break;
                        case  AINodeType.NODE:
                            nodes.Add(entity);
                            break;
                    }
                }
            
           // if (!_initialized)
               // InitialiseAI(ref nodes, ref drones);
        }

        private void InitialiseAI(ref List<Entity> pNodes, ref List<Entity> pDrones)
        {
            foreach (var drone in pDrones)
            {
                var dronePos = ComponentHelper.GetComponent<ComponentPosition>(drone, ComponentTypes.COMPONENT_POSITION);
                
                foreach (var node in pNodes)
                {
                    var closestNode = node;
                    var nodePos = ComponentHelper.GetComponent<ComponentPosition>(closestNode, ComponentTypes.COMPONENT_POSITION);
                    foreach (var nodeToCompare in pNodes)
                    {
                        var compareNodePos = ComponentHelper.GetComponent<ComponentPosition>(nodeToCompare, ComponentTypes.COMPONENT_POSITION);

                        if ((compareNodePos.Position - dronePos.Position).Length < (nodePos.Position - dronePos.Position).Length)
                            closestNode = nodeToCompare;
                    }

                    dronePos.Position = nodePos.Position;
                }
            }
        }
        
        private void CalculateCosts(ref List<Entity> pNodes)
        {

        }
    }
}