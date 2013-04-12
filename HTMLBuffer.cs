using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DeadDog
{
    /// <summary>
    /// Provides methods for dynamically buffering webpages and storing the retrieved data in appropriate types.
    /// </summary>
    /// <typeparam name="T">The super-type for the pages stored in memory.</typeparam>
    public abstract class HTMLBuffer<T> : IHTMLBuffer, IEnumerable<T>, IDisposable where T : HTMLPage
    {
        #region FileSection

        private class FileSection
        {
            private long start;

            private int bufferLevel;

            public FileSection(long start, int bufferLevel)
            {
                this.start = start;
                this.bufferLevel = bufferLevel;
            }

            public int BufferLevel
            {
                get { return bufferLevel; }
                set { bufferLevel = value; }
            }

            public void Read(FileStream fs, out string html, out URL responseURL)
            {
                fs.Seek(start, SeekOrigin.Begin);

                byte[] countB = new byte[4];
                fs.Read(countB, 0, 4);
                int count = BitConverter.ToInt32(countB, 0);

                byte[] urlB = new byte[count];
                fs.Read(urlB, 0, count);

                countB = new byte[4];
                fs.Read(countB, 0, 4);
                count = BitConverter.ToInt32(countB, 0);

                byte[] htmlB = new byte[count];
                fs.Read(htmlB, 0, count);

                responseURL = new URL(Encoding.Unicode.GetString(urlB));
                html = Encoding.Unicode.GetString(htmlB);
            }
        }

        #endregion

        private Encoding encoding = Encoding.ASCII;
        /// <summary>
        /// Gets or sets the default encoding used by the buffer. Initializes to ASCII encoding.
        /// </summary>
        public Encoding Encoding
        {
            get { return encoding; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                encoding = value;
            }
        }

        private bool loadedFromFile
        {
            get { return this.filepath != null; }
        }

        private string filepath;
        private Dictionary<URL, FileSection> fileDictionary;

        private Dictionary<URL, T> itemDictionary;
        /// <summary>
        /// When overridden in a derived type, this method will perform the actual parsing of information - from the read html to the proper object of type <typeparamref name="T"/> or a derivative.
        /// This method may never return null.
        /// </summary>
        /// <param name="html">The html-code that is to be converted.</param>
        /// <param name="request">The requested url from the http-request.</param>
        /// <param name="response">The response url from the http-request.</param>
        /// <returns>An object of type <typeparamref name="T"/> or one of it derivatives, that represents the loaded page.</returns>
        protected abstract T Convert(string html, URL request, URL response);

        /// <summary>
        /// Writes all the buffered data to a specified file, for later retrieval.
        /// </summary>
        /// <param name="filepath">The file to which the buffered data should be written. The file is overridden.</param>
        public void Save(string filepath)
        {
            Save(filepath, level => true);
        }
        /// <summary>
        /// Writes part of the buffered data to a specified file, for later retrieval.
        /// </summary>
        /// <param name="filepath">The file to which the buffered data should be written. The file is overridden.</param>
        /// <param name="shouldSave">A function that, from bufferlevels, determines which pages should be stored.</param>
        public void Save(string filepath, Func<int, bool> shouldSave)
        {
            List<URL> requestList = new List<URL>();
            List<URL> responseList = new List<URL>();
            List<string> htmlList = new List<string>();
            List<int> bufferList = new List<int>();

            foreach (var var in itemDictionary)
            {
                URL request = var.Key;
                URL response = null;
                string html = null;
                int buffer;

                if (var.Value != null)
                {
                    response = var.Value.ResponseURL;
                    html = var.Value.HTML;
                    buffer = var.Value.BufferLevel;
                }
                else if (loadedFromFile)
                {
                    buffer = fileDictionary[var.Key].BufferLevel;
                    if (shouldSave(buffer))
                        using (FileStream fs = new FileStream(filepath, FileMode.Open))
                            fileDictionary[var.Key].Read(fs, out html, out response);
                }
                else
                    throw new InvalidOperationException("There can be no null-values in the buffer.");

                if (shouldSave(buffer))
                {
                    requestList.Add(request);
                    responseList.Add(response);
                    htmlList.Add(html);
                    bufferList.Add(buffer);
                }
            }

            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
                for (int i = 0; i < requestList.Count; i++)
                {
                    byte[] req = Encoding.Unicode.GetBytes(requestList[i].Address);
                    byte[] res = Encoding.Unicode.GetBytes(responseList[i].Address);
                    byte[] html = Encoding.Unicode.GetBytes(htmlList[i]);

                    writer.Write((byte)0);
                    writer.Write(bufferList[i]);

                    writer.Write(req.Length);
                    writer.Write(req);

                    writer.Write(res.Length);
                    writer.Write(res);

                    writer.Write(html.Length);
                    writer.Write(html);
                }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLBuffer{T}"/> class from a prestored file. See the <see cref="Save(string)"/> method for more.
        /// </summary>
        /// <param name="filepath">The file from which the buffer should be restored.</param>
        public HTMLBuffer(string filepath)
            : this()
        {
            if (new FileInfo(filepath).Exists)
                this.filepath = filepath;
            else
                this.filepath = null;

            if (this.filepath != null)
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                    while (fs.Position < fs.Length)
                    {
                        byte flag = reader.ReadByte();
                        int buffer = reader.ReadInt32();

                        URL req = new URL(Encoding.Unicode.GetString(reader.ReadBytes(reader.ReadInt32())));

                        fileDictionary.Add(req, new FileSection(fs.Position, buffer));
                        itemDictionary.Add(req, null);

                        fs.Seek(reader.ReadInt32(), SeekOrigin.Current);
                        fs.Seek(reader.ReadInt32(), SeekOrigin.Current);
                    }
            }
        }
        /// <summary>
        /// Initializes a new empty instance of the <see cref="HTMLBuffer{T}"/> class.
        /// </summary>
        public HTMLBuffer()
        {
            this.filepath = null;
            this.fileDictionary = new Dictionary<URL, FileSection>();
            this.itemDictionary = new Dictionary<URL, T>();
        }

        /// <summary>
        /// Occurs when the buffer is about to read a url via a http-request.
        /// Allows to determine if one <see cref="URL"/> can be considered equivalent to another <see cref="URL"/> in the context of this <see cref="HTMLBuffer{T}"/>.
        /// </summary>
        public event TranslateURLEventHandler TranslateURL;
        /// <summary>
        /// Raises the <see cref="TranslateURL"/> event.
        /// </summary>
        /// <param name="e">A <see cref="TranslateURLEventArgs"/> that contains the event data.</param>
        protected virtual void OnTranslateURL(TranslateURLEventArgs e)
        {
            if (TranslateURL != null)
                TranslateURL(this, e);
        }

        private URL translate(URL url)
        {
            TranslateURLEventArgs e = new TranslateURLEventArgs(url);
            OnTranslateURL(e);
            return e.Result;
        }

        /// <summary>
        /// Reads a <see cref="URL"/> address using the default encoding.
        /// </summary>
        /// <param name="url">The <see cref="URL"/> to read.</param>
        /// <returns>An object of type <typeparamref name="T"/> or one of it derivatives, that represents the loaded page.
        /// This is read from buffer, if preloaded; or via the <see cref="Convert"/> method, if not.</returns>
        protected T ReadURL(URL url)
        {
            return ReadURL(url, this.encoding);
        }
        /// <summary>
        /// Reads a url-address using the default encoding.
        /// </summary>
        /// <param name="url">The url-address to read.</param>
        /// <returns>An object of type <typeparamref name="T"/> or one of it derivatives, that represents the loaded page.
        /// This is read from buffer, if preloaded; or via the <see cref="Convert"/> method, if not.</returns>
        protected T ReadURL(string url)
        {
            return ReadURL(url, this.encoding);
        }
        /// <summary>
        /// Reads a <see cref="URL"/> address using the specified encoding.
        /// </summary>
        /// <param name="url">The <see cref="URL"/> to read.</param>
        /// <param name="encoding">The encoding used for reading.</param>
        /// <returns>An object of type <typeparamref name="T"/> or one of it derivatives, that represents the loaded page.
        /// This is read from buffer, if preloaded; or via the <see cref="Convert"/> method, if not.</returns>
        protected T ReadURL(URL url, Encoding encoding)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            if (encoding == null)
                throw new ArgumentNullException("encoding");

            url = translate(url);

            T item = null;
            if (itemDictionary.ContainsKey(url))
                item = itemDictionary[url];

            if (item == null)
            {
                if (fileDictionary.ContainsKey(url))
                {
                    string html;
                    URL responseURL;
                    using (FileStream fs = new FileStream(this.filepath, FileMode.Open))
                        fileDictionary[url].Read(fs, out html, out responseURL);

                    item = Convert(html, url, responseURL);
                    if (item == null)
                        throw new InvalidOperationException("The " + typeof(Convert).Name + " method may not return null.");
                    item.Buffer = this;
                    if (itemDictionary.ContainsKey(url))
                        itemDictionary[url] = item;
                    else
                        itemDictionary.Add(url, item);
                }
                else
                {
                    URL newurl;
                    string html = url.GetHTML(encoding, out newurl);
                    newurl = translate(newurl);
                    item = Convert(html, url, newurl);
                    if (item == null)
                        throw new InvalidOperationException("The " + typeof(Convert).Name + " method may not return null.");
                    item.Buffer = this;

                    itemDictionary.Add(url, item);
                    if (url != newurl && !itemDictionary.ContainsKey(newurl))
                    {
                        T item2 = Convert(html, newurl, newurl);
                        if (item2 == null)
                            throw new InvalidOperationException("The " + typeof(Convert).Name + " method may not return null.");
                        item2.Buffer = this;
                        itemDictionary.Add(newurl, item2);
                    }
                }
            }
            return item;
        }
        /// <summary>
        /// Reads a url address using the specified encoding.
        /// </summary>
        /// <param name="url">The url-address to read.</param>
        /// <param name="encoding">The encoding used for reading.</param>
        /// <returns>An object of type <typeparamref name="T"/> or one of it derivatives, that represents the loaded page.
        /// This is read from buffer, if preloaded; or via the <see cref="Convert"/> method, if not.</returns>
        protected T ReadURL(string url, Encoding encoding)
        {
            return ReadURL(new URL(url), encoding);
        }

        #region Removing

        /// <summary>
        /// Removes an url-address from the <see cref="HTMLBuffer{T}"/>.
        /// </summary>
        /// <param name="url">The address to remove.</param>
        /// <returns>A boolean value indicating if the address was removed (if it pre-existed).</returns>
        protected bool Remove(URL url)
        {
            return itemDictionary.Remove(url);
        }
        /// <summary>
        /// Removes a page (of type <typeparamref name="T"/>) from the <see cref="HTMLBuffer{T}"/>.
        /// </summary>
        /// <param name="page">The page to remove.</param>
        /// <returns>A boolean value indicating if the page was removed (if it pre-existed).</returns>
        public bool Remove(T page)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            if (page.Buffer != this)
                return false;

            page.Buffer = null;
            return Remove(page.RequestedURL);
        }
        bool IHTMLBuffer.Remove(HTMLPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (page is T)
                return Remove(page as T);
            else
                return false;
        }
        /// <summary>
        /// Removes all pages at a specified bufferlevel from the <see cref="HTMLBuffer{T}"/>.
        /// </summary>
        /// <param name="bufferLevel">The bufferlevel that should be removed.</param>
        /// <returns>The number of pages removed from the <see cref="HTMLBuffer{T}"/>.</returns>
        public int Remove(int bufferLevel)
        {
            return Remove(level => level.BufferLevel == bufferLevel);
        }
        /// <summary>
        /// Removes all pages in a specified bufferlevel range.
        /// </summary>
        /// <param name="fromLevel">The min or max bufferlevel to remove.</param>
        /// <param name="toLevel">The min or max bufferlevel to remove.</param>
        /// <returns>The number of pages removed from the <see cref="HTMLBuffer{T}"/>.</returns>
        public int Remove(int fromLevel, int toLevel)
        {
            if (fromLevel < toLevel)
                return Remove(level => level.BufferLevel >= fromLevel && level.BufferLevel <= toLevel);
            else if (fromLevel > toLevel)
                return Remove(level => level.BufferLevel >= toLevel && level.BufferLevel <= fromLevel);
            else
                return Remove(fromLevel);
        }
        /// <summary>
        /// Removes all pages that meet a specified predicate.
        /// </summary>
        /// <param name="predicate">A function that determines if a page should be removed. The method must return true if the page is to be removed; false if not.</param>
        /// <returns>The number of pages removed from the <see cref="HTMLBuffer{T}"/>.</returns>
        protected int Remove(Func<T, bool> predicate)
        {
            int count = 0;
            URL[] urls = (from v in itemDictionary
                          where predicate(v.Value)
                          select v.Key).ToArray();
            foreach (URL u in urls)
            {
                Remove(u);
                count++;
            }

            return count;
        }

        #endregion

        void IDisposable.Dispose()
        {
            this.Dispose();
        }
        /// <summary>
        /// Disposes unmanaged resourced.
        /// </summary>
        protected virtual void Dispose()
        {
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            URL[] urls = itemDictionary.Keys.ToArray();
            for (int i = 0; i < urls.Length; i++)
                yield return itemDictionary[urls[i]];
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            URL[] urls = itemDictionary.Keys.ToArray();
            for (int i = 0; i < urls.Length; i++)
                yield return itemDictionary[urls[i]];
        }
    }
}
