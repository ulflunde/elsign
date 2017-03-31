using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
//using System.Threading.Tasks;
using Signicat.Basic.DocumentActionService;
//using Signicat.Basic.ValidationService;
using Signicat.Basic.ObjectModel;
using Signicat.Basic.DirectDebit;
using Signicat.Basic.Archive;
using Signicat.Basic.ArchiveService;
using Signicat.Basic.Authentication;
using Signicat.Basic.DocumentAction.Specialized;
using Signicat.Basic.DocumentAction;
using Signicat.Basic.Exceptions;
using Signicat.Basic.ObjectModel.Archive.v2;
using Signicat.Basic.ObjectModel.Archive;
using Signicat.Basic.ObjectModel.Authentication;
using Signicat.Basic.ObjectModel.Signature;
using Signicat.Basic.ObjectModel.SignatureOrder.v2;
using Signicat.Basic.ObjectModel.SignatureOrder;
using Signicat.Basic.Service;
using Signicat.Basic.Signature;
using Signicat.Basic.SignatureOrder.v2;
using Signicat.Basic.SignatureOrder;
using Signicat.Basic.Validation;
using Signicat.Basic.Viewer;
using Signicat.Basic;

namespace elsign.Controllers
{
    public class DocumentService
    {
        public const string DEMO_USERNAME = "demo";
        public const string DEMO_PASSWORD = "Bond007";

        public DocumentService()
        {
            return;
        }  // Constructor

        // Use the DocumentService to create a document order request.
        // A document order may contain several documents, tasks and subjects (people).
        // This is a simple example where one subject must sign one document
        // and where the result is a plain NemID SDO (Signed Document Object)
        public void How_to_create_a_simple_document_order_with_one_subject_and_one_document_using_Danish_NemID()
        {
            // The document id is what you get in response when uploading a document to the SDS
            string documentId = "04092013551868wie4tdlw9n8e6s834f3iwm92yq5i8d3gkgqit3vpm6ed";

            /*
            var request = new createrequestrequest
            {
                password = DEMO_PASSWORD,
                service = DEMO_USERNAME,
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
        }

        // After creating a PAdES using the packaging service,
        // the complete PAdES is available
        // for download from the Session Data Storage.
        public async System.Threading.Tasks.Task How_to_download_a_document_from_SDS()
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
                /*
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual("application/x-pades", response.Content.Headers.ContentType.ToString());
                */
            }
        }
    }
}
