namespace ProtectedAPI.Extensions
{
    internal static class JwtBearerEventsLoggingExtensions
    {
        public static void AuthenticationFailed(this ILogger logger, Exception e)
        {
            logger.LogError("Authentication failed Exception: {0}", e);
        }

        public static void TokenValidationSucceeded(this ILogger logger, string? issuer, string? scope)
        {
            logger.LogInformation("Token validation succeeded: Issuer: {0} Scope: {1}", issuer, scope);
        }
    }
}
