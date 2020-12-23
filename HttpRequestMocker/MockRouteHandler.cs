using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RepairsApi.Tests.ApiMocking
{
    public class MockRouteHandler
    {
        private static Regex _pathMatcher = new Regex(@"^{(.+)}\/?$");
        private string[] _segments;
        private readonly object? _methodObject;
        private NameValueCollection _routeVariables;
        private NameValueCollection _queryVariables;
        private NameValueCollection _headerVariables;
        private string? _body;
        private MethodInfo _method;
        private HttpMethod? _httpMethod = default;

        public MockRouteHandler(MethodInfo method, object? methodObject = null)
        {
            if (methodObject is null && !method.IsStatic)
            {
                throw new Exception($"{method.Name} Either needs to be static or an instance of the object it is in needs to be passed");
            }
                    
            _methodObject = methodObject;
            _routeVariables = new NameValueCollection();
            _queryVariables = new NameValueCollection();
            _headerVariables = new NameValueCollection();
            _method = method;
            var route = _method.GetCustomAttribute<RouteAttribute>();
            SetupHttpMethod();

            Helpers.Assert(route != null);

            var routeUri = new Uri(route!.Template, UriKind.Absolute);
            this._segments = routeUri.Segments.Select(seg => HttpUtility.UrlDecode(seg)).ToArray();
        }

        private void SetupHttpMethod()
        {
            IEnumerable<Attribute> attributes = _method.GetCustomAttributes();
            if (attributes.Any(attr => attr is HttpGetAttribute)) _httpMethod = HttpMethod.Get;
            else if (attributes.Any(attr => attr is HttpPostAttribute)) _httpMethod = HttpMethod.Post;
            else if (attributes.Any(attr => attr is HttpDeleteAttribute)) _httpMethod = HttpMethod.Delete;
            else if (attributes.Any(attr => attr is HttpPatchAttribute)) _httpMethod = HttpMethod.Patch;
            else if (attributes.Any(attr => attr is HttpPutAttribute)) _httpMethod = HttpMethod.Put;
            else if (attributes.Any(attr => attr is HttpHeadAttribute)) _httpMethod = HttpMethod.Head;
            else if (attributes.Any(attr => attr is HttpOptionsAttribute)) _httpMethod = HttpMethod.Options;
        }

        internal object? Execute()
        {
            object?[] parameters = _method.GetParameters().Select(param => ParseParam(param)).ToArray();

            return _method.Invoke(_methodObject, parameters);
        }

        private object? ParseParam(ParameterInfo param)
        {
            var attributes = param.GetCustomAttributes();

            if (attributes.Any(attr => attr is FromQueryAttribute)) return _queryVariables[param.Name];
            if (attributes.Any(attr => attr is FromBodyAttribute)) return JsonSerializer.Deserialize(_body!, param.ParameterType);
            if (attributes.Any(attr => attr is FromHeaderAttribute)) return _headerVariables[param.Name];
            else return _routeVariables[param.Name];
        }

        internal void ParseQuery(NameValueCollection queryParams)
        {
            this._queryVariables.Add(queryParams);
        }

        internal bool ParseSegments(string[] segments)
        {
            if (segments.Length != _segments.Length) return false;

            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] == _segments[i])
                {
                    continue;
                }
                else if (_pathMatcher.IsMatch(_segments[i]))
                {
                    _routeVariables.Add(_pathMatcher.Match(_segments[i]).Groups[1].Value, segments[i].Trim('/'));
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        internal async Task ParseBody(HttpContent? content)
        {
            if (content is null)
            {
                return;
            }

            var body = await content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return;
            }

            _body = body;
        }

        internal void ParseHeaders(HttpRequestHeaders headers)
        {
            foreach (var header in headers)
            {
                _headerVariables.Add(header.Key, header.Value.FirstOrDefault());
            }
        }

        internal bool IsValidMethod(HttpMethod method)
        {
            return _httpMethod == null || _httpMethod == method;
        }
    }
}
