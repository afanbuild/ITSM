/*******************************************************************
 * ��Ȩ���У�
 * Description������������Ƕ�׿ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
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
	///		ctrLinkFlows ��ժҪ˵����
	/// </summary>
	public partial class ctrLinkFlows : System.Web.UI.UserControl
	{

		private bool blnHasLinked = false;   //����Ƿ�������ӹ�������ʵ��

		#region ����
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
			// �ڴ˴������û������Գ�ʼ��ҳ��
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
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
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
					//e.Item.Cells[2].Text = "Ƕ��";
					e.Item.Cells[2].Text = @"<INPUT class='btnClass' id=""cmdStart_""" + e.Item.Cells[3].Text +
                                           @"  onclick='window.open(""" + Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + e.Item.Cells[3].Text +
						                   @"&PreMessageID=" + this.MessageID.ToString() + "&FlowJoinType=" + ((int)e_FlowJoinType.efjtNesting).ToString()+ 
                                           @""",""AddNewFlow"" ,""scrollbars=yes,resizable=yes,top=0,left=0,width=""+(window.availWidth-12)+"",height=""+(window.availHeight-35));' type=""button"" value=""Ƕ����"">";
				}
				else
				{
					//e.Item.Cells[2].Text = "�ν�";
					e.Item.Cells[2].Text = @"<INPUT class='btnClass' id=""cmdStart_""" + e.Item.Cells[3].Text +
                        @"  onclick='window.open(""" + Epower.ITSM.Base.Constant.ApplicationPath + "/Forms/OA_AddNew.aspx?NewWin=true&flowmodelid=" + e.Item.Cells[3].Text +
						@"&PreMessageID=" + this.MessageID.ToString() + "&FlowJoinType=" + ((int)e_FlowJoinType.efjtLink).ToString()+ 
						@""",""AddNewFlow"" ,""scrollbars=yes,resizable=yes,top=0,left=0,width=""+(window.availWidth-12)+"",height=""+(window.availHeight-35));' type=""button"" value=""�Ρ���"">";
				}	
				
			}
		}
	}
}
