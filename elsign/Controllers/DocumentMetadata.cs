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

        private const string KEY_DOCUMENTLIST = "document list";
        private const string KEY_LATEST_DOCUMENT_ID = "latest document ID";
        private const string KEY_LATEST_FILENAME = "latest filename";

        private class DocMetaData
        {
            public int filesize;
            public string filename;
            public string signicatDocumentId;

            /// <summary>
            /// Constructor (quick initializer).
            /// </summary>
            /// <param name="name"></param>
            /// <param name="size"></param>
            /// <param name="id"></param>
            public DocMetaData(string name, int size, string id)
            {
                filesize = size;
                filename = name;
                signicatDocumentId = id;
            }
        }  // class DocMetaData

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

        /// <summary>
        /// Fetch the last stored filename from the cache, and clears it from the cache.
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
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

        public static void AddToStoredDocuments(IMemoryCache cache, string filename, int filesize, string documentId)
        {
            List<DocMetaData> documentList;
            DocMetaData newDocument = new DocMetaData(filename, filesize, documentId);

            if (cache.TryGetValue(KEY_DOCUMENTLIST, out documentList))
            {
                cache.Remove(KEY_DOCUMENTLIST);
            }
            if (documentList == null)
            {
                documentList = new List<DocMetaData>();
            }
            documentList.Add(newDocument);
            cache.Set(KEY_DOCUMENTLIST, documentList, new TimeSpan(0, 30, 0));

            return;
        }

        public static string[] GetDocumentlist(IMemoryCache cache)
        {
            List<DocMetaData> documentList;
            string[] filelist = null;
            string[] idlist = null;

            if (cache.TryGetValue(KEY_DOCUMENTLIST, out documentList))
            {
                filelist = new string[documentList.Count];
                idlist = new string[filelist.Length];

                for (int i=0; i < filelist.Length; i++)
                {
                    filelist[i] = documentList[i].filename;
                    idlist[i] = documentList[i].signicatDocumentId;
                }
            }

            return filelist;
        }

    }  // class DocumentMetadata
}
