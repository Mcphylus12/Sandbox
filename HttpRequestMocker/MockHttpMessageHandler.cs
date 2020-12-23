using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RepairsApi.Tests.ApiMocking
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<MockRouteHandler> _config;

        public MockHttpMessageHandler(List<MockRouteHandler> config)
        {
            _config = config;
        }

        public static MockHttpMessageHandler FromClass<T>()
            where T : class
        {
            return BuildMock<T>(null, true);
        }

        public static MockHttpMessageHandler FromObject<T>(T oobject)
            where T : class
        {
            return BuildMock(oobject, false);
        }

        private static MockHttpMessageHandler BuildMock<T>(T? oobject, bool onlyStatic)
            where T : class
        {
            Type classType = typeof(T);

            var config = new List<MockRouteHandler>();

            config.AddRange(classType.GetMethods().Where(m => (!onlyStatic || m.IsStatic) && m.CustomAttributes.Any(attr => attr.AttributeType == typeof(RouteAttribute)))
                .Select(method => new MockRouteHandler(method, oobject)));

            return new MockHttpMessageHandler(config);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Helpers.Assert(request.RequestUri != null);

            if (request.Method != HttpMethod.Get)
            {
                throw new Exception("Mocking Is only supported for gets at the moment");
            }

            var mockFunction = await FindMockFunction(request);

            if (mockFunction != null)
            {
                object? result = mockFunction.Execute();

                if (result == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                httpResponseMessage.Content = new StringContent(JsonSerializer.Serialize(result));
                return httpResponseMessage;
            }

            throw new Exception("Mock Method not found");
        }

        private async Task<MockRouteHandler?> FindMockFunction(HttpRequestMessage request)
        {
            var requestUri = request.RequestUri!;
            var segments = requestUri.Segments;
            foreach (var potentialMock in _config)
            {
                if (!potentialMock.IsValidMethod(request.Method))
                {
                    break;
                }

                var segmentMatches = potentialMock.ParseSegments(segments);

                if (segmentMatches)
                {
                    var query = HttpUtility.ParseQueryString(requestUri.Query);
                    potentialMock.ParseQuery(query);
                    potentialMock.ParseHeaders(request.Headers);
                    await potentialMock.ParseBody(request.Content);
                    return potentialMock;
                }
            }

            return null;
        }
    }
}
