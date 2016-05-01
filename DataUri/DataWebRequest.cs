namespace DataUri
{
    using System;
    using System.Net;

    internal sealed class DataWebRequest : WebRequest
    {
        private readonly Uri uri;

        public DataWebRequest(Uri uri)
        {
            this.uri = uri;
        }

        public override WebResponse GetResponse()
        {
            return new DataWebResponse(this.uri);
        }

        public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            var result = new DataUriAsyncResult(state);
            callback.Invoke(result);
            return result;
        }

        public override WebResponse EndGetResponse(IAsyncResult asyncResult)
        {
            return this.GetResponse();
        }
    }
}
