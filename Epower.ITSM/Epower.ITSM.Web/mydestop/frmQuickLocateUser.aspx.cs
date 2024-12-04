using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
    /// frmQuickLocateUser 的摘要说明。
	/// </summary>
    public partial class frmQuickLocateUser : BasePage
	{
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			if(!IsPostBack)
			{
				BindData();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
		{
			string RootDeptID="1";
			DataTable dt = Exec_Query(RootDeptID);
            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
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
			this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

		}
		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDeptRoot"></param>
        /// <param name="bIncludeChildTree"></param>
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
			arrayQueryParam[0][0]="Name";
            if (strUserName != string.Empty)
			{
                arrayQueryParam[0][1] = strUserName;
			}
			else
			{
				arrayQueryParam[0][1]="";
			}

			Session["arrayQueryParam"]=arrayQueryParam;

			DataTable dt= UserDP.GetUsers(arrayQueryParam,sDeptRoot,true);
			return dt;
			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				DataGrid dg = (DataGrid)sender;
				for (int i = 0; i < e.Item.Cells.Count; i++)
				{
					// (DataView)e.Item.NamingContainer;
					if (i>0 && i<6)
					{
						int j = i -1;   //注意,因为前面有一个不可见的列
						e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                LinkButton lnklink = (LinkButton)e.Item.FindControl("lnklink");
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");
                // ID
                sbText.Append("arr[0] ='" + e.Item.Cells[0].Text + "';");
                // 名称
                sbText.Append("arr[1] ='" + lnklink.Text.Trim() + "';");
                sbText.Append("window.parent.returnValue = arr;");
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
        }
	}
}
