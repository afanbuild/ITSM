/****************************************************************************
 * 
 * description:常用操作类 
 * 
 * 如 cookies 操作等
 * 
 * Create by:yxq
 * Create Date:2011-09-06
 * *************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
    public class MClass
    {
        /// <summary>
        /// 保存登录信息
        /// </summary>
        /// <param name="userid">登录名</param>
        /// <param name="pwd">密码</param>
        /// <param name="identity">身份</param>
        public static void SaveCookie(string userid, string pwd, string identity)
        {
            ClearCookies();
            string CookieName = "E8ITSM";

            HttpCookie myCookie = null;
            myCookie = HttpContext.Current.Request.Cookies[CookieName];
            if (myCookie == null)
            {
                myCookie = new HttpCookie(CookieName);
                DateTime now = DateTime.Now;

                myCookie.Values["userid"] = userid;
                myCookie.Values["pwd"] = pwd;
                myCookie.Values["identity"] = identity;
                myCookie.Expires = now.AddDays(7);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            else
            {
                DateTime now = DateTime.Now;

                myCookie.Values["userid"] = userid;
                myCookie.Values["pwd"] = pwd;
                myCookie.Values["identity"] = identity;
                myCookie.Expires = now.AddDays(7);
                HttpContext.Current.Response.Cookies.Set(myCookie);
            }
        }
        /// <summary>
        /// 获取cookie里的资料
        /// </summary>
        /// <param name="CookieName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string CookieName, string key)
        {
            string keyvalue = "";
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
                keyvalue = HttpContext.Current.Request.Cookies[CookieName][key];

            return keyvalue;
        }

        public static string GetCookie(string key)
        {
            string keyvalue = "";
            string CookieName = "E8ITSM";
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
                keyvalue = HttpUtility.HtmlEncode(HttpContext.Current.Request.Cookies[CookieName][key]);

            return keyvalue;
        }
        /// <summary>
        /// 清理cookie的内容
        /// </summary>
        public static void ClearCookies()
        {
            string CookieName = "E8ITSM";
            HttpCookie myCookie = new HttpCookie(CookieName);

            myCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Set(myCookie);

        }

        public static string getString(HttpContext context, string query, string def)
        {
            if (context.Request[query] == null)
            {
                return def;
            }
            else
            {
                return context.Request[query].ToString().Trim();
            }
        }

        public static string getString(HttpContext context, string query)
        {
            return getString(context, query, "");
        }

        public static int getInt(HttpContext context, string query, int def)
        {
            if (context.Request[query] == null)
            {
                return def;
            }
            else
            {
                return CTools.ToInt(context.Request[query].ToString(), def);
            }
        }

        public static int getInt(HttpContext context, string query)
        {
            return getInt(context, query, 0);
        }

        public static long getLong(HttpContext context, string query, long def)
        {
            if (context.Request[query] == null)
            {
                return def;
            }
            else
            {
                return CTools.ToInt64(context.Request[query].ToString(), def);
            }
        }

        public static long getLong(HttpContext context, string query)
        {
            return getLong(context, query, 0);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="actpath">原大图地址</param>
        /// <param name="prepath">要保存的缩略图地址</param>
        /// <param name="swidth">缩略图宽度</param>
        /// <param name="sheight">缩略图宽度</param>
        public static int GetSmallImg(string actpath, string prepath, string fileExt, int swidth,int sheight)
        {
            //缩小的倍数   
            int iScale = 2;
            int width = 0;
            int height = 0;


            //从文件取得图片对象   
            System.Drawing.Image image = System.Drawing.Image.FromFile(actpath);

            //System.Drawing.Image img = image.GetThumbnailImage(image.Width / iScale, image.Height / iScale, null, IntPtr.Zero);
            ////保存普通缩略图   
            //img.Save(smallpath, System.Drawing.Imaging.ImageFormat.Gif);

            /*
            if (image.Width >= image.Height)
            {
                width = length;
                //height = image.Height / (image.Width / length);             
                height = (length * image.Height) / image.Width;
            }
            else
            {
                height = length;
                width = (length * image.Width) / image.Height;
            }
             * */

            int result =0;

            if (image.Width == 70 && image.Height == 70)
            {
                result = 1;
                return result;
            }
            width = swidth;
            height = sheight;



            //取得图片大小   
            System.Drawing.Size size = new System.Drawing.Size(width, height);
            //新建一个bmp图片   
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(size.Width, size.Height);
            //新建一个画板   
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法   
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空一下画布   
            g.Clear(System.Drawing.Color.Blue);
            //在指定位置画图   
            g.DrawImage(image, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);
            //保存高清晰度的缩略图   
            switch (fileExt)
            {
                case "jpg":
                    bitmap.Save(prepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case "bmp":
                    bitmap.Save(prepath, System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case "png":
                    bitmap.Save(prepath, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case "gif":
                    bitmap.Save(prepath, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                default:
                    bitmap.Save(prepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
            }
            g.Dispose();

            return result;
        }
    }
}
