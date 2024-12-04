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
    /// frmQuickLocateCust 的摘要说明。
	/// </summary>
    public partial class frmQuickLocateCust : BasePage
    {
        #region 是否查询IsSelect
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
	
		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
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
						int j = i -1;   //注意,因为前面有一个不可见的列
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

                // 标题（中文）
                sbText.Append("arr[1] ='" + e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "") + "';");   //用户名称

                sbText.Append("arr[2] ='" + e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "") + "';");   //联系人

                sbText.Append("arr[3] ='" + e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "") + "';");   //联系人电话

                sbText.Append("arr[4] ='" + e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "") + "';");   //id

                sbText.Append("arr[5] ='" + e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "") + "';");   //地址

                sbText.Append("arr[11] ='" + e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "") + "';");   //代码
                
                sbText.Append("arr[12] ='" + e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "") + "';");   //电子邮件

                sbText.Append("arr[13] ='" + e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "") + "';");   //服务单位


                //根据客户ID取得设备信息
                Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));
                sbText.Append("arr[6] ='" + ee.ID.ToString() + "';");   // 设备ID
                sbText.Append("arr[7] ='" + ee.Name.ToString() + "';");   // 设备名称
                sbText.Append("arr[8] ='" + ee.Positions.ToString() + "';");   // 设备位置
                sbText.Append("arr[9] ='" + ee.Code.ToString() + "';");   // 设备代码
                sbText.Append("arr[10] ='" + ee.SerialNumber.ToString() + "';");   // 设备SN

                sbText.Append("arr[14] ='" + ee.Breed.ToString() + "';");       // 设备品牌
                sbText.Append("arr[15] ='" + ee.Model.ToString() + "';");       // 设备型号

                sbText.Append("arr[16] ='" + ee.ListID.ToString() + "';");      // 资产目录ID
                sbText.Append("arr[17] ='" + ee.ListName.ToString() + "';");    // 资产目录

                sbText.Append("window.parent.returnValue = arr;");
                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
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
                    string value1 = e.Item.Cells[1].Text.Trim().Replace("&nbsp;", "");   //用户名称
                    string value2 = e.Item.Cells[3].Text.Trim().Replace("&nbsp;", "");   //联系人
                    string value3 = e.Item.Cells[4].Text.Trim().Replace("&nbsp;", "");   //联系人电话
                    string value4 = e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "");   //id
                    string value5 = e.Item.Cells[2].Text.Trim().Replace("&nbsp;", "");   //地址

                    string value6 = e.Item.Cells[5].Text.Trim().Replace("&nbsp;", "");  //代码
                    string value7 = e.Item.Cells[6].Text.Trim().Replace("&nbsp;", "");  //电子邮件
                    string value8 = e.Item.Cells[7].Text.Trim().Replace("&nbsp;", "");  //服务单位

                    //根据客户ID取得设备信息
                    Epower.ITSM.SqlDAL.Equ_DeskDP ee = new Epower.ITSM.SqlDAL.Equ_DeskDP();
                    ee = ee.GetEquByCustID(long.Parse(e.Item.Cells[0].Text.Trim().Replace("&nbsp;", "")));
                    string value9 = ee.ID.ToString();                                    //设备ID
                    string value10 = ee.Name.ToString();                                  //设备名称
                    string value11 = ee.Positions.ToString();                             //设备位置
                    string value12 = ee.Code.ToString();                                  //设备代码
                    string value13 = ee.SerialNumber.ToString();                         //设备SN

                    string value14 = ee.Breed.ToString();                                //设备品牌
                    string value15 = ee.Model.ToString();                                //设备型号

                    string value16 = ee.ListID.ToString();                              //资产目录ID
                    string value17 = ee.ListName;                                       //资产目录

                    // 向客户端发送
                    e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "','" + value2 + "','" + value3 + "','" 
                        + value4 + "','" + value5 + "','" + value6 + "','" + value7 + "','" + value8 + "','" + value9 + "','"
                        + value10 + "','" + value11 + "','" + value12 + "','" + value13 + "','" + value14 + "','" + value15 + "','" + value16 + "','" + value17 + "');");
                }
            }
        }
	}
}
