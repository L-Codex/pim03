namespace api.Utilities
{
    public sealed record ApiError(string Code, string Message);

    public static class ApiResponse
    {
        public static ApiResponse<T> Ok<T>(T data) => new(Success: true, Data: data, Error: null);

        public static ApiResponse<T> Fail<T>(ApiError error) =>
            new(Success: false, Data: default, Error: error);
    }

    public sealed record EmptyResponse();

    public sealed record ApiResponse<T>(bool Success, T? Data, ApiError? Error);
}
