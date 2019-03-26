using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Liberator.RESTore.Access
{
    public class AzureToken : IToken
    {
        /// <summary>
        /// The name of the user for the access token.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The password for the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The collection of scopes to be used
        /// </summary>
        public List<string> Scopes { get; set; }

        /// <summary>
        /// The ID for the client making the request.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Authority of the security token service (STS).
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// The result of ther authentication call
        /// </summary>
        public AuthenticationResult AuthenticationResult { get; set; }

        /// <summary>
        /// The auth token returned
        /// </summary>
        public string AuthToken { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        public AzureToken()
        {
            Scopes = new List<string>();
        }

        /// <summary>
        /// Adds a scope to the object.
        /// </summary>
        /// <param name="scope">The scope strinf to add.</param>
        public void AddScope(string scope)
        {
            Scopes.Add(scope);
        }

        /// <summary>
        /// Adds a range of scopes to the object.
        /// </summary>
        /// <param name="scopes">The collection of scope strings.</param>
        public void AddScopes(IEnumerable<string> scopes)
        {
            Scopes.AddRange(scopes);
        }

        /// <summary>
        /// Gets the access token for an endpoint using Azure STS
        /// </summary>
        /// <param name="userName">Name of the user requesting the token.</param>
        /// <param name="password">Password of the user requesting the token.</param>
        /// <param name="scopes">Scopes requested to access a protected API.</param>
        /// <param name="clientId">The ID for the client making the request.</param>
        /// <param name="authority">Authority of the security token service (STS).</param>
        /// <returns>The auth token as a string</returns>
        public string GetAccessToken(string userName, string password, string[] scopes, string clientId, string authority)
        {
            UserName = userName;
            Password = password;
            Scopes = scopes.ToList();
            AuthToken = RetrieveToken(clientId, authority);
            return AuthToken;
        }


        /// <summary>
        /// Retrieves a token from Azure
        /// </summary>
        /// <param name="clientId">The ID for the client making the request.</param>
        /// <param name="authority">Authority of the security token service (STS).</param>
        /// <returns>The access token as a string.</returns>
        private string RetrieveToken(string clientId, string authority)
        {
            try
            {
                PublicClientApplication publicClientApplication = new PublicClientApplication(clientId, authority);
                var securePassword = new SecureString();
                foreach (char c in Password) { securePassword.AppendChar(c); }
                AuthenticationResult = publicClientApplication.AcquireTokenByUsernamePasswordAsync(Scopes.ToArray(), UserName, securePassword).Result;
                return AuthenticationResult.AccessToken;
            }
            catch (Exception ex)
            {
                throw new RESToreException("Could not retrieve the tokemn as requested", ex);
            }
        }
    }
}
