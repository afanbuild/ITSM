//
// 文件名  ：ControlPage.CS
// 模块名称：
// 程序描述：
// 实现功能：用一个标准化的UserControl控制DataGrid的分页,替代DataGrid自身的分页功能
// 设计日期：2004-03-23
// 修改人  ：
// 修改日期：
// 版本    ：V1.0
//----------------------------------------------------------------
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	
	/// <summary>
	///控制DataGrid的分页。
	/// </summary>
	public partial class ControlPage : System.Web.UI.UserControl
	{
		#region var
		#endregion

		public event System.EventHandler On_PostBack;

		#region Property

        /// <summary>
        /// 分页控件的每页记录数
        /// </summary>
        private int iPageSize = 20;
        public int PageSize
        {
            set { iPageSize = value; }
        }
		
		/// <summary>
		/// 设置被控制的DataGrid
		/// 在Page_Load里设置本属性(包括回发)
		/// </summary>
		public DataGrid DataGridToControl
		{
			set
			{
				dgControl = value;
				dgControl.AllowPaging = true;
				dgControl.PagerStyle.Visible=false;
				int count = Convert.ToInt32(dplPageButtonCount.SelectedValue);
				if (count == 0)
					dgControl.AllowPaging = false;
				else
				{
					dgControl.AllowPaging = true;
					dgControl.PageSize = Convert.ToInt32(dplPageButtonCount.SelectedValue);
				}
				dgControl.DataBinding += new EventHandler(this.dgControl_DataBinding); 
				DisplayInformation();
			}
			get
			{
				return dgControl;
			}
		}


		public int PageButtonCountIndex
		{
			set
			{
				dplPageButtonCount.SelectedIndex = value;
			}
			get
			{
				return dplPageButtonCount.SelectedIndex;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public int CurrentPage
        {
            get
            {
                if (ViewState[this.ClientID.ToString() + "CurrentPage"] != null)
                {
                    if (ViewState[this.ClientID.ToString() + "CurrentPage"].ToString() != "0")
                    {
                        return (int)ViewState[this.ClientID.ToString() + "CurrentPage"];
                    }
                    else
                        return 1;
                }
                else
                    return 1;
            }
            set
            {
                ViewState[this.ClientID.ToString() + "CurrentPage"] = value;
                dgControl.CurrentPageIndex = value;
            }
        }

		#endregion

		#region private

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                for (int i = 0; i < dplPageButtonCount.Items.Count; i++)
                {
                    if (dplPageButtonCount.Items[i].Value == iPageSize.ToString())
                    {
                        dplPageButtonCount.SelectedIndex = i;
                        break;
                    }
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
			this.btnFirstPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnFirstPage_Click);
			this.btnPrevPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevPage_Click);
			this.btnNextPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnNextPage_Click);
			this.btnLastPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnLastPage_Click);

		}
		#endregion

		protected DataGrid dgControl;

		/// <summary> Push 首页 button /// </summary>
		private void btnFirstPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			dgControl.CurrentPageIndex = 0;
			dgControl.DataBind();
			On_PostBack(this,null);
		}

		/// <summary> Push 上一页 button /// </summary>
		private void btnPrevPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int index = dgControl.CurrentPageIndex - 1;
			if(index > 0)
				dgControl.CurrentPageIndex = index;
			else if(index == 0)
			{
				btnFirstPage_Click(null,null);
				return;
			}
			dgControl.DataBind();
			On_PostBack(this,null);	
		}

		/// <summary> Push 下一页 button /// </summary>
		private void btnNextPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int index = dgControl.CurrentPageIndex + 1;
			int max = dgControl.PageCount -1;
			if(index < max)
				dgControl.CurrentPageIndex = index;
			else if(index == max)
			{
				btnLastPage_Click(null,null);
				return;
			}
			dgControl.DataBind();
			On_PostBack(this,null);
		}

		/// <summary> Push 最后一页 button /// </summary>
		private void btnLastPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			dgControl.CurrentPageIndex = dgControl.PageCount - 1;
			dgControl.DataBind();
			On_PostBack(this,null);
		}

		/// <summary> 改变每页显示行数 /// </summary>
		protected void dplPageButtonCount_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgControl.CurrentPageIndex = 0;
			dgControl.DataBind();
			On_PostBack(this,null);

		}

		/// <summary> 数据绑定发生,需更新显示信息 /// </summary>
		private void dgControl_DataBinding(object sender, System.EventArgs e)
		{
			DisplayInformation();
		}

		/// <summary>
		/// 更新显示信息
		/// </summary>
		private void DisplayInformation()
		{
			try
			{
                if (dgControl.DataSource == null)
                    return;
				int nRowCount = ((DataView)dgControl.DataSource).Count;
				lblRowCount.Text = nRowCount.ToString();
				if(!dgControl.AllowPaging)
				{
                    if (nRowCount>0)
                        lblPageIndex.Text = "1 / 1";
                    else
                        lblPageIndex.Text = "0 / 0";
					return;
				}

				int nPageCount;
				if((nRowCount % dgControl.PageSize)  == 0)
				{
					nPageCount = Convert.ToInt32(nRowCount / dgControl.PageSize);
				}
				else
				{
					nPageCount = Convert.ToInt32(nRowCount / dgControl.PageSize) + 1;
				}
				if(dgControl.CurrentPageIndex >=nPageCount)
				{
					dgControl.CurrentPageIndex =0;
				}
				if (nPageCount == 0)
				{
					lblPageIndex.Text = "0 / 0";
				}
				else
				{
					lblPageIndex.Text = (dgControl.CurrentPageIndex + 1) + " / " + nPageCount;
				}
			}
			catch(Exception)
			{}
		}
		#endregion
	}
}
