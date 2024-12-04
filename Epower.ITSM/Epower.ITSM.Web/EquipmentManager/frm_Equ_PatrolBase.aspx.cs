/****************************************************************************
 * 
 * description:Ѳ��ά������Ӧ�ñ�
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-05
 * *************************************************************************/
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
using System.Xml;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Web.Controls;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_PatrolBase : BasePage
    {
        /// <summary>
        /// myFlowForms
        /// </summary>
        private FlowForms myFlowForms;

        #region PrintMode
        /// <summary>
        /// ��ӡ��ʽ 0 IE��ʽ��1 Report Service��ʽ
        /// </summary>
        public string PrintMode
        {
            get
            {
               return CommonDP.GetConfigValue("PrintMode", "PrintMode").ToString();
            }
        }
        #endregion 

        #region FlowID
        /// <summary>
        /// ȡ������FlowID
        /// </summary>
        public string FlowID
        {
            get
            {
                if (myFlowForms.oFlow != null)
                {
                    return myFlowForms.oFlow.FlowID.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }
        #endregion 

        #region Ӧ��ID

        /// <summary>
        /// Ӧ��ID
        /// </summary>
        public string AppID
        {
            get
            {
                if (ViewState["frm_Patrol_Base_AppID"] != null)
                {
                    return ViewState["frm_Patrol_Base_AppID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Patrol_Base_AppID"] = value;
            }
        }

        #endregion

        #region ����ģ��ID

        /// <summary>
        /// ����ģ��ID
        /// </summary>
        public string FlowModelID
        {
            get
            {
                if (ViewState["frm_Patrol_Base_FlowModelID"] != null)
                {
                    return ViewState["frm_Patrol_Base_FlowModelID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frm_Patrol_Base_FlowModelID"] = value;
            }
        }

        #endregion

        #region PatrolID
        /// <summary>
        /// 
        /// </summary>
        protected string PatrolID
        {
            get
            {
                if (ViewState["PatrolID"] == null)
                    ViewState["PatrolID"] = EPGlobal.GetNextID("Equ_PatrolServiceID").ToString();
                return ViewState["PatrolID"].ToString();
            }
            set
            {
                ViewState["PatrolID"] = value;
            }
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;

            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentVisible);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);
            myFlowForms.blnSMSNotify = true;

            if (!IsPostBack)
            {
                InitPage();
            }
            //��ֹ�û�ͨ��IE���˰�Ŧ�ظ��ύ
            //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
        }
        #endregion

        #region InitPage
        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
        }
        #endregion


        #region Master_mySetFormsValue
        /// <summary>
        /// 
        /// </summary>
        private void Master_mySetFormsValue()
        { 
            myFlowForms.CtrButtons1.Button2Visible = true;
            myFlowForms.CtrButtons1.ButtonName2 = "֪ʶ�ο�";
            myFlowForms.CtrButtons1.Button2Function = "FormDoKmRef();";

            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;
            this.AppID = myFlowForms.oFlow.AppID.ToString();//Ӧ��ID
            this.FlowModelID = myFlowForms.oFlow.FlowModelID.ToString();//����ģ��ID
            #region Master_mySetFormsValue
            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    CtrFlowTitle.Value = row["Title"].ToString();
                    CtrFlowRemark1.Value = row["Remark"].ToString();
                    lblRegUserName.Text = row["RegUserName"].ToString();
                    lblRegDeptName.Text = row["RegDeptName"].ToString();

                    PatrolID = row["ID"].ToString();
                }
                else 
                {
                    lblRegUserName.Text = Session["PersonName"].ToString();
                    lblRegDeptName.Text = Session["UserDeptName"].ToString();
                }
            }
            BindPatrolItem(PatrolID);
            #endregion

            #region set visible
            setFieldCollection setFields = myFlowForms.oFlow.setFields;

            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {

                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "title":
                        CtrFlowTitle.ContralState =  eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled != true)
                        {
                            CtrFlowTitle.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "remark":
                        CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled != true)
                        {
                            CtrFlowRemark1.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    case "patroldetail":
                        if (sf.Visibled == true)
                        {
                            SetgdPatrolReadOnly();
                        }
                        else
                        {
                            gdPatrol.Visible = false;
                        }
                        break;
                    default:
                        break;

                }
            }
            #endregion
            //���ö����Ƿ����¼��,����Ȩ�� ���ҷ� ��ǩ״̬ �����������
            if (myFlowForms.oFlow.MessageID == 0)
            {
                trShowMonitor.Visible = false;   //���ö���Ϊ���ɼ�
            }

            //���ö����Ƿ����¼��,����Ȩ�� ���ҷ� ��ǩ״̬ �����������
            if (CheckRight(Constant.dubanyijian) == false || myFlowForms.oFlow.ActorClass == e_ActorClass.fmInfluxActor
                || myFlowForms.oFlow.FlowStatus == e_FlowStatus.efsEnd || myFlowForms.oFlow.FlowID == 0)
            {
                this.CtrMonitor1.DealVisible = false;
            }
            //���ö�������
            CtrMonitor1.FlowID = myFlowForms.oFlow.FlowID;
            CtrMonitor1.AppID = myFlowForms.oFlow.AppID;
        }
        #endregion

        #region Master_mySetContentVisible
        /// <summary>
        /// 
        /// </summary>
        private void Master_mySetContentVisible()
        {
            #region Master_mySetContentVisible
            if (CtrFlowTitle.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowTitle.ContralState = eOA_FlowControlState.eReadOnly;
            if (CtrFlowRemark1.ContralState != eOA_FlowControlState.eHidden)
                CtrFlowRemark1.ContralState = eOA_FlowControlState.eReadOnly;

            SetgdPatrolReadOnly();
            #endregion
        }
        #endregion

        #region Master_myGetFormsValue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            #region Master_myGetFormsValue
            FieldValues fv = new FieldValues();
            fv.Add("PatrolID", PatrolID);
            fv.Add("Title", CtrFlowTitle.Value.Trim());
            fv.Add("Remark", CtrFlowRemark1.Value.Trim());
            fv.Add("RegUserID", Session["UserID"].ToString());
            fv.Add("RegUserName", Session["PersonName"].ToString());
            fv.Add("RegDeptID", Session["UserDeptID"].ToString());
            fv.Add("RegDeptName", Session["UserDeptName"].ToString());
            fv.Add("RegTime", DateTime.Now.ToString());

            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());

            XmlDocument xmlDoc = fv.GetXmlObject();
            #endregion
            return xmlDoc;
        }
        #endregion

        #region �ύǰִ���¼� Master_myPreSaveClickCustomize
        /// <summary>
        /// �ύǰִ���¼�
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            SaveDetailItem();
            return true;
        }
        #endregion

        #region �ݴ�ʱǰ��ִ���¼� Master_myPreClickCustomize
        /// <summary>
        /// �ݴ�ʱǰ��ִ���¼�
        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            SaveDetailItem();
            return true;
        }
        #endregion 

        #region Ѳ��ά������
        #region ����Ѳ��ά������ SaveDetailItem
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        private bool SaveDetailItem()
        {
            try
            {
                DataTable dt = GetDetailItem();
                Equ_PatrolItemDataDP ee = new Equ_PatrolItemDataDP();
                ee.SavePatrolDetailItem(dt, decimal.Parse(PatrolID));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        #region ȡ��Ѳ��ά����ϸ���� GetDetailItem
        /// <summary>
        /// ȡ��Ѳ��ά����ϸ����
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            DataTable dt = (DataTable)Session["ItemData"];
            dt.Rows.Clear();
            DataRow dr;

            foreach (DataGridItem row in gdPatrol.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    CtrDateAndTimeV2 CtrPatrolTime = (CtrDateAndTimeV2)row.FindControl("CtrPatrolTime");
                    CheckBox chkEffect = (CheckBox)row.FindControl("chkEffect");
                    string sItemData = chkEffect.Checked ? ((int)ePatrol_ItemState.Normal).ToString() : ((int)ePatrol_ItemState.UnNormal).ToString();
                    string sPatrolTime = string.Empty;
                    sPatrolTime = CtrPatrolTime.dateTime.ToString();
                    string sPatrolName = ((TextBox)row.FindControl("txtPatrolName")).Text.Trim();
                    string sRemark = ((TextBox)row.FindControl("txtRemark")).Text.Trim();

                    dr = dt.NewRow();
                    dr["ID"] = "0";
                    dr["PatrolID"] = PatrolID;
                    dr["FlowID"] = myFlowForms.oFlow!=null ? myFlowForms.oFlow.FlowID.ToString() : "0";
                    dr["EquID"] = row.Cells[0].Text.Trim();
                    dr["EquName"] = row.Cells[2].Text.Trim();
                    dr["ItemID"] = row.Cells[1].Text.Trim();
                    dr["ItemName"] = row.Cells[3].Text.Trim();
                    dr["ItemData"] = sItemData;
                    dr["PatrolTime"] = sPatrolTime;
                    dr["PatrolName"] = sPatrolName;
                    dr["Remark"] = sRemark;
                    dt.Rows.Add(dr);
                }
            }
            Session["ItemData"] = dt;
            return dt;
        }
        #endregion

        #region ��Ѳ��ά����ϸ���� BindPatrolItem
        /// <summary>
        /// ��Ѳ��ά����ϸ����
        /// </summary>
        /// <param name="id"></param>
        private void BindPatrolItem(string pPatrolID)
        {
            #region ���Ѳ��ά������
            DataTable dtItem = Equ_PatrolItemDataDP.GetPatrolItem(decimal.Parse(pPatrolID));
            DataView dv = new DataView(dtItem);
            Session["ItemData"] = dtItem;
            gdPatrol.DataSource = dv;

            gdPatrol.DataBind();
            gdPatrol.Visible = true;
            #endregion
        }
        #endregion

        #region ����Ѳ��ά����ϸ����Ϊֻ�� SetgdPatrolReadOnly
        /// <summary>
        /// ����Ѳ��ά����ϸ����Ϊֻ��
        /// </summary>
        private void SetgdPatrolReadOnly()
        {
            foreach (DataGridItem row in gdPatrol.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    ((CtrDateAndTimeV2)row.FindControl("CtrPatrolTime")).Visible = false;
                    ((TextBox)row.FindControl("txtPatrolName")).Visible = false;
                    ((TextBox)row.FindControl("txtRemark")).Visible = false;

                    ((Label)row.FindControl("lblPatrolTime")).Visible = true;
                    ((Label)row.FindControl("lblPatrolName")).Visible = true;
                    ((Label)row.FindControl("lblRemark")).Visible = true;
                    ((CheckBox)row.FindControl("chkEffect")).Enabled = false;
                }
                else if (row.ItemType == ListItemType.Header)
                {
                    ((Button)row.FindControl("btnadd")).Visible = false;
                }
            }
            gdPatrol.Columns[8].Visible = false;
        }
        #endregion

        #region Ѳ��ά��ɾ���¼� gdPatrol_ItemCommand
        /// <summary>
        /// Ѳ��ά��ɾ���¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gdPatrol_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string sArrItemID = string.Empty;
            DataTable dt = GetDetailItem();
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
            }
            else if (e.CommandName == "Add")
            {
                LoadItemDataMerge();
            }
            gdPatrol.DataSource = dt.DefaultView;
            gdPatrol.DataBind();
        }
        #endregion

        #region ���������ݣ�������ԭ������ LoadItemDataMerge
        /// <summary>
        /// ��������
        /// </summary>
        public void LoadItemDataMerge()
        {
            string sArr = string.Empty;
            string[] arr = this.hidItemArrID.Value.Trim().Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                sArr += arr[i] + ",";
            }

            if (!string.IsNullOrEmpty(sArr))
            {
                DataTable dtProblem = Equ_DeskDefineItemDP.GetEquItem(sArr);

                DataTable dt = GetDetailItem();
                dt.Merge(dtProblem);
                gdPatrol.DataSource = dt.DefaultView;
                gdPatrol.DataBind();
            }
        }
        #endregion 

        #region ����Ѳ����Ŀ btnadd_Click
        /// <summary>
        /// ����Ѳ����Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnadd_Click(object sender, EventArgs e)
        {
            LoadItemDataMerge();
        }
        #endregion

        #region gdPatrol_ItemDataBound
        /// <summary>
        /// gdPatrol_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gdPatrol_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //���ڿؼ����س�ʼֵ
                CtrDateAndTimeV2 CtrPatrolTime = (CtrDateAndTimeV2)e.Item.FindControl("CtrPatrolTime");
                Label lblPatrolTime = (Label)e.Item.FindControl("lblPatrolTime");
                Label lblItemData = (Label)e.Item.FindControl("lblItemData");
                if (lblPatrolTime.Text.Trim() != string.Empty)
                    CtrPatrolTime.dateTime = Convert.ToDateTime(lblPatrolTime.Text.Trim());
                ((CheckBox)e.Item.FindControl("chkEffect")).Checked = e.Item.Cells[9].Text.Trim() == ((int)ePatrol_ItemState.Normal).ToString() ? true : false;
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion 
        #endregion

        #region ���Ȩ�� CheckRight
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
    }
}