using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

#pragma warning disable 1591
namespace DeadDog.FTP
{
    public class FTPClient
    {
        private const int _DefaultUpdateIntervalSeconds = 300;
        private TimeSpan updateInterval;
        private string server;
        private NetworkCredential credentials;
        private FTPDirectoryInfo root;

        /// <summary>
        /// Gets the server adress. E.g. "ftp://myserver.com/" - Including both "ftp://" and "/"
        /// </summary>
        public string Server
        {
            get { return server; }
        }

        public FTPDirectoryInfo Root
        {
            get { return root; }
        }

        public TimeSpan UpdateInterval
        {
            get { return updateInterval; }
            set
            {
                if (value.TotalSeconds < 0)
                    throw new ArgumentOutOfRangeException("value");
                updateInterval = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DeadDog.Function.FTP class.
        /// </summary>
        /// <param name="server">The server adress. If "ftp://" is omitted in the parameter it is applied by the constructor.</param>
        /// <param name="username">The username used for logging on to the server.</param>
        /// <param name="password">The password used for logging on to the server.</param>
        public FTPClient(string server, string username, string password)
            : this(server, username, password, new TimeSpan(0, 0, _DefaultUpdateIntervalSeconds))
        {
        }
        public FTPClient(string server, string username, string password, TimeSpan updateInterval)
        {
            this.server = server.Replace('\\', '/').Trim().Trim('/') + "/";
            this.credentials = new NetworkCredential(username, password);
            if (this.server.Substring(0, 6).ToLower() != "ftp://")
                this.server = "ftp://" + this.server;

            this.root = new FTPDirectoryInfo("", DateTime.MinValue, this, null);

            if (updateInterval.TotalSeconds < 0)
                throw new ArgumentOutOfRangeException("updateInterval");

            this.updateInterval = updateInterval;
        }

        internal NetworkCredential Credentials
        {
            get { return credentials; }
        }

        /// <summary>
        /// Gets a bool indicating whether a connectiong could be established to the server.
        /// </summary>
        public bool TestConnection()
        {
            bool success = false;
            FtpWebRequest reqFTP;
            WebResponse response = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(server));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = credentials;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                response = reqFTP.GetResponse();
                success = true;
            }
            catch
            {
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return success;
        }
    }
}
#pragma warning restore 1591
