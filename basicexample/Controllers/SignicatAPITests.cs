using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Signicat;
using Signicat.Basic;  // v.2.0.3.0
using Signicat.Basic.Service;  // v.1.12.20.0

namespace ElectronicSignatures.basicexample.Controllers
{
    public class SignicatAPITests
    {
        SignicatAPITests()
        {
            return;
        }

        Signicat.Basic.Attribute signicat_attribute;
        Signicat.Basic.Infrastructure infrastructure_enum;
        Signicat.Basic.Saml saml;
        Signicat.Basic.SignicatException e = new SignicatException("Exception from Signicat");
        Signicat.Basic.Service.ArchiveService archive_service = new ArchiveService();
        Signicat.Basic.Service.AuthenticationService authentication_service = new AuthenticationService();
        Signicat.Basic.Service.DocumentOrderService documentorder_service = new DocumentOrderService();
        Signicat.Basic.Service.SignatureService signature_service = new SignatureService();
        Signicat.Basic.Service.ViewerService viewer_service = new ViewerService();
        Signicat.Basic.Service.DirectDebitService directdebit_service = new DirectDebitService();
    }
}