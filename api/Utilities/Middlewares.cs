using System.Text.Json;
using StackExchange.Redis;

namespace api.Utilities
{
    public class NonceMiddleware(RequestDelegate next, ConnectionMultiplexer redis)
    {
        private static readonly string[] ALLOWED_METHODS =
        [
            "GET",
            "POST",
            "PUT",
            "PATCH",
            "DELETE",
        ];
        private readonly RequestDelegate _next = next;
        private readonly IDatabase _redisDb = redis.GetDatabase();

        public async Task InvokeAsync(HttpContext context)
        {
            if (
                ALLOWED_METHODS.Contains(context.Request.Method)
                && context.Request.Headers.TryGetValue("Nonce", out var nonce)
            )
            {
                var nonceKey = $"nonce:{context.Request.Method}:{context.Request.Path}:{nonce}";

                string? cached = await _redisDb.StringGetAsync(nonceKey);

                if (
                    cached != null
                    && JsonSerializer.Deserialize<Response>(cached) is Response cachedResponse
                )
                {
                    Console.WriteLine($"Nonce {nonce} already used. Returning cached response.");

                    context.Response.StatusCode = cachedResponse.StatusCode;
                    if (cachedResponse.ContentType != null)
                    {
                        context.Response.ContentType = cachedResponse.ContentType;
                    }
                    foreach (var header in cachedResponse.Headers)
                    {
                        context.Response.Headers[header.Key] = header.Value;
                    }
                    if (cachedResponse.Body != null)
                    {
                        await context.Response.WriteAsync(cachedResponse.Body);
                    }
                    return;
                }

                var originalBodyStream = context.Response.Body;
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;

                await _next.Invoke(context);

                memStream.Seek(0, SeekOrigin.Begin);

                var responseBody = await new StreamReader(memStream).ReadToEndAsync();

                memStream.Seek(0, SeekOrigin.Begin);
                await memStream.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

                var response = new Response(
                    body: responseBody,
                    statusCode: context.Response.StatusCode,
                    contentType: context.Response.ContentType
                );

                foreach (var header in context.Response.Headers)
                {
                    response.Headers[header.Key] = header.Value;
                }

                await _redisDb.StringSetAsync(
                    nonceKey,
                    JsonSerializer.Serialize(response),
                    TimeSpan.FromMinutes(10)
                );
            }
            else
            {
                await _next(context);
            }
        }
    }
}
