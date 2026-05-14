namespace api.Utilities
{
    public class Response(string? body, int statusCode, string? contentType = null)
    {
        public string? Body { get; set; } = body;
        public int StatusCode { get; set; } = statusCode;
        public string? ContentType { get; set; } = contentType;
        public Dictionary<string, string[]> Headers { get; set; } = [];
    }
}
