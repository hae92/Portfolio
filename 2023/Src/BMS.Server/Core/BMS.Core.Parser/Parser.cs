using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BMS.Core.Parser
{
    public class Parser
    {
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static string Serialize(String obj)
        {
            JArray jsonArray = JArray.Parse(obj);

            return JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
        }

        public static string[] GetArray(String obj)
        {
            
            JArray jsonArray = JArray.Parse(obj);
            string[] result = new string[jsonArray.Count];

            for (int i = 0; i < jsonArray.Count; i++)
            {
                result[i] = jsonArray[i].ToString();
            }

            return result;
        }

        public static Dictionary<String, String> GetObject(String obj)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();
            JObject jsonObject = JObject.Parse(obj);

            result = jsonObject.Properties()
               .ToDictionary(p => p.Name, p => p.Value.ToString());

            return result;
        }

    }
}