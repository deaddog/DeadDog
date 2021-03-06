﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DeadDog
{
    /// <summary>
    /// Identifies a http-accesible address and uses <see cref="HttpWebRequest"/> and <see cref="HttpWebResponse"/> to allow simple http-download.
    /// </summary>
    public class URL : IEquatable<URL>
    {
        private static readonly Regex urlRegex = new Regex("^(?<protocol>https?)://(?<domain>[^/]+)((?<path>/([^/]+/)*)(?<page>.*))?$");
        private const int MAX_ATTEMPTS = 3;
        private const int THREAD_SLEEP = 2000;
        private const int BUFFER_SIZE = 8192;

        private string url
        {
            get { return string.Format("{0}://{1}{2}{3}", protocol, domain, path, page); }
        }
        private string protocol;
        private string domain;
        private string path;
        private string page;

        /// <summary>
        /// Initializes a new instance of the <see cref="URL"/> class.
        /// </summary>
        /// <param name="url">The http url associated with this instance. Must begin with "http://"</param>
        public URL(string url)
        {
            Match match = urlRegex.Match(url);

            if (!match.Success)
                throw new ArgumentException("The url \"" + url + "\" could not be parsed.", "url");

            protocol = match.Groups["protocol"].Value.ToLower();
            domain = match.Groups["domain"].Value.ToLower();
            path = replaceEncoding(match.Groups["path"].Value);
            page = match.Groups["page"].Value;

            path = Regex.Replace(path, "%[a-zA-Z0-9][a-zA-Z0-9]", m => replaceEncoding(m.Value.ToUpper()));
            path = Regex.Replace(path, "%[a-zA-Z0-9][a-zA-Z0-9]", m => replaceEncoding(m.Value.ToUpper()));

            if (!path.StartsWith("/"))
                path = "/";
        }

        private static string replaceEncoding(string input)
        {
            return Regex.Replace(input, "%(?<val>[a-zA-Z0-9][a-zA-Z0-9])", m =>
                {
                    int i = int.Parse(m.Groups["val"].Value, System.Globalization.NumberStyles.HexNumber);

                    if (
                        (i >= 0x41 && i <= 0x5A) ||
                        (i >= 0x61 && i <= 0x7A) ||
                        (i >= 0x30 && i <= 0x39) ||
                        i == 0x2D ||
                        i == 0x2E ||
                        i == 0x5F ||
                        i == 0x7E
                       )
                        return ((char)i).ToString();
                    else
                        return m.Value;
                });
        }

        /// <summary>
        /// Parses a link and returns a <see cref="URL"/> with respect to the page on which the link was found.
        /// </summary>
        /// <param name="link">The link to parse.</param>
        /// <returns>A <see cref="URL"/> representing the address pointed to from the current <see cref="URL"/> to <paramref name="link"/>.</returns>
        public URL GetURLFromLink(string link)
        {
            if (Regex.IsMatch(link, "^https?://"))
                return new URL(link);
            else if (Regex.IsMatch(link, "^//"))
                return new URL(this.protocol + ":" + link);
            else if (Regex.IsMatch(link, "^/"))
                return new URL(this.protocol + "://" + this.domain + link);
            else
                return new URL(this.protocol + "://" + this.domain + this.path + link);

            throw new ArgumentException("Unable to parse link.", "link");
        }

        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="String"/> using ASCII encoding.
        /// </summary>
        /// <returns>The contents of the URL as a <see cref="String"/>.</returns>
        public string GetHTML()
        {
            return GetHTML(Encoding.ASCII);
        }
        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="String"/> using automatic (html only) detection of encoding.
        /// </summary>
        /// <param name="detectEncoding">A boolean value indicating if encoding should be automatically determined.</param>
        /// <returns>The contents of the URL as a <see cref="String"/>.</returns>
        public string GetHTML(bool detectEncoding)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                LoadToStream(ms);
                buffer = ms.GetBuffer();
            }

            string temp = Encoding.ASCII.GetString(buffer);
            if (!detectEncoding)
                return temp;

            Encoding enc = DetermineEncoding(temp) ?? Encoding.ASCII;
            return enc.GetString(buffer);
        }
        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="String"/> using the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding used to convert the binary stream.</param>
        /// <returns>The contents of the URL as a <see cref="String"/>.</returns>
        public string GetHTML(Encoding encoding)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                LoadToStream(ms);
                buffer = ms.GetBuffer();
            }

            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="String"/> using ASCII encoding.
        /// </summary>
        /// <param name="readURL">When the method returns, contains the url address that was read (in case of redirection).</param>
        /// <returns>The contents of the URL as a <see cref="String"/>.</returns>
        public string GetHTML(out URL readURL)
        {
            return GetHTML(Encoding.ASCII, out readURL);
        }
        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="String"/> using the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding used to convert the binary stream.</param>
        /// <param name="readURL">When the method returns, contains the url address that was read (in case of redirection).</param>
        /// <returns>The contents of the URL as a <see cref="String"/>.</returns>
        public string GetHTML(Encoding encoding, out URL readURL)
        {
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                LoadToStream(ms, out readURL);
                buffer = ms.GetBuffer();
            }

            return encoding.GetString(buffer);
        }

        public static Encoding DetermineEncoding(string html)
        {
            Match m = Regex.Match(html, "charset=\"(?<charset>.*)\"");
            if (!m.Success)
                m = Regex.Match(html, "<meta .*?content=\".*?charset=(?<charset>.*)\"");
            if (m.Success)
            {
                string encodingName = m.Groups["charset"].Value;
                if (encodingName.Contains(" "))
                    encodingName = encodingName.Substring(0, encodingName.IndexOf(' '));

                Encoding enc;
                try
                {
                    enc = Encoding.GetEncoding(encodingName);
                }
                catch
                {
                    enc = null;
                }
                return enc;
            }
            return null;
        }

        public void LoadToStream(Stream stream)
        {
            URL url;
            LoadToStream(stream, out url);
        }
        public void LoadToStream(Stream stream, out URL readURL)
        {
            if (!stream.CanWrite)
                throw new ArgumentException("The stream must support writing.", "stream");

            byte[] buf = new byte[BUFFER_SIZE];

            HttpWebRequest request;
            HttpWebResponse response = null;
            Stream resStream = null;
            readURL = null;
            int attempt = 0;

            while (attempt < MAX_ATTEMPTS && resStream == null)
                try
                {
                    attempt++;

                    request = (HttpWebRequest)WebRequest.Create(this.url);
                    response = (HttpWebResponse)request.GetResponse();

                    readURL = new URL(request.Address.AbsoluteUri);

                    resStream = response.GetResponseStream();
                }
                catch
                {
                    if (attempt == MAX_ATTEMPTS)
                        throw new Exception("File could not be loaded after " + MAX_ATTEMPTS + " attempts.");
                    else
                        System.Threading.Thread.Sleep(THREAD_SLEEP);
                }

            if (response.ContentLength > int.MaxValue)
                throw new InvalidOperationException("Cannot read files larger than 2gb (UINT32 max)");

            int count = 0;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count > 0)
                    stream.Write(buf, 0, count);
            }
            while (count > 0);
            resStream.Dispose();
        }

        /// <summary>
        /// Creates a webrequest and returns the response stream converted to a <see cref="Image"/>.
        /// </summary>
        /// <returns>The contents of the URL as a <see cref="Image"/>.</returns>
        public Image GetImage()
        {
            Image image;
            MemoryStream ms = new MemoryStream();
            LoadToStream(ms);
            image = Image.FromStream(ms);
            //The MemoryStream should not be disposed, as the Image is bound to this stream.
            //The stream will be disposed by disposing the Image.
            //See http://stackoverflow.com/questions/336387/image-save-throws-a-gdi-exception-because-the-memory-stream-is-closed

            return image;
        }
        /// <summary>
        /// Creates a webrequest and writes the response stream to a file.
        /// </summary>
        /// <param name="localFile">The local path where the contents of the url is stored.</param>
        public void GetFile(string localFile)
        {
            using (FileStream fs = new FileStream(localFile, FileMode.Create, FileAccess.Write))
                LoadToStream(fs);
        }

        /// <summary>
        /// Gets the <see cref="URL"/> to which this <see cref="URL"/> redirects.
        /// </summary>
        /// <returns>An instance of <see cref="URL"/> to which this <see cref="URL"/> redirects.</returns>
        public URL GetURL()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            int attempt = 0;
            string uri = null;

            while (attempt < MAX_ATTEMPTS && uri == null)
                try
                {
                    attempt++;

                    request = (HttpWebRequest)WebRequest.Create(this.url);
                    response = (HttpWebResponse)request.GetResponse();

                    uri = request.Address.AbsoluteUri.ToString();
                }
                catch
                {
                    System.Threading.Thread.Sleep(THREAD_SLEEP);
                    if (attempt == MAX_ATTEMPTS)
                        throw new Exception("File could not be loaded after " + MAX_ATTEMPTS + " attempts.");
                }
                finally
                {
                    response.Close();
                }

            if (uri == null)
                throw new Exception("File could not be loaded.");

            return new URL(uri);
        }

        /// <summary>
        /// Gets the url-address associated with this instance.
        /// </summary>
        public string Address
        {
            get { return url; }
        }

        /// <summary>
        /// Gets the protocol (http or https) for this <see cref="URL"/>.
        /// </summary>
        public string Protocol
        {
            get { return protocol; }
        }
        /// <summary>
        /// Gets the domain, including subdomain and toplevel domain for this <see cref="URL"/> (eg. "mail.google.com").
        /// </summary>
        public string Domain
        {
            get { return domain; }
        }
        /// <summary>
        /// Gets the path for this <see cref="URL"/>, not including the page name (eg. "/myfiles/").
        /// </summary>
        public string Path
        {
            get { return path; }
        }
        /// <summary>
        /// Gets the page name for this <see cref="URL"/>, not including the path (eg. "index.html").
        /// </summary>
        public string Page
        {
            get { return page; }
        }

        /// <summary>
        /// Specifies whether this <see cref="URL"/> contains the same address as the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to test.</param>
        /// <returns>true, if <paramref name="obj"/> is an <see cref="URL"/> and has the same address as this <see cref="URL"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is URL)
                return this.Equals(obj as URL);
            else
                return base.Equals(obj);
        }
        /// <summary>
        /// Specifies whether this <see cref="URL"/> contains the same address as the specified <see cref="URL"/>.
        /// </summary>
        /// <param name="other">The <see cref="URL"/> to test.</param>
        /// <returns>true, if <paramref name="other"/> has the same address as this <see cref="URL"/>.</returns>
        public bool Equals(URL other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return this.url.Equals(other.url);
        }
        /// <summary>
        /// Returns a hash code for this <see cref="URL"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="URL"/>.</returns>
        public override int GetHashCode()
        {
            return url.GetHashCode();
        }

        /// <summary>
        /// Compares two <see cref="URL"/> objects for value-equality.
        /// </summary>
        /// <param name="a">An <see cref="URL"/> to compare.</param>
        /// <param name="b">An <see cref="URL"/> to compare.</param>
        /// <returns>true, if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, false.</returns>
        public static bool operator ==(URL a, URL b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// Compares two <see cref="URL"/> objects for value-inequality.
        /// </summary>
        /// <param name="a">An <see cref="URL"/> to compare.</param>
        /// <param name="b">An <see cref="URL"/> to compare.</param>
        /// <returns>false, if <paramref name="a"/> and <paramref name="b"/> are equal; otherwise, true.</returns>
        public static bool operator !=(URL a, URL b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="URL"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="URL"/></returns>
        public override string ToString()
        {
            return "URL [" + url + "]";
        }
    }
}
