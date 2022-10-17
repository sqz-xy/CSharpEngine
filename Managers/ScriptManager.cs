using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using OpenTK;

namespace OpenGL_Game.Managers
{
    class ScriptManager
    {
        public ScriptManager()
        {
            // Set the json serializer settings
            JsonSerializerSettings jss = new JsonSerializerSettings();
            jss.TypeNameHandling = TypeNameHandling.All;
        }

       /// <summary>
       /// Reads a json script to create entities and their components
       /// </summary>
       /// <param name="pScriptName">The name of the script</param>
       /// <param name="pEntityManager">The entity manager</param>
       public void ReadJsonScript(string pScriptName, ref EntityManager pEntityManager)
       {
           JsonSerializer serializer = new JsonSerializer();
           JsonTextReader reader = new JsonTextReader(new StreamReader(pScriptName));
           // Support multiple objects
           reader.SupportMultipleContent = true;

           // For each object
           while (true)
           {
               if (!reader.Read())
               {
                   break;
               }

               JObject obj = (Newtonsoft.Json.Linq.JObject)serializer.Deserialize(reader);
               
               // Get the name of the object
               JToken token = obj.SelectToken("Name");
               Entity newEntity = new Entity(token.ToString());

               // Get the components
               token = obj.SelectToken("Components");
               JArray jsonComponents = JArray.Parse(token.ToString());

               // Compare json components with the entity components
               foreach (var componentType in Enum.GetNames(typeof(ComponentTypes)))
               {
                   foreach (JObject jsonComponent in jsonComponents)
                   {
                       foreach (JProperty property in jsonComponent.Properties())
                       {
                           if (property.Name != componentType)
                               continue;

                           // If the entity component matches the json component, convert it from json to an entity component
                           newEntity.AddComponent(GetComponent(componentType, (string) property.Value)); 
                       }
                   }
               }
               pEntityManager.AddEntity(newEntity);
           }
       }

       /// <summary>
       /// Converts component strings into component objects
       /// </summary>
       /// <param name="pComponentType">The type of component to create</param>
       /// <param name="pComponentValue">The values for the component</param>
       /// <returns>An IComponent object</returns>
       private IComponent GetComponent(string pComponentType, string pComponentValue)
       {
           // May need to be changed in the future depending on the values of future component types
           // For each component type, return the component object, for components requiring vectors, split and format the string
           float[] values = new float[3];
           switch (pComponentType)
           {
               case "COMPONENT_POSITION":
                   values = Array.ConvertAll(pComponentValue.Split(' '), float.Parse);
                   return new ComponentPosition(new Vector3(values[0], values[1], values[2]));
               break;
               case "COMPONENT_GEOMETRY":
                   return new ComponentGeometry(pComponentValue);
               break;
               case "COMPONENT_TEXTURE":
                   return new ComponentTexture(pComponentValue);
               break;
               case "COMPONENT_VELOCITY":
                   values = Array.ConvertAll(pComponentValue.Split(' '), float.Parse);
                   return new ComponentVelocity(new Vector3(values[0], values[1], values[2]));
               break;
               default:
                   return null;
           }
       }
    }
}