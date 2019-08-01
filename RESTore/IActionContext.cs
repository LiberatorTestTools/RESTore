using System.Collections.Generic;
using System.Runtime.InteropServices;
using Liberator.RESTore.Access;

namespace Liberator.RESTore
{
    public interface IActionContext
    {
        /// <summary>
        /// Allows the user to set and execute a DELETE request.
        /// </summary>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Delete([DefaultParameterValue(null), Optional] string url);

        /// <summary>
        /// Allows the user to set and execute a GET request.
        /// </summary>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Get([DefaultParameterValue(null), Optional] string url);

        /// <summary>
        /// Gets an authorisation token based on the details of the passed token retrieval class.
        /// NB: currently configured for Azure only. AWS and other applications to follow.
        /// </summary>
        /// <param name="token">The object used to retrieve the token.</param>
        /// <param name="accessToken">The access token as a string.</param>
        /// <returns>The WhenContext for the call.</returns>
        WhenContext GetAuthToken(IToken token, out string accessToken);

        /// <summary>
        /// Allows a user to define the basic parameters of a primitive load test.
        /// </summary>
        /// <param name="seconds">Number of seconds to run the load test for.</param>
        /// <param name="threads">The number of threads to allow during the test.</param>
        /// <returns>The WhenContext object with the load test parameters set.</returns>
        WhenContext LoadTest([DefaultParameterValue(60), Optional] int seconds, [DefaultParameterValue(1), Optional] int threads);

        /// <summary>
        /// Allows the user to set and execute a PATCH request.
        /// </summary>
        /// <param name="url">The URL to send the PATCH request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Patch([DefaultParameterValue(null), Optional] string url);

        /// <summary>
        /// Adds a single path parameter to the url.
        /// </summary>
        /// <param name="parameter">The parameter in the url.</param>
        /// <param name="value">The value to replace it with.</param>
        /// <returns>The When Context that we are building.</returns>
        WhenContext PathParameter(string parameter, object value);

        /// <summary>
        /// Adds multiple path parameters to the url.
        /// </summary>
        /// <param name="pathParameters">The parameters with their name and value pairs.</param>
        /// <returns>The When Context that we are building.</returns>
        WhenContext PathParameters(Dictionary<string, object> pathParameters);

        /// <summary>
        /// Allows the user to set and execute a POST request.
        /// </summary>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Post([DefaultParameterValue(null), Optional] string urlll);

        /// <summary>
        /// Allows the user to set and execute a PUT request.
        /// </summary>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Put([DefaultParameterValue(null), Optional] string url);

        /// <summary>
        /// Allows the user to set and execute a HEAD request.
        /// </summary>
        /// <param name="url">The URL to send the HEAD request to.</param>
        /// <returns>The ExecutionContext that represents the executing query.</returns>
        ExecutionContext Head([Optional, DefaultParameterValue(null)]string url);
    }
}