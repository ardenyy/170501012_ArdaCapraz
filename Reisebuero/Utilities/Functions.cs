using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Reisebuero.Utilities
{
    public static class Functions
    {
        public static T CopyObject<T>(T obj)
        {
            string serialized = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(serialized);
        }
    }
}
