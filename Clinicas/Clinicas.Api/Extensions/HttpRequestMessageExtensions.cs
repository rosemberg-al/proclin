using System.IO;
using System.Net;
using System.Net.Http;

namespace Clinicas.Api.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage CreateCSVResponse(this HttpRequestMessage request, MemoryStream file)
        {
            var result = request.CreateResponse(HttpStatusCode.OK);

            result.Content = new ByteArrayContent(file.ToArray());
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");

            return result;
        }
        public static HttpResponseMessage CreateXMLResponse(this HttpRequestMessage request, MemoryStream file)
        {
            var result = request.CreateResponse(HttpStatusCode.OK);

            result.Content = new ByteArrayContent(file.ToArray());
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");

            return result;
        }
    }
}