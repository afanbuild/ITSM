/*******************************************************************
 * ��Ȩ���У�
 * Description��֪ʶ��������ѯ��
 * 
 * 
 * Create By  ��zhumingchun
 * Create Date��2007-09-28
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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frm_KBBaseQuery : BasePage
    {
        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowQueryButton(true);
            this.Master.ShowNewButton(true);
            this.Master.MainID = "1";     //����ҳ���ID��ţ����Ϊ��ѯҳ�棬������Ϊ1
        }
        #endregion 

        #region ���� Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {            
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=400");
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

        #region ҳ���ʼ�� Page_Load
        /// <summary>
        /// ҳ���ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();

            cpKBBase.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                //������ʾ
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();
                                            
                InitDropDown();
                //������ʼ����
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate =CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    ctrDateSelectTime1.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
                ctrDateSelectTime1.EndTime = DateTime.Now.ToShortDateString();

                Bind();
                Session["FromUrl"] = "../InformationManager/frm_KBBaseQuery.aspx";
                //Ӧ�ù���Աɾ��Ȩ��
           
            }

            //grd.Columns[grd.Columns.Count - 1].Visible = CheckRight(Constant.admindeleteflow);

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.InfKBQuery];
            #region  ɾ��Ȩ���߼�����
            //yanghw  ��������ɾ��Ȩ��  Ȼ�����жϲ������Ȩ��
            //Ӧ�ù���Աɾ��Ȩ��
            if (CheckRight(Constant.admindeleteflow))
            {
                grd.Columns[grd.Columns.Count - 1].Visible = true;
            }
            else
            {
                if (re != null)
                    grd.Columns[grd.Columns.Count - 1].Visible = re.CanDelete;
                else
                    grd.Columns[grd.Columns.Count - 1].Visible = false;

            }
            //����Ȩ��
            if (re.CanAdd != true)
            {
                this.Master.ShowNewButton(false);
            }


            #endregion
        }
        #endregion 

        #region ����datagrid��ͷ��ʾ ����ǰ 2013-05-20
        /// <summary>
        /// ����datagrid��ͷ��ʾ
        /// </summary>
        private void SetHeaderText()
        {
            grd.Columns[0].HeaderText = PageDeal.GetLanguageValue("info_Title");
            grd.Columns[1].HeaderText = PageDeal.GetLanguageValue("info_PKey");
            grd.Columns[2].HeaderText = PageDeal.GetLanguageValue("info_TypeName");
        }
        #endregion

        #region  ��ʼ�������б� InitDropDown
        /// <summary>
        /// ��ʼ�������б�
        /// </summary>
        private void InitDropDown()
        {
            cboStatus.Items.Add(new ListItem("����״̬", "-1"));
            cboStatus.Items.Add(new ListItem("--���ڴ���", ((int)e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--��������", ((int)e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--������ͣ", ((int)e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--������ֹ", ((int)e_FlowStatus.efsAbort).ToString()));
            cboStatus.SelectedIndex = 1;
        }
        #endregion s
         
        #region ������ Bind
        /// <summary>
        /// ������
        /// </summary>
        private void Bind()
        {
            #region bind
            int iRowCount = 0;
            string sWhere = "";
            if (cboStatus.SelectedValue != "-1")
            {
                sWhere += " AND status = " + cboStatus.SelectedValue;
            }
            if (txtTitle.Text.Trim() != string.Empty)
            {
                sWhere += " And Title like " + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");
            }
            if (txtPKey.Text.Trim() != string.Empty)
            {
                sWhere += " And PKey like " + StringTool.SqlQ("%" + txtPKey.Text.Trim() + "%");
            }
            if (txtTags.Text.Trim() != string.Empty)
            {
                sWhere += " And Tags like " + StringTool.SqlQ("%" + txtTags.Text.Trim() + "%");
            }
            if (txtContent.Text.Trim() != string.Empty)
            {
                sWhere += " And Content like " + StringTool.SqlQ("%" + txtContent.Text.Trim() + "%");
            }
            if (ctrDateSelectTime1.BeginTime.Trim() != string.Empty)
                sWhere += " And RegTime >=to_date(" + StringTool.SqlQ(ctrDateSelectTime1.BeginTime.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (ctrDateSelectTime1.EndTime.Trim() != string.Empty)
                sWhere += " And RegTime <to_date(" + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTime1.EndTime).AddDays(1).ToShortDateString()) + ",'yyyy-MM-dd HH24:mi:ss')";

            DataTable dt = Epower.ITSM.SqlDAL.Inf_InformationDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.InfKBQuery], sWhere, this.cpKBBase.PageSize, this.cpKBBase.CurrentPage, ref iRowCount);
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            grd.DataBind();
            this.cpKBBase.RecordCount = iRowCount;
            this.cpKBBase.Bind();
            #endregion
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

        

        #region ���� grd_ItemCreated
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grd_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i < 5)
                    {
                        j = i - 0;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
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
            Bind();
        }
        #endregion 

        protected void grd_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[4].Text.Trim() == "20")
                {
                    e.Item.Cells[4].Text = "���ڴ���";
                }
                else if (e.Item.Cells[4].Text.Trim() == "30")
                {
                    e.Item.Cells[4].Text = "��������";
                }
                else if (e.Item.Cells[4].Text.Trim() == "40")
                {
                    e.Item.Cells[4].Text = "������ͣ";
                }
                else if (e.Item.Cells[4].Text.Trim() == "50")
                {
                    e.Item.Cells[4].Text = "������ֹ";
                }


                string lngFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
            }
        }
        /// <summary>
        /// ɾ���󡣵��ð�ť�¼������°󶨡�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
