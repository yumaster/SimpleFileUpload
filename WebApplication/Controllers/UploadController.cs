using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Common;

namespace WebApplication.Controllers
{
    /// <summary>
    /// 基于WebUploader插件的图片上传实例
    /// </summary>
    public class UploadController : Controller
    {
        public static readonly string appId = "yozojqut3Leq7916";
        public static readonly string appKey = "5f83670ada246fc8e0d15ef9****";

        #region 图片上传
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 直接上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// 简单上传
        /// </summary>
        /// <returns></returns>
        public ActionResult SimpleUpload()
        {
            return View();
        }

        /// <summary>
        /// 模态框上传
        /// </summary>
        /// <returns></returns>
        public ActionResult ModalUpload()
        {
            return View();
        }

        /// <summary>
        /// 上传图片文件方法
        /// </summary>
        /// <param name="form">表单参数</param>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImg(FormCollection form, HttpPostedFileBase file)
        {
            try
            {
                string filePathName = string.Empty;
                string localPath = string.Empty;

                localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Upload/Images/");

                if (Request.Files.Count == 0)
                {
                    throw new Exception("请选择上传文件！");
                }

                string ex = Path.GetExtension(file.FileName);

                filePathName = Guid.NewGuid().ToString("N") + ex;
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                string imgAddress = Path.Combine(localPath, filePathName);
                file.SaveAs(imgAddress);

                FileHelper.AddWaterText(imgAddress, "编码：30GAA02AA010$名称：#2澄清池润滑水进水阀$时间：" + DateTime.Now + "", imgAddress, 255, 14);

                return Json(new
                {
                    Status = 200,
                    Message = "上传图片成功！",
                    Data = localPath
                });
            }
            catch (Exception)
            {
                //扔出异常
                throw;
            }
        }


        #endregion

        #region 文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        public ActionResult FileUpload()
        {
            return View();
        }
        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="form"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile(FormCollection form, HttpPostedFileBase file)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("appId", new string[] { appId });
            string sign = Signclient.generateSign(appKey, dic);
            try
            {
                if (Request.Files.Count == 0)
                {
                    throw new Exception("请选择上传文件！");
                }
                using (HttpClient client = new HttpClient())
                {
                    var postContent = new MultipartFormDataContent();
                    HttpContent fileStreamContent = new StreamContent(file.InputStream);
                    postContent.Add(fileStreamContent, "file", file.FileName);
                    var requestUri = "http://dmc.yozocloud.cn/api/file/upload?appId=" + appId + "&sign=" + sign + "";
                    var response = client.PostAsync(requestUri, postContent).Result;
                    Task<string> t = response.Content.ReadAsStringAsync();
                    return Json(new
                    {
                        Status = response.StatusCode.GetHashCode(),
                        Message = response.StatusCode.GetHashCode() == 200 ? "上传文件成功" : "上传文件失败",
                        Data = t.Result
                    });
                }
            }
            catch (Exception ex)
            {
                //扔出异常
                throw;
            }
        }
        #endregion

        #region 文件删除

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        public ActionResult DelFile()
        {
            return View();
        }
        /// <summary>
        /// 删除文件版本
        /// </summary>
        /// <returns></returns>
        public ActionResult DelFileVersion()
        {
            return View();
        }


        [HttpGet]
        public ActionResult FileDelete(string fileId)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("fileId", new string[] { fileId });
            dic.Add("appId", new string[] { appId });
            string sign = Signclient.generateSign(appKey, dic);
            using (HttpClient client = new HttpClient())
            {
                var requestUri = "http://dmc.yozocloud.cn/api/file/delete/file?fileId=" + fileId + "&appId=" + appId + "&sign=" + sign + "";
                var response = client.GetAsync(requestUri).Result;
                Task<string> t = response.Content.ReadAsStringAsync();
                return Json(new
                {
                    Status = response.StatusCode.GetHashCode(),
                    Message = response.StatusCode.GetHashCode() == 200 ? "请求成功" : "请求失败",
                    Data = t.Result
                },JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult FileVersionDelete(string fileVersionId)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("fileVersionId", new string[] { fileVersionId });
            dic.Add("appId", new string[] { appId });
            string sign = Signclient.generateSign(appKey, dic);
            using (HttpClient client = new HttpClient())
            {
                var requestUri = "http://dmc.yozocloud.cn/api/file/delete/version?fileVersionId=" + fileVersionId + "&appId=" + appId + "&sign=" + sign + "";
                var response = client.GetAsync(requestUri).Result;
                Task<string> t = response.Content.ReadAsStringAsync();
                return Json(new
                {
                    Status = response.StatusCode.GetHashCode(),
                    Message = response.StatusCode.GetHashCode() == 200 ? "请求成功" : "请求失败",
                    Data = t.Result
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 新建文档
        /// <summary>
        /// 文档类型，文件名
        /// </summary>
        /// <param name="templateType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult NewDoc(string templateType, string fileName)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("templateType", new string[] { templateType });
            dic.Add("fileName", new string[] { fileName });
            dic.Add("appId", new string[] { appId });
            string sign = Signclient.generateSign(appKey, dic);
            using (HttpClient client = new HttpClient())
            {
                var requestUri = "http://dmc.yozocloud.cn/api/file/template?templateType=" + templateType + "&fileName=" + fileName + "&appId=" + appId + "&sign=" + sign + "";
                var response = client.GetAsync(requestUri).Result;
                Task<string> t = response.Content.ReadAsStringAsync();
                return Json(new
                {
                    Status = response.StatusCode.GetHashCode(),
                    Message = response.StatusCode.GetHashCode() == 200 ? "删除文件版本成功" : "删除文件版本失败",
                    Data = t.Result
                });
            }
        }
        #endregion

        /// <summary>
        /// 在线编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult FileEdit()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetFileEdit(string fileversionId)
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            dic.Add("fileVersionId", new string[] { fileversionId });
            dic.Add("appId", new string[] { appId });
            string sign = Signclient.generateSign(appKey, dic);
            string ret = "http://eic.yozocloud.cn/api/edit/file?fileVersionId=" + fileversionId + "&appId=" + appId + "&sign=" + sign + "";
            return Redirect(ret);
        }

        [HttpPost]
        [Route("3rd/edit/callBack")]
        public ActionResult EditCallBack(string oldFileId, string newFileId, string message, int errorCode)
        {

            //文件ID
            //575716913322135553
            //文件版本 依次累加 0 1 2 3 4
            //575716913322135553_0 、 7

            return Json(new
            {
                oldFileId = oldFileId,
                newFileId = newFileId,
                message = message,
                errorCode = errorCode
            });
        }
    }
}