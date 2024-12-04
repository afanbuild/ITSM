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

using EpowerCom;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_Issue_Template : BasePage
    {
        #region ���Զ���

        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //����·��
        RightEntity re = null;

        /// <summary>
        /// 
        /// </summary>
        protected bool IsShow
        {
            get { if (Request["IsShow"] != null) return true; else return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string TemplateID
        {
            get
            {
                return this.Master.MainID.ToString();
            }
        }

        #endregion

        #region ���ø����尴ť�¼�

        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.IssueShortCutReqTemplate;            
            this.Master.IsCheckRight = true;
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();

            if (IsShow)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
                IsCanEdit();
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frm_Issue_Template.aspx");
        }

        #endregion

        #region ɾ��

        /// <summary>
        /// ɾ��
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //ǿ����ػ���ʧЧ 
                //HttpRuntime.Cache.Insert("CommCacheValidMastCustomer", false);

                //������ҳ��
                Master_Master_Button_GoHistory_Click();
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            #region �ж�ģ�������Ƿ���� yxq 2011-09-27
            long id = this.Master.MainID.Trim() == "" ? 0 : long.Parse(this.Master.MainID.Trim());
            string tname = CtrFTTemplateName.Value.Trim();
            string sWhere = " and AppID=1026 And TemplateID<>" + id + " and TemplateName=" + StringTool.SqlQ(tname);
            if (EA_ShortCutTemplateDP.IsExist(sWhere))
            {
                PageTool.MsgBox(Page, "�Ѿ�����ģ����Ϊ" + tname + "��ģ��,����������!");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion

            #region �ж��Ƿ�ѡ��������ģ��
            if (hidFlowModel.Value.Trim() == string.Empty || hidFlowModel.Value.Trim() == "0")
            {
                PageTool.MsgBox(Page, "��ѡ������ģ��!");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion


            if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();

                InitObject(ee);
                ee.InsertRecorded(ee);
                this.Master.MainID = ee.TemplateID.ToString().Trim();
            }
            else
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                InitObject(ee);
                ee.UpdateRecorded(ee);
                this.Master.MainID = ee.TemplateID.ToString();
            }
            //LoadData();
        }

        #endregion

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (!IsShow)
                Response.Redirect("frm_Issue_TemplateMain.aspx");
            else
                Response.Write("<script>window.close();</script>");
        }

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //������ҳ��
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                //��ʼ���¼������б���������
                InitServRootType();
                LoadData();
            }
            else
            {
                LoadFlowModels(rblTempOwner.SelectedValue); //���°������� ��ֹ�ط�������������İ�ֵ
                ddlFlowModel.SelectedIndex = ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(hidFlowModel.Value.ToString()));//��ֵ�����б�
                txtServiceLevel.Text = hidServiceLevel.Value;    //����иı�ˢ��ʱ����ֵ����ʾԭ����ֵ
            }

            if (hidListName.Value != "")
                txtListName.Text = hidListName.Value;
        }

        #endregion

        #region ��ʼ���¼������б���������
        /// <summary>
        /// ��ʼ���¼������б���������
        /// </summary>
        private void InitServRootType()
        {
            #region �¼����
            DataTable dt = CatalogDP.GetCatasByRootID(1001);

            ddlServRootType.DataValueField = "CatalogID";
            ddlServRootType.DataTextField = "CatalogName";
            ddlServRootType.DataSource = dt.DefaultView;
            ddlServRootType.DataBind();

            ddlServRootType.Items.Insert(0, new ListItem("--�����¼����--", "1001"));

            #endregion

            #region Ӱ���
            dt = CatalogDP.GetCatasByRootID(1023);

            ddlRootEffect.DataValueField = "CatalogID";
            ddlRootEffect.DataTextField = "CatalogName";
            ddlRootEffect.DataSource = dt.DefaultView;
            ddlRootEffect.DataBind();

            ddlRootEffect.Items.Insert(0, new ListItem("--����Ӱ���--", "1023"));

            #endregion

            #region ������
            dt = CatalogDP.GetCatasByRootID(1024);

            ddlInstancy.DataValueField = "CatalogID";
            ddlInstancy.DataTextField = "CatalogName";
            ddlInstancy.DataSource = dt.DefaultView;
            ddlInstancy.DataBind();

            ddlInstancy.Items.Insert(0, new ListItem("--����������--", "1024"));

            #endregion

            #region �¼���Դ
            dt = CatalogDP.GetCatasByRootID(1041);

            ddlFrom.DataValueField = "CatalogID";
            ddlFrom.DataTextField = "CatalogName";
            ddlFrom.DataSource = dt.DefaultView;
            ddlFrom.DataBind();

            ddlFrom.Items.Insert(0, new ListItem("--�����¼���Դ--", "1041"));

            #endregion

            #region �ر�����
            dt = CatalogDP.GetCatasByRootID(1040);

            ddlRootReason.DataValueField = "CatalogID";
            ddlRootReason.DataTextField = "CatalogName";
            ddlRootReason.DataSource = dt.DefaultView;
            ddlRootReason.DataBind();

            ddlRootReason.Items.Insert(0, new ListItem("--�����ر�����--", "1040"));

            #endregion
        }
        #endregion

        #region ��ȡ����ģ��

        /// <summary>
        /// ��ȡ����ģ��
        /// </summary>
        /// <param name="strOwner"></param>
        private void LoadFlowModels(string strOwner)
        {
            long lngOwner = long.Parse(strOwner);
            ddlFlowModel.Items.Clear();
            DataTable dt = AppFieldConfigDP.GetFlowModelList(1026, (long)Session["UserID"], lngOwner);
            ddlFlowModel.DataSource = dt.DefaultView;
            ddlFlowModel.DataTextField = "FlowName";
            ddlFlowModel.DataValueField = "OFlowModelID";
            ddlFlowModel.DataBind();
            ddlFlowModel.Items.Insert(0, new ListItem("", ""));            
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��������
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID));
                CtrFTTemplateName.Value = ee.TemplateName;
                
                rblTempOwner.SelectedIndex = rblTempOwner.Items.IndexOf(rblTempOwner.Items.FindByValue(ee.Owner.ToString())); //��ֵ��ѡ��ť
               
                LoadFlowModels(rblTempOwner.SelectedValue);
                SetXmlObjectValue(ee.TemplateXml);//����XML�Կؼ���ֵ
                if (ddlFlowModel.Items.FindByValue(ee.OFlowModelID.ToString()) != null)
                {
                    ddlFlowModel.SelectedIndex= ddlFlowModel.Items.IndexOf(ddlFlowModel.Items.FindByValue(ee.OFlowModelID.ToString()));//��ֵ�����б�                    
                    hidFlowModel.Value = ee.OFlowModelID.ToString();
                }
                else
                {
                    ddlFlowModel.SelectedItem.Value = "";
                }
            }
            else
            {
                LoadFlowModels(rblTempOwner.SelectedValue);
            }
        }

        #endregion

        #region ����XML�Կؼ���ֵ

        /// <summary>
        /// ����XML�Կؼ���ֵ
        /// </summary>
        /// <param name="sXml"></param>
        private void SetXmlObjectValue(string sXml)
        {
            if (sXml.Length == 0)
                return;

            FieldValues fv = new FieldValues(sXml);
            if (fv.GetFieldValue("ServiceLevelID") != null)
            {
                hidServiceLevelID.Value = fv.GetFieldValue("ServiceLevelID").Value;
            }
            if (fv.GetFieldValue("ServiceLevel") != null)
            {
                hidServiceLevel.Value = fv.GetFieldValue("ServiceLevel").Value;
                txtServiceLevel.Text = hidServiceLevel.Value;
            }

            #region  �¼����
            if (fv.GetFieldValue("ServiceRootTypeID") != null)
            {
                if (fv.GetFieldValue("ServiceRootTypeID").Value != "0" && fv.GetFieldValue("ServiceRootTypeID").Value != "")
                {
                    ddlServRootType.SelectedIndex = ddlServRootType.Items.IndexOf(ddlServRootType.Items.FindByValue(fv.GetFieldValue("ServiceRootTypeID").Value));      //��
                    ctrFlowServiceType.Visible = true;

                    //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlServRootType.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //�������ĸ�
                        ctrFlowServiceType.RootID = long.Parse(ddlServRootType.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("ServiceTypeID").Value != "0" && fv.GetFieldValue("ServiceTypeID").Value != "")
                    {
                        //�����Ĭ�ϳ�ֵ��������ѡ����Ĭ��ֵ
                        ctrFlowServiceType.CatelogID = long.Parse(fv.GetFieldValue("ServiceTypeID").Value);
                        ctrFlowServiceType.CatelogValue = fv.GetFieldValue("ServiceType").Value;
                    }
                }
            }
            #endregion

            #region  Ӱ���
            if (fv.GetFieldValue("RootEffectID") != null)
            {
                if (fv.GetFieldValue("RootEffectID").Value != "0" && fv.GetFieldValue("RootEffectID").Value != "")
                {
                    ddlRootEffect.SelectedIndex = ddlRootEffect.Items.IndexOf(ddlRootEffect.Items.FindByValue(fv.GetFieldValue("RootEffectID").Value));      //��
                    CtrFlowEffect.Visible = true;

                    //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlRootEffect.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //�������ĸ�
                        CtrFlowEffect.RootID = long.Parse(ddlRootEffect.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("EffectID").Value != "0" && fv.GetFieldValue("EffectID").Value != "")
                    {
                        //�����Ĭ�ϳ�ֵ��������ѡ����Ĭ��ֵ
                        CtrFlowEffect.CatelogID = long.Parse(fv.GetFieldValue("EffectID").Value);
                        CtrFlowEffect.CatelogValue = fv.GetFieldValue("EffectName").Value;
                    }
                }
            }
            #endregion

            #region  ������
            if (fv.GetFieldValue("RootInstancyID") != null)
            {
                if (fv.GetFieldValue("RootInstancyID").Value != "0" && fv.GetFieldValue("RootInstancyID").Value != "")
                {
                    ddlInstancy.SelectedIndex = ddlInstancy.Items.IndexOf(ddlInstancy.Items.FindByValue(fv.GetFieldValue("RootInstancyID").Value));      //��
                    CtrFlowInstancy.Visible = true;

                    //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlInstancy.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //�������ĸ�
                        CtrFlowInstancy.RootID = long.Parse(ddlInstancy.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("InstancyID").Value != "0" && fv.GetFieldValue("InstancyID").Value != "")
                    {
                        //�����Ĭ�ϳ�ֵ��������ѡ����Ĭ��ֵ
                        CtrFlowInstancy.CatelogID = long.Parse(fv.GetFieldValue("InstancyID").Value);
                        CtrFlowInstancy.CatelogValue = fv.GetFieldValue("RootInstancy").Value;
                    }
                }
            }
            #endregion

            #region  �¼���Դ
            if (fv.GetFieldValue("RootFromID") != null)
            {
                if (fv.GetFieldValue("RootFromID").Value != "0" && fv.GetFieldValue("RootFromID").Value != "")
                {
                    ddlFrom.SelectedIndex = ddlFrom.Items.IndexOf(ddlFrom.Items.FindByValue(fv.GetFieldValue("RootFromID").Value));      //��
                    CtrFlowFrom.Visible = true;

                    //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlFrom.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //�������ĸ�
                        CtrFlowFrom.RootID = long.Parse(ddlFrom.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("FromID").Value != "0" && fv.GetFieldValue("FromID").Value != "")
                    {
                        //�����Ĭ�ϳ�ֵ��������ѡ����Ĭ��ֵ
                        CtrFlowFrom.CatelogID = long.Parse(fv.GetFieldValue("FromID").Value);
                        CtrFlowFrom.CatelogValue = fv.GetFieldValue("FromName").Value;
                    }
                }
            }
            #endregion


            #region  �ر�״̬����
            if (fv.GetFieldValue("RootReasonID") != null)
            {
                if (fv.GetFieldValue("RootReasonID").Value != "0" && fv.GetFieldValue("RootReasonID").Value != "")
                {
                    ddlRootReason.SelectedIndex = ddlRootReason.Items.IndexOf(ddlRootReason.Items.FindByValue(fv.GetFieldValue("RootReasonID").Value));      //��
                    CtrFlowReason.Visible = true;

                    //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                    DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlRootReason.SelectedValue));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //�������ĸ�
                        CtrFlowReason.RootID = long.Parse(ddlRootReason.SelectedItem.Value);
                    }

                    if (fv.GetFieldValue("ReasonID").Value != "0" && fv.GetFieldValue("ReasonID").Value != "")
                    {
                        //�����Ĭ�ϳ�ֵ��������ѡ����Ĭ��ֵ
                        CtrFlowReason.CatelogID = long.Parse(fv.GetFieldValue("ReasonID").Value);
                        CtrFlowReason.CatelogValue = fv.GetFieldValue("ReasonName").Value;
                    }
                }
            }
            #endregion

            if (fv.GetFieldValue("EquListID") != null)
            {
                hidListID.Value = fv.GetFieldValue("EquListID").Value;
            }

            if (fv.GetFieldValue("EquListName") != null)
            {
                hidListName.Value = fv.GetFieldValue("EquListName").Value;
                txtListName.Text = hidListName.Value;
            }

        }

        #endregion

        #region ���ݳ�������

        /// <summary>
        /// ���ݳ�������
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(EA_ShortCutTemplateDP ee)
        {
            //���ö�Ӧ��Ӧ��ID
            ee.AppID = 1026; //��ͬģ��ҳ���Ӧ��Ӧ��ID��ͬ����Ҫ�޸�

            ee.OFlowModelID = long.Parse(hidFlowModel.Value);
            ee.TemplateType = (int)e_ITSMShortCutReqType.eitsmscrtIssue;   //����̶����¼��Ŀ�������
                       
            if (rblTempOwner.Items[0].Selected == true)
                ee.Owner = 0;
            else if (rblTempOwner.Items[1].Selected == true)
                ee.Owner = 3;
            //else if (rblTempOwner.Items[2].Selected == true)
            //    ee.Owner = 3;
            //else if (rblTempOwner.Items[3].Selected == true)
            //    ee.Owner = 5;

            ee.TemplateName = CtrFTTemplateName.Value.Trim();
            ee.TemplateXml = GetFieldsXml();   //��ȡXML
        }
        #endregion

        #region ��ȡ����

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        private string GetFieldsXml()
        {
            FieldValues fv = new FieldValues();
            fv.Add("ServiceLevelID", (hidServiceLevelID.Value.Trim() == "" ? "0" : hidServiceLevelID.Value.Trim()));
            fv.Add("ServiceLevel", hidServiceLevel.Value.Trim());

            #region  �¼����

            //��
            fv.Add("ServiceRootTypeID", ddlServRootType.SelectedItem.Value.ToString().Trim());
            fv.Add("ServiceRootType", ddlServRootType.SelectedItem.Text.Trim());

            //��
            fv.Add("ServiceTypeID", ctrFlowServiceType.CatelogID.ToString().Trim());
            fv.Add("ServiceType", ctrFlowServiceType.CatelogValue.Trim());

            #endregion

            #region Ӱ���

            //��
            fv.Add("RootEffectID", ddlRootEffect.SelectedItem.Value.ToString().Trim());
            fv.Add("RootEffect", ddlRootEffect.SelectedItem.Text.Trim());

            //��
            fv.Add("EffectID", CtrFlowEffect.CatelogID.ToString().Trim());
            fv.Add("EffectName", CtrFlowEffect.CatelogValue.Trim());

            #endregion

            #region ������

            //��
            fv.Add("RootInstancyID", ddlInstancy.SelectedItem.Value.ToString().Trim());
            fv.Add("RootInstancy", ddlInstancy.SelectedItem.Text.Trim());

            //��
            fv.Add("InstancyID", CtrFlowInstancy.CatelogID.ToString().Trim());
            fv.Add("InstancyName", CtrFlowInstancy.CatelogValue.Trim());

            #endregion

            #region �¼���Դ

            //��
            fv.Add("RootFromID", ddlFrom.SelectedItem.Value.ToString().Trim());
            fv.Add("RootFrom", ddlFrom.SelectedItem.Text.Trim());

            //��
            fv.Add("FromID", CtrFlowFrom.CatelogID.ToString().Trim());
            fv.Add("FromName", CtrFlowFrom.CatelogValue.Trim());

            #endregion

            #region �ر�����

            //��
            fv.Add("RootReasonID", ddlRootReason.SelectedItem.Value.ToString().Trim());
            fv.Add("RootReason", ddlRootReason.SelectedItem.Text.Trim());

            //��
            fv.Add("ReasonID", CtrFlowReason.CatelogID.ToString().Trim());
            fv.Add("ReasonName", CtrFlowReason.CatelogValue.Trim());

            #endregion


            fv.Add("EquListID", this.hidListID.Value.Trim() == string.Empty ? "0" : this.hidListID.Value.Trim());
            fv.Add("EquListName", this.hidListName.Value.Trim());


            return fv.GetXmlObject().InnerXml;
        }

        #endregion

        #region �¼�������ʱ
        /// <summary>
        /// �¼�������ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlServRootType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlServRootType.SelectedValue != "0")
            {
                //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlServRootType.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    ctrFlowServiceType.Visible = true;
                    ctrFlowServiceType.RootID = long.Parse(ddlServRootType.SelectedValue);
                }
                else
                {                    
                    ctrFlowServiceType.RootID = 1001;
                    ddlServRootType.SelectedIndex = ddlServRootType.Items.IndexOf(ddlServRootType.Items.FindByValue("1001"));
                    PageTool.MsgBox(Page, "���¼������û���Ӽ�,����ѡ��Ϊ����!");
                }
            }
            else
            {
                ctrFlowServiceType.RootID = 1001;
            }

            ctrFlowServiceType.CatelogID = 0;
            ctrFlowServiceType.CatelogValue = "";
        }
        #endregion

        #region Ӱ��ȸ���ʱ
        /// <summary>
        /// Ӱ��ȸ���ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRootEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRootEffect.SelectedValue != "0")
            {
                //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlRootEffect.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    CtrFlowEffect.Visible = true;
                    CtrFlowEffect.RootID = long.Parse(ddlRootEffect.SelectedValue);
                }
                else
                {
                    CtrFlowEffect.RootID = 1023;
                    ddlRootEffect.SelectedIndex = ddlRootEffect.Items.IndexOf(ddlRootEffect.Items.FindByValue("1023"));
                    PageTool.MsgBox(Page, "��Ӱ�����û���Ӽ�,����ѡ��Ϊ����!");
                }
            }
            else
            {
                CtrFlowEffect.RootID = 1023;
            }
            CtrFlowEffect.CatelogID = 0;
            CtrFlowEffect.CatelogValue = "";
        }
        #endregion

        #region �����ȸ���ʱ
        /// <summary>
        /// �����ȸ���ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlInstancy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInstancy.SelectedValue != "0")
            {
                //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlInstancy.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    CtrFlowInstancy.Visible = true;
                    CtrFlowInstancy.RootID = long.Parse(ddlInstancy.SelectedValue);
                }
                else
                {
                    CtrFlowInstancy.RootID = 1024;
                    ddlInstancy.SelectedIndex = ddlInstancy.Items.IndexOf(ddlInstancy.Items.FindByValue("1024"));
                    PageTool.MsgBox(Page, "�˽�������û���Ӽ�,����ѡ��Ϊ����!");
                }
            }
            else
            {
                CtrFlowInstancy.RootID = 1024;
            }
            CtrFlowInstancy.CatelogID = 0;
            CtrFlowInstancy.CatelogValue = "";
        }
        #endregion

        #region �¼���Դ����ʱ
        /// <summary>
        /// �¼���Դ����ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRootFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFrom.SelectedValue != "0")
            {
                //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlFrom.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    CtrFlowFrom.Visible = true;
                    CtrFlowFrom.RootID = long.Parse(ddlFrom.SelectedValue);
                }
                else
                {
                    CtrFlowFrom.RootID = 1041;
                    ddlFrom.SelectedIndex = ddlFrom.Items.IndexOf(ddlFrom.Items.FindByValue("1041"));
                    PageTool.MsgBox(Page, "���¼���Դ��û���Ӽ�,����ѡ��Ϊ����!");
                }
            }
            else
            {
                CtrFlowFrom.RootID = 1041;
            }

            CtrFlowFrom.CatelogID = 0;
            CtrFlowFrom.CatelogValue = "";
        }
        #endregion

        #region �ر�״̬����
        /// <summary>
        /// �ر�״̬����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRootReason.SelectedValue != "0")
            {
                //�ж�ѡ���������Ƿ�����������У���󶨣����򣬲���             
                DataTable dt = CatalogDP.GetCatasByRootID(long.Parse(ddlRootReason.SelectedValue));
                if (dt != null && dt.Rows.Count > 0)
                {
                    CtrFlowReason.Visible = true;
                    CtrFlowReason.RootID = long.Parse(ddlRootReason.SelectedValue);
                }
                else
                {
                    CtrFlowReason.RootID = 1040;
                    ddlRootReason.SelectedIndex = ddlRootReason.Items.IndexOf(ddlRootReason.Items.FindByValue("1040"));
                    PageTool.MsgBox(Page, "�˹ر�״̬������û���Ӽ�,����ѡ��Ϊ����!");
                }
            }
            else
            {
                CtrFlowReason.RootID = 1040;
            }
            CtrFlowReason.CatelogID = 0;
            CtrFlowReason.CatelogValue = "";
        }
        #endregion

        #region �Ƿ��ܱ༭

        /// <summary>
        /// �Ƿ��ܱ༭
        /// </summary>
        private void IsCanEdit()
        {
            CtrFTTemplateName.ContralState = eOA_FlowControlState.eReadOnly;
            rblTempOwner.Enabled = false;
            ddlFlowModel.Enabled = false;
            txtListName.Enabled = false;
            cmdListName.Visible = false;
            ddlRootEffect.Enabled = false;
            CtrFlowEffect.ContralState = eOA_FlowControlState.eReadOnly;
            ddlInstancy.Enabled = false;
            CtrFlowInstancy.ContralState = eOA_FlowControlState.eReadOnly;
            ddlFrom.Enabled = false;
            CtrFlowFrom.ContralState = eOA_FlowControlState.eReadOnly;
            ddlRootReason.Enabled = false;
            CtrFlowReason.ContralState = eOA_FlowControlState.eReadOnly;
            ddlServRootType.Enabled = false;
            ctrFlowServiceType.ContralState = eOA_FlowControlState.eReadOnly;
            txtServiceLevel.Enabled = false;
            cmdPopServiceLevel.Visible = false;
        }

        #endregion
    }
}
