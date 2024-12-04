using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using System.Xml;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmOA_NEWS : BasePage
    {
        #region  变量

        private objFlow oFlow;

        private FlowForms myFlowForms;

        public string FlowID
        {
            get
            {
                if (ViewState["frmFlow_QuestHouse_FlowID"] != null)
                {
                    return ViewState["frmFlow_QuestHouse_FlowID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["frmFlow_QuestHouse_FlowID"] = value;
            }
        }

        #endregion

        public string sApplicationUrl = Epower.ITSM.Base.Constant.ApplicationPath;     //虚拟路径

        protected void Page_Load(object sender, EventArgs e)
        {
            myFlowForms = (FlowForms)this.Master;
            if (!IsPostBack)
            {
                myFlowForms.EmailValue = true;
                PageInit();

                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                //截止时间默认显示当年的最后一天

                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.Year);
                sb.Append("-12-31 23:59:59");
                ctrOutDate.dateTime = DateTime.Parse(sb.ToString());
            }
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentReadOnly);
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(Master_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(Master_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);
        }

        #region PageInit

        /// <summary>
        /// 页面加载
        /// </summary>
        private void PageInit()
        {
            #region 信息类别初始化


            ddlType.Items.Clear();
            DataTable dt = NewsDp.GetNewType();
            if (dt != null)
            {
                ListItem li;
                foreach (DataRow dr in dt.Rows)
                {
                    li = new ListItem(dr["TypeName"].ToString(), dr["TypeId"].ToString());
                    ddlType.Items.Add(li);
                }


            }
            ListItem item = new ListItem("==请选择文章类型==", "0");
            this.ddlType.Items.Insert(0, item);

            #endregion

            #region 显示范围初始化


            if (StringTool.String2Int(Session["UserOrgID"].ToString()) == 1) //省局的人员
            {
                ddlIsInner.Items.Add(new ListItem("全体人员", ((int)eOA_ReadRange.AllRange).ToString()));
            }
            ddlIsInner.Items.Add(new ListItem("本单位人员", ((int)eOA_ReadRange.OrgRange).ToString()));
            ddlIsInner.Items.Add(new ListItem("本部门人员", ((int)eOA_ReadRange.DeptRange).ToString()));
            ddlIsInner.Items[0].Selected = true;

            #endregion
        }
        #endregion

        #region 设置页面为只读 Master_mySetContentReadOnly

        /// <summary>
        /// 设置页面为只读

        /// </summary>
        void Master_mySetContentReadOnly()
        {
            #region Master_mySetContentReadOnly

            if (txtTitle.Visible == true)
                lblTitle.Visible = true;
            txtTitle.Visible = false;
            lblTitleWarning.Visible = false;

            if (ddlType.Visible == true)
                lblType.Visible = true;
            ddlType.Visible = false;
            lblTypeWarning.Visible = false;

            if (ddlDispFlag.Visible == true)
                lblDispFlag.Visible = true;
            ddlDispFlag.Visible = false;

            if (ddlIsAlert.Visible == true)
                lblIsAlert.Visible = true;
            ddlIsAlert.Visible = false;

            if (txtWriter.Visible == true)
                lblWriter.Visible = true;
            txtWriter.Visible = false;
            lblWriterWarning.Visible = false;

            if (ddlIsInner.Visible == true)
                lblIsInner.Visible = true;
            ddlIsInner.Visible = false;

            //if (FreeTextBox1.Visible == true)
            //    lblContent.Visible = true;
            //FreeTextBox1.Visible = false;

            //UEditor1.UEditorReadOnly = true;
            UEditor1.Visible = false;

            if (ctrOutDate.ContralState != eOA_FlowControlState.eHidden)
                ctrOutDate.ContralState = eOA_FlowControlState.eReadOnly;

            if (ctrPubDate.ContralState != eOA_FlowControlState.eHidden)
                ctrPubDate.ContralState = eOA_FlowControlState.eReadOnly;
            #endregion
        }

        #endregion

        #region 取得页面值生成XML Master_myGetFormsValue

        /// <summary>
        /// 取得页面值生成XML
        /// </summary>
        /// <returns></returns>
        XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            oFlow = myFlowForms.oFlow;
            FieldValues fv = new FieldValues();
            fv.Add("TITLE", txtTitle.Text.Trim());
            fv.Add("TYPE", ddlType.SelectedItem.Value);
            fv.Add("TypeName", ddlType.SelectedItem.Text);
            fv.Add("WRITER", txtWriter.Text.Trim());
            fv.Add("INPUTDATE", DateTime.Now.ToString());
            fv.Add("PUBDATE", ctrPubDate.dateTime.ToString());
            fv.Add("OUTDATE", ctrOutDate.dateTime.ToString());
            fv.Add("CONTENT", UEditor1.Content.Trim());
            fv.Add("INPUTUSER", Session["UserID"].ToString());
            fv.Add("DISPFLAG", ddlDispFlag.SelectedItem.Value);
            fv.Add("PHOTO", "");
            fv.Add("FOCUSNEWS", "");
            fv.Add("BULLETIN", "");
            fv.Add("FILENAME", "");
            fv.Add("SOFTNAME", "");
            fv.Add("INORGID", Session["UserOrgID"].ToString());
            fv.Add("INDEPTID", Session["UserDeptID"].ToString());
            fv.Add("ISINNER", ddlIsInner.SelectedItem.Value);
            fv.Add("ISALERT", ddlIsAlert.SelectedItem.Value);
            fv.Add("FLAG", "0");
            fv.Add("EmailNotify", myFlowForms.EmailValue.ToString());
            fv.Add("SMSNotify", myFlowForms.NotifyValue.ToString());
            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }

        #endregion

        #region Master_mySetFormsValue
        /// <summary> 
        /// 设置窗体值

        /// </summary>
        void Master_mySetFormsValue()
        {
            oFlow = myFlowForms.oFlow;
            ImplDataProcess dp = new ImplDataProcess(myFlowForms.oFlow.AppID);
            DataSet ds = dp.GetFieldsDataSet(myFlowForms.oFlow.FlowID, myFlowForms.oFlow.OpID);
            DataTable dt = ds.Tables[0];
            myFlowForms.FormTitle = myFlowForms.oFlow.FlowName;
            if (dt.Rows.Count > 0)
            {
                #region 标题

                txtTitle.Text = dt.Rows[0]["TITLE"].ToString();//标题
                lblTitle.Text = dt.Rows[0]["TITLE"].ToString();

                #endregion

                #region 信息类别

                if (ddlType.Items.FindByValue(dt.Rows[0]["TYPEID"].ToString()) != null)//信息类别
                {
                    ddlType.SelectedValue = dt.Rows[0]["TYPEID"].ToString();
                    lblType.Text = ddlType.SelectedItem.Text;
                }

                #endregion

                #region 是否显示
                string str = dt.Rows[0]["DISPFLAG"].ToString();
                if (ddlDispFlag.Items.FindByValue(dt.Rows[0]["DISPFLAG"].ToString()) != null)//是否显示
                {
                    //ddlDispFlag.SelectedItem.Value = dt.Rows[0]["DISPFLAG"].ToString();
                    //lblDispFlag.Text = ddlDispFlag.SelectedItem.Text;
                    ddlDispFlag.Items.FindByValue(str).Selected = true;
                    lblDispFlag.Text = ddlDispFlag.SelectedItem.Text;
                }

                #endregion

                #region 是否弹出
                string str2 = dt.Rows[0]["ISALERT"].ToString();
                if (ddlIsAlert.Items.FindByValue(dt.Rows[0]["ISALERT"].ToString()) != null)//是否弹出
                {
                    //ddlIsAlert.SelectedItem.Value = dt.Rows[0]["ISALERT"].ToString();
                    //lblIsAlert.Text = ddlIsAlert.SelectedItem.Text;
                    ddlIsAlert.Items.FindByValue(str2).Selected = true;
                    lblIsAlert.Text = ddlIsAlert.SelectedItem.Text;
                }

                #endregion

                #region 发布人


                txtWriter.Text = dt.Rows[0]["WRITER"].ToString();//发布人

                lblWriter.Text = dt.Rows[0]["WRITER"].ToString();

                #endregion

                #region 显示范围

                if (ddlIsInner.Items.FindByValue(dt.Rows[0]["ISINNER"].ToString()) != null)//显示范围
                {
                    //ddlIsInner.SelectedItem.Value = dt.Rows[0]["ISINNER"].ToString();
                    ddlIsInner.SelectedIndex = ddlIsInner.Items.IndexOf(ddlIsInner.Items.FindByValue(dt.Rows[0]["ISINNER"].ToString()));
                    lblIsInner.Text = ddlIsInner.SelectedItem.Text;
                }

                #endregion

                #region 发布时间

                ctrPubDate.dateTime = CTools.ToDateTime(dt.Rows[0]["PUBDATE"].ToString());//发布时间

                #endregion

                #region 截止时间

                ctrOutDate.dateTime = CTools.ToDateTime(dt.Rows[0]["OUTDATE"].ToString());//截止时间

                #endregion

                #region 具体内容

                UEditor1.Content = dt.Rows[0]["CONTENT"].ToString();//具体内容
                lblContent.Text = dt.Rows[0]["CONTENT"].ToString();

                #endregion
            }

            #region 设置页面展示

            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                {
                    continue;
                }
                switch (sf.Name.ToLower())
                {
                    case "title":
                        txtTitle.Visible = false;
                        lblTitleWarning.Visible = false;
                        if (sf.Visibled)
                            lblTitle.Visible = true;
                        break;
                    case "type":
                        ddlType.Visible = false;
                        lblTypeWarning.Visible = false;
                        if (sf.Visibled)
                            lblType.Visible = true;
                        break;
                    case "dispflag":
                        ddlDispFlag.Visible = false;
                        if (sf.Visibled)
                            lblDispFlag.Visible = true;
                        break;
                    case "isalert":
                        ddlIsAlert.Visible = false;
                        if (sf.Visibled)
                            lblIsAlert.Visible = true;
                        break;
                    case "writer":
                        txtWriter.Visible = false;
                        lblWriterWarning.Visible = false;
                        if (sf.Visibled)
                            lblWriter.Visible = true;
                        break;
                    case "isinner":
                        ddlIsInner.Visible = false;
                        if (sf.Visibled)
                            lblIsInner.Visible = true;
                        break;
                    case "pubdate":
                        ctrPubDate.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            ctrPubDate.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "outdate":
                        ctrOutDate.ContralState = eOA_FlowControlState.eReadOnly;
                        if (!sf.Visibled)
                            ctrOutDate.ContralState = eOA_FlowControlState.eHidden;
                        break;
                    case "content":
                        //UEditor1.UEditorReadOnly = true;
                        //if (sf.Visibled)
                        //    lblContent.Visible = true;  
                        UEditor1.Visible = false;
                        lblContent.Visible = true;
                        if (!sf.Visibled)
                        {
                            lblContent.Visible = false;
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion


        }
        #endregion

        #region  提交前执行事件Master_myPreClickCustomize

        /// <summary>
        /// 提交前执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreClickCustomize(long lngActionID, string strActionName)
        {
            DateTime dtPubDate = ctrPubDate.dateTime; //发布时间
            DateTime dtOutDate = ctrOutDate.dateTime; //截止时间

            if (dtOutDate < dtPubDate)
            {
                PageTool.MsgBox(this, PageDeal.GetLanguageValue("OA_OutDate") + "不能小于" + PageDeal.GetLanguageValue("OA_PubDate") + "!");
                return false;
            }

            return true;
        }

        #endregion

        #region 暂存时前面执行事件 Master_myPreSaveClickCustomize

        /// <summary>
        /// 暂存时前面执行事件

        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
        }

        #endregion

    }


}
