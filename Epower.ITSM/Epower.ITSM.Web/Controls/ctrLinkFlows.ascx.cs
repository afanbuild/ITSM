/*******************************************************************
 * 版权所有：
 * Description：工作流流程嵌套控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Epower.DevBase.BaseTools;
	using EpowerCom;
	using EpowerGlobal;

	/// <summary>
	///		ctrLinkFlows 的摘要说明。
	/// </summary>
	public partial class ctrLinkFlows : System.Web.UI.UserControl
	{

		private bool blnHasLinked = false;   //标记是否存在连接过的流程实例

		#region 属性
		public long MessageID
		{
			get
			{
				
				if(ViewState[this.ID+"MessageID"]==null)
					return 0;
				else
					return StringTool.String2Long(ViewState[this.ID+"MessageID"].ToString());
			}
			set{ViewState[this.ID+"MessageID"]=value;}
		}

		public long UserID
		{
			get
			{
				
				if(ViewState[this.ID+"UserID"]==null)
					return 0;
				else
					return StringTool.String2Long(ViewState[this.ID+"UserID"].ToString());
			}
			set{ViewState[this.ID+"UserID"]=value;}
		}

        public bool IsVisible
        {
            get
            {

                if (ViewState[this.ID + "IsVisible"] == null)
                    return true;
                else
                    return (bool)ViewState[this.ID + "IsVisible"];
            }
            set { ViewState[this.ID + "IsVisible"] = value; }
        }

        

		

		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			if(!Page.IsPostBack)
			{
                if(this.IsVisible == true)
				    DoBindData();
			}
		}

		private void DoBindData()
		{
			DataSet ds = FlowModel.GetMessageLinkedFlowModel(this.UserID,this.MessageID);
            if (ds != null)
            {
                DataView dv = ds.Tables[0].DefaultView;

                if (dv.Count > 0)
                {
                    dgLinkFlow.Visible = true;
                    dgLinkFlow.DataSource = dv;

                    dgLinkFlow.DataBind();
                }
            }
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.dgLinkFlow.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgLinkFlow_ItemDataBound);

		}
		#endregion

		private void dgLinkFlow_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				

				//e.Item.Style.Remove("ForeColor");
				if (e.Item.Cells[1].Text == "0")
				{
					//e.Item.Cells[2].Text = "嵌套";
					e.Item.Cells[2].Text = @"<INPUT class='btnClass' id=""cmdStart_""" + e.Item.Cells[3].Text +
                                           @"  onclick='window.open(""" + Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + e.Item.Cells[3].Text +
						                   @"&PreMessageID=" + this.MessageID.ToString() + "&FlowJoinType=" + ((int)e_FlowJoinType.efjtNesting).ToString()+ 
                                           @""",""AddNewFlow"" ,""scrollbars=yes,resizable=yes,top=0,left=0,width=""+(window.availWidth-12)+"",height=""+(window.availHeight-35));' type=""button"" value=""嵌　套"">";
				}
				else
				{
					//e.Item.Cells[2].Text = "衔接";
					e.Item.Cells[2].Text = @"<INPUT class='btnClass' id=""cmdStart_""" + e.Item.Cells[3].Text +
                        @"  onclick='window.open(""" + Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + e.Item.Cells[3].Text +
						@"&PreMessageID=" + this.MessageID.ToString() + "&FlowJoinType=" + ((int)e_FlowJoinType.efjtLink).ToString()+ 
						@""",""AddNewFlow"" ,""scrollbars=yes,resizable=yes,top=0,left=0,width=""+(window.availWidth-12)+"",height=""+(window.availHeight-35));' type=""button"" value=""衔　接"">";
				}	
				
			}
		}
	}
}
