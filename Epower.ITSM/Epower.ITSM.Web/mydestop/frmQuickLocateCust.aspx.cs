using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using System.Text;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.MyDestop
{
	/// <summary>
    /// frmQuickLocateCust ��ժҪ˵����
	/// </summary>
    public partial class frmQuickLocateCust : BasePage
    {
        #region �Ƿ��ѯIsSelect
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] != null && Request["IsSelect"] == "true")
                    return true;
                else return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.TableVisible = false;
        }
        #endregion 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			if(!IsPostBack)
			{
                if (Request["Name"]!=null)
                    BindData(Request["Name"].ToString());

                

			}
		}

        /// <summary>
        /// 
        /// </summary>
        private void BindData(string sCust)
		{
            string sSql = " and (E.ShortName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.FullName like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.CustomCode like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.LinkMan1 like " + StringTool.SqlQ("%" + sCust + "%")
                       + " or E.Tel1 like " + StringTool.SqlQ("%" + sCust + "%") + ")";
            Br_ECustomerDP ee = new Br_ECustomerDP();
            DataTable dt = ee.GetCustomerServic(sSql, string.Empty);

            dgUserInfo.Columns[1].HeaderText = PageDeal.GetLanguageValue("LitCustName");
            dgUserInfo.Columns[2].HeaderText = PageDeal.GetLanguageValue("LitCustAddress");
            dgUserInfo.Columns[3].HeaderText = PageDeal.GetLanguageValue("LitContact");
            dgUserInfo.Columns[4].HeaderText = PageDeal.GetLanguageValue("LitCTel");
            dgUserInfo.Columns[5].HeaderText = PageDeal.GetLanguageValue("LitCustomCode");
            dgUserInfo.Columns[6].HeaderText = PageDeal.GetLanguageValue("LitCustEmail");
            dgUserInfo.Columns[7].HeaderText = PageDeal.GetLanguageValue("LitMastShortName");

            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
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
			this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

		}
		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				DataGrid dg = (DataGrid)sender;
				for (int i = 0; i < e.Item.Cells.Count; i++)
				{
					// (DataView)e.Item.NamingContainer;
					if (i>0 && i<8)
					{
						int j = i -1;   //ע��,��Ϊǰ����һ�����ɼ�����
						e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
					}
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgUserInfo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = new Array();");

                // ���⣨���ģ�
                sbText.Append("arr[1] ='" + e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "") + "';");   //�û�����

                sbText.Append("arr[2] ='" + e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "") + "';");   //��ϵ��

                sbText.Append("arr[3] ='" + e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "") + "';");   //��ϵ�˵绰

                sbText.Append("arr[4] ='" + e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "") + "';");   //id

                sbText.Append("arr[5] ='" + e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "") + "';");   //��ַ

                sbText.Append("arr[11] ='" + e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "") + "';");   //����
                
                sbText.Append("arr[12] ='" + e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "") + "';");   //�����ʼ�

                sbText.Append("arr[13] ='" + e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "") + "';");   //����λ


                //���ݿͻ�IDȡ���豸��Ϣ
                Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));
                sbText.Append("arr[6] ='" + ee.ID.ToString() + "';");   // �豸ID
                sbText.Append("arr[7] ='" + ee.Name.ToString() + "';");   // �豸����
                sbText.Append("arr[8] ='" + ee.Positions.ToString() + "';");   // �豸λ��
                sbText.Append("arr[9] ='" + ee.Code.ToString() + "';");   // �豸����
                sbText.Append("arr[10] ='" + ee.SerialNumber.ToString() + "';");   // �豸SN

                sbText.Append("arr[14] ='" + ee.Breed.ToString() + "';");       // �豸Ʒ��
                sbText.Append("arr[15] ='" + ee.Model.ToString() + "';");       // �豸�ͺ�

                sbText.Append("arr[16] ='" + ee.ListID.ToString() + "';");      // �ʲ�Ŀ¼ID
                sbText.Append("arr[17] ='" + ee.ListName.ToString() + "';");    // �ʲ�Ŀ¼

                sbText.Append("window.parent.returnValue = arr;");
                // �رմ���
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // ��ͻ��˷���
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }
        }

        protected void dgUserInfo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (IsSelect)
                {
                    string value1 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");   //�û�����
                    string value2 = e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "");   //��ϵ��
                    string value3 = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");   //��ϵ�˵绰
                    string value4 = e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "");   //id
                    string value5 = e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "");   //��ַ

                    string value6 = e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "");  //����
                    string value7 = e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "");  //�����ʼ�
                    string value8 = e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "");  //����λ

                    //���ݿͻ�IDȡ���豸��Ϣ
                    Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                    ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));
                    string value9 = ee.ID.ToString();                                    //�豸ID
                    string value10 = ee.Name.ToString();                                  //�豸����
                    string value11 = ee.Positions.ToString();                             //�豸λ��
                    string value12 = ee.Code.ToString();                                  //�豸����
                    string value13 = ee.SerialNumber.ToString();                         //�豸SN

                    string value14 = ee.Breed.ToString();                                //�豸Ʒ��
                    string value15 = ee.Model.ToString();                                //�豸�ͺ�

                    string value16 = ee.ListID.ToString();                              //�ʲ�Ŀ¼ID
                    string value17 = ee.ListName;                                       //�ʲ�Ŀ¼

                    // ��ͻ��˷���
                    e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "','" + value2 + "','" + value3 + "','" 
                        + value4 + "','" + value5 + "','" + value6 + "','" + value7 + "','" + value8 + "','" + value9 + "','"
                        + value10 + "','" + value11 + "','" + value12 + "','" + value13 + "','" + value14 + "','" + value15 + "','" + value16 + "','" + value17 + "');");
                }
            }
        }
	}
}
