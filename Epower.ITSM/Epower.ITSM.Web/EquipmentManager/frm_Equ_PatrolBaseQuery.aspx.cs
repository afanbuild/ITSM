/*******************************************************************
 * ��Ȩ���У�
 * Description��Ѳ��ά������ѯ��
 * 
 * 
 * Create By  ��zhumingchun
 * Create Date��2007-10-06
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_PatrolBaseQuery : BasePage
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

        #region ����
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=410");
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

            cpfPatrolBase.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                InitDropDown();
                //������ʼ����
                string sQueryBeginDate = string.Empty;
                sQueryBeginDate = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01";
                ctrDateTime.BeginTime = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                ctrDateTime.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                Bind();
                Session["FromUrl"] = "../EquipmentManager/frm_Equ_PatrolBaseQuery.aspx";

            }

            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.EquPatrolQuery];
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
            if (cboStatus.SelectedValue != "-1")  //״̬
            {
                sWhere += " AND status = " + cboStatus.SelectedValue;
            }
            if (CtrFlowTitle.Value.Trim() != string.Empty)  //����
            {
                sWhere += " And Title like " + StringTool.SqlQ("%" + CtrFlowTitle.Value.Trim() + "%");
            }
            if (txtRegName.Text.Trim() != string.Empty)   //�Ǽ���
            {
                sWhere += " And RegUserName like " + StringTool.SqlQ("%" + txtRegName.Text.Trim() + "%");
            }
            if (txtRegDeptName.Text.Trim() != string.Empty)  //�Ǽ��˲���
            {
                sWhere += " And RegDeptName like " + StringTool.SqlQ("%" + txtRegDeptName.Text.Trim() + "%");
            }
            string strBeginDate = ctrDateTime.BeginTime;
            string strEndDate = ctrDateTime.EndTime;

            if (strBeginDate.Trim() != string.Empty)   //�Ǽ�����
                sWhere += " And RegTime >=to_date(" + StringTool.SqlQ(strBeginDate.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (strEndDate.Trim() != string.Empty)
                sWhere += " And RegTime <to_date(" + StringTool.SqlQ(DateTime.Parse(strEndDate).AddDays(1).ToShortDateString()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (txtEquName.Text.Trim() != string.Empty)  //�豸/��Ʒ����	
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and EquName like " + StringTool.SqlQ("%" + txtEquName.Text.Trim() + "%") + ")";
            }
            if (txtItemName.Text.Trim() != string.Empty)  //Ѳ����
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and ItemName like " + StringTool.SqlQ("%" + txtItemName.Text.Trim() + "%") + ")";
            }

            if (txtPatrolName.Text.Trim() != string.Empty)  //Ѳ����
            {
                sWhere += " And ID in (select PatrolID from Equ_PatrolItemData where 1=1 and PatrolName like " + StringTool.SqlQ("%" + txtPatrolName.Text.Trim() + "%") + ")";
            }

            
            DataTable dt = Equ_PatrolDataDP.GetFieldsTable(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Epower.ITSM.Base.Constant.EquPatrolQuery], sWhere,this.cpfPatrolBase.PageSize, this.cpfPatrolBase.CurrentPage, ref iRowCount);
            grd.DataSource = dt.DefaultView;
            grd.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            grd.DataBind();
            this.cpfPatrolBase.RecordCount = iRowCount;
            this.cpfPatrolBase.Bind();

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
                for (int i = 0; i < e.Item.Cells.Count-2; i++)
                {
                    if (i >= 3)
                    {
                        j = i - 3;
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
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                string strFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + strFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
            }
        }

        /// <summary>
        /// ɾ����¼��, ˢ������.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
