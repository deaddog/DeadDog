using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

#pragma warning disable 1591
namespace DeadDog.FTP
{
    public class FTPFileInfo : FTPInfo
    {
        private DateTime lastSizeRead = DateTime.MinValue;
        private long size;

        internal FTPFileInfo(string name, DateTime lastAccessTime, FTPClient client, FTPDirectoryInfo parent)
            : base(name, lastAccessTime, client, parent)
        {
        }

        public long Size
        {
            get
            {
                if (lastSizeRead.Add(this.UpdateInterval) < DateTime.Now)
                {
                    //Get File Size
                    FtpWebRequest reqSize = (FtpWebRequest)FtpWebRequest.Create(new Uri(Fullname));
                    reqSize.Credentials = client.Credentials;
                    reqSize.Method = WebRequestMethods.Ftp.GetFileSize;
                    reqSize.UseBinary = true;
                    FtpWebResponse respSize = (FtpWebResponse)reqSize.GetResponse();
                    this.size = respSize.ContentLength;
                    respSize.Close();
                }
                return size;
            }
        }

        public string Extension
        {
            get { return this.Name.Contains(".") ? this.Name.CutToLast('.', CutDirection.Left, false) : ""; }
        }

        public void Download(System.IO.FileInfo file)
        {
            FileStream outputStream = new FileStream(file.FullName, FileMode.Create);
            downloadToStream(outputStream);
            outputStream.Close();
            outputStream.Dispose();
        }
        public MemoryStream Download()
        {
            MemoryStream outputStream = new MemoryStream();
            downloadToStream(outputStream);
            return outputStream;
        }

        private void downloadToStream(System.IO.Stream stream)
        {
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(this.Fullname));
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = client.Credentials;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            long cl = response.ContentLength;
            int bufferSize = 2048;
            int readCount;
            byte[] buffer = new byte[bufferSize];

            readCount = ftpStream.Read(buffer, 0, bufferSize);
            while (readCount > 0)
            {
                stream.Write(buffer, 0, readCount);
                readCount = ftpStream.Read(buffer, 0, bufferSize);
            }

            ftpStream.Close();
            ftpStream.Dispose();
            response.Close();
        }

        public override void Refresh()
        {
            //Refresh size
            long a = Size;
        }
    }
}
#pragma warning restore 1591
