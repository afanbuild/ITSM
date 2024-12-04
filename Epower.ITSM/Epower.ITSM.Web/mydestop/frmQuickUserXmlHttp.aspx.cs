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
    /// frmQuickUserXmlHttp ��ժҪ˵����
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
                    //�����Ƿ������� 0 ��, 1 ��
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
                if (icount == 1)  //�ҵ�Ψһ
                {
                    sReturnValue = dt.Rows[0]["UserId"].ToString() + "," + dt.Rows[0]["name"].ToString();
                }
                else if (icount > 1)  //�ҵ����
                {
                    sReturnValue = "0";
                }
                Response.Clear();
                //�����Ƿ������� 0 ��, 1 ��
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
            //�û�����
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
