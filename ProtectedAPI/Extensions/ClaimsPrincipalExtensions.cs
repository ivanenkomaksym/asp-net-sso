using System.Globalization;
using System.Security.Claims;

namespace ProtectedAPI.Extensions
{
    public static class OpenIdConnectClaimTypes
    {
        public const string Scope = "scope";
        public const string IssuerValue = "iss";
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetIssuerValue(this ClaimsPrincipal principal, bool throwIfNotFound = true)
        {
            return principal.FindFirstValue(OpenIdConnectClaimTypes.IssuerValue, throwIfNotFound);
        }

        public static string? GetScopeValue(this ClaimsPrincipal principal, bool throwIfNotFound = false)
        {
            return principal.FindFirstValue(OpenIdConnectClaimTypes.Scope, throwIfNotFound);
        }

        public static string? FindFirstValue(this ClaimsPrincipal principal, string claimType, bool throwIfNotFound = false)
        {
            ArgumentNotNull(principal, nameof(principal));

            var value = principal.FindFirst(claimType)?.Value;
            if (throwIfNotFound && string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "The supplied principal does not contain a claim of type {0}", claimType));
            }

            return value;
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the given argument is null.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <param name="argumentValue">The argument value to test.</param>
        /// <param name="argumentName">The name of the argument to test.</param>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
