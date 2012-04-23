using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored
{
    using System.Security.Cryptography.X509Certificates;

    using DevDefined.OAuth.Tests;

    public class WhoScoredOAuth
    {
        public void WhoScoredConsumer()
        {
            X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

            string requestUrl = "https://www.google.com/accounts/OAuthGetRequestToken";
            string userAuthorizeUrl = "https://www.google.com/accounts/accounts/OAuthAuthorizeToken";
            string accessUrl = "https://www.google.com/accounts/OAuthGetAccessToken";
            string callBackUrl = "http://www.mysite.com/callback";
        }
    }


}