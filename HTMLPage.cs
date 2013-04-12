using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Represent a webpage (accessed via http) by storing it's html text.
    /// </summary>
    public class HTMLPage
    {
        private string html;
        private URL request, response;
        private int bufferLevel;
        private IHTMLBuffer buffer;

        /// <summary>
        /// Gets the actual html associated with the <see cref="HTMLPage"/>.
        /// </summary>
        public string HTML
        {
            get { return html; }
        }
        /// <summary>
        /// Gets the requested url when this page was downloaded.
        /// </summary>
        public URL RequestedURL
        {
            get { return request; }
        }
        /// <summary>
        /// Gets the url (in response) from which html was downloaded.
        /// </summary>
        public URL ResponseURL
        {
            get { return response; }
        }
        /// <summary>
        /// Gets or sets the level (importance) of this page within it's buffer.
        /// </summary>
        public int BufferLevel
        {
            get { return bufferLevel; }
            set { bufferLevel = value; }
        }
        /// <summary>
        /// Gets the <see cref="IHTMLBuffer"/> that manages this <see cref="HTMLPage"/>.
        /// </summary>
        internal protected IHTMLBuffer Buffer
        {
            get { return buffer; }
            internal set { buffer = value; }
        }

        /// <summary>
        /// Removes this <see cref="HTMLPage"/> from its managing buffer.
        /// </summary>
        public void Remove()
        {
            if (buffer != null)
                buffer.Remove(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLPage"/> class.
        /// </summary>
        /// <param name="html">The html-code associated with the <see cref="HTMLPage"/>.</param>
        /// <param name="request">The requested url that is associated with the <see cref="HTMLPage"/>.</param>
        /// <param name="response">The response url that is associated with the <see cref="HTMLPage"/>.</param>
        public HTMLPage(string html, URL request, URL response)
        {
            this.html = html;
            this.request = request;
            this.response = response;
            this.BufferLevel = 0;
            this.buffer = null;
        }

        /// <summary>
        /// Converts a string of html into a unicode text-string.
        /// </summary>
        /// <param name="html">The html to convert.</param>
        /// <returns>The converted html.</returns>
        protected string decodeHTML(string html)
        {
            if (html == null)
                throw new ArgumentNullException("html");
            if (html == string.Empty)
                return string.Empty;

            string result;
#if NET3
            try { result = System.Web.HttpUtility.HtmlDecode(html); }
#elif NET4
            try { result = System.Net.WebUtility.HtmlDecode(html); }
#endif
            catch { result = html; }
            return result;
        }

        /// <summary>
        /// Converts this <see cref="HTMLPage"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="HTMLPage"/>.</returns>
        public override string ToString()
        {
            return string.Format("Req: {0} HTML: {1}", request.Address, html.Length);
        }
    }
}
