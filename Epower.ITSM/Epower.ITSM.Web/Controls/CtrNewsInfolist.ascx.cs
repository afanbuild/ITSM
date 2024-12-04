/*******************************************************************
 * ��Ȩ���У�
 * Description����������չʾ�ؼ�
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
	using Epower.ITSM.SqlDAL;
	using Epower.ITSM.Base;

	/// <summary>
	///		CtrNewsInfolist ��ժҪ˵����
	/// </summary>
	public partial class CtrNewsInfolist : System.Web.UI.UserControl
	{

		/// <summary>
		/// ��Χ��ȫ����Ա/��λ��������Ա��
		/// </summary>
		public int Range
		{
			set{ViewState[this.ID+"Range"]=value;}
			get
			{
				if(ViewState[this.ID+"Range"]!=null)
					return int.Parse(ViewState[this.ID+"Range"].ToString());
				else
					return 1;
			}
		}

		/// <summary>
		/// ������λID
		/// </summary>
		public long OrgID
		{
			set{ViewState[this.ID+"OrgID"]=value;}
			get
			{
				if(ViewState[this.ID+"OrgID"]!=null)
					return long.Parse(ViewState[this.ID+"OrgID"].ToString());
				else
					return 0;
			}
		}

		/// <summary>
		///��������ID
		/// </summary>
		public long DeptID
		{
			set{ViewState[this.ID+"DeptID"]=value;}
			get
			{
				if(ViewState[this.ID+"DeptID"]!=null)
					return long.Parse(ViewState[this.ID+"DeptID"].ToString());
				else
					return 0;
			}
		}

		/// <summary>
		///ÿ��Ҫ��ʾ�ļ�¼��
		/// </summary>
		public int cnt
		{
			set{ViewState[this.ID+"cnt"]=value;}
			get
			{
				if(ViewState[this.ID+"cnt"]!=null)
					return int.Parse(ViewState[this.ID+"cnt"].ToString());
				else
					return 0;
			}
		}

		/// <summary>
		///ÿ��Ҫ��ʾ�ļ�¼��
		/// </summary>
		public int sum
		{
			set{ViewState[this.ID+"sum"]=value;}
			get
			{
				if(ViewState[this.ID+"sum"]!=null)
					return int.Parse(ViewState[this.ID+"sum"].ToString());
				else
					return 0;
			}
		}

        /// <summary>
        ///�Ƿ���ʾȫ��
        /// </summary>
        public bool IsAll
        {
            set { ViewState[this.ID + "IsAll"] = value; }
            get
            {
                if (ViewState[this.ID + "IsAll"] != null)
                    return bool.Parse(ViewState[this.ID + "IsAll"].ToString());
                else
                    return false;
            }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			string strNewsTitle="";
			string strNewsType="";
			int inttmp=0;
			int intsum=-1;
			if (this.sum!=-1)
			{
				intsum=this.sum;
			}
			DataTable dt;
            dt = NewsDp.GetShowNewsInner(cnt, this.Range, this.OrgID, this.DeptID, IsAll);
			foreach(DataRow dr in dt.Rows)
			{
				if (strNewsType=="" || strNewsType!=dr["TypeID"].ToString().Trim())
				{
					if (strNewsType!="")
					{
						strNewsTitle+="</Table>";
					}
					strNewsTitle+="<table width='98%' class='listContent'>";
					strNewsTitle+="<tr>";
					strNewsTitle+="	<td colspan='2' align='left' class='listTitle'>&nbsp;&nbsp;&nbsp;&nbsp;"+dr["TypeName"].ToString().Trim()+"��</td>";
					strNewsTitle+="</tr>";
					strNewsType=dr["TypeID"].ToString().Trim();
				}
				if(dr["Title"].ToString().Trim()!="")
				{
					strNewsTitle+="<tr>";
                    strNewsTitle += "<td width='5%' align='center' valign='middle' class='list'><img src='../Images/arrow_purple.gif'></td>";
					strNewsTitle+="<td width='95%' class='list'>";
                    if (dr["TimeOutFlag"].ToString() == "0")
                    {
                        strNewsTitle += "<a href='../Forms/ShowNews.aspx?NewsID=" + dr["NewsId"].ToString() + "' target='_blank' class='itx'><font color='#478ABC'>" + StringTool.ParseForHtml(dr["title"].ToString()) + "</font></a>";
                    }
                    else
                    {
                        strNewsTitle += "<a href='../Forms/ShowNews.aspx?NewsID=" + dr["NewsId"].ToString() + "' target='_blank' class='itx'>" + StringTool.ParseForHtml(dr["title"].ToString()) + "</a>";
                    }
					strNewsTitle+="</td>";
					strNewsTitle+="</tr>";
				}
				inttmp=inttmp+1;
				if (inttmp==intsum)
				{
					break;
				}
			}

			if (inttmp==0) 
			{
				strNewsTitle+="<table width='98%' class='listContent'>";
				strNewsTitle+="<tr>";
                strNewsTitle += "	<td colspan='2' align='left' class='listTitle'>&nbsp;&nbsp;&nbsp;&nbsp;��������Ϣ</td>";
				strNewsTitle+="</tr>";
			}
			else if (inttmp>=5 && this.sum!=-1) 
			{
				strNewsTitle+="<table width='98%' border='0' cellpadding='0' cellspacing='0'>";
				strNewsTitle+="<tr>";
                //strNewsTitle += "	<td colspan='2' align='right' class='list'><a href='../Forms/FrmNewsInfo_list.aspx' target='MainFrame'>>>����</a></td>";
                strNewsTitle += "	<td colspan='2' align='right' class='list'></td>";
				strNewsTitle+="</tr>";
			}
			strNewsTitle+="</table>";

			litNewsTitle.Text=strNewsTitle;
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

		}
		#endregion
	}
}
