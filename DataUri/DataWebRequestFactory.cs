namespace DataUri
{
    using System;
    using System.Net;

    public sealed class DataWebRequestFactory : IWebRequestCreate
    {
        public static void Register()
        {
            WebRequest.RegisterPrefix("data", new DataWebRequestFactory());
        }

        public WebRequest Create(Uri uri)
        {
            return new DataWebRequest(uri);
        }
    }
}
