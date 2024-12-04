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
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
	/// frmSelectDept ��ժҪ˵����
	/// </summary>
    public partial class frmselectdeptSubBank : BasePage
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (CtrdepttreeSubBank1 != null)
            {

                // �ڴ˴������û������Գ�ʼ��ҳ��
                if (Request.QueryString["LimitCurr"] != null)
                {
                    string strTmp = Request.QueryString["LimitCurr"];
                    CtrdepttreeSubBank1.LimitCurr = bool.Parse(strTmp);
                    //CtrDeptTree1.LimitCurr = true;
                }
                if (Request.QueryString["CurrDeptID"] != null)
                {
                    CtrdepttreeSubBank1.CurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                    // ��¼��ǰ����
                    Session["OldDeptID"] = DeptDP.GetDeptParentID(CtrdepttreeSubBank1.CurrDeptID);
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
