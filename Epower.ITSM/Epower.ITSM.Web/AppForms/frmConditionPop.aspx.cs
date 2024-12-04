/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：设置高级查询条件为选择类型时的弹出页面

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-05-20 
 * 
 * 修改日志：
 * 修改时间：
 * 修改描述：

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
using Epower.ITSM.Base;
using System.Text;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmConditionPop : BasePage
    {
        public string sApplicationUrl = Constant.ApplicationPath;     //虚拟路径

        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
        }
        public string Column
        {
            get { return String.IsNullOrEmpty(Request["Column"]) ? "" : Request["Column"].ToString(); }
        }
        public string CondType
        {
            get { return String.IsNullOrEmpty(Request["CondType"]) ? "" : Request["CondType"].ToString(); }
        }
        public string TableName
        {
            get { return String.IsNullOrEmpty(Request["TableName"]) ? "" : Request["TableName"].ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (CondType)
                {
                    case "CATA": // 常用类别
                        string strWhere = " where ColumnName=" + StringTool.SqlQ(Column) + " and TableName = " + StringTool.SqlQ(TableName);
                        DataTable dt = Br_ConditionDP.GetDataTable(strWhere, "");
                        long lngRootID = 1;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lngRootID = long.Parse(dt.Rows[0]["CataRootID"].ToString() == "" ? "1" : dt.Rows[0]["CataRootID"].ToString());
                        }
                        ctrFlowCataDropListNew1.Visible = true;
                        ctrFlowCataDropListNew1.RootID = lngRootID;
                        break;
                    case "DEPT": // 部门类型
                        DeptPicker1.Visible = true;
                        break;
                    case "USER":// 用户类型
                        UserPicker1.Visible = true;
                        break;
                    case "DATE": // 时间类型
                        ctrDateTime.Visible = true;
                        break;
                }
            }

        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            string arr1 = "";
            string arr2 = "";

            switch (CondType)
            {
                case "CATA": // 常用类别
                    arr1 = ctrFlowCataDropListNew1.CatelogID.ToString();
                    arr2 = ctrFlowCataDropListNew1.CatelogValue.Trim();
                    break;
                case "DEPT": // 部门类型
                    arr1 = DeptPicker1.DeptID.ToString();
                    arr2 = DeptPicker1.DeptName.Trim();
                    break;
                case "USER": //用户类型
                    arr1 = UserPicker1.UserID.ToString();
                    arr2 = UserPicker1.UserName.ToString();
                    break;
                case "DATE": // 时间类型
                    arr1 = ctrDateTime.dateTimeString;
                    arr2 = ctrDateTime.dateTimeString;
                    break;
            }

            StringBuilder sbText = new StringBuilder();

            sbText.Append("<script language='javascript'>");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "HidTag") + "').value='" + arr1 + "';");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "txtValue") + "').value='" + arr2 + "';");
            sbText.Append("window.opener.document.getElementById('" + ObjID.Replace("cmdPop", "hidValue") + "').value='" + arr2 + "';");
            sbText.Append("top.close();");

            sbText.Append("</script>");
            Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());

        }
    }
}
