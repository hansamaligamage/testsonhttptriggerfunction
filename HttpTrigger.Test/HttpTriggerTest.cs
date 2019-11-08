using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using Xunit;

namespace HttpTrigger.Test
{
    public class HttpTriggerTest
    {
        private readonly ILogger logger = CreateLogger();

        [Fact]
        public async void Http_trigger_should_return_known_string()
        {
            var request = CreateHttpRequest("name", "Hansamali");
            var response = (OkObjectResult)await HttpTriggerFunction.Run(request, logger);
            Assert.Equal("Hello, Hansamali", response.Value);
        }

        [Theory]
        [MemberData(nameof(TestFactory.Data), MemberType = typeof(TestFactory))]
        public async void Http_trigger_should_return_known_string_from_passed_data(string queryStringKey, string queryStringValue)
        {
            var request = CreateHttpRequest(queryStringKey, queryStringValue);
            var response = (OkObjectResult)await HttpTriggerFunction.Run(request, logger);
            Assert.Equal($"Hello, {queryStringValue}", response.Value);
        }

        #region reusable methods
        private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var querystring = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return querystring;
        }

        public static DefaultHttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue))
            };
            return request;
        }

        public static ILogger CreateLogger()
        {
            return NullLoggerFactory.Instance.CreateLogger("logger");
        }
        #endregion
    }

    public class TestFactory
    {
        #region Test Data
        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] { "name", "Hansamali" },
                new object[] { "name", "Gamage" }
            };
        }
        #endregion
    }
}
