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
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
	/// <summary>
    /// frmBr_MastCustomerEdit ��ժҪ˵����
	/// </summary>
    public partial class frmBr_MastCustomerEdit : BasePage
	{
		RightEntity re = null;

        /// <summary>
        /// 
        /// </summary>
        protected string CustomID
        {
            get
            {
                return this.Master.MainID.ToString();
            }
        }

        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.ServiceDept;
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            if(string.IsNullOrEmpty(this.Master.MainID))
            {
                this.trRight.Visible = false;
            }
            else
            {
                //�ͻ����ϣ��ʲ����Ϸ�Χ����
                if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")
                {
                    this.trRight.Visible = true;
                }
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmBr_MastCustomerEdit.aspx");
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                if (Br_ECustomerDP.GetmastCustId(this.Master.MainID.Trim()) != true)
                {
                    Br_MastCustomerDP ee = new Br_MastCustomerDP();
                    ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                    //ǿ����ػ���ʧЧ 
                    HttpRuntime.Cache.Insert("CommCacheValidMastCustomer", false);

                    //������ҳ��
                    Master_Master_Button_GoHistory_Click();
                }
                else
                {
                    PageTool.MsgBox(this, "����λ���ڿͻ����õ�������ɾ����");
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Br_MastCustomerDP ee = new Br_MastCustomerDP();
                ee.Deleted = (int)Epower.ITSM.Base.eRecord_Status.eNormal;
                ee.RegUserID = decimal.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                InitObject(ee);
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.ID.ToString().Trim();
                CtrSetUserOtherRight1.OperateID = int.Parse(this.Master.MainID);

                //�ͻ����ϣ��ʲ����Ϸ�Χ����
                if (CommonDP.GetConfigValue("Other", "DataLimit") != null && CommonDP.GetConfigValue("Other", "DataLimit") == "1")
                {
                    this.trRight.Visible = true;
                }
            }
            else
            {
                Br_MastCustomerDP ee = new Br_MastCustomerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.ID.ToString();
            }

            //ǿ����ػ���ʧЧ 
            HttpRuntime.Cache.Insert("CommCacheValidMastCustomer", false);

            TableImg.Visible = true;
            Table1.Visible = true;
        }

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect("frmBr_MastCustomerMain.aspx");
        }

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
	    {
            //������ҳ��
            SetParentButtonEvent();
			if(!IsPostBack)
			{
                CtrSetUserOtherRight1.OperateType = 10;
                if (string.IsNullOrEmpty(this.Master.MainID))
                {
                    CtrSetUserOtherRight1.OperateID = 0;
                }
                else
                {
                    CtrSetUserOtherRight1.OperateID = int.Parse(this.Master.MainID);
                }
                if (!this.Master.GetEditRight())    //�Ƿ���ֻ��
                    CtrSetUserOtherRight1.IsReadOnly = true;
				LoadData();
			}
            if (this.Master.MainID.Trim() == string.Empty)  //������ϵ��
            {
                TableImg.Visible = false;
                Table1.Visible = false;
            }
            else
            {
                TableImg.Visible = true;
                Table1.Visible = true;
            }
		}

		private void LoadData()
		{
			if(!string.IsNullOrEmpty(this.Master.MainID.Trim()))
			{
                Br_MastCustomerDP ee = new Br_MastCustomerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));

				txtAddress.Text=ee.Address.ToString();
				ctrFCDServiceType.CatelogID=long.Parse(ee.EnterpriseType.ToString());
                ctrFCDServiceType.CatelogValue = ee.EnterpriseTypeName.ToString();
                ctrFCDWTType.CatelogID = long.Parse(ee.CustomerType.ToString());
                ctrFCDWTType.CatelogValue = ee.CustomerTypeName.ToString();
                txtCustomCode.Text = ee.CustomCode.ToString();
				txtFax1.Text=ee.Fax1.ToString();
                CtrFTFullName.Value = ee.FullName.ToString();
                CtrFTLinkMan1.Value = ee.LinkMan1.ToString();
                CtrFTShortName.Value = ee.ShortName.ToString();
                CtrFTTel1.Value = ee.Tel1.ToString();
				txtWebSite.Text=ee.WebSite.ToString();
                txtContent.Text = ee.ServiceProtocol.ToString();

			}
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

		}
		#endregion

		private void InitObject(Br_MastCustomerDP ee)
		{
			ee.Address=txtAddress.Text;
            ee.EnterpriseType = ctrFCDServiceType.CatelogID;
            ee.EnterpriseTypeName = ctrFCDServiceType.CatelogValue.Trim();
            ee.CustomerType = ctrFCDWTType.CatelogID;
            ee.CustomerTypeName = ctrFCDWTType.CatelogValue.Trim();
            ee.CustomCode = txtCustomCode.Text.Trim();
			ee.Fax1=txtFax1.Text;
            ee.FullName = CtrFTFullName.Value;
            ee.LinkMan1 = CtrFTLinkMan1.Value;
            ee.ShortName = CtrFTShortName.Value;
            ee.Tel1 = CtrFTTel1.Value;
			ee.WebSite=txtWebSite.Text;
            ee.ServiceProtocol = txtContent.Text.Trim();
		}
	}
}
