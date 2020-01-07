using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Owin
{
    /// <summary>
    /// Useful utilities that add functionality to the <see cref="Microsoft.Owin.IOwinContext"/> class.
    /// </summary>
    internal static class OwinContextUtilities
    {
        /// <summary>
        /// Determines whether the current request supports the <paramref name="encoding"/>.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>True if the request contains the encoding string in the HEADERS config object.</returns>
        internal static bool SupportsEncoding(this IOwinContext context, string encoding)
        {
            IEnumerable<string> acceptedEncoding = context.Request.Headers["Accept-Encoding"]?.Split(',');
            return acceptedEncoding.Any(x => string.Equals(x, encoding, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines if the current request is an AJAX request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request contains the 'X-Requested-With' key in the HEADERS config object.</returns>
        internal static bool IsAjaxRequest(this IOwinContext context)
            => context.Request.Headers.ContainsKey("X-Requested-With")
               && context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        /// <summary>
        /// Determines if the current request is a HTML request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request finds the 'HTML' key in the HEADERS' Accept config object.</returns>
        internal static bool IsHtmlRequest(this IOwinContext context)
            => context.Request?.Headers != null
               && context.Request.Headers.ContainsKey("Accept")
               && (context.Request.Headers["Accept"].Contains("html") || context.Request.Headers["Accept"].Contains("*/*"));

        /// <summary>
        /// Determines if the current request is a SignalR request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request finds 'signalr' in the request path</returns>
        internal static bool IsWebSocketRequest(this IOwinContext context)
            => context.Request.Path.ToString().Contains("signalr");

        /// <summary>
        /// Determines if the current request is a file request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request contains a file extension in the request path</returns>
        internal static bool IsFileRequest(this IOwinContext context)
            => !string.IsNullOrEmpty(Path.GetExtension(context.Request.Path.ToString()));

        /// <summary>
        /// Determines if the current request is a bundle request.
        /// </summary>
        /// <param name="context">The current request context</param>
        /// <returns>True if the current request finds 'bundles' in the request path</returns>
        internal static bool IsBundleRequest(this IOwinContext context)
            => context.Request.Path.ToString().Contains("bundles");

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static async Task WriteToOutput(this IOwinContext context, string output)
        {
            context.Response.ContentType = "text/javascript";
            await context.Response.WriteAsync(output).ConfigureAwait(true);

            if (context.SupportsEncoding("gzip"))
                context.Response.Body = new GZipStream(context.Response.Body, CompressionMode.Compress);
            else if (context.SupportsEncoding("deflate"))
                context.Response.Body = new DeflateStream(context.Response.Body, CompressionMode.Compress);
        }
    }
}