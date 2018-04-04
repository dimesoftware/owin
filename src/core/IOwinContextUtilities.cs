using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Owin
{
    /// <summary>
    /// Utilities that extend the <see cref="IOwinContext"/> class
    /// </summary>
    public static class OwinContextUtilities
    {
        /// <summary>
        /// Checks if the encoding is supported
        /// </summary>
        ///  <param name="context">The context</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>True if the encoding is supported</returns>
        public static bool SupportsEncoding(this IOwinContext context, string encoding)
        {
            IEnumerable<string> acceptedEncoding = context.Request.Headers["Accept-Encoding"]?.Split(',');
            return acceptedEncoding.Any(x => string.Equals(x, encoding, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if the request is an AJAX request
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns>True if the request is an AJAX request</returns>
        public static bool IsAjaxRequest(this IOwinContext context)
            => context.Request.Headers.ContainsKey("X-Requested-With")
               && context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        /// <summary>
        /// Checks if the request is a web sockets request
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns>True if the request is a web sockets request</returns>
        public static bool IsWebSocketRequest(this IOwinContext context)
            => context.Request.Path.ToString().Contains("signalr");

        /// <summary>
        /// Checks if the request is a file request
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns>True if the request is a file request</returns>
        public static bool IsFileRequest(this IOwinContext context) 
            => !string.IsNullOrEmpty(Path.GetExtension(context.Request.Path.ToString()));
    }
}