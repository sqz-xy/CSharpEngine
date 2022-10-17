using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

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
               
               JToken token = obj.SelectToken("name");
               Entity newEntity = new Entity(token.ToString());

               var test = Enum.GetNames(typeof(ComponentTypes));
               // foreach (var componentType in Enum.GetNames(typeof(ComponentTypes)))
               //{

               //}
           }
       }
    }
}