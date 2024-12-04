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
	/// frmUserInfo 的摘要说明。
	/// </summary>
    public partial class frmuserqueryMult : BasePage
	{
        #region 是否限制选择范围
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// 是否限制部门范围
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

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion 

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            btnQuery_Click();
        }
        #endregion 

		//protected System.Web.UI.WebControls.Button cmdLoad;
		long deptId ;
		string strUserID;
        bool bIncludeChildTree = false;

        protected string strZHHiden = "";
        protected string strZHShow = "display:none;";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			ControlPageUserInfo.On_PostBack+=new EventHandler(ControlPageUserInfo_On_PostBack);
			ControlPageUserInfo.DataGridToControl=dgUserInfo;
			strUserID=Session["UserID"].ToString();
            ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);


            if (Request.QueryString["ExtAll"] != null)
            {
                if (Request.QueryString["ExtAll"] == "true")
                {
                    bIncludeChildTree = true;
                }
            }
			if(!IsPostBack)
			{
				deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
				// 记录当前部门
				Session["OldDeptID"] = deptId;
				hidDeptID.Value=deptId.ToString();
				hidQueryDeptID.Value =hidDeptID.Value;
				txtDeptName.Text=DeptDP.GetDeptName(deptId);//获取部门名

				//bIncludeChildTree=false;
				LoadData();
				BindData();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            btnQuery_Click();
        }

        /// <summary>
        /// 
        /// </summary>
		private void LoadData()
		{
			string RootDeptID="1";
			DataTable dt = Exec_Query(RootDeptID,bIncludeChildTree);
			Session["USERINFO_DATA"] = dt;
		}

        /// <summary>
        /// 
        /// </summary>
		private void BindData()
		{
			DataTable dt=(DataTable)Session["USERINFO_DATA"];
			this.dgUserInfo.DataSource = dt.DefaultView;
			this.dgUserInfo.DataBind();
		}
	
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
		protected string GetVisible(string UserID)
		{
			string sResult;
			if(strUserID.Equals(UserID))
				sResult="VISIBILITY:hidden";
			else
				sResult="VISIBILITY:visible";

			return sResult;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="FunctionName"></param>
        /// <param name="UserName"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
		protected string GetEmailAction(string Title,string FunctionName,string UserName,string Email)
		{
			string sResult="";

			if(Email.Trim().Length>0)
				sResult="<A href='#' title='点击["+Title+"]将 "+UserName+" 加入"+Title+"列表' "+
						"onclick=\""+FunctionName+"('"+UserName+"','"+Email+"')\""+
						">"+Title+"</A>";
			return sResult;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ControlPageUserInfo_On_PostBack(object sender, EventArgs e)
		{
			//LoadData();
			BindData();
		}

        /// <summary>
        /// 
        /// </summary>
		private void btnQuery_Click()
		{	
			bIncludeChildTree=true;
			LoadData();
			BindData();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDeptRoot"></param>
        /// <param name="bIncludeChildTree"></param>
        /// <returns></returns>
		private DataTable Exec_Query(string sDeptRoot,bool bIncludeChildTree)
		{
			string[][] arrayQueryParam = new string[8][];
													  
			for (int i = 0; i < arrayQueryParam.Length; i++) 
			{
				arrayQueryParam[i] = new string[2];
			}


			//部门ID
			arrayQueryParam[0][0]="DeptID";
			if(this.hidQueryDeptID.Value.Trim().Length>0)
			{
				arrayQueryParam[0][1]=this.hidQueryDeptID.Value.Trim();
			}
			else
			{
				arrayQueryParam[0][1]="";
			}

            //职位
            arrayQueryParam[1][0] = "Position";
            if (ctrFCDServiceType.CatelogID != 1014)
            {
                arrayQueryParam[1][1] = ctrFCDServiceType.CatelogValue.Trim();
            }
            else
            {
                arrayQueryParam[1][1] = "";
            }


			//学历
			arrayQueryParam[2][0]="Education";
			arrayQueryParam[2][1]=ddlEdu.SelectedValue;
	

			//Email
			arrayQueryParam[3][0]="Email";
			if(txtEmail.Text.Trim().Length>0)
			{
				arrayQueryParam[3][1]=txtEmail.Text.Trim();
			}
			else
			{
				arrayQueryParam[3][1]="";
			}

			//登陆账号
			arrayQueryParam[4][0]="LoginName";
			if(txtLoginName.Text.Trim().Length>0)
			{
				arrayQueryParam[4][1]=txtLoginName.Text.Trim();
			}
			else
			{
				arrayQueryParam[4][1]="";
			}


			//用户姓名
			arrayQueryParam[5][0]="Name";
			if(txtName.Text.Trim().Length>0)
			{
				arrayQueryParam[5][1]=txtName.Text.Trim();
			}
			else
			{
				arrayQueryParam[5][1]="";
			}

			//电话
			arrayQueryParam[6][0]="TEL";
			if(this.txtTEL.Text.Trim().Length>0)
			{
				arrayQueryParam[6][1]=txtTEL.Text.Trim();
			}
			else
			{
				arrayQueryParam[6][1]="";
			}

			//根部门FullID
			
			arrayQueryParam[7][0]="SortBy";
			arrayQueryParam[7][1]=this.ddlSort.SelectedValue;
			

			Session["arrayQueryParam"]=arrayQueryParam;

			DataTable dt= UserDP.GetUsers(arrayQueryParam,sDeptRoot,bIncludeChildTree);
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

        protected void btnSelect_Click(object sender, EventArgs e)
        {
          
            StringBuilder stUserIDList = new StringBuilder();
            StringBuilder stUserNameList = new StringBuilder();
            foreach (DataGridItem itm in dgUserInfo.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        stUserIDList.Append(itm.Cells[1].Text + ",");
                        stUserNameList.Append(itm.Cells[2].Text + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 用户ID列表
            sbText.Append("arr[0] ='" + stUserIDList.ToString() + "';");
            // 用户名列表
            sbText.Append("arr[1] ='" + stUserNameList.ToString() + "';");
            sbText.Append("arr[2] ='" + "edit" + "';");
            sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            ClientScript.RegisterClientScriptBlock(GetType(), DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            StringBuilder stUserIDList = new StringBuilder();
            StringBuilder stUserNameList = new StringBuilder();
            foreach (DataGridItem itm in dgUserInfo.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        stUserIDList.Append(itm.Cells[1].Text + ",");
                        stUserNameList.Append(itm.Cells[2].Text + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 用户ID列表
            sbText.Append("arr[0] ='" + stUserIDList.ToString() + "';");
            // 用户名列表
            sbText.Append("arr[1] ='" + stUserNameList.ToString() + "';");
            sbText.Append("arr[2] ='" + "add" + "';");
            sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            ClientScript.RegisterClientScriptBlock(GetType(), DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
	}
}
