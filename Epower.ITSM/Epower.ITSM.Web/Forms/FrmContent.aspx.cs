/************************************************************************************
 * ��Ȩ���У�
 * Description:���������
 * 
 * 
 * 
 * Create By:zhumingchun
 * Create Date:2007-02-14
 * **********************************************************************************/
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
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web
{
	/// <summary>
	/// FrmContent ��ժҪ˵����
	/// </summary>
	public partial class FrmContent : BasePage
	{

        #region ��������
		private long lngUserID = 0;
		private long lngLastMessageID = 0;
		protected string sCheckEnable = "true";  //�Ƿ��Զ����
		protected long lngCheckTime = 300000;    //���ʱ����
		#endregion

        #region �ű����÷�����
        /// <summary>
        /// ��ʾͼƬ
        /// </summary>
        /// <param name="isRead"></param>
        /// <returns></returns>
		protected string GetStatusImage(int isRead)
		{
			string ret = @"..\Images\page\flow_status_newnormal.gif";
			if (isRead > 0)
				ret = @"..\Images\page\flow_status_normal.gif";
			return ret;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
		protected bool GetVisible(int i)
		{
			bool t = false;
			if(i>0) 
				t= true;
			return t;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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
		protected string GetUrl(decimal lngMessageID)
		{
			//��ʱû�����ҳ
			string sUrl="javascript:window.open('flow_Normal.aspx?MessageID="+lngMessageID.ToString()+"','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetMyRegUrl(decimal lngFlowID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion 

        #region ������
        /// <summary>
        /// ����ĸ��ҳ�水ť
        /// </summary>
        protected void SetParentButtonEvent()
        {
            //this.Master.TableVisible = false;
        }

        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			lngUserID = long.Parse(Session["UserID"].ToString());

            sCheckEnable = CommonDP.GetConfigValue("AutoCheckNewItems", "AutoCheckNewItemsEnable");
            lngCheckTime = long.Parse(CommonDP.GetConfigValue("AutoCheckNewItems", "AutoCheckNewItems"));
			if(lngCheckTime < 60000)
			{
				//��С���1����
				lngCheckTime = 60000;
			}
			if(!Page.IsPostBack)
			{
                if (Request["TypeContent"] != null)
                {
                    Session["TypeContent"] = Request["TypeContent"].ToString();
                    Session["FromUrl"] = Constant.ApplicationPath + "/Forms/frmContent.aspx?TypeContent=MyReg";
                }
                else
                {
                    Session["TypeContent"] = "0";
                    Session["FromUrl"] = Constant.ApplicationPath + "/Forms/frmContent.aspx";
                }

				
				CtrTitle1.Title = "�ҵ�����";
				DoDataBind();
				hidLastMessageID.Value = lngLastMessageID.ToString();
			}
		}

        /// <summary>
        /// ������
        /// </summary>
		private void DoDataBind()
		{
			DataSet ds;
			DataTable dt;
			DataView dv;
			try
			{
                if (Session["TypeContent"].ToString() != "0")
                {
                    switch (Session["TypeContent"].ToString())
                    {
                        case "MyReg":
                            //�ҵǼ�����
                            dt = MessageCollectionDP.GetMyRegMessageUnFinished((long)Session["UserID"], 20);
                            // ������BUG��ʱ����
                            gridMyReg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMyReg_ItemDataBound);
                            gridMyReg.ItemCreated += new DataGridItemEventHandler(dgMyReg_ItemCreated);
                            dv = dt.DefaultView;
                            gridMyReg.DataSource = dv;
                            gridMyReg.DataBind();

                            //trTitle.Visible = true;
                            if (dv.Count == 0)
                            {
                                trMyReg1.Visible = false;
                                trMyReg2.Visible = false;
                                trNoneData.Visible = true;
                            }
                            else
                            {
                                trMyReg1.Visible = true;
                                trMyReg2.Visible = true;
                                gridMyReg.Visible = true;
                                trNoneData.Visible = false;
                            }

                            //trTitle1.Visible = false;
                            trReceiveMsg1.Visible = false;
                            trReceiveMsg2.Visible = false;
                            trUndoMsg1.Visible = false;
                            trUndoMsg2.Visible = false;
                            trReadMsg1.Visible = false;
                            trReadMsg2.Visible = false;
                            trWaiting1.Visible = false;
                            trWaiting2.Visible = false;
                            trAttention1.Visible = false;
                            trAttention2.Visible = false;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    trMyReg1.Visible = false;
                    trMyReg2.Visible = false;

                    // ����������
                    ds = ReceiveList.GetReceiveMessageList(lngUserID);
                    gridReceiveMsg.Visible = true;
                    gridReceiveMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                    gridReceiveMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                    dv = ds.Tables[0].DefaultView;
                    gridReceiveMsg.DataSource = dv;
                    gridReceiveMsg.DataBind();
                    if (dv.Count == 0)
                    {
                        trReceiveMsg1.Visible = false;
                        trReceiveMsg2.Visible = false;
                    }

                    //��������
                    dt = MessageCollectionDP.GetUndoMessage(lngUserID, 10);
                    gridUndoMsg.Visible = true;
                    gridUndoMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                    gridUndoMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                    dv = dt.DefaultView;
                    gridUndoMsg.DataSource = dv;
                    gridUndoMsg.DataBind();
                    if (dv.Count == 0)
                    {
                        trUndoMsg1.Visible = false;
                        trUndoMsg2.Visible = false;
                    }

                    //��֪����
                    dt = MessageCollectionDP.GetUnReadMessage(lngUserID, 10);
                    gridReadMsg.Visible = true;
                    gridReadMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                    gridReadMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                    dv = dt.DefaultView;
                    gridReadMsg.DataSource = dv;
                    gridReadMsg.DataBind();

                    if (dv.Count == 0)
                    {
                        trReadMsg1.Visible = false;
                        trReadMsg2.Visible = false;
                    }

                    //���������
                    dt = MessageCollectionDP.GetWaitingMessage(lngUserID, 10);
                    gridWaitingMsg.Visible = true;
                    gridWaitingMsg.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridMsg_ItemDataBound);
                    gridWaitingMsg.ItemCreated += new DataGridItemEventHandler(gridMsg_ItemCreated);
                    dv = dt.DefaultView;
                    gridWaitingMsg.DataSource = dv;
                    gridWaitingMsg.DataBind();
                    if (dv.Count == 0)
                    {
                        trWaiting1.Visible = false;
                        trWaiting2.Visible = false;
                    }

                    //��ע����
                    dt = AttentionDP.GetMyAttention((long)Session["UserID"]);
                    gridAttention.Visible = true;
                    gridAttention.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridAttention_ItemDataBound);
                    dv = dt.DefaultView;
                    gridAttention.DataSource = dv;
                    gridAttention.DataBind();
                    if (dv.Count == 0)
                    {
                        trAttention1.Visible = false;
                        trAttention2.Visible = false;
                    }

                    if (trReceiveMsg1.Visible == false && trUndoMsg1.Visible == false && trReadMsg1.Visible == false
                        && trWaiting1.Visible == false && trAttention1.Visible == false)
                    {
                        this.litlShowNone.Visible = true;
                    }
                }
			}
			catch
			{
				//ͳһ����չʾҳ��
				throw;
				//Response.Redirect("ErrMessage.aspx?Souce="+e.Source + "&Desc=" + e.ToString());
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void gridMsg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //��¼������ϢID���Զ����
                long lngID = long.Parse(e.Item.Cells[7].Text.Trim());
                if (lngID > lngLastMessageID)
                    lngLastMessageID = lngID;

                switch ((e_ActorClass)(int.Parse(e.Item.Cells[11].Text.Trim())))
                {
                    case e_ActorClass.fmMasterActor:
                        e.Item.Cells[11].Text = "����";
                        break;
                    case e_ActorClass.fmAssistActor:
                        e.Item.Cells[11].Text = "Э��";
                        break;
                    case e_ActorClass.fmInfluxActor:
                        e.Item.Cells[11].Text = "��ǩ";
                        break;
                    case e_ActorClass.fmReaderActor:
                        e.Item.Cells[11].Text = "��֪";
                        break;
                    default:
                        break;
                }

                //������Ԥ�ƴ���ʱ��δ����ģ������ʾ
                if (int.Parse(e.Item.Cells[8].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BackColor = Color.DarkOrange;
                    }
                }

                if (long.Parse(e.Item.Cells[9].Text.Trim()) == (long)e_AppTypes.eatCommon)
                {
                    //ͨ���������⴦��
                    if (e.Item.Cells[10].Text.Trim().StartsWith(e.Item.Cells[6].Text.Trim()))
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[10].Text;
                    }
                    else
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[6].Text.Trim() + "[" + e.Item.Cells[10].Text + "]";
                    }
                }
                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "MessageID").ToString();
                e.Item.Attributes.Add("onclick", "window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                e.Item.CssClass = "hand";
            }
		}

        protected void gridAttention_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string lngMessageID = DataBinder.Eval(e.Item.DataItem, "MessageID").ToString();
                e.Item.Attributes.Add("onclick", "window.open('flow_Normal.aspx?MessageID=" + lngMessageID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                e.Item.CssClass = "hand";
            }
        }

        protected void gridMyReg_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string Flowid = DataBinder.Eval(e.Item.DataItem, "Flowid").ToString();                
                e.Item.Attributes.Add("onclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + Flowid.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                e.Item.CssClass = "hand";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridAttention_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "del")
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    string sID = e.Item.Cells[0].Text.Trim("&nbsp;".ToCharArray());
                    if (sID != "")
                    {
                        AttentionDP.DeleteAttention(long.Parse(sID));
                        DoDataBind();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridMsg_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				DataGrid dg = (DataGrid)sender;
				for (int i = 0; i < e.Item.Cells.Count; i++)
				{
					// (DataView)e.Item.NamingContainer;
					if (i ==3 || i == 4 || i==5 || i==6)
					{
						e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridAttention_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {
                if (i == 5 || i == 6 || i == 7)
                {
                    int j = i - 5;  //ǰ����5�����ص���
                    e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                }
            }
        }
        #endregion 

     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridReceiveMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 3 || i == 4 || i == 5 || i == 6)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 3 || i == 4 || i == 5 || i == 6 || i == 7)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridReadMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 3 || i == 4 || i == 5 || i == 6)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridWaitingMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 5 || i == 6 || i == 7)
                    {
                        int j = i - 5;  //ǰ����5�����ص���
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgMyReg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 1 || i == 2)
                    {
                        int j = i - 1;  //ǰ����1�����ص���
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
	}
}
