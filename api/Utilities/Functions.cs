namespace api.Utilities
{
    public static class Functions
    {
        public static string GetConnectionString(WebApplicationBuilder builder)
        {
            var fromEnv = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(fromEnv))
            {
                return fromEnv;
            }

            return builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found."
                );
        }
    }
}
