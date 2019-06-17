namespace Liberator.RESTore.Access
{
    /// <summary>
    /// Base interface for tokens
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// The name of the user for the access token.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// The password for the user
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets the access token for an endpoint using Azure STS
        /// </summary>
        /// <param name="userName">Name of the user requesting the token.</param>
        /// <param name="password">Password of the user requesting the token.</param>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="clientId">The ID for the client making the request.</param>
        /// <param name="authority">Authority of the security token service (STS).</param>
        /// <returns>The auth token as a string</returns>
        string GetAccessToken(string userName, string password, string[] scopes, string clientId, string authority);
    }
}
