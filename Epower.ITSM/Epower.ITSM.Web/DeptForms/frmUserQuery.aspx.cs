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
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmUserInfo 的摘要说明。
	/// </summary>
	public partial class frmUserQuery : BasePage
	{
		//protected System.Web.UI.WebControls.Button cmdLoad;
		long deptId ;
		string strUserID;
		bool bIncludeChildTree;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			ControlPageUserInfo.On_PostBack+=new EventHandler(ControlPageUserInfo_On_PostBack);
			ControlPageUserInfo.DataGridToControl=dgUserInfo;

			

			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);

			strUserID=Session["UserID"].ToString();

			btnDel.Attributes.Add("onclick", "if(confirm('是否真的要删除？')){return true;}else{return false;}");
            if (!IsPostBack)
            {
                deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
                // 记录当前部门
                Session["OldDeptID"] = deptId;
                hidDeptID.Value = deptId.ToString();
                hidQueryDeptID.Value = hidDeptID.Value;
                txtDeptName.Text = DeptDP.GetDeptName(deptId);//获取部门名

                //只有sa 或 admin才有权限重设密码
                string UserName = Session["UserName"].ToString().Trim().ToLower();
                dgUserInfo.Columns[9].Visible = (UserName.Equals("sa") || UserName.Equals("admin"));

                bIncludeChildTree = false;
                LoadData();
                BindData();
            }
            else
            {
                if (hidDeptName.Value.Trim() != string.Empty && hidDeptName.Value.Trim() != txtDeptName.Text.Trim())
                {
                    txtDeptName.Text = hidDeptName.Value.Trim();
                }
            }
		}

		private void LoadData()
		{
			DataTable dt = Exec_Query(Session["RootDeptID"].ToString(),bIncludeChildTree);
			Session["USERINFO_DATA"] = dt;
		}

		private void BindData()
		{
			DataTable dt=(DataTable)Session["USERINFO_DATA"];
			this.dgUserInfo.DataSource = dt.DefaultView;
			this.dgUserInfo.DataBind();
		}
	

		protected string GetVisible(string UserID)
		{
			string sResult;
			if(strUserID.Equals(UserID))
				sResult="VISIBILITY:hidden";
			else
				sResult="VISIBILITY:visible";

			return sResult;
		}


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

		

		

		private void btnDel_Click(object sender, System.EventArgs e)
		{
			ArrayList MyList;
			int result = 0;
			
			for (int i=0; i<Request.Form.Count; i++)
			{		
				string requestname = Request.Form.GetKey(i).ToString();

				if (requestname.StartsWith("Chk"))
				{
					result=1;
					MyList = new ArrayList();
					MyList.Add(requestname.Remove(0,3).ToString());		
					for (int j=0; j<MyList.Count; j++)
					{
						long lngUserID=StringTool.String2Long(MyList[j].ToString());

						UserEntity.Set_Status(lngUserID, StringTool.String2Long(Session["UserID"].ToString()),eO_Deleted.eDeleted);
					}					
				}
                if (result > 0)
                {
                    //强制缓存失效
                    HttpRuntime.Cache.Insert("EpCacheValidUser", false);
                    HttpRuntime.Cache.Insert("EpCacheValidUserDept", false);
                }
			}
			if(result==0)
			{
				Response.Write("<script>alert('请先选择要删除的信息！');</script>");
			}
			LoadData();
			BindData();
		}

		private void ControlPageUserInfo_On_PostBack(object sender, EventArgs e)
		{
			//LoadData();
			BindData();
		}

		private void btnQuery_Click(object sender, System.EventArgs e)
		{	
			bIncludeChildTree=true;
			LoadData();
			BindData();
		}



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
			arrayQueryParam[1][0]="Position";
			if(txtPosition.Text.Trim().Length>0)
			{
				arrayQueryParam[1][1]=txtPosition.Text.Trim();
			}
			else
			{
				arrayQueryParam[1][1]="";
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

		private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				DataGrid dg = (DataGrid)sender;
				for (int i = 0; i < e.Item.Cells.Count; i++)
				{
					// (DataView)e.Item.NamingContainer;
					if (i>1 && i<7)
					{
						int j = i -1;   //注意,因为前面有一个不可见的列
						e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
					}
				}
			}
		}

		

	}
}
