using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.DeptForms.right
{
    public partial class PageRightCatalog : BasePage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitTreeView();                
            }
        }

        public string IfromUrl=string.Empty;

        #region 初始化树 InitTreeView
        /// <summary>
        /// 初始化树结构
        /// </summary>
        private void InitTreeView()
        {
      
            DataTable dt = RightDP.GetRightType();       //取得权限类别
            tvSubject.Nodes.Clear();
            TreeNode root;
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    root = new TreeNode();
                    root.Text = dr["OpCatalog"].ToString();
                    root.Value = dr["OpCatalog"].ToString() + "_" + i.ToString();
                    if (i == 0)
                    {
                        root.Expanded = true ;
                    }
                    else
                    {
                        root.Expanded = false;
                    }
                    root.ImageUrl = "../../Images/biaote_3.gif";
                    tvSubject.Nodes.Add(root);
                    treeViewNode(root, dr["OpCatalog"].ToString(), i);                
                    i++;
                }
            }
        }


        private void treeViewNode(TreeNode ParentRoot, string OpCatalogName,int selectIndex)
        {
            DataTable dt = OperateDP.GetAllOperate((long)Session["SystemID"], OpCatalogName);       //取得权限类别

            TreeNode root;
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    root = new TreeNode();
                    root.Text = dr["OpName"].ToString();
                    root.Value = dr["operateId"].ToString();

                    root.Expanded = true;
                    root.ImageUrl = "../../Images/catalog.ico";
                    root.Target = "cataloginfo";
                    root.NavigateUrl = "frmrightSeach.aspx?operateId=" + dr["operateId"].ToString();
                    ParentRoot.ChildNodes.Add(root);
                    if (selectIndex == 0)
                    {
                        if (i == 0)
                        {
                            IfromUrl = "<SCRIPT>window.parent.cataloginfo.location='frmrightSeach.aspx?operateId=" + dr["operateId"].ToString() + "';</SCRIPT>";

                            Response.Write(IfromUrl);
                        }
                    }

                    i++;
                }
            }
        }

        #endregion 
    }
}
