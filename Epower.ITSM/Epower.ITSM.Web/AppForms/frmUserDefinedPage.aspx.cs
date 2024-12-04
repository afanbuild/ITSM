/*******************************************************************
 * ��Ȩ���У� �����зǷ���Ϣ�������޹�˾
 * �������Զ����
 * 
 * 
 * ������ ��zhumingchun
 * �������ڣ�2010-08-13
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmUserDefinedPage : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.UserDefinedPage;
            this.Master.IsCheckRight = true;
            this.Master.ShowDeleteButton(true);
            this.Master.ShowSaveButton(true);
            this.Master.ShowNewButton(false);
            this.Master.ShowBackUrlButton(false);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_SaveFinish_Click += new Global_BtnClick(Master_Master_Button_SaveFinish_Click);
        }
        #endregion 

        #region ������¼�Master_Master_Button_SaveFinish_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_SaveFinish_Click()
        {
            this.Master.ShowNewButton(false);
            this.Master.ShowBackUrlButton(false);
        }
        #endregion

        #region ɾ���¼� Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            string sFlowModedID = dpdFlow.SelectedValue.ToString();
            if (sFlowModedID == "0")
                return;
            App_DefinePageDP ee = new App_DefinePageDP();
            ee.DeleteRecorded(long.Parse(sFlowModedID));

            Response.Redirect("frmUserDefinedPage.aspx?FlowModelID=" + dpdFlow.SelectedValue.ToString());
        }
        #endregion

        #region �����¼� Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            string sFlowModedID = dpdFlow.SelectedValue.ToString();
            if (sFlowModedID == "0")
            {
                PageTool.MsgBox(this, "��ѡ������ģ�ͣ�");
                this.Master.IsSaveSuccess = false;
                return;
            }
            else
            {
                App_DefinePageDP ee = new App_DefinePageDP();
                ee.PageName = txtPageName.Text.Trim();
                ee.ContentXml = TxtContent.Text.Trim();
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.FlowModelID = long.Parse(sFlowModedID);
                if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    ee.InsertRecorded(ee);
                }
                else
                {
                    ee.UpdateRecorded(ee);
                }
                this.Master.MainID = ee.FlowModelID.ToString();
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
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                dpdFlow.Items.Clear();
                DataTable dt = App_DefinePageDP.GetFlowModelList(201);  //��ȡ�Զ��������ģ��
                dpdFlow.DataSource = dt.DefaultView;
                dpdFlow.DataTextField = "FlowName";
                dpdFlow.DataValueField = "OFlowModelID";
                dpdFlow.DataBind();

                dpdFlow.Items.Insert(0, new ListItem("", "0"));

                InitObject();  //��ʼ��
            }
        }
        #endregion 

        #region dpdFlow_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("frmUserDefinedPage.aspx?FlowModelID=" + dpdFlow.SelectedValue.ToString());

        }
        #endregion 

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        protected void InitObject()
        {
            if (Request["FlowModelID"] == null)
                return;
            dpdFlow.SelectedIndex = dpdFlow.Items.IndexOf(dpdFlow.Items.FindByValue(Request["FlowModelID"].ToString()));
            this.lblFlowModelID.Text = dpdFlow.SelectedValue.ToString();

            string sFlowModelID = dpdFlow.SelectedValue.ToString();
            if (sFlowModelID == "0")
                return;

            App_DefinePageDP ee = new App_DefinePageDP();
            ee = ee.GetReCorded(long.Parse(sFlowModelID));

            txtPageName.Text = ee.PageName;
            TxtContent.Text = ee.ContentXml;

            if (ee.FlowModelID != 0)
            {
                this.Master.MainID = dpdFlow.SelectedValue.ToString();
            }
        }
        #endregion 
    }
}
