using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WhoScored
{
    using System.Security.Cryptography.X509Certificates;

    using DevDefined.OAuth.Consumer;
    using DevDefined.OAuth.Framework;
    using DevDefined.OAuth.Tests;

    public class WhoScoredRequest
    {
        private void WhoScoredConsumer()
        {
            X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

            string requestUrl = ConfigurationManager.AppSettings["requestUrl"];
            string userAuthorizeUrl = ConfigurationManager.AppSettings["userAuthorizeUrl"];
            string accessUrl = ConfigurationManager.AppSettings["accessUrl"];
            string callBackUrl = ConfigurationManager.AppSettings["callBackUrl"];

            var consumerContext = new OAuthConsumerContext
			{
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
				SignatureMethod = SignatureMethod.HmacSha1,                
				Key = certificate.PrivateKey
			};

			var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl);

			// get a request token from the provider
			//IToken requestToken = session.GetRequestToken();

			// generate a user authorize url for this token (which you can use in a redirect from the current site)
			//string authorizationLink = session.GetUserAuthorizationUrlForToken(requestToken, callBackUrl);

            const string Verifier = "8yeydIESPsinquIQ";
			// exchange a request token for an access token
			//IToken accessToken = session.ExchangeRequestTokenForAccessToken(requestToken, Verifier);
            session.AccessToken = new TokenBase();
            session.AccessToken.Token = ConfigurationManager.AppSettings["accessTokenKey"];
            session.AccessToken.TokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

			// make a request for a protected resource
            string matchesArchive = "?file=matchesarchive&version=1.1";

			string responseText = session.Request().Get().ForUrl(string.Format("http://chpp.hattrick.org/chppxml.ashx{0}", matchesArchive)).ToString();        
        }


        /// <summary>
        /// creates oAuth session
        /// </summary>
        /// <returns>session</returns>
        private OAuthSession GetSession()
        {
            X509Certificate2 certificate = TestCertificates.OAuthTestCertificate();

            string requestUrl = ConfigurationManager.AppSettings["requestUrl"];
            string userAuthorizeUrl = ConfigurationManager.AppSettings["userAuthorizeUrl"];
            string accessUrl = ConfigurationManager.AppSettings["accessUrl"];

            var consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
                SignatureMethod = SignatureMethod.HmacSha1,
                Key = certificate.PrivateKey
            };

            var session = new OAuthSession(consumerContext, requestUrl, userAuthorizeUrl, accessUrl);

            session.AccessToken = new TokenBase();
            session.AccessToken.Token = ConfigurationManager.AppSettings["accessTokenKey"];
            session.AccessToken.TokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

            return session;
        }


        /// <summary>
        /// sends request
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns>response as string</returns>
        public string MakeRequest(string requestUri)
        {
            return GetSession().Request().Get().ForUrl(requestUri).ToString();            
        }
    }


}