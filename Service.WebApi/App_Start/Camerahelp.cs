using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;

namespace Service.WebApi.App_Start
{
  
    public class Camerahelp
    {
        Bitmap bitmap;
        private static Logger loginfo = LogManager.GetCurrentClassLogger();
     
        public uint SDK(string num, string zhakou)
        {         
           
                uint iLastErr;
                bool m_bInitSDK = CHCNetSDK.NET_DVR_Init();
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                if (iLastErr != 0)
                {
                    loginfo.Error(zhakou + "第" + num.ToString() + "道摄像头SDK错误信息：" + iLastErr.ToString());

                }
                if (iLastErr == 0)
                {
                    loginfo.Info(zhakou + "第" + num.ToString() + "道摄像头SDK成功");
                }
            return iLastErr;
        }

        public int Logon(string ip, string port, string username, string password, string num, string zhakou)
        {
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            int m_lUserID2 = CHCNetSDK.NET_DVR_Login_V30(ip, Convert.ToInt32(port), username, password, ref DeviceInfo);
            uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            if (iLastErr != 0)
            {
                loginfo.Error(zhakou + "第" + num.ToString() + "道摄像头登录错误信息：" + iLastErr.ToString());
        
            }
            if (iLastErr == 0)
            {
                loginfo.Info(zhakou + "第" + num.ToString() + "道摄像头SDK成功");
            }
       
            return m_lUserID2;
        }

        public void Exit()
        {
            CHCNetSDK.NET_DVR_Cleanup();
        }

        public void logout(int m_lUserID, string num, string zhakou)
        {
            bool re = CHCNetSDK.NET_DVR_Logout(m_lUserID);
            uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            if (iLastErr != 0)
            {
                loginfo.Error(zhakou + "第" + num.ToString() + "道摄像头登出错误信息：" + iLastErr.ToString());
            }
            if (iLastErr == 0)
            {
                loginfo.Info(zhakou + "第" + num.ToString() + "道摄像头登出成功");
            }


        }

        public void position(int preset, int m_lUserID, string remark2)
        {
            uint ps = Convert.ToUInt32(preset);
            if (ps != 0)
            {
                try
                {
                    bool result = CHCNetSDK.NET_DVR_PTZPreset_Other(m_lUserID, 1, 39, ps);
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    if (iLastErr != 0)
                    {                      
                        loginfo.Info(remark2 + "摄像头位置错误信息：" + iLastErr.ToString());
                    }
                    if (iLastErr == 0)
                    {
                        loginfo.Info(remark2 + "摄像头位置成功:" + ps.ToString());
                    }

                }
                catch (Exception exception)
                {

                    loginfo.Error(exception, "position");
                }
            }
        }


        public void Setposition( int m_lUserID, string remark2)
        {
           
                try
                {
                    bool result = CHCNetSDK.NET_DVR_PTZPreset_Other(m_lUserID, 1, 8, 2);
                    uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    if (iLastErr != 0)
                    {
                        loginfo.Info(remark2 + "摄像头位置错误信息：" + iLastErr.ToString());
                    }
                    if (iLastErr == 0)
                    {
                        loginfo.Info(remark2 + "摄像头位置成功:" + 2.ToString());
                    }

                }
                catch (Exception exception)
                {

                    loginfo.Error(exception, "position");
                }
            
        }


