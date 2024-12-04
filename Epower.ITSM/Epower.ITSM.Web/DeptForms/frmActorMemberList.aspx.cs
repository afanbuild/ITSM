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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorMemberList 的摘要说明。
	/// </summary>
	public partial class frmActorMemberList : BasePage
	{
		long actorId;

		protected bool GetVisible(int i)
		{
			bool t = false;
			if( i == (int)eO_ActorObject.eCondUserActor ) 
				t= true;
			return t;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ControlActorMembers.On_PostBack +=new EventHandler(ControlActorMembers_On_PostBack);
			CtrTitle.Title="用户组成员维护";
			btnDel.Attributes.Add("onclick", "if(confirm('是否真的要删除？')){return true;} else {return false;}");
			actorId = long.Parse(Request.Params.Get("actorId"));
			hidActorID.Value=actorId.ToString();
			if(!IsPostBack)
			{
				LoadData();
				BindData();
			}
			
			ControlActorMembers.DataGridToControl = dgUserInfo;
		}

		private void LoadData()
		{
            
			//获取用户组描述
			ActorEntity ae=new ActorEntity(actorId);
			this.divDESC.InnerText =ae.ActorDesc;

			string strRangeID = Session["RangeID"].ToString();
			//DataTable dt = ActorControl.GetActorList(actorId,strRangeID);
			string DeptFullID=DeptDP.GetDeptFullID(StringTool.String2Long(Session["RootDeptID"].ToString()));
			DataTable dt = ActorControl.GetActorList(actorId,strRangeID,DeptFullID);
            
            Session["ACTORMEMBER_DATA"] = dt;
		}

        private void LoadDataQuery()
        {
            string username = txtUserName.Text.Trim();
            UserEntity userentity = new UserEntity();
            DataTable dtUser = userentity.GetUserIDByUserName(username);


            //获取用户组描述
            ActorEntity ae = new ActorEntity(actorId);
            this.divDESC.InnerText = ae.ActorDesc;

            string strRangeID = Session["RangeID"].ToString();
            //DataTable dt = ActorControl.GetActorList(actorId,strRangeID);
            string DeptFullID = DeptDP.GetDeptFullID(StringTool.String2Long(Session["RootDeptID"].ToString()));
            DataTable dt = ActorControl.GetActorListAll(actorId, strRangeID, DeptFullID);

            string objectid = "1=1";
            int num = 0;

            if (dtUser.Rows.Count > 0)
            {
                foreach (DataRow dr in dtUser.Rows)
                {
                    if (dtUser.Rows.Count > 0)
                    {
                        if (num <= 0)
                        {
                            objectid += " and objectid=" + dr["userid"].ToString();
                        }
                        else
                        {
                            objectid += " or objectid=" + dr["userid"].ToString();
                        }
                        num += 1;
                    }
                }

            }
            else {
                Session["ACTORMEMBER_DATA"] = dt.Clone();
                return;
                
            }
            

            DataRow[] drs = dt.Select(objectid);

            DataTable dtSeletedSchema = dt.Clone();
            if (drs.Length > 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    dtSeletedSchema.ImportRow(drs[i]);
                }
            }

            Session["ACTORMEMBER_DATA"] = dtSeletedSchema;
        }

		private void BindData()
		{
			this.dgUserInfo.DataSource = ((DataTable)Session["ACTORMEMBER_DATA"]).DefaultView;
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

		}
		#endregion

		protected void btnDel_Click(object sender, System.EventArgs e)
		{
			ArrayList MyList;
			int result = 0;
			
			MyList = new ArrayList();
			for (int i=0; i<Request.Form.Count; i++)
			{		
				string requestname = Request.Form.GetKey(i).ToString();

				if (requestname.StartsWith("Chk"))
				{
//					string a = Request.Form[requestname];
//					if(Request.Form[requestname] == "")
//					{
					result=1;
				
					MyList.Add(requestname.Remove(0,3).ToString());	
	
					for (int j=0; j<MyList.Count; j++)
					{
						string sId=MyList[j].ToString();
						ActorMemberEntity ame=new ActorMemberEntity();
						ame.ID=StringTool.String2Long(sId);
						ame.Delete();
						//UserControls.DelUserInfo(Convert.ToInt32(actorid));
					}			
//					}
							
				}
				
			}
			
			if(result==0)
			{
				Response.Write("<script>alert('请先选择要删除的信息！');</script>");
			}
			LoadData();
			BindData();
		}

		private void ControlActorMembers_On_PostBack(object sender, EventArgs e)
		{
			LoadData();
			BindData();
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			
		}
       
		protected void cmdLoad_Click(object sender, System.EventArgs e)
		{
			LoadData();
			BindData();
		}

		protected string GetVisible(string IsInRange)
		{
			string sResult;
			if(IsInRange.Equals("0"))
				sResult="VISIBILITY:hidden";
			else
				sResult="VISIBILITY:visible";

			return sResult;
		}

        protected void cmdQuery_Click(object sender, EventArgs e)
        {
            LoadDataQuery();
            BindData();
        }

        protected void dgUserInfo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblname = (Label)e.Item.FindControl("lblName");
                
                HiddenField hidactorname=(HiddenField)e.Item.FindControl("hidactorName");
                HiddenField hidobjectname=(HiddenField)e.Item.FindControl("hidobjectName");
                lblname.Text = hidobjectname.Value.ToString() + "->" + hidactorname.Value.ToString();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {

            }
        }

	}
}
