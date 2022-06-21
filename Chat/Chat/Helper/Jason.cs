using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Helper
{
    public class Jason<T>
    {
        public T Deserialize(string obj)
        {
           return JsonConvert.DeserializeObject<T>(obj);
        }

        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
