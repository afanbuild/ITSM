/*******************************************************************
 * 版权所有：
 * Description：删除流程
 * 
 * 
 * Create By  ：
 * Create Date：2007-11-21
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using System.Text;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Common
{
    public partial class frmFlowDelete : BasePage
    {
        private long lngID=0;   //流程实例编号【注： 在流程终止时带的是消息ID,MessageID】

        private bool blnAbort = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["isAbort"] != null)
            {
                if (Request["isAbort"].ToString() == "true")
                {
                    blnAbort = true;
                    Page.Title = "终止流程";
                }
            }
        }
        #region 获取传过来隐藏按钮的id
        public string Opener_hiddenDelete
        {
            get
            {

                if (Request.QueryString["Opener_hiddenDelete"] != null)
                {
                    return Request.QueryString["Opener_hiddenDelete"];
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 判断哪个页面传过来的
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            Message msg = new Message();
            ZHServiceDP dp = new ZHServiceDP();
            if (Request["FlowID"] != null)  //终止时带的是MessageID
                lngID = long.Parse(Request["FlowID"].ToString());

            string cstno = dp.GetCstIessNo(Request["FlowID"].ToString());//查询对应的事件
           

            if (blnAbort == true)
            {
                msg.AutoStopFlow((long)Session["UserID"],lngID, txtRemark.Text.Trim());
            }
            else
            {
                msg.AdminDeleteFlow(lngID, (long)Session["UserID"], txtRemark.Text.Trim());
            }

            if (cstno != "")
            {
                DeleteEmail(cstno);
            }

            //关闭窗体
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 失败
            sbText.Append("arr[0] ='0';");
            // 名称
            if (TypeFrm == "cst_Issue_list" || TypeFrm == "frmFlow_QuestHouseQuery" || TypeFrm == "frm_KBBaseQuery")
            {
                sbText.Append("window.opener.document.getElementById('" + Opener_hiddenDelete + "').click();");
            }
           // sbText.Append("window.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
           // Response.Write(sbText.ToString());
        }


        #region 删除已发邮件记录
        public void DeleteEmail(string cstno)
        {
            ZHServiceDP dp = new ZHServiceDP();

            if (cstno != "")
            {
                dp.DeleteEmail(cstno);//删除已发邮件
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //关闭窗体
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 失败
            sbText.Append("arr[0] ='1';");
            // 名称
            sbText.Append("window.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            // 向客户端发送
            Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
    }
}
