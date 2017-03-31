using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace elsign.Tests
{
    /// <summary>
    /// Summary description for ElectronicSignature_UnitTest
    /// </summary>
    [TestClass]
    public class ElectronicSignature_UnitTest
    {
        public ElectronicSignature_UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        // Use the DocumentService to create a document order request.
        // A document order may contain several documents, tasks and subjects (people).
        // This is a simple example where one subject must sign one document
        // and where the result is a plain NemID SDO (Signed Document Object)
        [TestMethod]
        public void How_to_create_a_simple_document_order_with_one_subject_and_one_document_using_Danish_NemID()
        {
            // The document id is what you get in response when uploading a document to the SDS
            string documentId = "04092013551868wie4tdlw9n8e6s834f3iwm92yq5i8d3gkgqit3vpm6ed";
            /*
            var request = new createrequestrequest
            {
                password = "Bond007",
                service = "demo",
                request = new request[]
                 {
             new request
             {
                 clientreference = "cliref1",
                 language = "da",
                 profile = "demo",
                 document = new document[]
                 {
                     new sdsdocument
                     {
                         id = "doc_1",
                         refsdsid = documentId,
                         description = "Terms and conditions"
                     }
                 },
                 subject = new subject[]
                 {
                     new subject
                     {
                         id = "subj_1",
                         nationalid = "1909740939"
                     }
                 },
                 task = new task[]
                 {
                     new task
                     {
                         id = "task_1",
                         subjectref = "subj_1",
                         bundleSpecified = true,
                         bundle = false,
                         documentaction = new documentaction[]
                         {
                              new documentaction
                              {
                                  type = documentactiontype.sign,
                                  documentref = "doc_1"
                              }
                         },
                         signature = new signature[]
                         {
                             new signature
                             {
                                 responsiveSpecified=true,
                                 responsive = true,
                                 method = new method[]
                                 {
                                     new method
                                         {
                                            value = "nemid-sign"
                                         }
                                 }
                             }
                         }
                     }
                 }
             }
                 }
            };
            createrequestresponse response;
            using (var client = new DocumentEndPointClient())
            {
                response = client.createRequest(request);
            }
            String signHereUrl =
                String.Format("https://preprod.signicat.com/std/docaction/demo?request_id={0}&task_id={1}", response.requestid[0], request.request[0].task[0].id);
            Console.WriteLine(signHereUrl);
            Assert.IsNotNull(response);
            Assert.IsNull(response.artifact);
            Assert.IsNotNull(response.requestid);
            */
            return;
        }  // How_to_create_a_simple_document_order_with_one_subject_and_one_document_using_Danish_NemID()

        // After creating a PAdES using the packaging service,
        // the complete PAdES is available
        // for download from the Session Data Storage.
        [TestMethod]
        public async Task How_to_download_a_document_from_SDS()
        {
            string padesDocumentId = "300820131anomvdrt18vhkajyo1n00891l21i449i7tt5n2fqu911bkmh4";
            var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("demo", "Bond007") };

            using (var client = new HttpClient(httpClientHandler))
            {
                HttpResponseMessage response = await client.GetAsync("https://preprod.signicat.com/doc/demo/sds/" + padesDocumentId);
                byte[] pades = await response.Content.ReadAsByteArrayAsync();
                string filename = padesDocumentId + "_" + DateTime.Now.ToString("yyMMdd_HHmmss") + ".pdf";
                File.WriteAllBytes(filename, pades);


                Console.WriteLine(Directory.GetCurrentDirectory() + "\\" + filename);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("application/x-pades", response.Content.Headers.ContentType.ToString());
            }
        }  // How_to_download_a_document_from_SDS()

    }  // class ElectronicSignature_UnitTest
}
