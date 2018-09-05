using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Service.WebApi.Models;
using System.IO;

namespace Service.WebApi.Controllers
{
    public class CameraController : ApiController
    {
        private static Logger m_loginfo = LogManager.GetCurrentClassLogger();
        private static App_Start.Camerahelp m_camerahelp = new App_Start.Camerahelp();
        private static App_Start.GetFtppic m_GetFtppic = new App_Start.GetFtppic();
        private static App_Start.FtpHelper m_FtpHelper = new App_Start.FtpHelper();
        private static string mOld_FTP_ip = ConfigurationManager.AppSettings["Old_FTP_ip"];
        private static string mOld_FTP_port = ConfigurationManager.AppSettings["Old_FTP_port"];
        private static string mOld_FTP_user = ConfigurationManager.AppSettings["Old_FTP_user"];
        private static string mOld_FTP_password = ConfigurationManager.AppSettings["Old_FTP_password"];
        private static string mNew_FTP_ip = ConfigurationManager.AppSettings["New_FTP_ip"];
        private static string mNew_FTP_port = ConfigurationManager.AppSettings["New_FTP_port"];
        private static string mNew_FTP_user = ConfigurationManager.AppSettings["New_FTP_user"];
        private static string mNew_FTP_password = ConfigurationManager.AppSettings["New_FTP_password"];
        private static string m_ip = ConfigurationManager.AppSettings["ip"];
        private static string m_path = ConfigurationManager.AppSettings["path"];
        private static string m_cut = ConfigurationManager.AppSettings["cut"];
        private static string beginX = ConfigurationManager.AppSettings["beginX"];
        private static string beginY = ConfigurationManager.AppSettings["beginY"];
        private static string getX = ConfigurationManager.AppSettings["getX"];
        private static string getY = ConfigurationManager.AppSettings["getY"];
        string ftpIP;
        string ftpPort;
        string ftpUseid;
        string ftpPassword;
        int m_lUserID;
        [HttpPost]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg CameraPicture(dynamic obj)//,string channel,string container1,string container2, string truck,string remark1,string remark2 
        {
            ResultMsg msg = new ResultMsg();
            string[] pic = new string[2];
            int[] xyz = new int[3];
            try
            {
               
               //业务逻辑
               
            }
            catch (Exception ex)
            {
                m_loginfo.Error("Error:CameraPicture:"+ obj);
                m_loginfo.Error(ex);
                msg.Pass = false;
                msg.Msg = "接口异常";
                return msg;             
            }
            finally {
              
            }
            msg.Pass = true;
            msg.Msg = "";
            msg.Data = pic[0];
            return msg;
        }

        

    }
}
