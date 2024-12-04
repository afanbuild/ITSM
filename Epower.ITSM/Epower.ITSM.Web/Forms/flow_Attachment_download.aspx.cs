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
    /// flow_Attachment_download ��ժҪ˵����
    /// </summary>
    public partial class flow_Attachment_download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string strFileCatalog = CommonDP.GetConfigValue("TempCataLog", "FileCataLog");

            string strFileName = "";
            string filepath = "";

            //���ϴ˾���֤ ����ҳ��ʱ�Ƿ�������¼ϵͳ,������Ч,��ֹ�����������¼URL���¸���й��
            // 2006-09-03   ����ȫ�������� ��֤URERID�ĺϷ���
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
            string strMonthPath = string.Empty;    //ȡ�ļ����ƺ��·�·��

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

                //add by ���� 2009/08/10  ������ֳ������ļ������ر��ضϵ�BUG
                Encoding code = Encoding.GetEncoding("gb2312");
                Response.ContentEncoding = code;
                Response.HeaderEncoding = code;
                // ������������������ϴ���ȥ��

                // ���δ˴��� ���� 2009/08/10  ������ֳ������ļ������ر��ضϵ�BUG  ��������������˴���ָ�
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
                Response.Write("<script>alert('��������Ӧ�������ļ��Ѿ���Ӧ�ó����ⱻɾ����')</script>");
                Response.Write("<script>window.history.back()</script>");
            }


        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN���õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}
