using System.Collections.Generic;

namespace App.Infrastructure.Cassette
{
    public static class MetaData
    {
        static readonly Dictionary<object, Dictionary<string, object>> objectsWithMetadata =
            new Dictionary<object, Dictionary<string, object>>();

        public static void SetMetaData(this object obj, string key, object value)
        {
            Dictionary<string, object> metadata;
            if (!objectsWithMetadata.TryGetValue(obj, out metadata))
            {
                metadata = new Dictionary<string, object>();
                objectsWithMetadata[obj] = metadata;
            }

            metadata[key] = value;
        }

        public static bool TryGetMetaData<T>(this object obj, string key, out T output)
        {
            Dictionary<string, object> metadata;
            object value;
            if (objectsWithMetadata.TryGetValue(obj, out metadata) &&
                metadata.TryGetValue(key, out value))
            {
                output = (T)value;
                return true;
            }
            else
            {
                output = default(T);
                return false;
            }
        }

        public static T GetMetaData<T>(this object obj, string key)
        {
            return (T)objectsWithMetadata[obj][key];
        }

        public static T GetMetaDataOrDefault<T>(this object obj, string key, T defaultValue)
        {
            Dictionary<string, object> metadata;
            if (objectsWithMetadata.TryGetValue(obj, out metadata))
            {
                object result;
                return metadata.TryGetValue(key, out result) ? (T) result : defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}