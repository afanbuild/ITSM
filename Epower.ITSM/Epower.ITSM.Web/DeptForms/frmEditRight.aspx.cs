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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmEditRight 的摘要说明。
	/// </summary>
	public partial class frmEditRight : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(this.Request.QueryString["rightid"]!=null)
				{
					string sId=this.Request.QueryString["rightid"].ToString();
				
					LoadObjectType_RightRange();
					LoadData(StringTool.String2Long(sId));
				}
			}
		}

		#region 加载数据[LoadObjectType_RightRange,LoadData]
		/// <summary>
		/// 加载操作类别下拉列表
		/// </summary>
		private void LoadObjectType_RightRange()
		{
			ListItem lt;
			#region 对象类别
			//ListItem lt=new ListItem("","0");
			//dpdObjectType.Items.Add(lt);

			lt=new ListItem("部门",((int)eO_RightObject.eDeptRight).ToString());
			dpdObjectType.Items.Add(lt);
				
			lt=new ListItem("人员",((int)eO_RightObject.eUserRight).ToString());
			dpdObjectType.Items.Add(lt);

			lt=new ListItem("用户组",((int)eO_RightObject.eActorRight).ToString());
			dpdObjectType.Items.Add(lt);
			#endregion

			#region 权限范围
			//lt=new ListItem("","0");
			//dpdRightRange.Items.Add(lt);
			
			
			lt=new ListItem("个人",((int)eO_RightRange.ePersonal).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所在部门",((int)eO_RightRange.eDeptDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所属部门",((int)eO_RightRange.eDept).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所在机构",((int)eO_RightRange.eOrgDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("所属机构",((int)eO_RightRange.eOrg).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("全局",((int)eO_RightRange.eFull).ToString());
			dpdRightRange.Items.Add(lt);


			#endregion
			
		}
		/// <summary>
		/// 加载一个操作的详细资料
		/// </summary>
		/// <param name="lngId"></param>
		private void LoadData(long lngId)
		{
			RightEntity re;
			if(lngId==0)
			{
				re=new RightEntity();
			}
			else
			{
				re=new RightEntity(lngId);
			}
			labID.Text=re.RightID.ToString();
			txtObjectId.Text=re.ObjectID.ToString();
			txtObjectName.Text=re.ObjectName ;
			hidDeptList.Value = re.ExtDeptList;
			txtDeptNames.Text = DeptDP.GetDeptListNames(re.ExtDeptList);

			if((int)re.ObjectType==0)
				dpdObjectType.SelectedIndex  =0;
			else
				dpdObjectType.SelectedValue=((int)re.ObjectType).ToString();

			txtOperateId.Text=re.OperateID.ToString();
			txtOperateName.Text=re.OperateName.ToString();
			uRight.RightValue=re.RightValue;

			if((int)re.RightRange==0)
				dpdRightRange.SelectedIndex =0;
			else
				dpdRightRange.SelectedValue=((int)re.RightRange).ToString();
			

		}
		#endregion


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
            string strRangeID = Session["RangeID"].ToString();

			if(StringTool.String2Long(txtOperateId.Text.ToString()) == 0)
			{
				PageTool.MsgBox(this,"请选择操作项!");
				return;
			}
			if(StringTool.String2Long(txtObjectId.Text.ToString()) == 0)
			{
				PageTool.MsgBox(this,"请选择授权对象!");
				return;
			}
			if(uRight.RightValue == 0)
			{
				PageTool.MsgBox(this,"请选择权限值!");
				return;
			}
			
			
			RightEntity re=new RightEntity();
			re.ObjectID=StringTool.String2Long(txtObjectId.Text.ToString());
			re.ObjectType=(eO_RightObject)(int.Parse(dpdObjectType.SelectedValue.ToString()));
			re.OperateID=StringTool.String2Long(txtOperateId.Text.ToString());
			re.RightID=StringTool.String2Long(labID.Text.ToString());
			re.RightValue=uRight.RightValue;
			re.RightRange=(eO_RightRange)(int.Parse(dpdRightRange.SelectedValue.ToString()));
			re.ExtDeptList = hidDeptList.Value;
            re.RangeID = strRangeID;
            
			try
			{
				string OtherRightID=RightEntity.Get_RightID(re.OperateID.ToString(),re.ObjectID.ToString(),dpdObjectType.SelectedValue.ToString());
				//if(RightEntity.Is_Record_Exist(re.OperateID.ToString(),re.ObjectID.ToString(),dpdObjectType.SelectedValue.ToString()))
				if(!OtherRightID.Equals("0") && !OtherRightID.Equals(labID.Text.ToString()))
					PageTool.MsgBox(this,"对象已对该操作项设定权限,请从新输入!");
				else
				{
					
					
					re.Save();

					long lngUserID = (long)Session["UserID"];
					if(int.Parse(dpdObjectType.SelectedValue.ToString()) == (int)eO_RightObject.eUserRight 
						&& re.ObjectID == lngUserID)
					{
						//如果更新本人的权限,同时更新HEADER
						Session["UserAllRights"] = RightDP.getUserRightTable(lngUserID);
				
						//PageTool.AddJavaScript(this,"window.opener.parent.topFrame.location.reload(); ");
					}
                  //  PageTool.AddJavaScript(this, "window.opener.document.getElementById('cmdAdd').click();");
                    if (Request["TypeFrm"] != null && Request["TypeFrm"] == "frmrightSearch")
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('cmdFind').click();window.close();");
                    }
                    else if (Request["TypeFrm"] != null && Request["TypeFrm"] == "frmright")
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('cmdFind').click();window.close();");
                    }
                    else
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('ctrSetUserOtherRight2_dgEA_ExtendRights_ctl01_cmdAdd').click();window.close();");
                    }
				}
			}
			catch(Exception ee)
			{
				PageTool.MsgBox(this,"保存权限时出现错误,错误为:\n"+ee.Message.ToString());
			}
		}
	}
}