        public string[] picture(string zhakou, string num, int m_lUserID,string ip,string path,string cameraip)
        {
            string[] pic = new string[2];
            string  date = DateTime.Now.ToString("yyyy-MM-dd");
            string  sPath1 = @path + "\\" + date + "\\" + cameraip.ToString()+"\\" + num.ToString() + "\\";
            string  sPath2 = @"ftp://" + ip +  "/" + date + "/" + cameraip.ToString() + "/" + num.ToString() + "/";

            try
            {
                if (!Directory.Exists(sPath1))
                {
                    Directory.CreateDirectory(sPath1);
                }
            }
            catch (Exception ex)
            {
                loginfo.Error(ex);
            }
            DateTime time = DateTime.Now;
            bool result = false;
            string sPicFileName = sPath1 + time.ToString("yyyyMMddHHmmssfff");
            string sPicFileName2 = sPath2 + time.ToString("yyyyMMddHHmmssfff");
            CHCNetSDK.NET_DVR_JPEGPARA para = new CHCNetSDK.NET_DVR_JPEGPARA();
            para.wPicSize = 0xff;
      
            para.wPicQuality = 0;
            result = CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, 1, ref para, sPicFileName + ".jpg");
            uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            if (iLastErr != 0)
            {
                loginfo.Error(zhakou + "第" + num.ToString() + "道摄像头拍照错误信息：" + iLastErr.ToString());
   
                pic[0] = sPicFileName2 + ".jpg";
                pic[1] = sPicFileName + ".jpg";
            }
            if (iLastErr == 0)
            {
                loginfo.Info(zhakou + "第" + num.ToString() + "道摄像头拍照成功");
                pic[0] = sPicFileName2 + ".jpg";
                pic[1]= sPicFileName+".jpg";
            }
            return pic;
        }

        /// <summary>
        /// 截取图片方法
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="beginX">开始位置-X</param>
        /// <param name="beginY">开始位置-Y</param>
        /// <param name="getX">截取宽度</param>
        /// <param name="getY">截取长度</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileExt">后缀名</param>
        public string CutImage(string url, int beginX, int beginY, int getX, int getY, string path,string ip,string num, string fileExt,string name)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sPath1 = @path + "\\" + date + "\\" + num.ToString() + "\\";
            string sPath2 = @"ftp://" + ip + "/" + date + "/" + num.ToString() + "/";
            DateTime time = DateTime.Now;
            string sPicFileName = sPath1 + time.ToString("yyyyMMddHHmmss");
            string sPicFileName2 = sPath2 + time.ToString("yyyyMMddHHmmss");
            try
            {
                if (!Directory.Exists(sPath1))
                {
                    Directory.CreateDirectory(sPath1);
                }
            }
            catch (Exception ex)
            {
                loginfo.Error(ex);
            }

            try
            {
                bitmap = new Bitmap(url);//原图   
            }
            catch(Exception ex)
            {
               
                return "参数无效";
            }
            if (((beginX + getX) <= bitmap.Width) && ((beginY + getY) <= bitmap.Height))
            {
                Bitmap destBitmap = new Bitmap(getX, getY);//目标图
                Rectangle destRect = new Rectangle(0, 0, getX, getY);//矩形容器
                Rectangle srcRect = new Rectangle(beginX, beginY, getX, getY);
                int x = 100;
                int y = 100;
                // Graphics draw = Graphics.FromImage(bmp2);
                Graphics draw = Graphics.FromImage(destBitmap);
                draw.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);

