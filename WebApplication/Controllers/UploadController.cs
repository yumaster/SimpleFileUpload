using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// 上传文件方法
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

                return Json(new {
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
    }
}