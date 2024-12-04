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
	/// frmOperates 的摘要说明。
	/// </summary>
	public partial class frmOperates : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button cmdAdd;

		protected bool GetVisible(int i)
		{
			bool t = false;
			if(i>10000) 
				t= true;
			return t;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			cmdDelete.Attributes.Add("onclick", "if(!confirm('删除将无法恢复,确定要删除吗？')){return false;}");
			if(!IsPostBack)
			{
				uTitle.Title="操作项维护";

				LoadOperateType();

				LoadData();
				BindData();
			}

			ControlPageOperate.On_PostBack +=new EventHandler(ControlPageOperate_On_PostBack);
			//设定分页控件属性
			ControlPageOperate.DataGridToControl=dgOperate;
		}
		
		private void LoadOperateType()
		{
			ListItem lt=new ListItem("功能类",((int)eO_OperateType.eFunction).ToString());
			dpdOpType.Items.Add(lt);
				
			lt=new ListItem("分析类",((int)eO_OperateType.eQuery).ToString());
			dpdOpType.Items.Add(lt);

			lt=new ListItem("全部","0");
			dpdOpType.Items.Insert(0,lt);		
		}

		#region 加载查询数据

		protected void cmdFind_Click(object sender, System.EventArgs e)
		{
			LoadData();
			BindData();
		}

		//绑定数据到网格
		private void BindData()
		{
			dgOperate.DataSource=((DataTable)Session["OPERATE_EDIT"]).DefaultView;
			dgOperate.DataBind();

			
		}

		//加载数据并缓存
		private void LoadData()
		{
			DataTable dt=new DataTable();
			long lngOPID = 0;
			long lngSystemID = (long)Session["SystemID"];
			if(txtOpID.Text.Trim()=="" && dpdOpType.SelectedValue=="0")
			{
				dt=OperateControl.GetAllOperate(lngSystemID);
			}

			if(txtOpID.Text.Trim()!="")
			{
				try
				{
					lngOPID = long.Parse(txtOpID.Text.ToString());
				}
				catch
				{
					lngOPID = 0;
				}
				dt=OperateControl.GetOperateByOperateID(lngOPID,lngSystemID);
			}

			if(dpdOpType.SelectedValue!="0")
			{
				dt=OperateControl.GetOperateByOperateType((eO_OperateType)(int.Parse(dpdOpType.SelectedValue)),lngSystemID);
			}
			Session["OPERATE_EDIT"]=dt;
		}

		#endregion
		
		#region 删除

		

		//删除一个操作
		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			foreach(DataGridItem item in dgOperate.Items)
			{
				if(((CheckBox)item.Cells[0].FindControl("chkSelect")).Checked)
				{
					string sId=((Label)item.Cells[2].FindControl("labOperateID")).Text.ToString();
					OperateEntity oe=new OperateEntity();
					oe.OperatrID=long.Parse(sId);
					oe.Delete();
				}
			}
			LoadData();
			BindData();
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

		private void ControlPageOperate_On_PostBack(object sender, EventArgs e)
		{
			LoadData();
			BindData();
		}
	}
}
