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
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    public partial class frmnews_list : BasePage
    {
        RightEntity re = null;

        #region 设置父窗体按钮事件SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.NewQuery;
            this.Master.IsCheckRight = true;
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowNewButton(true);
            this.Master.MainID = "1";     //设置页面的ID编号，如果为查询页面，则设置为1
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=1028");
        }
        #endregion

        #endregion

        #region 页面加载 Page_Load

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            SetParentButtonEvent();
            re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.NewQuery];
            cpNews.On_PageIndexChanged += new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);

            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
                SetHeaderText();                              

                LoadData();
                Session["FromUrl"] = "../Forms/frmnews_list.aspx";
                this.dgNews.Columns[dgNews.Columns.Count - 4].Visible = CheckRight(Constant.admindeleteflow);//流程删除
            }
        }

        #endregion

        #region 设置datagrid标头显示 余向前 2013-05-20
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgNews.Columns[1].HeaderText = PageDeal.GetLanguageValue("OA_TypeName");
            dgNews.Columns[2].HeaderText = PageDeal.GetLanguageValue("OA_Title");
            dgNews.Columns[3].HeaderText = PageDeal.GetLanguageValue("OA_Writer");
            dgNews.Columns[4].HeaderText = PageDeal.GetLanguageValue("OA_DispFlag");
            dgNews.Columns[5].HeaderText = PageDeal.GetLanguageValue("OA_IsAlert");
            dgNews.Columns[6].HeaderText = PageDeal.GetLanguageValue("OA_PubDate");
            dgNews.Columns[7].HeaderText = PageDeal.GetLanguageValue("OA_OutDate");            
        }
        #endregion

        #region 检查权限 CheckRight
        /// <summary>
        /// 检查权限
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

        #region 数据加载 LoadData

        /// <summary>
        /// 数据加载
        /// </summary>
        private void LoadData()
        {
            int iRowCount=0;
            DataTable dt = NewsDp.GetNewsdetail_two(StringTool.String2Long(Session["UserOrgID"].ToString()),
                StringTool.String2Long(Session["UserID"].ToString()), 
                StringTool.String2Long(Session["UserDeptID"].ToString()), re,
                cpNews.PageSize,cpNews.CurrentPage,ref iRowCount);

            this.dgNews.DataSource = dt;
            this.dgNews.DataBind();

            this.cpNews.RecordCount = iRowCount;
            this.cpNews.Bind();
        }

        #endregion

        #region 显示页面地址 GetUrl
        /// <summary>
        /// 显示页面地址
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + 
   "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }

        #endregion

        #region dg事件

        /// <summary>
        /// dg项绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgNews_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
           e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string sID = DataBinder.Eval(e.Item.DataItem, "NewsId").ToString();
                DateTime pdt = DateTime.Parse(e.Item.Cells[12].Text.Trim());
                if (pdt < DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// dv项创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgNews_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < 9)
                    {
                        int j = i - 1;  //前面有5个隐藏的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        /// <summary>
        /// gv删除
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgNews_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            LoadData();
        }

        #endregion

        /// <summary>
        /// 删除记录后, 刷新数据.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void hidd_btnDelete_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
