using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using EpowerCom;
using System.IO;
using Epower.ITSM.SqlDAL;
using System.Text;
using System.Net;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// flow_Attachment_download 的摘要说明。
    /// </summary>
    public partial class flow_Attachment_download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");

            string strFileName = "";
            string filepath = "";

            //加上此句验证 下载页面时是否正常登录系统,否则无效,防止被搜索引擎记录URL导致附件泄密
            // 2006-09-03   更安全的做法是 验证URERID的合法性
            long lngUserID = (long)Session["UserID"];

            long lngFileID = 0;
            string strIDAndName = Request.QueryString["fileid"];
            if (strIDAndName.IndexOf(" ") > -1)
            {
                lngFileID = long.Parse(strIDAndName.Substring(0, strIDAndName.IndexOf(" ")));
            }
            else
            {
                strIDAndName = HttpUtility.UrlDecode(Page.Request.QueryString["fileid"]);
                if (strIDAndName.IndexOf(" ") > -1)
                    lngFileID = long.Parse(strIDAndName.Substring(0, strIDAndName.IndexOf(" ")));

            }

            //lngFileID = long.Parse(Request.QueryString["fileid"]);
            string strMonthPath = string.Empty;    //取文件名称和月份路径

            if (Request["Type"] != null && Request["Type"] == "eKB")
            {
                strFileName = Epower.ITSM.SqlDAL.Inf_InformationDP.GetAttachmentName(lngFileID, ref strMonthPath);
            }
            else if (Request["Type"] != null && Request["Type"] == "eNews")
            {
                strFileName = NewsEntity.GetAttachmentName(lngFileID, ref strMonthPath);
            }
            else if (Request["Type"] != null && Request["Type"] == "eZC")
            {
                strFileName = Equ_DeskDP.GetAttachmentName(lngFileID, ref strMonthPath);
            }
            else if (Request["Type"] != null && Request["Type"] == "eZZ")
            {
                strFileName = EA_ServicesTemplateDP.GetAttachmentName(lngFileID, ref strMonthPath);
            }
            else
            {
                strFileName = MessageDep.GetDownloadFilePath(lngFileID, ref strMonthPath);
            }

            if (strFileCatalog.EndsWith(@"\") == false)
            {
                if (strMonthPath != string.Empty)
                    filepath = strFileCatalog + @"\" + strMonthPath + @"\" + lngFileID.ToString();
                else
                    filepath = strFileCatalog + @"\" + lngFileID.ToString();
            }
            else
            {
                if (strMonthPath != string.Empty)
                    filepath = strFileCatalog + strMonthPath + @"\" + lngFileID.ToString();
                else
                    filepath = strFileCatalog + lngFileID.ToString();
            }


            if (File.Exists(filepath))
            {
                string filename = strFileName;

                System.IO.FileInfo file = new FileInfo(filepath);
                Response.Clear();
                Response.ClearHeaders();

                //add by 郭亮 2009/08/10  解决部分长中文文件名下载被截断的BUG
                Encoding code = Encoding.GetEncoding("gb2312");
                Response.ContentEncoding = code;
                Response.HeaderEncoding = code;
                // 如果是其它环境，以上代码去掉

                // 屏蔽此代码 郭亮 2009/08/10  解决部分长中文文件名下载被截断的BUG  如果其它环境，此代码恢复
                filename = HttpUtility.UrlEncode(filename);

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");

                Response.AddHeader("Content-Length", file.Length.ToString());

                Response.Flush();
                Response.WriteFile(file.FullName);

                Response.End();

            }
            else
            {
                Response.Write("<script>alert('附件所对应的物理文件已经在应用程序外被删除！')</script>");
                Response.Write("<script>window.history.back()</script>");
            }


        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
