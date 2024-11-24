using System.Collections.Generic;

namespace KarizmaPlatform.Connection.Core.Base
{
    public class Vault
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            data.Add(key, value);
        }

        public void Remove(string key)
        {
            data.Remove(key);
        }

        public TValue Get<TValue>(string key)
        {
            if (data.TryGetValue(key, out var value))
                return (TValue)value;

            return default!;
        }
    }
}