using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;


namespace elsign.Controllers
{
    public class DocumentMetadata
    {
        public const string DEMO_USERNAME = "demo";
        public const string DEMO_PASSWORD = "Bond007";

        private const string KEY_LATEST_DOCUMENT_ID = "latest document ID";
        private const string KEY_LATEST_FILENAME = "latest filename";

        public static string StoreFilename(string name, IMemoryCache cache)
        {
            cache.Set(KEY_LATEST_FILENAME, name, new TimeSpan(0, 30, 0));
            return name;
        }

        public static string StoreDocumentID(string documentId, IMemoryCache cache)
        {
            cache.Set(KEY_LATEST_DOCUMENT_ID, documentId, new TimeSpan(0, 30, 0));
            return documentId;
        }

        public static string GetLastFilename(IMemoryCache cache)
        {
            string fname = null;

            if (cache.TryGetValue(KEY_LATEST_FILENAME, out fname))
            {
                cache.Remove(KEY_LATEST_FILENAME);
            }

            return fname;
        }

        public static string GetLastDocumentID(IMemoryCache cache)
        {
            string documentId = null;

            if (cache.TryGetValue(KEY_LATEST_DOCUMENT_ID, out documentId))
            {
                cache.Remove(KEY_LATEST_DOCUMENT_ID);
            }

            return documentId;
        }

    }
}
