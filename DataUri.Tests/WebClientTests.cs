using System;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataUri.Tests
{
    [TestClass]
    public class WebClientTests
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            DataWebRequestFactory.Register();
        }

        [TestMethod]
        public void DataUri_Basic_DownloadString()
        {
            var text = "Hello World!";
            var dataUri = new Uri($"data:,{text}");
            using (WebClient webClient = new WebClient())
            {
                var result = webClient.DownloadString(dataUri);
                Assert.AreEqual(text, result);
            }
        }

        [TestMethod]
        public void DataUri_Basic_DownloadStringTaskAsync()
        {
            var text = "Hello World!";
            var dataUri = new Uri($"data:,{text}");
            using (WebClient webClient = new WebClient())
            {
                var result = webClient.DownloadStringTaskAsync(dataUri).Result;
                Assert.AreEqual(text, result);
            }
        }

        [TestMethod]
        public void DataUri_Advanced_DownloadString()
        {
            var obj = @"
{
    ""greeting"": ""Hello World!""
}
";
            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(obj));
            var dataUri = new Uri($"data:application/json;charset=utf-8;base64,{data}");
            using (WebClient webClient = new WebClient())
            {
                var result = webClient.DownloadString(dataUri);
                Assert.AreEqual(obj, result);
            }
        }

        [TestMethod]
        public void DataUri_Advanced_DownloadStringTaskAsync()
        {
            var obj = @"
{
    ""greeting"": ""Hello World!""
}
";
            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(obj));
            var dataUri = new Uri($"data:application/json;charset=utf-8;base64,{data}");
            using (WebClient webClient = new WebClient())
            {
                var result = webClient.DownloadStringTaskAsync(dataUri).Result;
                Assert.AreEqual(obj, result);
            }
        }
    }
}
