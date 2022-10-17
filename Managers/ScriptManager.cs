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
       {}

       public void readJSONScript(string pScriptName, EntityManager pEntityManager)
       {
           JsonSerializerSettings jss = new JsonSerializerSettings();
           jss.TypeNameHandling = TypeNameHandling.All;

           JsonSerializer serializer = new JsonSerializer();

           JsonTextReader reader = new JsonTextReader(new StreamReader(pScriptName));
           reader.SupportMultipleContent = true;

           while (true)
           {
               if (!reader.Read())
               {
                   break;
               }

               JObject obj = (Newtonsoft.Json.Linq.JObject)serializer.Deserialize(reader);
               
               JToken token = obj.SelectToken("Name");
               Entity newEntity = new Entity(token.ToString());

               token = obj.SelectToken("Components");
               JArray jsonComponents = JArray.Parse(token.ToString());

               foreach (var componentType in Enum.GetNames(typeof(ComponentTypes)))
               {
                   foreach (JObject jsonComponent in jsonComponents)
                   {
                       foreach (JProperty property in jsonComponent.Properties())
                       {
                           if (property.Name != componentType)
                               continue;

                           newEntity.AddComponent(GetComponent(componentType, (string) property.Value)); 
                       }
                   }
               }
               pEntityManager.AddEntity(newEntity);
           }
       }

       private IComponent GetComponent(string pComponentType, string pComponentValue)
       {
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