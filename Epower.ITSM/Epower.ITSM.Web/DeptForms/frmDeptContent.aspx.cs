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


        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
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
