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

using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmPubCstRequest : BasePage
    {
        #region 变量申明
        private long lngUserID = 0;
        private long lngLastID = 0;
        protected string sCheckEnable = "true";  //是否自动检测
        protected long lngCheckTime = 300000;    //检测时间间隔
        #endregion

        #region 脚本调用方法区
     
        /// <summary>
        /// 接收的ＵＲＬ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngMessageID)
        {
            //暂时没处理分页
            string sUrl = "javascript:window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        /// <summary>
        /// 获取快照的脚本
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngAppID"></param>
        /// <returns></returns>
        protected string GetFlowShotInfo(decimal lngID)
        {
            return "GetFlowShotInfo(this," + lngID.ToString()  + ");";
        }

        #endregion

        #region 方法区
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            lngUserID = long.Parse(Session["UserID"].ToString());

            if (!Page.IsPostBack)
            {
                if (Request["type"] != null)
                {
                    Session["FromUrl"] = "../AppForms/frmPubCstRequest.aspx";
                }
                DoDataBind();
                hidLastMessageID.Value = lngLastID.ToString();

                #region #删除权限 --yanghw 2011-09-15
                //删除权限
                RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.noRequest];
                if (re != null)
                {
                    gridReceiveMsg.Columns[gridReceiveMsg.Columns.Count - 4].Visible = re.CanDelete;
                }
                else
                {
                    gridReceiveMsg.Columns[gridReceiveMsg.Columns.Count - 4].Visible = false;
                }
                #endregion 
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="TypeContent"></param>
        private void DoDataBind()
        {
            DataTable dt;
            DataView dv;
            int iSize = 8;
           
            dt = cst_RequestDP.GetDataTable(iSize,""," order by id desc ");
            gridReceiveMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
            gridReceiveMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
            dv = dt.DefaultView;
            gridReceiveMsg.DataSource = dv;
            gridReceiveMsg.DataBind();

            //绑定模板
            Update_Grid_By_DataTable();

        }

        /// <summary>
        /// 更新Grid 绑定事件模板
        /// </summary>
        /// <param name="dt"></param>
        private void Update_Grid_By_DataTable()
        {
            
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            DataTable dt = ee.GetMyTemplaties((long)Session["UserID"], e_ITSMShortCutReqType.eitsmscrtIssue, false);
           

            //对Grid中的每行重新赋值
            for (int i = 0; i < gridReceiveMsg.Items.Count; i++)
            {


                DropDownList ddlTemplaties = ((DropDownList)gridReceiveMsg.Items[i].Cells[2].FindControl("ddlIssueTemplates"));
                ddlTemplaties.Items.Clear();
                 ddlTemplaties.DataSource = dt.DefaultView;
                ddlTemplaties.DataTextField = "TemplateName";
                ddlTemplaties.DataValueField = "IDAndOFlowModelID";
                ddlTemplaties.DataBind();
                
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //记录最大的消息ID供自动检测
                long lngID = long.Parse(e.Item.Cells[5].Text.Trim());
                if (lngID > lngLastID)
                    lngLastID = lngID;
              
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 6; i++)
                {
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                }
            }
        }
        #region gridReceiveMsg_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridReceiveMsg_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "deal")
            {
                DropDownList ddlTemplaties = ((DropDownList)e.Item.Cells[2].FindControl("ddlIssueTemplates"));
                Label lbl = (Label)e.Item.Cells[0].FindControl("lblSubject");
                
                string flowmodelid = "0";
                string templateid = "0";
                if (ddlTemplaties.Items.Count > 0)
                {
                    string temp = ddlTemplaties.SelectedValue;
                    flowmodelid = temp.Split("|".ToCharArray())[1];
                    templateid = temp.Split("|".ToCharArray())[0];
                    Session["IssueShortCutReqSubject"] = lbl.Text.Trim();

                    Session["IssueShortCutReqContext"] = e.Item.Cells[6].Text.ToString();
                    Session["IssueShortCutReqCTel"] = e.Item.Cells[7].Text.ToString();
                    Session["IssueShortCutReqPubID"] = e.Item.Cells[5].Text.ToString();
                }
                Response.Write("<script>window.open('../Forms/oa_AddNew.aspx?flowmodelid=" + flowmodelid + "&IsFirst=true&ep=" + templateid + "','MainFrame','scrollbars=no,status=yes ,resizable=yes,width=680,height=500');</script>");

            }
            if (e.CommandName == "dele")
            {
                long lngID = long.Parse(e.Item.Cells[5].Text.Trim());
                cst_RequestDP.SetRequestNoUse(lngID, (long)Session["UserID"]);

                DoDataBind();
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridReceiveMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                Button lnkdeal = (Button)e.Item.Cells[4].FindControl("lnkdeal");
                e.Item.Attributes.Add("ondblclick", "document.all." + lnkdeal.ClientID + ".click();");

                ((Label)e.Item.FindControl("lblSubject")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "ID").ToString() + ",400);");
            }
        }

        #endregion
    }
}
