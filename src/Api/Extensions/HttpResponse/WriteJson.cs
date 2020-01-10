using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Template.Api.Extensions.HttpResponse
{
    public static class HttpResponseExtensions
    {
        public static void WriteJson<T>(this Microsoft.AspNetCore.Http.HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";

            var json = JsonSerializer.Serialize(obj, obj.GetType());
            response.WriteAsync(json, Encoding.UTF8);
        }
    }
}
