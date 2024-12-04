using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// OA_MessageQuery ��ժҪ˵����
	/// </summary>
	public partial class OA_MessageQuery : BasePage
	{
        /// <summary>
        /// ��ǰѡ��ı�ǩ
        /// </summary>
        protected eOA_ListBookMark eCurr
        {
            get
            {
                if (ViewState["eCurr"] == null)
                    return (eOA_ListBookMark)ViewState["eCurr"];
                else
                    return eOA_ListBookMark.eEmpty;
            }
            set
            {
                ViewState["eCurr"] = value;
            }
        }

		#region �ؼ�

	
		#endregion

        /// <summary>
        /// ����ĸ��ҳ�水ť
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }

		protected string GetStatusImage(int isRead)
		{
			string ret = @"..\Images\page\flow_status_newnormal.gif";
			if (isRead > 0)
				ret = @"..\Images\page\flow_status_normal.gif";
			return ret;
		}

		protected bool GetVisible(int i)
		{
			bool t = false;
			if(i>0) 
				t= true;
			return t;
		}

	

		protected bool GetImportanceVisible(int i)
		{
			bool t = false;
			if(i==2) 
				t= true;
			return t;
		}

		


		/// <summary>
		/// ���յģգң�
		/// </summary>
		/// <param name="lngNoticeID"></param>
		/// <returns></returns>
		protected string GetUrl(decimal lngMessageID,int iStatus)
		{
			//��ʱû�����ҳ
			string sUrl = "";
			if(iStatus == (int)e_MessageStatus.emsFinished)
			{
				sUrl="javascript:window.open('flow_Finished.aspx?MessageID="+lngMessageID.ToString()+"','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
			}
			else
			{
				sUrl="javascript:window.open('flow_Normal.aspx?MessageID="+lngMessageID.ToString()+"','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
			}
			return sUrl;

			
		}

        /// <summary>
        /// ��ȡ���յĽű�
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngAppID"></param>
        /// <returns></returns>
        protected string GetFlowShotInfo(decimal lngFlowID, decimal lngAppID)
        {
            return "GetFlowShotInfo(this," + lngFlowID.ToString() + "," + lngAppID.ToString() + ");";
        }
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			CtrTitle1.Title="���������ѯ";
            cpFlow.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
			if(!Page.IsPostBack)
			{
                Session["PersonAttention"] = AttentionDP.GetMyAttention(long.Parse(Session["UserID"].ToString()));
                BindDataBookMark(eOA_ListBookMark.eEmpty) ;
                Session["FromUrl"] = Constant.ApplicationPath + "/Forms/OA_MessageQuery.aspx";
			}

		}

        private void BindData()
        {
            BindDataBookMark(eCurr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iBMK"></param>
		private void BindDataBookMark(eOA_ListBookMark iBMK)
		{
            eCurr = iBMK;
            DataTable dt = new DataTable();
            int iRowCount = 0;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            sOrder = "  ORDER BY MessageID DESC";
            if (iBMK == eOA_ListBookMark.eEmpty)
            {
                XmlDocument xmlDoc = CtrMessageQuery1.GetAllValues();
                dt = MessageBookMarkDP.GetMessagesForCond(xmlDoc.InnerXml, sOrder, this.cpFlow.PageSize, this.cpFlow.CurrentPage, ref iRowCount); 
            }
            else
            {
                dt = MessageBookMarkDP.GetMessageBookMarkList((long)Session["UserID"], iBMK, sOrder, this.cpFlow.PageSize, this.cpFlow.CurrentPage, ref iRowCount);
            }
					
			gridUndoMsg.DataSource=dt.DefaultView;
			gridUndoMsg.Attributes.Add("style","word-break:break-all;word-wrap:break-word");
			gridUndoMsg.DataBind();
            this.cpFlow.RecordCount = iRowCount;
            this.cpFlow.Bind();
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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.gridUndoMsg.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridUndoMsg_ItemCreated);
			this.gridUndoMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridUndoMsg_ItemDataBound);

		}
		#endregion

		protected void cmdQuery_Click(object sender, System.EventArgs e)
		{
            eCurr = eOA_ListBookMark.eEmpty;
            BindData();
		}

		private void gridUndoMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{

				switch ((e_ActorClass)(int.Parse(e.Item.Cells[12].Text.Trim())))
				{
					case e_ActorClass.fmMasterActor:
						e.Item.Cells[12].Text = "����";
						break;
					case e_ActorClass.fmAssistActor:
						e.Item.Cells[12].Text = "Э��";
						break;
					case e_ActorClass.fmInfluxActor:
						e.Item.Cells[12].Text = "��ǩ";
						break;
					case e_ActorClass.fmReaderActor:
						e.Item.Cells[12].Text = "��֪";
						break;
                    case e_ActorClass.fmCommunicActor:
                        e.Item.Cells[12].Text = "��ͨ";
                        break;
					default:
						break;
				}
				

				//������Ԥ�ƴ���ʱ��δ����ģ������ʾ
				if (int.Parse(e.Item.Cells[8].Text.Trim()) < 0)
				{
					for (int i=0;i<e.Item.Cells.Count;i++)
					{
						e.Item.Cells[i].BackColor = Color.DarkOrange;
					}
				}

				if(long.Parse(e.Item.Cells[9].Text.Trim()) == (long)e_AppTypes.eatCommon)
				{
					//ͨ���������⴦��
					if(e.Item.Cells[10].Text.Trim().StartsWith(e.Item.Cells[6].Text.Trim()))
					{
						e.Item.Cells[6].Text = e.Item.Cells[10].Text;
					}
					else
					{
						e.Item.Cells[6].Text = e.Item.Cells[6].Text.Trim() + "[" + e.Item.Cells[10].Text + "]";
					}
				}
				
				//�ж��Ƿ��Ѿ������˹�ע
				string sMessageID=e.Item.Cells[7].Text;
				sMessageID=sMessageID==""?"0":sMessageID;
                DataTable dt = (DataTable)Session["PersonAttention"];
                DataRow[] dtrows = dt.Select("messageid=" + sMessageID);
                if (dtrows.Length > 0)
                {
                    CheckBox chkAttention = (CheckBox)e.Item.Cells[e.Item.Cells.Count - 1].FindControl("chkAttention");
                    chkAttention.Visible = false;
                }

                //����˫��Ч��
                int status = Convert.ToInt32(e.Item.Cells[11].Text.ToString());
                if (status == (int)e_MessageStatus.emsFinished)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('flow_Finished.aspx?MessageID=" + sMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('flow_Normal.aspx?MessageID=" + sMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void cmdAttention_Click(object sender, System.EventArgs e)
		{
            int i = 0;
			foreach(DataGridItem itm in gridUndoMsg.Items)
			{
				if (itm.ItemType == ListItemType.Item || itm.ItemType == ListItemType.AlternatingItem)
				{
					CheckBox chk=(CheckBox)itm.Cells[itm.Cells.Count-1].FindControl("chkAttention");
					string sMessageID=itm.Cells[7].Text;
					sMessageID=sMessageID==""?"0":sMessageID;
					if(chk.Checked)
					{
						long nFlowID=FlowDP.GetFlowIDByMessageId(long.Parse(sMessageID));
						AttentionDP.AddAttention(nFlowID,long.Parse(sMessageID),(long)Session["UserID"]);
                        i++;
					}
				}
			}
            if (i > 0)
            {
                Session["PersonAttention"] = AttentionDP.GetMyAttention(long.Parse(Session["UserID"].ToString()));
                Epower.DevBase.BaseTools.PageTool.MsgBox(this.Page, "���ݱ���ɹ���");
            }
            BindData();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void lkbOverTime_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmOverTime);
		}

		protected void lkbInTime_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmInTime);
		}

		protected void lkbMaster_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmMaster);
		}

		protected void lkbAssist_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmAssist);
		}

		protected void lkbReader_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmReader);
		}

		protected void lkbInFlux_Click(object sender, System.EventArgs e)
		{
			BindDataBookMark(eOA_ListBookMark.elbmInflux);
		}

		private void gridUndoMsg_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				DataGrid dg = (DataGrid)sender;
				for (int i = 0; i < e.Item.Cells.Count; i++)
				{
					// (DataView)e.Item.NamingContainer;
					if (i==3 || i==4 || i==5 || i==6)
					{
						e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
					}
				}
			}
		}
	}
}
