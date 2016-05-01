namespace DataUri
{
    using System;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    internal sealed class DataWebResponse : WebResponse
    {
        private readonly string mediatype = "text/plain";
        private readonly Encoding charset = Encoding.ASCII;
        private readonly byte[] data;

        public DataWebResponse(Uri uri)
        {
            var match = Regex.Match(uri.ToString(), "data:(?<mediatype>[^;,]+/[^;,]+)?(?:;charset=(?<charset>[^;,]+))?(?<base64>;base64)?,(?<data>.*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (!string.IsNullOrWhiteSpace(match.Groups["mediatype"].Value))
            {
                this.mediatype = match.Groups["mediatype"].Value;
            }

            if (!string.IsNullOrWhiteSpace(match.Groups["charset"].Value))
            {
                this.charset = Encoding.GetEncoding(match.Groups["charset"].Value);
            }

            if (!string.IsNullOrWhiteSpace(match.Groups["base64"].Value))
            {
                this.data = Convert.FromBase64String(match.Groups["data"].Value);
            }
            else
            {
                this.data = this.charset.GetBytes(match.Groups["data"].Value);
            }
        }

        public override System.IO.Stream GetResponseStream()
        {
            return new System.IO.MemoryStream(this.data);
        }

        public override long ContentLength => this.data.Length;

        public override string ContentType => this.mediatype;
    }
}
