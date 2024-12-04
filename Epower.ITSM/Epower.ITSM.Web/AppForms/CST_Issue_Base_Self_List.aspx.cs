/*******************************************************************
 * ��Ȩ���У�
 * Description���¼�������ѯ��
 * Create By  ��SuperMan
 * Create Date��2011-08-17
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Xml;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.Web.AppForms
{
    public partial class CST_Issue_Base_Self_List : BasePage
    {
        #region ��������

        #region TypeID
        /// <summary>
        /// 
        /// </summary>
        protected string TypeID
        {
            get
            {
                if (ViewState["TypeID"] != null)
                    return ViewState["TypeID"].ToString();
                else
                    return "0";
            }
            set
            {
                ViewState["TypeID"] = value;
            }
        }
        #endregion

        bool blnDisplayDelay = false;
        RightEntity reTrace = null;         //�������Ȩ�� 

        static string staCustInfo = "";
        static string staMsgDateBegion = "";

        #endregion

        #region �ж��Ƿ���ʾ��ʱ��ť

        /// <summary>
        /// �ж��Ƿ���ʾ��ʱ��ť
        /// </summary>
        /// <param name="status"></param>
        /// <param name="flowdiffMinute"></param>
        /// <returns></returns>
        protected string GetDelayVisible(int status, int flowdiffMinute)
        {
            string strRet = "display:none";

            if (status == (int)e_FlowStatus.efsHandle && blnDisplayDelay == true)
            {
                if (flowdiffMinute < 0)
                {
                    strRet = "";
                }
            }
            return strRet;

        }

        #endregion

        #region ��ȡ����ҳ��ַ

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns> //decimal
        protected string GetUrl(string lngFlowID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
        
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion
        #region ��ȡ��ť���� ���������/����

        /// <summary>
        /// ��ȡ��ť���� ���������/����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected string GetButtonValue(string status) //int
        {
            int status1 = Convert.ToInt32(status);
            string strRet = "����";
            if (status1 == (int)e_FlowStatus.efsEnd)
            {
                strRet = "����/�ط�";

            }
            return strRet;
        }

        #endregion

        #region  ���ɲ�ѯXML�ַ��� GetXmlValueNew1
        /// <summary>
        /// GetXmlValueNew1
        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValueNew1()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            #region CustInfo    �ͻ�����

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "CustInfo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "�������¼����ţ��ͻ���Ϣ" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);

            #endregion

            #region SericeNo    �¼�����

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "SericeNo");
            xmlEle.SetAttribute("Value", this.Master.TxtKeyName.Value.Trim().ToString() == "�������¼����ţ��ͻ���Ϣ" ? "" : this.Master.TxtKeyName.Value.Trim().ToString());
            xmlRoot.AppendChild(xmlEle);

            #endregion

            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }
        #endregion


        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
        }

        #endregion

        #region ҳ�����
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();

            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);

            //��÷������Ȩ��
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx";
            this.Master.TableVisible = false;
            string strPara = "";
            if (Request.QueryString["BookMark"] != null)
            {
                strPara = Request.QueryString["BookMark"];
            }
            if (!Page.IsPostBack)
            {
                SetHeaderText();

                switch (strPara)
                {
                    case "":
                        BindData();
                        break;
                    case "1":
                        lkbMy_Click(this.lkbMy, System.EventArgs.Empty);
                        break;
                    case "2":
                        lkbHandle_Click(this.lkbHandle, System.EventArgs.Empty);
                        break;
                    case "3":
                        lkbEnd_Click(this.lkbEnd, System.EventArgs.Empty);
                        break;
                    case "5":
                        lkbOverTimeF_Click(this.lkbOverTimeF, System.EventArgs.Empty);
                        break;
                    case "6":
                        lkbOverTimeU_Click(this.lkbOverTimeU, System.EventArgs.Empty);
                        break;
                    case "7":
                        lkbUnFeedBack_Click(this.lkbOverTimeU, System.EventArgs.Empty);
                        break;
                    default:
                        BindData();
                        break;

                }

            }
        }

        /// <summary>
        /// ������ͷ���� ������ 2013-05-16
        /// </summary>
        void SetHeaderText()
        {
            gridUndoMsg.Columns[1].HeaderText = PageDeal.GetLanguageValue("CST_ServiceNO");
            gridUndoMsg.Columns[2].HeaderText = PageDeal.GetLanguageValue("CST_CustName");
            gridUndoMsg.Columns[3].HeaderText = PageDeal.GetLanguageValue("CST_CustPhone");
            gridUndoMsg.Columns[4].HeaderText = PageDeal.GetLanguageValue("CST_ServiceLevel");
            gridUndoMsg.Columns[5].HeaderText = PageDeal.GetLanguageValue("CST_CustTime");
            gridUndoMsg.Columns[6].HeaderText = PageDeal.GetLanguageValue("CST_EquName");
            gridUndoMsg.Columns[7].HeaderText = PageDeal.GetLanguageValue("CST_Subject");
            gridUndoMsg.Columns[8].HeaderText = PageDeal.GetLanguageValue("CST_Content");

        }
        #endregion

        #region ���ٿ�����ǩ�����

        /// <summary>
        /// ���ҵǼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMy_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx?BookMark=1";
            TypeID = "1";
            BindData();
        }

        /// <summary>
        /// ���ڴ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbHandle_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx?BookMark=2";
            TypeID = "2";
            BindData();
        }

        /// <summary>
        /// ��ʱ�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEnd_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx?BookMark=3";
            TypeID = "3";
            BindData();
        }

        /// <summary>
        /// ��ʱ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOverTimeF_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx2?BookMark=5";
            TypeID = "5";
            BindData();
        }

        /// <summary>
        /// ��ʱδ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOverTimeU_Click(object sender, EventArgs e)
        {
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx?BookMark=6";
            TypeID = "6";
            BindData();
        }

        /// <summary>
        /// δ�ط��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbUnFeedBack_Click(object sender, EventArgs e)
        {
            TypeID = "7";
            Session["FromUrl"] = "../AppForms/CST_Issue_Base_Self_List.aspx?BookMark=7";
            BindData();
        }

        #endregion

        #region ���Ȩ�� CheckRight
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        #region ɾ������gridUndoMsg_DeleteCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            BindData();
        }
        #endregion

        #region ���尴ť�¼�

        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //��������������Ԥ�ƴ���ʱ��δ����ģ������ʾ
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[gridUndoMsg.Columns.Count - 5].Text);

                if (int.Parse(e.Item.Cells[gridUndoMsg.Columns.Count - 4].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }

                ((Label)e.Item.FindControl("Lb_ServiceNo")).Attributes.Add("onmouseover", "ShowDetailsInfo(this," + DataBinder.Eval(e.Item.DataItem, "SMSID").ToString() + ",400);");
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 5; i++)
                {
                    if (i > 0)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        #endregion

        #region �Զ��巽��

        /// <summary>
        /// ���ݰ�
        /// </summary>
        private void BindData()
        {
            int iRowCount = 0;
            eOA_TracePeriod tp = (eOA_TracePeriod)int.Parse(ddlPeriod.SelectedValue);
            ZHServiceDP ee = new ZHServiceDP();
            DataTable dt = null;

            switch (TypeID)
            {
                case "1":
                    dt = ee.GetIssuesForMy(long.Parse(Session["UserID"].ToString()), tp,this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "2":
                    dt = ee.GetIssuesForHandle(long.Parse(Session["UserID"].ToString()), tp, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "3":
                    dt = ee.GetIssuesForEnd(long.Parse(Session["UserID"].ToString()), tp, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "4":
                    dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), tp, 48, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "5":
                    dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), tp, true, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "6":
                    dt = ee.GetIssuesForOverTime(long.Parse(Session["UserID"].ToString()), tp, false, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                case "7":
                    dt = ee.GetIssuesForUnFeedBack(long.Parse(Session["UserID"].ToString()), tp, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
                default://��ѯ�����ڴ���ģ�����ʱ��Ϊ��һ���µ�Ȩ�޷�Χ�ڵļ�¼
                    dt = ee.GetIssuesForCondNew_Init("", long.Parse(Session["UserID"].ToString()), this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
                    break;
            }
            if (dt != null)
            {
                gridUndoMsg.DataSource = dt;
                gridUndoMsg.DataBind();
            }
            this.cpCST_Issue.RecordCount = iRowCount;
            this.cpCST_Issue.Bind();
        }

        #endregion
    }
}
