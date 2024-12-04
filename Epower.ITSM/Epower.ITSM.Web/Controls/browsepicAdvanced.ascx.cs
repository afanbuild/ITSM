using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.Log;
using EpowerCom;
using EpowerGlobal;
using System.Drawing;

namespace Epower.ITSM.Web.Controls
{
    public partial class browsepicAdvanced : System.Web.UI.UserControl
    {
        private string strTmpCatalog = "";
        private string strFileCatalog = "";



        public String TmpPicURL
        {
            get { return hfPicOriginalURL.Value; }
        }

        public HttpPostedFile File
        { get { return fileUPicture.PostedFile; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            // 临时文件目录
            strTmpCatalog = CommonDP.GetConfigValue("TempCataLog", "TempCataLog");
            strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");
            PreviewUploadPic();
            PictureUpload();
        }

        private void PictureUpload()
        {
            HttpPostedFile File1 = fileUPicture.PostedFile;

            string strTmpSubPath = "";    //建立临时子路径,防止多用户冲突
            string strTmpPath = "";
            string strFileName = "";
            string strTmpSubPath2 = "";

            if (File1 != null && File1.FileName.Trim() != String.Empty)
            {

                try
                {
                    //设置临时路径
                    Random rnd = new Random();
                    strTmpSubPath = rnd.Next(100000000).ToString();
                    strTmpSubPath2 = strTmpSubPath;


                    if (strTmpCatalog.EndsWith(@"\") == false)
                    {
                        strTmpPath = strTmpCatalog + @"\" + strTmpSubPath;
                    }
                    else
                    {
                        strTmpPath = strTmpCatalog + strTmpSubPath;
                    }
                    MyFiles.AutoCreateDirectory(strTmpPath);

                    long lngNextFileID = EPGlobal.GetNextID("FILE_ID");

                    //添加文件					
                    strFileName = strTmpPath + @"\" + String.Format("{0}.jpg", lngNextFileID.ToString());

                    File1.SaveAs(strFileName);

                    //存储图片地址到隐藏域
                    hfPicOriginalURL.Value = String.Format("{0}|{1}", strTmpSubPath2, lngNextFileID);

                }
                catch
                {
                    throw;
                }
            }
        }

        private void PreviewUploadPic()
        {
            String tmpDir = Request.QueryString["tmpdir"];
            String tmpFileId = Request.QueryString["tmpfileid"];

            if (String.IsNullOrEmpty(tmpDir) || String.IsNullOrEmpty(tmpFileId))
                return;

            String filePath = String.Format(@"{0}{1}\{2}.jpg",
                strTmpCatalog, tmpDir, tmpFileId);

            if (!System.IO.File.Exists(filePath))
                return;

            String thImg = String.Format(@"{0}{1}\{2}-thumbnail.jpg",
                strTmpCatalog, tmpDir, tmpFileId);
            // 如果不存在缩略图，就生成
            if (!System.IO.File.Exists(thImg))
            {
                MakeThumbnail(filePath, thImg, 70, 70, "HW");
            }


            using (System.IO.FileStream fs = new System.IO.FileStream(thImg, System.IO.FileMode.Open))
            {
                long lngFileLength = fs.Length;
                Byte[] buffer = new Byte[lngFileLength];
                fs.Read(buffer, 0, buffer.Length);

                System.Drawing.Image imgPreview = System.Drawing.Image.FromStream(fs);

                Response.ContentType = "image/jpeg";
                Response.BinaryWrite(buffer);
                Response.End();
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}