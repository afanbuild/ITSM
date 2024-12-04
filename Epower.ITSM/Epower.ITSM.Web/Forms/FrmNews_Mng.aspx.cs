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
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// FrmNews_Mng 的摘要说明。
	/// </summary>
	public partial class FrmNews_Mng : BasePage
	{

	    long lngUserID;

		RightEntity re = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面


            this.Master.OperatorID = Constant.XinXiWeiHu;

            this.Master.MainID = "1";
			re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.XinXiWeiHu];

			ControlPageNewsInfo.DataGridToControl=DgNews;
			ControlPageNewsInfo.On_PostBack +=new EventHandler(ControlPageNewsInfo_On_PostBack);


			lngUserID=StringTool.String2Long(Session["UserID"].ToString());

			if(!IsPostBack)
			{
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                DgNews.Columns[1].HeaderText = PageDeal.GetLanguageValue("litTypeId");
                DgNews.Columns[2].HeaderText = PageDeal.GetLanguageValue("litTitle");
                DgNews.Columns[3].HeaderText = PageDeal.GetLanguageValue("litWriter");
                DgNews.Columns[4].HeaderText = PageDeal.GetLanguageValue("litDispFlag");
  
                DgNews.Columns[6].HeaderText = PageDeal.GetLanguageValue("litPubDate");
                DgNews.Columns[7].HeaderText = PageDeal.GetLanguageValue("litOutDate");
                DgNews.Columns[8].HeaderText = PageDeal.GetLanguageValue("litWriter");

				LoadData();
				BindData();

                Session["FromUrl"] = "../Forms/FrmNews_Mng.aspx";
			}
		}

        /// <summary>
        /// 
        /// </summary>
		private void LoadData()
		{
            DataTable dt = NewsDp.GetNewsdetail(StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()), re);
			Session["News_Data"] = dt;
		}

        /// <summary>
        /// 
        /// </summary>
		private void BindData()
		{
			this.DgNews.DataSource =((DataTable)Session["News_Data"]).DefaultView;
			this.DgNews.DataBind();            
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
			this.DgNews.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DgNews_edit);
			this.DgNews.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DgNews_delete);
			this.DgNews.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DgNews_ItemDataBound);

		}
		#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void ControlPageNewsInfo_On_PostBack(object sender, EventArgs e)
		{
			LoadData();
			BindData();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
		private void DgNews_delete(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			 string IntNewsId;
				    IntNewsId=e.Item.Cells[0].Text;
					NewsEntity News=new NewsEntity();
					News.NewsId=StringTool.String2Int(IntNewsId);
					News.Delete(); 
					LoadData();
					BindData();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
		private void DgNews_edit(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			  Response.Redirect("inputnew.aspx?NewsId="+e.Item.Cells[0].Text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DgNews_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "NewsId").ToString();    

                Button btnEdit = (Button)e.Item.Cells[8].FindControl("btnEdit");
                Button btnDelete = (Button)e.Item.Cells[9].FindControl("btnDelete");
               
                //如果过了无效期，不能修改
                DateTime pdt = DateTime.Parse(e.Item.Cells[11].Text.Trim());
                if (pdt<DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
                //如果有权限
                if (re.CanModify)
                {
                    btnEdit.Enabled = true;
                    e.Item.Attributes.Add("ondblclick", "window.open('inputnew.aspx?NewsId=" + sID + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                }
                if (re.CanDelete)
                {
                    btnDelete.Enabled = true;
                }

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
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i>0 && i<8)
                    {
                        int j = i - 1;  //前面有5个隐藏的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }	
	}
}
