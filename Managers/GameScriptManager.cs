using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenGL_Game.OBJLoader;

using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace OpenGL_Game.Managers
{
    public class GameScriptManager : ScriptManager
    {
        public GameScriptManager()
        {
            // Set the json serializer settings
            var jss = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

       /// <summary>
       /// Reads a json script to create entities and their components
       /// </summary>
       /// <param name="pScriptName">The name of the script</param>
       /// <param name="pEntityManager">The entity manager</param>
       public override void LoadEntities(string pScriptName, ref EntityManager pEntityManager)
       {
           var serializer = new JsonSerializer();
           var reader = new JsonTextReader(new StreamReader(pScriptName));
           reader.SupportMultipleContent = true;            // Support multiple objects
           
           // For each object
           while (true)
           {
               if (!reader.Read())
                   break;

               JObject obj;
               try
               {
                   obj = (JObject) serializer.Deserialize(reader);
               }
               catch  (JsonSerializationException e)
               {
                   Console.WriteLine(e.Message);
                   return;
               }
               
               // Get the name of the object
               var token = obj.SelectToken("Name");
               if (token == null) {continue; }
               var newEntity = new Entity(token.ToString());

                // isRenderable?
                token = obj.SelectToken("IsRenderable");
                if (token == null) { continue; }
                var isRenderable = bool.Parse(token.ToString());

                // Get the components
                token = obj.SelectToken("Components");
                if (token == null) {continue; }
                var jsonComponents = JArray.Parse(token.ToString());

               
               // Compare json components with the entity components
               foreach (var componentType in Enum.GetNames(typeof(ComponentTypes)))
               {
                   foreach (JObject jsonComponent in jsonComponents)
                   {
                       foreach (var property in jsonComponent.Properties())
                       {
                           if (property.Name != componentType)
                               continue;

                           // If the entity component matches the json component, convert it from json to an entity component
                           newEntity.AddComponent(GetComponent(componentType, (string) property.Value)); 
                       }
                   }
               }

                pEntityManager.AddEntity(newEntity, isRenderable);
           }
       }

        /// <summary>
        /// Converts component strings into component objects
        /// </summary>
        /// <param name="pComponentType">The type of component to create</param>
        /// <param name="pComponentValue">The values for the component</param>
        /// <returns>An IComponent object</returns>
        public override IComponent GetComponent(string pComponentType, string pComponentValue)
        {
            // May need to be changed in the future depending on the values of future component types
            // For each component type, return the component object, for components requiring vectors, split and format the string
            // Are components part of the game or part of the engine because if they are game then I cant hard code them like this
            // If they cant be hard coded then change the script to use the object names and then use Activator.CreateInstance (Requires empty constructor)
            // Then modify components to accept data through a separate method, outlined in IComponent "public void AddData(String Data)"
            switch (pComponentType)
            {
                case "COMPONENT_POSITION":
                    var posValues = Array.ConvertAll(pComponentValue.Split(' '), float.Parse);
                    return new ComponentPosition(new Vector3(posValues[0], posValues[1], posValues[2]));
                case "COMPONENT_GEOMETRY":
                    return new ComponentGeometry(pComponentValue);
                case "COMPONENT_CONTROLLABLE":
                    return new ComponentControllable(bool.Parse(pComponentValue));
                case "COMPONENT_TEXTURE":
                    return new ComponentTexture(pComponentValue);
                case "COMPONENT_VELOCITY":
                    var velValues = Array.ConvertAll(pComponentValue.Split(' '), float.Parse);
                    return new ComponentVelocity(new Vector3(velValues[0], velValues[1], velValues[2]));
                case "COMPONENT_DIRECTION":
                    var dirValues = Array.ConvertAll(pComponentValue.Split(' '), float.Parse);
                    return new ComponentDirection(new Vector3(dirValues[0], dirValues[1], dirValues[2]));
                case "COMPONENT_AUDIO":
                    var audioValues = pComponentValue.Split(' ');
                    return new ComponentOpenALAudio(audioValues[0], bool.Parse(audioValues[1]));
                case "COMPONENT_COLLISION_SPHERE":
                    return new ComponentCollisionSphere(float.Parse(pComponentValue));
                case "COMPONENT_HEALTH":
                    return new ComponentHealth(int.Parse(pComponentValue));
                case "COMPONENT_COLLISION_AABB":
                    var AABValues = pComponentValue.Split(' ');
                    return new ComponentCollisionAABB(float.Parse(AABValues[0]), float.Parse(AABValues[1]), float.Parse(AABValues[2]));
                case "COMPONENT_DAMAGE":
                    return new ComponentDamage(int.Parse(pComponentValue));
                case "COMPONENT_AI":
                    AICostTypes costType;
                    var aiValues = pComponentValue.Split(' ');
                    Enum.TryParse(aiValues[0], out costType);
                    return new ComponentAI(costType, bool.Parse(aiValues[1]));
                case "COMPONENT_SPEED":
                    return new ComponentSpeed(float.Parse(pComponentValue));
                case "COMPONENT_SHADER":
                    var shaderValues = pComponentValue.Split(' ');
                    switch (shaderValues[0])
                    {
                        case "Default":
                            return new ComponentShaderDefault();
                        case "NoLights":
                            return new ComponentShaderNoLights(shaderValues[1], shaderValues[2]);
                        case "Billboard":
                            return new ComponentShaderBillboard(shaderValues[1], shaderValues[2]);
                        default: 
                            return new ComponentShaderDefault();
                    }
                default:
                    return null;
            }
        }

        public override void LoadControls(string pScriptName, ref GameInputManager pInputManager)
        {
            var serializer = new JsonSerializer();
            var reader = new JsonTextReader(new StreamReader(pScriptName));
            reader.SupportMultipleContent = true;            // Support multiple objects

            // For each object
            while (true)
            {
                if (!reader.Read())
                    break;
                
                JObject obj;
                try
                {
                    obj = (JObject)serializer.Deserialize(reader);
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                
                // Get the name of the object
                var token = obj.SelectToken("Controls");
                if (token == null) { continue; }
                var jsonControls = JArray.Parse(token.ToString());

                foreach (JObject jsonControl in jsonControls)
                {
                    foreach (var property in jsonControl.Properties())
                    {
                        GetControls(property.Name, (string)property.Value, ref pInputManager);
                    }
                }
            }
        }    
        
        public override void GetControls(string pAction, string pBind, ref GameInputManager pInputManager)
        {
           // Reset binds, error checking, look for enum mapping function
           if (pBind.Split('.')[0] != "MouseButton")
           {
                Enum.TryParse(pBind, out Key keyBind);
                pInputManager._keyBinds.Add(pAction, keyBind);
           }
           else
           {
                Enum.TryParse(pBind, out MouseButton mouseBind);
                pInputManager._mouseBinds.Add(pAction, mouseBind);
           }
        }
    }
}