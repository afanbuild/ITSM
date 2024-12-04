/*****************************************************************************************************
 * Des���Զ������ѯ
 * 
 * 
 * 
 * Create By:zmc    
 * Create Date:2010-08-18
 * **************************************************************************************************/
using System;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.NormalQuery
{
    public partial class frmNormalQueryDefine : BasePage
    {
        long lngOFlowModelID = 0;
        long lngFlowModelID = 0;
        long lngOperateID = 0;

        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region ҳ���ʼ��
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FlowModelID"] != null)
            {
                lngFlowModelID = long.Parse(Request["FlowModelID"].ToString());
                lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);
            }
            if (Request["OperateID"] != null)
                lngOperateID = long.Parse(Request["OperateID"].ToString());

            imgBegin.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtSendDateBegin.ClientID + ", 'winpop', 234, 261);return false");
            imgEnd.Attributes.Add("onclick", "fPopUpDlg('../Controls/Calendar/calendar.htm',document.all." + txtSendDateEnd.ClientID + ", 'winpop', 234, 261);return false");

            DataTable dt1 = FlowModel.GetCommAppFlowModelFields(lngOFlowModelID);

            DataRow dr1 = null;
            if (dt1.Rows.Count > 0)
            {
                dr1 = dt1.Rows[0];
            }

            SetParentButtonEvent();

            cpHol.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);

            if (!IsPostBack)
            {
                CtrTitle1.Title = "�Զ������̸��ٲ�ѯ";
                InitDropList();

                //��ѯ������ֵ
                Control[] arrControl = { tabMain };
                PageDeal.SetPageQueryParam(arrControl, cpHol, "frmNormalQueryDefine");

                Bind();

                Session["FromUrl"] = Constant.ApplicationPath + "/NormalQuery/frmNormalQueryDefine.aspx?FlowModelID=" + lngFlowModelID.ToString() + "&OperateID=" + lngOperateID.ToString();

                dgDispatch.Columns[dgDispatch.Columns.Count - 1].Visible = false;  //ɾ������Ȩ��
            }

            //�����ѯ����
            Control[] arrContrl1 = { tabMain };
            PageDeal.GetPageQueryParam(arrContrl1, cpHol, "frmNormalQueryDefine");
        }
        #endregion

        #region ��ʼ�������б�
        protected void InitDropList()
        {
            cboStatus.Items.Add(new ListItem("����״̬", "-1"));
            cboStatus.Items.Add(new ListItem("--���ڴ���", ((int)e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--��������", ((int)e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--������ͣ", ((int)e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--������ֹ", ((int)e_FlowStatus.efsAbort).ToString()));

            txtSendDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtSendDateEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }
        #endregion

        #region ��ѯ�¼�
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            Bind();

        }
        #endregion

        #region ������ Bind
        /// <summary>
        /// ������
        /// </summary>
        private void Bind()
        {
            string sWhere = string.Empty;

            #region �̶���������
            if (cboStatus.SelectedValue != "-1")
            {
                sWhere += " AND status = " + cboStatus.SelectedValue;
            }
            if (DeptPicker1.DeptID != 0)
            {
                sWhere += " AND deptid = " + DeptPicker1.DeptID;
            }
            if (UserPicker1.UserID != 0)
            {
                sWhere += " AND applyid = " + UserPicker1.UserID;
            }
            if (txtTitle.Text.Trim() != string.Empty)
                sWhere += " and flowname like" + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");

            if (txtSendDateBegin.Text.Trim() != string.Empty)
                sWhere += " And StartDate >=" + StringTool.SqlQ(txtSendDateBegin.Text.Trim());
            if (txtSendDateEnd.Text.Trim() != string.Empty)
                sWhere += " And StartDate <" + StringTool.SqlQ(DateTime.Parse(txtSendDateEnd.Text).AddDays(1).ToShortDateString());

            #endregion

            int iRowCount = 0;
            long lngUserID = (long)Session["UserID"];
            long lngDeptID = (long)Session["UserDeptID"];
            long lngOrgID = (long)Session["UserOrgID"];
            DataTable dt = Epower.ITSM.SqlDAL.NormalAppDP.GetDefineQuery(lngUserID, lngDeptID, lngOrgID, (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[lngOperateID]
                , sWhere, lngOFlowModelID.ToString(), this.cpHol.PageSize, this.cpHol.CurrentPage, ref iRowCount);
            dgDispatch.DataSource = dt.DefaultView;
            dgDispatch.DataBind();
            this.cpHol.RecordCount = iRowCount;
            this.cpHol.Bind();
        }
        #endregion

        #region ��ʾҳ���ַ GetUrl
        /// <summary>
        /// ��ʾҳ���ַ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion

        #region �������� dgDispatch_ItemCreateds
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDispatch_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < e.Item.Cells.Count - 2)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region ɾ�������¼� dgDispatch_DeleteCommand
        /// <summary>
        /// ɾ�������¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgDispatch_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            Bind();
        }
        #endregion

        #region ���Ȩ��CheckRight
        /// <summary>
        /// ���Ȩ��
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

        #region dgDispatch_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDispatch_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                String sID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + sID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        #endregion 
    }
}
