using Service.WebApi.Models;
using System.Web.Http;
using NLog;
using System.Net;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace Service.WebApi.Controllers
{

    public class TestController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //一下api为测试模板案例
        //参考：http://www.cnblogs.com/landeanfen/p/5337072.html

        [HttpGet]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg OnePrams(string id)
        {
            ResultMsg resultMsg = new ResultMsg();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,OnePrams";
            resultMsg.Data = id;
            logger.Info("GetProduct");
            return resultMsg;
        }
        [HttpGet]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg TwoPrams(string id,string name)
        {
            ResultMsg resultMsg = new ResultMsg();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,TwoPrams";
            resultMsg.Data = id + name;
            logger.Info("GetProduct");
            return resultMsg;
        }
        [HttpGet]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg ThreePrams(string id, bool isok,int age)
        {
            ResultMsg resultMsg = new ResultMsg();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,ThreeArrPrams";
            resultMsg.Data = id + isok + age;
            logger.Info("GetProduct");
            return resultMsg;
        }

        //get 传递实体需要加[FromUri]特性
        [HttpGet]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg ItemPrams([FromUri]TestItem item)
        {
            ResultMsg resultMsg = new ResultMsg();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,ItemPrams";
            resultMsg.Data = item;
            logger.Info("GetProduct");
            return resultMsg;
        }

        [HttpPost]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg OneBodyPrams(dynamic obj)//[FromBody]
        {
            ResultMsg resultMsg = new ResultMsg();
            string m = Request.Method.ToString();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,OneBodyPrams";
            resultMsg.Data = obj;
            logger.Info("OneBodyPrams");
            return resultMsg;
        }

        [HttpPost]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public ResultMsg TwoBodyPrams(TestItem item)
        {
            ResultMsg resultMsg = new ResultMsg();
            string m=Request.Method.ToString();
            resultMsg.Pass = true;
            resultMsg.Msg = "操作成功,TwoBodyPrams";
            resultMsg.Data = item;
            logger.Info("TwoBodyPrams");
            return resultMsg;
        }

        [HttpPost]
        [TokenAuthorize(tokenKey = "TokenKey")]
        public async Task<ResultMsg> File()//上传文件实例
        {
            ResultMsg resultMsg = new ResultMsg();
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    logger.Error("上传格式不是multipart/form-data");
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                string root = HttpContext.Current.Server.MapPath("/UploadFiles/");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                var provider = new MultipartFormDataStreamProvider(root);
                StringBuilder sb = new StringBuilder();
                await Request.Content.ReadAsMultipartAsync(provider);//接受数据
                foreach (MultipartFileData file in provider.FileData)//接收文件
                {
                    Debug.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
                    Debug.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
                }
                foreach (var key in provider.FormData.AllKeys)//接收FormData
                {
                    Debug.WriteLine(key,provider.FormData[key]);
                }

                //// 如何上传文件到文件名.
                //foreach (var file in provider.FileData)
                //{
                //    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                //    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                //    if (fileinfo.Length > 0)
                //    {
                //        string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                //        fileinfo.CopyTo(Path.Combine(root, fileinfo.Name + fileExt), true);
                //        fileinfo.Delete();//删除原文件
                //    }
                //    else {
                //        resultMsg.Pass = false;
                //        resultMsg.Msg = "文件长度错误";
                //    }
                //}
            }
            catch (System.Exception e)
            {
                logger.Error(e, "FilePrams服务器错误");
            }
            return resultMsg;
        }
    }
}
