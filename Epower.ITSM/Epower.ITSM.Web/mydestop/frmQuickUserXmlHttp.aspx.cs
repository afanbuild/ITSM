using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
    /// frmQuickUserXmlHttp 的摘要说明。
	/// </summary>
    public partial class frmQuickUserXmlHttp : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request["UserID"] != null)
            {
                DataTable dt2 = Exec_QuerUer(long.Parse(Request["UserID"].ToString()));
                if (dt2.Rows.Count > 0)
                {
                    string sReturnValue = "-1";

                    sReturnValue = dt2.Rows[0]["LOGINNAME"].ToString() + "," + dt2.Rows[0]["Name"].ToString() + "," + dt2.Rows[0]["MOBILE"].ToString() + "," + dt2.Rows[0]["TELNO"].ToString() + "," + dt2.Rows[0]["DeptName"].ToString() + "," + dt2.Rows[0]["DeptId"].ToString();
                    Response.Clear();
                    //返回是否有新增 0 无, 1 有
                    Response.Write(sReturnValue);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                string RootDeptID = "1";
                DataTable dt = Exec_Query(RootDeptID);

                string sReturnValue = "-1";
                int icount = dt.Rows.Count;
                if (icount == 1)  //找到唯一
                {
                    sReturnValue = dt.Rows[0]["UserId"].ToString() + "," + dt.Rows[0]["name"].ToString();
                }
                else if (icount > 1)  //找到多个
                {
                    sReturnValue = "0";
                }
                Response.Clear();
                //返回是否有新增 0 无, 1 有
                Response.Write(sReturnValue);
                Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDeptRoot"></param>
        /// <returns></returns>
        private DataTable Exec_Query(string sDeptRoot)
        {
            string[][] arrayQueryParam = new string[1][];

            for (int i = 0; i < arrayQueryParam.Length; i++)
            {
                arrayQueryParam[i] = new string[2];
            }
            string strUserName = "";
            if (Request["UserName"] != null)
                strUserName = Request["UserName"].ToString();
            //用户姓名
            arrayQueryParam[0][0] = "Name";
            if (strUserName != string.Empty)
            {
                arrayQueryParam[0][1] = strUserName;
            }
            else
            {
                arrayQueryParam[0][1] = "";
            }

            Session["arrayQueryParam"] = arrayQueryParam;

            DataTable dt = Epower.DevBase.Organization.SqlDAL.UserDP.GetUsers(arrayQueryParam, sDeptRoot, true);
            return dt;
        }

        private DataTable Exec_QuerUer(long userId)
        {
            return Epower.DevBase.Organization.SqlDAL.UserDP.getExecUser(userId);

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
