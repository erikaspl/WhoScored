using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhoScored
{
    using System.Security.Cryptography.X509Certificates;

    using DevDefined.OAuth.Consumer;
    using DevDefined.OAuth.Framework;
    using DevDefined.OAuth.Tests;

    public class WhoScoredOAuth
    {
        public void WhoScoredConsumer()
        {
            X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

            string requestUrl = "https://chpp.hattrick.org/oauth/request_token.ashx";
            string userAuthorizeUrl = "https://chpp.hattrick.org/oauth/authorize.aspx";
            string accessUrl = "https://chpp.hattrick.org/oauth/access_token.ashx";
            string callBackUrl = "http://www.mysite.com/callback";

            var consumerContext = new OAuthConsumerContext
			{
				ConsumerKey = "yn6RurDptfhQo8Kt3HBsQq",
                ConsumerSecret = "",
				SignatureMethod = SignatureMethod.HmacSha1,
				Key = certificate.PrivateKey
			};

			var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl);
				//.WithQueryParameters(new { scope = "http://chpp.hattrick.org/" });

			// get a request token from the provider
			IToken requestToken = session.GetRequestToken();

			// generate a user authorize url for this token (which you can use in a redirect from the current site)
			string authorizationLink = session.GetUserAuthorizationUrlForToken(requestToken, callBackUrl);

			// exchange a request token for an access token
			IToken accessToken = session.ExchangeRequestTokenForAccessToken(requestToken);

			// make a request for a protected resource
			string responseText = session.Request().Get().ForUrl("http://chpp.hattrick.org/chppxml.ashx").ToString();        
        }
    }


}