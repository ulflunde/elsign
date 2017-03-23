using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;


namespace elsign.Controllers
{
    public class DocumentMetadata
    {
        private const string KEY_LATEST_DOCUMENT_ID = "latest document ID";

        public static string StoreDocumentID(string documentId, IMemoryCache cache)
        {
            cache.Set(KEY_LATEST_DOCUMENT_ID, documentId, new TimeSpan(0, 30, 0));
            return documentId;
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
