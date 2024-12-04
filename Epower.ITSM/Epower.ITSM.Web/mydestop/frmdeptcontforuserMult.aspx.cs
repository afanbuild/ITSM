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

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
    /// frmdeptcontforuserMult ��ժҪ˵����
	/// </summary>
    public partial class frmdeptcontforuserMult : BasePage
	{
        #region �Ƿ�����ѡ��Χ
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// �Ƿ����Ʋ��ŷ�Χ
        /// </summary>
        public bool IsLimit
        {
            get
            {
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(!this.IsPostBack)
			{
				string DeptID="0";
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    CtrDeptTree1.IsPower = long.Parse(Session["RootDeptID"].ToString());
                }
                else
                {
                    CtrDeptTree1.IsPower = 1;
                }
				if(Session["UserDeptID"] !=null)
				{
					DeptID=Session["UserDeptID"].ToString();
					if(Session["OldDeptID"] !=null)
					{
						DeptID=Session["OldDeptID"].ToString();
					}
                    Response.Write("<script>window.parent.document.all.DeptID="+DeptID+"</script>");
                   // Response.Write("<SCRIPT>window.parent.userinfo.location='frmUserQueryMult.aspx?DeptID=" + DeptID + "&LimitCurr=" + IsLimit.ToString() + "';</SCRIPT>");
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
