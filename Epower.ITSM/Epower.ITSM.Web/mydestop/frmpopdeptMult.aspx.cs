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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using System.Text;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
    /// <summary>
    /// frmpopdeptMult 的摘要说明。
    /// </summary>
    public partial class frmpopdeptMult : BasePage
    {
        public string RootImg
        {
            get
            {
                if (ViewState["RootImg"] != null)
                    return ViewState["RootImg"].ToString();
                else
                    return "..\\Images\\root.gif";
            }
            set
            {
                ViewState["RootImg"] = value;
            }
        }

        public string NodeImg
        {
            get
            {
                if (ViewState["NodeImg"] != null)
                    return ViewState["NodeImg"].ToString();
                else
                    return "..\\Images\\FlowDesign\\333.ico";
            }
            set
            {
                ViewState["NodeImg"] = value;
            }
        }

        protected long lngCurrDeptID = 1;
        protected void Page_Load(object sender, System.EventArgs e)
        {           
            if (Request.QueryString["CurrDeptID"] != null)
            {
                if (Request.QueryString["CurrDeptID"].Length > 0)
                    lngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                else
                    lngCurrDeptID = 1;
            }           
        }


        /// <summary>
        /// 获取路径
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        //=====================================
        private string strNodeIndex = "0";
        // private long lngCurrDeptID = 0;


        ///// <summary>
        ///// 设置树的高度
        ///// </summary>
        //public System.Web.UI.WebControls.Unit TreeHeight
        //{
        //    set { tvDept.Height = value; }
        //}

        ///// <summary>
        ///// 设置树的宽度
        ///// </summary>
        //public System.Web.UI.WebControls.Unit TreeWidth
        //{
        //    set { tvDept.Width = value; }
        //}

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (CtrDeptTree.CurrentDeptId < 1)
            {
                Response.Write("<script>alert('请选择需要的部门！');</script>");
                return;
            }

            string txtDept = CtrDeptTree.CurrentDeptName;
            string tvValue = CtrDeptTree.CurrentDeptId.ToString();

            if (lsbDeptTo.Items.Count == 0)
            {
                lsbDeptTo.Items.Add(new ListItem(txtDept, tvValue));
                return;
            }
            bool isExist = false;
            for (int i = 0; i < lsbDeptTo.Items.Count; i++)
            {
                if (txtDept == lsbDeptTo.Items[i].Text || tvValue == lsbDeptTo.Items[i].Value)
                {
                    isExist = true;
                    break;
                }
            }
            if (isExist)
            {
                Response.Write("<script>alert('警告，选择部门重复！');</script>");
            }
            else
            {
                lsbDeptTo.Items.Add(new ListItem(txtDept, tvValue));
            }


        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lsbDeptTo.Items.Count; i++)
            {
                hidDeptID.Value += lsbDeptTo.Items[i].Value + ",";
                hidDeptName.Value += lsbDeptTo.Items[i].Text + ",";
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append(" var value='" + hidDeptID.Value + "@" + hidDeptName.Value + "';");
            sbText.Append("if(value !='')");
            sbText.Append("{");
            sbText.Append(" if(value.length>1) ");
            sbText.Append("{");
            sbText.Append("arr=value.split('@');");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtDept').value = arr[1];");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidDeptName').value = arr[1];");
            sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidDept').value = arr[0];");
            sbText.Append("}");
            sbText.Append("}");
            sbText.Append("window.close();");
            sbText.Append("</script>");

            //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), DateTime.Now.Ticks.ToString(), sbText.ToString(), true);

            Response.Write(sbText.ToString());

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lsbDeptTo.Items.Clear();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            lsbDeptTo.Items.Remove(lsbDeptTo.SelectedItem);
        }
    }
}
