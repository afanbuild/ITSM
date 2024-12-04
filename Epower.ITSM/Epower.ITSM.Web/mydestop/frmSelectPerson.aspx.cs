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

using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.mydestop
{
    public partial class frmSelectPerson : BasePage
    {
        protected string Type
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "-1";
            }
        }

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }

        private string strNodeIndex = "0";
        private long lngCurrDeptID = 0;


        /// <summary>
        /// 设置树的高度
        /// </summary>
        public System.Web.UI.WebControls.Unit TreeHeight
        {
            set { tvDept.Height = value; }
        }

        /// <summary>
        /// 设置树的宽度
        /// </summary>
        public System.Web.UI.WebControls.Unit TreeWidth
        {
            set { tvDept.Width = value; }
        }

        /// <summary>
        /// 显示部门树的时候是否包含本部门及下级部门

        /// </summary>
        public bool LimitCurr
        {
            get
            {
                return ViewState["LimitCurr"] == null ? false : bool.Parse(ViewState["LimitCurr"].ToString());
            }
            set { ViewState["LimitCurr"] = value; }
        }

        /// <summary>
        /// 当前部门编号
        /// </summary>
        public long CurrDeptID
        {
            get { return ViewState["CurrDeptID"] == null ? 0 : StringTool.String2Long(ViewState["CurrDeptID"].ToString()); }
            set { ViewState["CurrDeptID"] = value; }
        }




        /// <summary>
        /// 是否设置权限
        /// </summary>
        public long IsPower
        {
            get { return ViewState["IsPower"] == null ? 0 : StringTool.String2Long(ViewState["IsPower"].ToString()); }
            set { ViewState["IsPower"] = value; }
        }



        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["DeptID"] == null)
            {
                return;
            }
            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }
                InitTreeView();

            }
            //lsbStaff.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
        }

        private void LoadData(long lngDeptId)
        {
            DataTable dt = new DataTable();
            dt = DeptControl.GetDeptUserList(lngDeptId);

            lsbStaff.DataSource = dt.DefaultView;
            lsbStaff.DataTextField = "Name";
            lsbStaff.DataValueField = "userid";
            lsbStaff.DataBind();
        }

        private void InitTreeView()
        {
            long lngRootID = 1;
            if (IsPower != 0)
            {
                lngRootID = IsPower;
            }

            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                //如果是租用的方式,只显示所在机构一层的部门树
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
            }

            //long lngRootID = (long)Session["UserDeptID"];
            if (Session["OldDeptID"] != null)
            {
                lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());
            }
            else
            {
                lngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
            }

            if (lngRootID == 0)
            {
                return;
            }


            ODeptCollection dc = DeptControl.GetAllDeptCollection();


            tvDept.Nodes.Clear();
            TreeNode root = new TreeNode();

            root.Text = dc.GetODept(lngRootID).Name;
            root.Value = lngRootID.ToString();
            //root.ImageUrl = @"..\Images\flow_modify.ico";
            root.ImageUrl = @"..\Images\Flow\1.bmp";
            root.Expanded = true;
            tvDept.Nodes.Add(root);
            tvDept.SelectedNodeStyle.BackColor = Color.Yellow;
            tvDept.SelectedNodeStyle.ForeColor = Color.Red;

            AddSubDepts(ref root, dc, lngRootID, "0.");
            
            tvDept.Attributes.Add("onclick", "if (typeof(Tree_Click) != 'undefined') { Tree_Click();}");
            tvDept.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
        }

        private void AddSubDepts(ref TreeNode root, ODeptCollection dc, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
            foreach (ODept d in dc)
            {
                if (d.ParentID == lngID && d.ID != d.ParentID)
                {
                    //当限制显示当前部门时判断
                    if (d.ID != this.CurrDeptID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = d.ID.ToString();
                        node.Text = d.Name;
                        node.Expanded = false;
                        if (d.IsTemp == 0)
                        {
                            node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                        }
                        else
                        {
                            node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                        }
                        AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;

                    }
                }
            }
        }

        protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        {


            long deptId = Convert.ToInt32(tvDept.SelectedNode.Value.ToString());
            LoadData(deptId);

            
        }
    }
}
