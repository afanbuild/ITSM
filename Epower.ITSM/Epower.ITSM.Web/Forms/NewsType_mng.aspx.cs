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
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// NewsType_mng 的摘要说明。
	/// </summary>
	public partial class NewsType_mng : BasePage
	{

		RightEntity re = null;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
			re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.XinXiLeiBie];

            if (re.CanAdd == false)
            {
                BtAddType.Visible = false;
            }
            else
                BtAddType.Visible = true;

			if(!IsPostBack)
			{
				BindData();
			}
		}

		private void BindData()
		{
			
			this.DgNewsType.DataSource =  NewsDp.GetNewTypedetail();
			this.DgNewsType.DataBind();
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
			this.DgNewsType.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DgNewsType_PageIndexChanged);
			this.DgNewsType.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DgNewsType_ItemDataBound);

		}
		#endregion

		protected bool GetVisible(int i)
		{
			bool t = false;
			if(i<=10) 
				t= true;
			return t;
		}

		protected void Add_NewsType(object sender, System.EventArgs e)
		{
		   string TypeName=this.TxtTypeName.Text.Trim();
			if(TxtTypeName.Text.Trim().Equals(""))
			{
				PageTool.MsgBox(this,"请输入类别名称"); 
			}
			else
			{
				NewsTypeEntity NewsType=new NewsTypeEntity();
				NewsType.TypeId =StringTool.String2Int(this.txtID.Text);
				NewsType.TypeName=this.TxtTypeName.Text.Trim();
				NewsType.IsInner=e0A_IsInner.eTrue;
				NewsType.IsOuter=e0A_IsOuter.eFalse;
				NewsType.Description = txtDesc.Text;
				NewsType.Save();
				TxtTypeName.Text="";
				txtDesc.Text= "";
				BindData();
		   
				txtID.Text = "0";
				BtAddType.Text="添加类别";
				btnCancel.Enabled =false;
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string TypeName=this.TxtTypeName.Text.Trim();
			if(TxtTypeName.Text.Trim().Equals(""))
			{
				PageTool.MsgBox(this,"请输入类别名称"); 
			}
			else
			{
				NewsTypeEntity NewsType=new NewsTypeEntity();
				NewsType.TypeId =StringTool.String2Int(this.txtID.Text);
				NewsType.TypeName=this.TxtTypeName.Text.Trim();
				NewsType.IsInner=e0A_IsInner.eTrue;
				NewsType.IsOuter=e0A_IsOuter.eFalse;
				NewsType.Description = txtDesc.Text;
				NewsType.Save();
				TxtTypeName.Text="";
				txtDesc.Text = "";
				BindData();
		   
				txtID.Text = "0";
				BtAddType.Text="添加类别";
				BtAddType.Enabled = true;
				btnCancel.Enabled =false;
				btnSave.Enabled =false;
			}
		}



		private void DgNewsType_PageIndex(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			this.DgNewsType.CurrentPageIndex=e.NewPageIndex;
			this.DgNewsType.DataBind();
		}

		private void DgNewsType_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(!e.Item.Cells[1].Text.Equals("&nbsp;"))
			{
				if(re.CanModify == false)
				{
					e.Item.Cells[5].Enabled = false;
				}
				if(re.CanDelete == false)
				{
					e.Item.Cells[6].Enabled = false;
				}
			}

		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			txtID.Text = "0";
			TxtTypeName.Text  ="";
			txtDesc.Text = "";
			
			//BtAddType.Text ="添加类别";

			TxtTypeName.Enabled =true; 
			
			BtAddType.Enabled =true;

			btnCancel.Enabled =false;
			btnSave.Enabled =false;

		}

		private void DgNewsType_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			DgNewsType.CurrentPageIndex=e.NewPageIndex;
			DgNewsType.DataBind();

		}

        protected void DgNewsType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                txtID.Text = e.Item.Cells[0].Text.Trim();
                TxtTypeName.Text = e.Item.Cells[3].Text.Trim().Replace("&nbsp;","");
                txtDesc.Text = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");
                //BtAddType.Text ="保 存";
                BtAddType.Enabled = false;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
            }
            else if (e.CommandName == "Delete")
            {
                string IntTypeId;
                IntTypeId = e.Item.Cells[0].Text.Trim();
                NewsTypeEntity NewsType = new NewsTypeEntity();
                NewsType.TypeId = StringTool.String2Int(IntTypeId);
                NewsType.Delete();
                BindData();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DgNews_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 2; i++)
                {
                    if (i > 2)
                    {
                        int j = i - 3;  //前面有5个隐藏的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }	

	}
}