                ImageFormat format = ImageFormat.Png;
                switch (fileExt.ToLower())
                {
                    case "png":
                        format = ImageFormat.Png;
                        break;
                    case "bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case "gif":
                        format = ImageFormat.Gif;
                        break;
                    case "jpg":
                        format = ImageFormat.Jpeg;
                        break;
                }
                destBitmap.Save(sPicFileName + name+"."+ fileExt, format);
                bitmap.Dispose();
                return sPicFileName2 + name + "." + fileExt;

            }
            else
            {
                loginfo.Error("截取范围超出图片范围");
                return "截取范围超出图片范围";
            }


        }

      



        /// <summary>
        /// yushipaizhao
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string HttpDownloadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();

            //创建本地文件写入流
            Stream stream = new FileStream(path, FileMode.Create);

            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            return path;
        }


        public string picturey(string yushiip, string num,  string ip, string path)
        {       
            string[] pic = new string[2];
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string sPath1 = @path + "\\" + date + "\\" + yushiip.ToString() + "\\" + num.ToString() + "\\";
            string sPath2 = @"ftp://" + ip + "/" + date + "/" + yushiip.ToString() + "/" + num.ToString() + "/";

            try
            {
                if (!Directory.Exists(sPath1))
                {
                    Directory.CreateDirectory(sPath1);
                }
            }
            catch
            {

            }
            DateTime time = DateTime.Now;
            string sPicFileName = sPath1 + time.ToString("yyyyMMddHHmmssfff");
            string sPicFileName2 = sPath2 + time.ToString("yyyyMMddHHmmssfff");

            HttpDownloadFile("http://" + yushiip + ":85" + "/images/snapshot.jpg", sPicFileName + ".jpg");
            loginfo.Info("http://" + yushiip + ":85" + "/images/snapshot.jpg" + sPicFileName + ".jpg");
            return sPicFileName2 + ".jpg";
        }

        /// <summary>
        /// ameraset
        /// </summary>
        /// <param name="m_lUserID"></param>
        /// <param name="x1"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void hqtz(int m_lUserID, int x1,int y,int z)
        {
            try
            {
                CHCNetSDK.NET_DVR_PTZPOS x = new CHCNetSDK.NET_DVR_PTZPOS();
                Int32 nSize = Marshal.SizeOf(x);
                IntPtr ptrPicCfg = Marshal.AllocHGlobal(nSize);
                Marshal.StructureToPtr(x, ptrPicCfg, false);
                x = (CHCNetSDK.NET_DVR_PTZPOS)Marshal.PtrToStructure(ptrPicCfg, typeof(CHCNetSDK.NET_DVR_PTZPOS));
                x.wAction = 3;
                x.wPanPos = Convert.ToUInt16(x1);
                x.wTiltPos = Convert.ToUInt16(y);
                x.wZoomPos = Convert.ToUInt16(z);
                IntPtr ptzPosptr = Marshal.AllocHGlobal((Int32)nSize);
                Marshal.StructureToPtr(x, ptzPosptr, false);
                bool re1 = CHCNetSDK.NET_DVR_SetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_SET_PTZPOS, 1, ptzPosptr, (UInt32)nSize);
                uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                loginfo.Info("NET_DVR_SetDVRConfig:" + iLastErr);
            }
            catch (Exception ex)
            {

                loginfo.Error("hqtz"+ex); ;
            }
            
         
        }


        /// <summary>
        /// cameraget
        /// </summary>
        /// <param name="m_lUserID"></param>
        /// <returns></returns>
        public int[] sptz( int m_lUserID)
        {                                     
            int[] xyz=new int[3] ;
            UInt32 dwreturn = 0;
            CHCNetSDK.NET_DVR_PTZPOS x = new CHCNetSDK.NET_DVR_PTZPOS();
            Int32 nSize = Marshal.SizeOf(x);
            IntPtr ptrPicCfg = Marshal.AllocHGlobal(nSize);
            Marshal.StructureToPtr(x, ptrPicCfg, false);
            bool re = CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_PTZPOS, 1, ptrPicCfg, (UInt32)nSize, ref dwreturn);
            uint iLastErr = CHCNetSDK.NET_DVR_GetLastError();
            x = (CHCNetSDK.NET_DVR_PTZPOS)Marshal.PtrToStructure(ptrPicCfg, typeof(CHCNetSDK.NET_DVR_PTZPOS));
            
            loginfo.Info("NET_DVR_GetDVRConfig:" + iLastErr);
            xyz[0] = Convert.ToInt32(x.wPanPos);
            xyz[1] = Convert.ToInt32(x.wTiltPos);
            xyz[2] = Convert.ToInt32(x.wZoomPos);
            loginfo.Info("xyz:"+xyz[0] + xyz[1] + xyz[2]);

            return xyz;
        }
    }
}