/****************************************************************************
 * 
 * description:�ͻ�ѡ��
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-18
 * *************************************************************************/
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
using System.Xml;

using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Common
{
	/// <summary>
    /// frmcusShow ��ժҪ˵����
	/// </summary>
    public partial class frmcusShow : BasePage
	{
        RightEntity reTrace = null;


        /// <summary>
        /// �Ƿ���´���,�´��ڷ���ʱ�ر�
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["newWin"] != null) return true; else return false; }
        }


        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }

        /// <summary>
        /// �ͻ���չ����ģ��ID
        /// </summary>
        protected long CustSchemaID
        {
            get
            {
                if (ViewState["CustSchemaID"] != null)
                    return long.Parse(ViewState["CustSchemaID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["CustSchemaID"] = value;
            }
        }

        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
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
            ControlPageIssues.On_PostBack += new EventHandler(ControlPageIssues_On_PostBack);
            ControlPageIssues.DataGridToControl = gridUndoMsg;
			if(!IsPostBack)
			{

                reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.CustomerService];

                //������ʾ
                PageDeal.SetLanguage(this.Controls[0]);

				LoadData();

                BindData();

                //Session["FromUrl"] = "close";
			}
		}

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (IsNewWin) //�رմ��ڵ����
            {
                PageTool.AddJavaScript(this, "window.close();");
                return;
            }
            if (Request["FlowID"] != null)
                Response.Redirect("frmDRMUserSelect.aspx?FlowID=" + Request["FlowID"].ToString());
            else
                Response.Redirect("frmDRMUserSelect.aspx");
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
		private void LoadData()
		{
			if(!string.IsNullOrEmpty(this.Master.MainID.Trim()))
			{
                Br_ECustomerDP ee = new Br_ECustomerDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                lblRefUser.Text = EpowerCom.EPSystem.GetUserName((long)ee.UserID);

                lblCustDeptName.Text = ee.CustDeptName.ToString();
                lblShortName.Text = ee.ShortName.ToString();
                lblfullname.Text = ee.FullName.ToString();
                lblCustomCode.Text = ee.CustomCode.ToString();
                lblCustomerTypeName.Text = ee.CustomerTypeName.ToString();
                lblLinkMan1.Text = ee.LinkMan1.ToString();
                lblJob.Text = ee.Job.ToString();
                lblTel1.Text = ee.Tel1.ToString();
				lblAddress.Text=ee.Address.ToString();
                lblRights.Text = ee.Rights.ToString();
                lblRemark.Text = ee.Remark.ToString();
                lblEmail.Text = ee.Email.Trim();
                if (lblEmail.Text.Length > 0)
                {
                    lblEmail.NavigateUrl = "mailto:" + lblEmail.Text;
                }
                

                Br_MastCustomerDP pBr_MastCustomerDP = new Br_MastCustomerDP();
                pBr_MastCustomerDP = pBr_MastCustomerDP.GetReCorded(long.Parse(ee.MastCustID.ToString()));
                lblMastCust.Text = pBr_MastCustomerDP.ShortName.ToString();

                #region �ͻ���չģ��
                DataTable dt = new DataTable();
                dt = Br_SubjectDP.GetSubject();
                if (dt.Rows.Count > 0)
                {
                    //ģ��
                    CustSchemaID = long.Parse(Br_SubjectDP.GetSubject().Rows[0]["CatalogID"].ToString());
                }
                #endregion

                CustSchemeCtr1.ControlXmlValue = ee.SchemaValue;
                CustSchemeCtr1.BrCategoryID = CustSchemaID;
			}
		}

        #region �����¼
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            //��ʱû�����ҳ
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";

            return sUrl;


        }

        #region  ���ɲ�ѯXML�ַ��� GetXmlValue
        /// <summary>
        /// ���ɲ�ѯXML�ַ���

        /// </summary>
        /// <returns></returns>
        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;
            #region �ʲ�
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "CustID");
            xmlEle.SetAttribute("Value", this.Master.MainID == string.Empty ? "-1" : this.Master.MainID);
            xmlRoot.AppendChild(xmlEle);
            #endregion
            #region
            if (Request["FlowID"] != null)
            {
                xmlEle = xmlDoc.CreateElement("Field");
                xmlEle.SetAttribute("FieldName", "FlowID");
                xmlEle.SetAttribute("Value", Request["FlowID"].ToString());
                xmlRoot.AppendChild(xmlEle);
            }
            #endregion
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ZHServiceDP.GetIssuesForCond(xmlDoc.InnerXml, long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString())
           , long.Parse(Session["UserOrgID"].ToString()), reTrace);
            Session["Cust_Data_Issues"] = dt;
            ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            BindToTable();
            if (dt.Rows.Count < 1)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e_FlowStatus fs;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                //��������������Ԥ�ƴ���ʱ��δ����ģ������ʾ
                fs = (e_FlowStatus)int.Parse(e.Item.Cells[7].Text);

                if (int.Parse(e.Item.Cells[8].Text.Trim()) < 0 && fs != e_FlowStatus.efsEnd)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
                    {
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + i.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPageIssues_On_PostBack(object sender, EventArgs e)
        {
            BindToTable();
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindToTable()
        {
            DataTable dt = (DataTable)Session["Cust_Data_Issues"];

            //gridUndoMsg.DataSource = dw;
            gridUndoMsg.DataSource = dt.DefaultView;
            gridUndoMsg.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            gridUndoMsg.DataBind();
        }
        #endregion 
	}
}
