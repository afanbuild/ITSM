/*******************************************************************
 * ��Ȩ���У�
 * Description������Ϣ
 * 
 * 
 * Create By  ��zhumingchun
 * Create Date��2007-10-31
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.ITSM.Base;
using EpowerGlobal;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Forms.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmShowMessageList : Epower.ITSM.Web.BasePage
    {
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion 

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            string strUrl = Session["MainUrl"].ToString();
            Response.Redirect(strUrl);
        }
        #endregion

        #region ҳ����� Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                //txtDateBegin.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                //txtDateEnd.Text = DateTime.Now.ToShortDateString();
                //������ʼ����
                string sQueryBeginDate = "";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                {
                    sQueryBeginDate = CommonDP.GetConfigValue("Other","QueryBeginDate").ToString();
                }
                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToShortDateString();

                Bind();
            }
        }
        #endregion 

        #region ��ѯ�¼�Master_Master_Button_Query_Click
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        void Master_Master_Button_Query_Click()
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
            long lngUserID = long.Parse(Session["userid"].ToString());
            int intNum = 100;
            string sWhere = string.Empty;
            DataTable dt = new DataTable();
            string DateBeginTime="";
            string DateEndTime="";
            if (ctrDateSelectTime1.BeginTime.Trim() != string.Empty)
            {
                DateBeginTime = ctrDateSelectTime1.BeginTime.Trim();
            }
            if (ctrDateSelectTime1.EndTime.Trim() != string.Empty)
            {
                DateEndTime = ctrDateSelectTime1.EndTime.Trim();
            }
            if (ddltReadState.SelectedValue == "-1")
            {
                int icount = 0;
                //δ��
                sWhere = " ";
                dt = SMSDp.GetSMS(lngUserID, intNum, sWhere, 0, DateBeginTime, DateEndTime);
                DataList1.DataSource = dt.DefaultView;
                DataList1.DataBind();
                icount = dt.Rows.Count;

                //�Ѷ�
                sWhere = "";
                dt = SMSDp.GetSMS(lngUserID, intNum, sWhere, 1, DateBeginTime, DateEndTime);
                DataList2.DataSource = dt.DefaultView;
                DataList2.DataBind();
                icount += dt.Rows.Count;
                this.lblcount.Text = icount.ToString();
            }
            else if (ddltReadState.SelectedValue == "0")
            {
                dt = SMSDp.GetSMS(lngUserID, intNum, sWhere, int.Parse(ddltReadState.SelectedValue), DateBeginTime, DateEndTime);
                DataList1.DataSource = dt.DefaultView;
                DataList1.DataBind();
                this.lblcount.Text = dt.Rows.Count.ToString();
            }
            else
            {
                dt = SMSDp.GetSMS(lngUserID, intNum, sWhere, int.Parse(ddltReadState.SelectedValue), DateBeginTime, DateEndTime);
                DataList2.DataSource = dt.DefaultView;
                DataList2.DataBind();
                this.lblcount.Text = dt.Rows.Count.ToString();
            }   
        }
        #endregion 

        #region ɾ����Ϣ���Ѷ���Ϣ
        /// <summary>
        /// ɾ����Ϣ,�Ѷ���Ϣ
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //ɾ����Ϣ
            if (e.CommandName == "delete")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                long lngID = long.Parse(lblID.Text.Trim());
                SMSDp.DeleteSMS(lngID);
                Bind();
            }
            else if (e.CommandName == "read")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                long lngID = long.Parse(lblID.Text.Trim());
                SMSDp.ReadSMS(lngID);
                Bind();
            }
            
        }
        #endregion 
    }
}
