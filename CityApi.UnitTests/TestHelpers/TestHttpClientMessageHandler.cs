namespace CityApi.UnitTests.TestHelpers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Allows mocking of the GetAsync method of a HttpClient by providing a HttpMessageHandler that can handle the GetAsync call
    /// </summary>
    /// <example>
    /// var testHandler = new TestHttpClientMessageHandler(HttpStatusCode.OK, "my response content");
    /// using (var testHttpClient = new HttpClient(testHandler))
    /// {
    /// var response = await testHttpClient.GetAsync("my URL");
    /// etc.
    /// }
    /// </example>
    public class TestHttpClientMessageHandler : HttpMessageHandler
    {
        private readonly string _content;
        private readonly HttpStatusCode _statusCode;

        /// <summary>
        ///     Constructor allowing you to specify the response status code and content
        /// </summary>
        /// <param name="statusCode">Whatever HttpStatusCode you want to return</param>
        /// <param name="content">Whatever content you want to return</param>
        public TestHttpClientMessageHandler(HttpStatusCode statusCode, string content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        /// <summary>
        ///     Constructor allowing you to specify the response status code with empty content
        /// </summary>
        /// <param name="statusCode">Whatever HttpStatusCode you want to return</param>
        public TestHttpClientMessageHandler(HttpStatusCode statusCode) : this(statusCode, string.Empty)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode) {Content = new StringContent(_content)};
            return Task.FromResult(response);
        }
    }
}