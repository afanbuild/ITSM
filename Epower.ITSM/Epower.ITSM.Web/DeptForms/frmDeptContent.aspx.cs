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
using Microsoft.Web.UI.WebControls;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
namespace Epower.ITSM.Web.Forms
{

    public partial class frmDeptContent : BasePage
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            if (!this.IsPostBack)
            {
                string DeptID = "0";
                if (Session["OldDeptID"] != null)
                    DeptID = Session["OldDeptID"].ToString();
                else
                    DeptID = Session["RootDeptID"].ToString();

                if (Session["RootDeptID"] != null && Session["RootDeptID"].ToString() != "0")
                {
                    Response.Write("<SCRIPT>window.parent.deptinfo.location='frmdeptedit.aspx?deptid=" + DeptID + "';</SCRIPT>");
                }
            }

           
        }


        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
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
