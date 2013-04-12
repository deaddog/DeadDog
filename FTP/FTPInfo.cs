using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#pragma warning disable 1591
namespace DeadDog.FTP
{
    public abstract class FTPInfo
    {
        protected TimeSpan UpdateInterval
        {
            get { return this._client.UpdateInterval; }
        }

        private string name;
        private DateTime lastAccessTime;
        private FTPClient _client;
        private FTPDirectoryInfo parent;

        internal FTPInfo(string name, DateTime lastAccessTime, FTPClient client, FTPDirectoryInfo parent)
        {
            this.name = name.Trim(' ', '\n', '\r', '\t', '\\', '/');
            this.lastAccessTime = lastAccessTime;
            this._client = client;
            this.parent = parent;
        }

        protected FTPClient client
        {
            get { return _client; }
        }

        public bool Exists
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return name; }
        }
        public virtual string Fullname
        {
            get { return parent == null ? _client.Server : parent.Fullname + name; }
        }

        public DateTime LastAccessTime
        {
            get { return lastAccessTime; }
        }

        public FTPDirectoryInfo Parent
        {
            get { return parent; }
        }
        public FTPDirectoryInfo Root
        {
            get { return _client.Root; }
        }

        public abstract void Refresh();

        public override string ToString()
        {
            return Fullname;
        }
    }
}
#pragma warning restore 1591
