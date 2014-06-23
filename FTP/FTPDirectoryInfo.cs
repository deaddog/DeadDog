﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

#pragma warning disable 1591
namespace DeadDog.FTP
{
    public class FTPDirectoryInfo : FTPInfo
    {
        private DateTime lastRead = DateTime.MinValue;

        private Dictionary<string, FTPDirectoryInfo> directories;
        private Dictionary<string, FTPFileInfo> files;

        internal FTPDirectoryInfo(string name, DateTime lastAccessTime, FTPClient client, FTPDirectoryInfo parent)
            : base(name, lastAccessTime, client, parent)
        {
            this.directories = new Dictionary<string, FTPDirectoryInfo>(StringComparer.CurrentCultureIgnoreCase);
            this.files = new Dictionary<string, FTPFileInfo>(StringComparer.CurrentCultureIgnoreCase);
        }

        public bool IsRoot
        {
            get { return this.Parent == null; }
        }
        public override string Fullname
        {
            get { return this.Name.Length > 0 ? base.Fullname + "/" : base.Fullname; }
        }

        public FTPFileInfo File(string name)
        {
            Refresh();

            if (files.ContainsKey(name))
                return files[name];
            else
                return null;
        }
        public IEnumerable<FTPFileInfo> Files(string name)
        {
            return Files(x => x.Name.Substring(0, x.Name.Length - x.Extension.Length).Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
        public IEnumerable<FTPFileInfo> Files()
        {
            foreach (FTPFileInfo file in files.Values)
                yield return file;
        }
        public IEnumerable<FTPFileInfo> Files(Func<FTPFileInfo, bool> predicate)
        {
            foreach (FTPFileInfo file in files.Values)
                if (predicate(file))
                    yield return file;
        }

        public FTPDirectoryInfo Directory(string name)
        {
            Refresh();
            name = name.Replace('\\', '/').Trim();

            if (directories.ContainsKey(name))
                return directories[name];
            else
                return null;
        }
        public IEnumerable<FTPDirectoryInfo> Directories()
        {
            Refresh();
            foreach (FTPDirectoryInfo dir in directories.Values)
                yield return dir;
        }
        public IEnumerable<FTPDirectoryInfo> Directories(Func<FTPDirectoryInfo, bool> predicate)
        {
            Refresh();
            foreach (FTPDirectoryInfo dir in directories.Values)
                if (predicate(dir))
                    yield return dir;
        }

        public FTPFileInfo Upload(Stream stream, string name)
        {
            stream.Seek(0, SeekOrigin.Begin);

            try
            {
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(Fullname + name));
                ftp.Credentials = this.client.Credentials;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                ftp.ContentLength = stream.Length;
                var requestStream = ftp.GetRequestStream();

                byte[] buffer = new byte[8192];
                int c = 0;

                while ((c = stream.Read(buffer, 0, buffer.Length)) > 0)
                    requestStream.Write(buffer, 0, c);
                requestStream.Close();

                FtpWebResponse response = ftp.GetResponse() as FtpWebResponse;

                var v = response.StatusDescription;
                response.Close();
                if (!v.StartsWith("226"))
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }

            return new FTPFileInfo(name, DateTime.Now, this.client, this);
        }

        public FTPFileInfo Upload(string filepath)
        {
            return Upload(filepath, Path.GetFileName(filepath));
        }
        public FTPFileInfo Upload(string filepath, string name)
        {
            if (!new FileInfo(filepath).Exists)
                return null;

            int count = 3;
            FTPFileInfo ftpFile;
            FileStream stream = null;
            MemoryStream ms = null;

        go:
            try
            {
                stream = new FileStream(filepath, FileMode.Open);
                ms = new MemoryStream((int)stream.Length);
            }
            catch (IOException e)
            {
                if (count-- > 0)
                    goto go;
            }

            byte[] buffer = new byte[8192];
            int c = 0;

            while ((c = stream.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, c);
            stream.Close();
            stream.Dispose();

            ftpFile = Upload(ms, name);
            ms.Close();
            ms.Dispose();

            return ftpFile;
        }

        public override void Refresh()
        {
            if (lastRead.Add(this.client.UpdateInterval) > DateTime.Now)
                return;
            lastRead = DateTime.Now;

            StreamReader reader = null;
            try
            {
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(Fullname));
                ftp.Credentials = this.client.Credentials;
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                FtpWebResponse response = ftp.GetResponse() as FtpWebResponse;
                CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0)
                    {
                        string cutto = line.First("AM", "PM") == 0 ? "AM" : "PM";
                        cutto = line.CutToFirst(cutto, CutDirection.Right, false);
                        cutto = culture.DateTimeFormat.GetMonthName(int.Parse(cutto.Substring(0, 2))) + " " +
                          cutto.Substring(3, 2) + ", " + cutto.Substring(6);

                        DateTime date;
                        if (!DateTime.TryParse(cutto, out date))
                            date = DateTime.MinValue;
                        string sName = line.Substring(39).Trim();
                        if (line.Contains("<DIR>"))
                        {
                            if (!directories.ContainsKey(sName))
                                directories.Add(sName, new FTPDirectoryInfo(sName, date, this.client, this));
                        }
                        else
                        {
                            if (!files.ContainsKey(sName))
                                files.Add(sName, new FTPFileInfo(sName, date, this.client, this));
                        }
                    }
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                ex.Response.Close();
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

        /*private void renameParent(string parentPath)
        {
            if (this.IsRoot)
                this.path = this.name;
            else
                this.path = parentPath + "/" + this.name;

            for (int i = 0; i < directories.Count; i++)
                renameParent(this.path);
        }*/
    }
}
#pragma warning restore 1591
