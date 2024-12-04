/*
 *	by duanqs
 * 
 */
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
	/// frmActorEdit 的摘要说明。
	/// </summary>
	public partial class frmActorEdit : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle.Title="用户组维护";
			cmdDelete.Attributes.Add("onclick", "if(confirm('是否真的要删除？')){return true;}else{return false;}");
				
			if(!IsPostBack)
			{
				if(this.Request.QueryString["actorId"]!=null)
				{
					string sActorId=this.Request.QueryString["actorId"].ToString();
					hidActorID.Value=sActorId;
                    txtNo.Text = sActorId;
					if(long.Parse(sActorId) < 10000)
					{
						//系统预定义角色，不能删除和编辑
						cmdDelete.Enabled = false;
						cmdSave.Enabled = false;
					}
					else
					{
						cmdDelete.Enabled = true;
						cmdSave.Enabled = true;
					}
					LoadData(StringTool.String2Long(sActorId));
				}
                if (Session["UserID"].ToString() != "1")
                {
                    DeptPicker1.ContralState = Epower.ITSM.Base.eOA_FlowControlState.eReadOnly;
                }
			}
		}
        private void LoadData(long lngActorID)
        {
            ActorEntity ae = new ActorEntity(lngActorID);
            txtActorDesc.Text = ae.ActorDesc;
            txtActorName.Text = ae.ActorName;

            hidPActorID.Value = ae.ParentID.ToString();
            txtPActorName.Text = new ActorEntity(ae.ParentID).ActorName;

                long lngDeptID = 0;
                string strDeptName = string.Empty;
                DeptDP.GetDeptIDNameByFullID(ae.RangeID, ref lngDeptID, ref strDeptName);
                DeptPicker1.DeptID = lngDeptID;
                DeptPicker1.DeptName = strDeptName;

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

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			ActorEntity ae=new ActorEntity(StringTool.String2Long(hidActorID.Value.ToString()));
			
			ae.ActorName=txtActorName.Text.ToString();
			ae.ActorDesc=txtActorDesc.Text;
			//ae.ParentID=StringTool.String2Int(hidPActorID.Value.ToString());
			ae.ActorID=StringTool.String2Int(hidActorID.Value.ToString());
			
			
			if(ae.ActorName.Trim()=="")
			{
				labMsg.Text="用户组名称不能为空!";
				return;
			}
			else
			{
				labMsg.Text="";
			}

			try
			{
				if(ae.ActorID == 0)
				{
					//新增时
					ae.SystemID = (long)Session["SystemID"];
					ae.RangeID = Session["RangeID"].ToString();
				}
                ae.SystemID = (long)Session["SystemID"];
                if (DeptPicker1.DeptID != 0 && DeptPicker1.DeptID!=1)
                {
                    ae.RangeID = DeptDP.GetDeptFullID(DeptPicker1.DeptID);
                    //Session["usergroupdeptid"] = DeptPicker1.DeptID.ToString();
                    //Session["usergroupdeptname"] = DeptPicker1.DeptName.ToString();
                }
				ae.Save();

                HttpRuntime.Cache.Insert("EpCacheValidSysActors", false);

                txtNo.Text = ae.ActorID.ToString();
                PageTool.MsgBox(this, "用户组保存成功！");
				PageTool.AddJavaScript(this,"window.parent.contents.location='frmActorTree.aspx'");
			}
			catch(Exception ee)
			{
                PageTool.MsgBox(this, "保存数据时出现错误，错误为：\\n" + ee.Message.ToString());
			}
		}

		protected void cmdAdd_Click(object sender, System.EventArgs e)
		{
			txtActorDesc.Text="";
			txtActorName.Text="";
			txtPActorName.Text="";
			hidActorID.Value="";
            txtNo.Text = string.Empty;
			hidPActorID.Value="";
			cmdDelete.Enabled = true;
			cmdSave.Enabled = true;
            if (Session["UserID"].ToString() != "1")
            {
                long lngDeptID = 0;
                string strDeptName = string.Empty;
                DeptDP.GetDeptIDNameByFullID(Session["RangeID"].ToString(), ref lngDeptID, ref strDeptName);
                DeptPicker1.DeptID = lngDeptID;
                DeptPicker1.DeptName = strDeptName;
            }
		}

		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			ActorEntity ae=new ActorEntity();
			ae.ActorID=StringTool.String2Int(hidActorID.Value.ToString());
			try
			{
               // ActorDP.GetUsersForActor(ae.ActorID);
                DataTable dt = ActorDP.GetActorList(ae.ActorID);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        ae.Delete();
                        HttpRuntime.Cache.Insert("EpCacheValidSysActors", false);
                        PageTool.AddJavaScript(this, "window.parent.contents.location='frmActorTree.aspx';window.location='about:blank'");
                        PageTool.MsgBox(this, "用户组删除成功!");
                    }
                    else
                    {
                        PageTool.MsgBox(this, "用户组有所属成员!无法删除!");
                    }
                }
			}
			catch(Exception ee)
			{
                PageTool.MsgBox(this, "删除用户组时出现错误，错误为：\\n" + ee.Message.ToString());
			}
		}
	}
}
