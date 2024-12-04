//
// �ļ���  ��ControlPage.CS
// ģ�����ƣ�
// ����������
// ʵ�ֹ��ܣ���һ����׼����UserControl����DataGrid�ķ�ҳ,���DataGrid����ķ�ҳ����
// ������ڣ�2004-03-23
// �޸���  ��
// �޸����ڣ�
// �汾    ��V1.0
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
	///����DataGrid�ķ�ҳ��
	/// </summary>
	public partial class ControlPage : System.Web.UI.UserControl
	{
		#region var
		#endregion

		public event System.EventHandler On_PostBack;

		#region Property

        /// <summary>
        /// ��ҳ�ؼ���ÿҳ��¼��
        /// </summary>
        private int iPageSize = 20;
        public int PageSize
        {
            set { iPageSize = value; }
        }
		
		/// <summary>
		/// ���ñ����Ƶ�DataGrid
		/// ��Page_Load�����ñ�����(�����ط�)
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
			this.btnFirstPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnFirstPage_Click);
			this.btnPrevPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevPage_Click);
			this.btnNextPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnNextPage_Click);
			this.btnLastPage.Click += new System.Web.UI.ImageClickEventHandler(this.btnLastPage_Click);

		}
		#endregion

		protected DataGrid dgControl;

		/// <summary> Push ��ҳ button /// </summary>
		private void btnFirstPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			dgControl.CurrentPageIndex = 0;
			dgControl.DataBind();
			On_PostBack(this,null);
		}

		/// <summary> Push ��һҳ button /// </summary>
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

		/// <summary> Push ��һҳ button /// </summary>
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

		/// <summary> Push ���һҳ button /// </summary>
		private void btnLastPage_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			dgControl.CurrentPageIndex = dgControl.PageCount - 1;
			dgControl.DataBind();
			On_PostBack(this,null);
		}

		/// <summary> �ı�ÿҳ��ʾ���� /// </summary>
		protected void dplPageButtonCount_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgControl.CurrentPageIndex = 0;
			dgControl.DataBind();
			On_PostBack(this,null);

		}

		/// <summary> ���ݰ󶨷���,�������ʾ��Ϣ /// </summary>
		private void dgControl_DataBinding(object sender, System.EventArgs e)
		{
			DisplayInformation();
		}

		/// <summary>
		/// ������ʾ��Ϣ
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
