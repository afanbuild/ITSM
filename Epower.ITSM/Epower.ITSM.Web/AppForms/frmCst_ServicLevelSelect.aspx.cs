using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;


namespace Epower.ITSM.Web.AppForms
{
    public partial class frmServicLevelSelect : BasePage
    {
        #region 是否查询IsSelect
        protected bool IsSelect
        {
            get
            {
                if (Request["IsSelect"] == null)
                    return false;
                else
                    return true;
            }
        }
        #endregion
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            
            this.Master.MainID = "1";
            this.Master.TableVisible = false;
        }
        #endregion

        long lngCustID = 0;
        long lngEquID = 0;
        long lngTypeID = 0;
        long lngKindID = 0;
        long lngEffID = 0;
        long lngInsID = 0;

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            ControlPage1.On_PostBack += new EventHandler(ControlPage1_On_PostBack);
            ControlPage1.DataGridToControl = dgCst_ServiceLevel;

            GetCstProperties();

            if (!IsPostBack)
            {
               
                DataTable dt = LoadData(true);
                dgCst_ServiceLevel.DataSource = dt.DefaultView;
                dgCst_ServiceLevel.DataBind();

                labMsg.Text = "以下服务级别符合当前事件单条件:";
            }
        }


        private void GetCstProperties()
        {
            string s = "";
            string tmp = "";

            if (Request.QueryString["CustID"] != null)
            {
                tmp = Request.QueryString["CustID"].Trim();
                if (tmp.Length > 0 && tmp != "-1")
                {
                    if (tmp != "undefined")
                    {
                        lngCustID = long.Parse(tmp);
                        if (Page.IsPostBack == false)
                        {
                            Br_ECustomerDP ec = new Br_ECustomerDP();
                            ec = ec.GetReCorded(lngCustID);
                            labCust.Text = ec.ShortName;

                            Br_MastCustomerDP mc = new Br_MastCustomerDP();
                            mc = mc.GetReCorded((long)ec.MastCustID);
                            labMastCust.Text = mc.ShortName;
                        }
                    }
                }
            }

            if (Request.QueryString["EquID"] != null)
            {
                tmp = Request.QueryString["EquID"].Trim();
                if (tmp.Length > 0 && tmp != "-1")
                {
                    if (tmp != "undefined")
                    {
                        lngEquID = long.Parse(tmp);
                        if (Page.IsPostBack == false)
                        {
                            Equ_DeskDP equ = new Equ_DeskDP();
                            equ = equ.GetReCorded(lngEquID);
                            labEqu.Text = equ.Name;
                        }
                    }
                }
            }

            tmp = Request.QueryString["TypeID"].Trim();
            if (tmp.Length > 0 && tmp != "-1")
            {
                if (tmp != "undefined")
                {
                    lngTypeID = long.Parse(tmp);
                    if (Page.IsPostBack == false)
                    {
                        labType.Text = CatalogDP.GetCatalogName(lngTypeID);
                    }
                }
            }

            tmp = Request.QueryString["EffID"].Trim();
            if (tmp.Length > 0 && tmp != "-1")
            {
                if (tmp != "undefined")
                {
                    lngEffID = long.Parse(tmp);
                    if (Page.IsPostBack == false)
                    {
                        labEff.Text = CatalogDP.GetCatalogName(lngEffID);
                    }
                }
            }

            tmp = Request.QueryString["InsID"].Trim();
            if (tmp.Length > 0 && tmp != "-1")
            {
                if (tmp != "undefined")
                {
                    lngInsID = long.Parse(tmp);
                    if (Page.IsPostBack == false)
                    {
                        labIns.Text = CatalogDP.GetCatalogName(lngInsID);
                    }
                }
            }

            s = Request.QueryString["CustID"] + " c " + Request.QueryString["EquID"] + " e " + Request.QueryString["TypeID"] + " t " +
                Request.QueryString["KindID"] + " k " + Request.QueryString["EffID"] + " eff " + Request.QueryString["InsID"] + " i ";

           
        }

        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData(bool isCond)
        {
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            
            Cst_ServiceLevelDP ee = new Cst_ServiceLevelDP();
            if (isCond == false)
            {
                dt = ee.GetDataTable(sWhere, sOrder); 
            }
            else
            {
                dt = ee.GetDataTableForSelect(lngCustID,lngEquID,lngTypeID,lngKindID,lngEffID,lngInsID); 
            }
            Session["Cst_ServiceLevelSelect"] = dt;
            return dt;
        }
        #endregion

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            DataTable dt;
            if (Session["Cst_ServiceLevelSelect"] == null)
            {
                //有一段时间没有动,SESSION过期情况
                dt = LoadData(true);
            }
            else
            {
                dt = (DataTable)Session["Cst_ServiceLevelSelect"];
            }
            dgCst_ServiceLevel.DataSource = dt.DefaultView;
            dgCst_ServiceLevel.DataBind();
        }
        #endregion

        #region ControlPage1_On_PostBack
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion

        #region  dgCst_ServiceLevel_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemCommand(object source, DataGridCommandEventArgs e)
        {  
          
        }
        #endregion

        #region dgCst_ServiceLevel_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < e.Item.Cells.Count - 5)
                    {
                        j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgCst_ServiceLevel_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCst_ServiceLevel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               //规范鼠标动作
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

               //绑定时限指标等

                Label lblID = (Label)e.Item.FindControl("lblID");
                Label lblLevalName = (Label)e.Item.FindControl("lblLevelName");

                //long lngLevelID = long.Parse(e.Item.Cells[0].Text.ToString());
                long lngLevelID = long.Parse(lblID.Text);

                Cst_SLGuidDP ee = new Cst_SLGuidDP();
                DataTable dt = ee.GetDataByLevelID(lngLevelID);

                string sLimit = "";
                foreach (DataRow row in dt.Rows)
                {
                    sLimit += row["guidname"].ToString().Trim() + ":" + row["TimeLimit"].ToString().Trim() + GetTimeUnit(row["TimeUnit"].ToString().Trim()) + ",";
                }
                if (sLimit.EndsWith(","))
                    sLimit = sLimit.Substring(0, sLimit.Length - 1);

                e.Item.Cells[3].Text = sLimit;

                if (IsSelect)
                {
                    //string value1 = e.Item.Cells[0].Text.Trim();
                    string value1 = lblID.Text;
                    string value2 =lblLevalName.Text;
                    string value3 = e.Item.Cells[2].Text.Trim().Replace("；", "@").Replace(";", "@").Replace("\r\n","<br/>");
                    string value4 = e.Item.Cells[3].Text.Trim();
                    //==zxl==
                    if (Request["TypeFrm"] != null)
                    {
                        string requestType = Request.QueryString["TypeFrm"].ToString();
                        if (requestType == "CST_Issue_Service") 
                        {
                            ((Button)e.Item.FindControl("lnkSelect")).Attributes.Add("onclick", "CST_Issue_Service_Select('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "');");
                            
                        }
                        if (requestType == "CST_Issue_Base")
                        {
                            ((Button)e.Item.FindControl("lnkSelect")).Attributes.Add("onclick", "CST_Issue_Base('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "');");
                        }

                        if (requestType == "frm_Issue_Template")
                        {
                            ((Button)e.Item.FindControl("lnkSelect")).Attributes.Add("onclick", "frm_Issue_Template('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "');");
                        }
                    }
                    else
                    {
                        
                        ((Button)e.Item.FindControl("lnkSelect")).Attributes.Add("onclick", "SErverLeveSelect('" + value1 + "','" + value2 + "','" + value3 + "','" + value4 + "');");
                    }
                }

            }
           
        }

        private string GetTimeUnit(string code)
        {
            string ret="单位";
            if (code == "0")
                ret = "分钟";
            if (code == "1")
                ret = "小时";
            if (code == "2")
                ret = "天";
            if (code == "3")
                ret = "分钟";
            if (code == "4")
                ret = "小时";

            return ret;
        }



        #endregion

        protected void cmdAllQuery_Click(object sender, EventArgs e)
        {
            ControlPage1.DataGridToControl.CurrentPageIndex = 0;
            DataTable dt = LoadData(false);
            dgCst_ServiceLevel.DataSource = dt.DefaultView;
            dgCst_ServiceLevel.DataBind();
            labMsg.Text = "所有有效服务级别:";
        }

        protected void cmdCondQuery_Click(object sender, EventArgs e)
        {
            ControlPage1.DataGridToControl.CurrentPageIndex = 0;
            DataTable dt = LoadData(true);
            dgCst_ServiceLevel.DataSource = dt.DefaultView;
            dgCst_ServiceLevel.DataBind();

            labMsg.Text = "以下服务级别符合当前事件单条件:";
        }
    }
}
