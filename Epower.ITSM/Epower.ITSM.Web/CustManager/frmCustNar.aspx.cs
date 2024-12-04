using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Epower.ITSM.SqlDAL;
using System.Data;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Collections;

namespace Epower.ITSM.Web.CustManager
{
    public partial class frmCustNar : BasePage
    {
        
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            if (Page.IsPostBack == false)
            {
                InitTreeView();
                LoadIssueTemplaties();
                if (CheckRight(Constant.CustomerService) == false)
                {
                    cmdOK.Enabled = false;

                    txtCustAddr.Enabled = false;
                    cmdCust.Disabled = true;
                    txtEqu.Enabled = false;
                    cmdEqu.Disabled = true;
                }
            }
        }

        private void InitTreeView()
        {
            #region 今日信息导航
            long lngRootID = 1;

            OCatalogCollection cc = CatalogControl.GetAllCatalogCollection();
            tvCatalog.Nodes.Clear();
            Microsoft.Web.UI.WebControls.TreeNode root = new Microsoft.Web.UI.WebControls.TreeNode();
            root.Text = "事件信息";
            
            root.ID = "0_" + lngRootID.ToString();
            root.ImageUrl = @"..\Images\biaote_3.gif";
            root.Expanded = true;
            tvCatalog.Nodes.Add(root);

            Microsoft.Web.UI.WebControls.TreeNode node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.Text = "由我处理";
            node.ID = "isMeCheck";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=isMeCheck";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);


            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.Text = "我参与处理";
            node.ID = "isMePartakeCheck";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=isMePartakeCheck";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);


            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.Text = "超时事件";
            node.ID = "overtimeEvent";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=overtimeEvent";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);

            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.ID = "overtime48Event";
            node.Text = "超48小时事件";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=overtime48Event";
            node.Target = "MainFrame";          
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);


            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.ID = "overtimeEventfulfill";
            node.Text = "超时完成";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=overtimeEventfulfill";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);

            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.ID = "overtimeEventNOfulfill";
            node.Text = "超时未完成";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=overtimeEventNOfulfill";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);


            node = new Microsoft.Web.UI.WebControls.TreeNode();
            node.ID = "notReturnVisit";
            node.Text = "未回访事件";
            node.NavigateUrl = "../AppForms/CST_IssueListLog.aspx?param=notReturnVisit";
            node.Target = "MainFrame";
            node.ImageUrl = @"..\Images\biaote_4.gif";
            node.Expanded = true;
            root.Nodes.Add(node);
            #endregion            

            AddSubCatalogs(ref root, cc, lngRootID, "1053,1056","");//1053-客户级别 1056-客户状态 后期从配置文件中设置
           
            
          
        }

 
        private void AddSubCatalogs(ref Microsoft.Web.UI.WebControls.TreeNode root, OCatalogCollection cc, long lngID, string strIndex,string sPID)
        {
            Microsoft.Web.UI.WebControls.TreeNode node;
            int iPoint = 0;
            foreach (OCatalog c in cc)
            {
                if (c.ParentID == lngID && c.ID != c.ParentID && (lngID != 1 || strIndex.IndexOf(c.ID.ToString()) >=0))
                {
                        node = new Microsoft.Web.UI.WebControls.TreeNode();
                        node.ID = sPID + (sPID==""?"":"_")  + c.ID.ToString();
                        node.Text = c.Name;
                        
                        node.Expanded = true;

                        if (strIndex.IndexOf(c.ID.ToString()) < 0)
                        {
                            node.NavigateUrl = "frmBr_EcustomerToday.aspx?param=" + node.ID;
                            node.Target = "MainFrame";
                        }
                        
                         AddSubCatalogs(ref node, cc, c.ID, strIndex,node.ID);
                        
                        if (c.ParentID == 1)
                        {
                            //根分类下面的 直接添加到树
                            node.ImageUrl = @"..\Images\biaote_3.gif";
                            tvCatalog.Nodes.Add(node);
                        }
                        else
                        {
                            node.ImageUrl = @"..\Images\biaote_4.gif";
                            root.Nodes.Add(node);
                        }


                        iPoint++;

                   
                }
            }
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            Session["IssueShortCutReqSubject"] = txtSubject.Text.Trim();
            Session["IssueShortCutReqContext"] = txtContext.Text.Trim();
            Session["IssueShortCutReqCustID"] = hidCustID.Value.Trim();
            Session["IssueShortCutReqCustName"] = txtCustAddr.Text.Trim();
            Session["IssueShortCutReqEquID"] = hidEqu.Value.Trim();
            Session["IssueShortCutReqEquName"] = txtEqu.Text.Trim();

            string temp = ddlTemplaties.SelectedValue;
            string flowmodelid = "0";
            string templateid = "0";
            flowmodelid = temp.Split("|".ToCharArray())[1];
            templateid = temp.Split("|".ToCharArray())[0];

            Response.Write("<script>window.open('../Forms/oa_AddNew.aspx?flowmodelid=" + flowmodelid + "&IsFirst=true&ep=" + templateid + "','MainFrame','scrollbars=no,status=yes ,resizable=yes,width=680,height=500');</script>");

        }

        private void LoadIssueTemplaties()
        {
            //EA_ShortCutTemplateDP
            ddlTemplaties.Items.Clear();
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            DataTable dt = ee.GetMyTemplaties((long)Session["UserID"], e_ITSMShortCutReqType.eitsmscrtIssue, false);
            ddlTemplaties.DataSource = dt.DefaultView;
            ddlTemplaties.DataTextField = "TemplateName";
            ddlTemplaties.DataValueField = "IDAndOFlowModelID";
            ddlTemplaties.DataBind();
            if (dt.Rows.Count == 0)
                cmdOK.Enabled = false;
        }

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion
    }
}
