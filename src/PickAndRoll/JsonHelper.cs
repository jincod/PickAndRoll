using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PickAndRoll
{
    public static class JsonHelper
    {
        public static Dictionary<string, object> DeserializeAndFlatten(JObject token)
        {
            var dict = new Dictionary<string, object>();
            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in token.Children<JProperty>())
                        FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                    break;

                case JTokenType.Array:
                    var index = 0;
                    foreach (var value in token.Children())
                    {
                        FillDictionaryFromJToken(dict, value, Join(prefix, index.ToString()));
                        index++;
                    }

                    break;

                default:
                    dict.Add(prefix, ((JValue) token).Value);
                    break;
            }
        }

        private static string Join(string prefix, string name)
        {
            return string.IsNullOrEmpty(prefix) ? name : prefix + "." + name;
        }
    }
}