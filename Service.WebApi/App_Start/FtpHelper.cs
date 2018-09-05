using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using NLog;

namespace Service.WebApi.App_Start
{

    public class FtpHelper
    {
        private static Logger m_loginfo = LogManager.GetCurrentClassLogger();



        public Dictionary<string, Image> imageDic = new Dictionary<string, Image>();//图片字典
        /// <summary>
        /// 拼接ftp地址
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string FtpCombination(string filePath,string ftpIp, string ftpPort)
        {
            string path = filePath.Replace(@"\", "/");
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            return "ftp://" + ftpIp + ":" + ftpPort + path;
        }

        public void FtpDownload(string urlFileName,string ftpUserID, string ftpPassword,string localfilePath,string localfileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                //string fileName = Path.GetFileName(urlFileName);
                //string filePath, 
                FileStream outputStream = new FileStream(Path.Combine(localfilePath, localfileName), FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(urlFileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                ftpStream.Dispose();
                outputStream.Close();
                outputStream.Dispose();
                response.Close();
                response.Dispose();
                // return buf;
            }
            catch (Exception ex)
            {
                m_loginfo.Error("Error:FtpDownload");
                m_loginfo.Error(ex);
            }

        }

        
    }
}