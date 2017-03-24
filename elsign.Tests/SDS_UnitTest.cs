using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace elsign.Tests
{
    [TestClass]
    public class SDS_UnitTest
    {
        [TestMethod]
        public async Task SessionDataStorage_Upload()
        {
            var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("demo", "Bond007") };
            using (var client = new HttpClient(httpClientHandler))
            {
                HttpContent content = new ByteArrayContent(File.ReadAllBytes("C:/Users/E214595/Source/Repos/ElectronicSignatures/elsign/wwwroot/data/dummy-document.pdf"));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                HttpResponseMessage response =
                    await client.PostAsync("https://preprod.signicat.com/doc/demo/sds", content);
                string documentId = await response.Content.ReadAsStringAsync();

                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Assert.IsTrue(documentId.Length > 0);
            }
        }

        [TestMethod]
        public void SessionDataStorage_Download()
        {
        }
    }
}
