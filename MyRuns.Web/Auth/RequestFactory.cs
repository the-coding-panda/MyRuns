using System;
using RestSharp.Portable;
using RestSharp.Portable.OAuth2.Infrastructure;
using RestSharp.Portable.HttpClient;

namespace MyRuns.Web.Auth
{
    public class RequestFactory : IRequestFactory
    {
        #region IRequestFactory implementation

        public IRestClient CreateClient()
        {
            return new RestClient();
        }

        public IRestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource);
        }

        #endregion
    }
}
