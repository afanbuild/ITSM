/****************************************************************************
 * 
 * description:其它业务权限管理
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-19
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

using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ctrSetUserOtherRight : System.Web.UI.UserControl
    {
        #region OperateType
        /// <summary>
        /// 业务对象类别【30:资产权限】
        /// </summary>
        public int OperateType
        {
            get { return int.Parse(ViewState["OperateType"].ToString()); }
            set { ViewState["OperateType"] = value; }
        }
        #endregion
        #region OperateID
        /// <summary>
        /// 业务对象ID【资产类别中:对应资产类别】
        /// </summary>
        public int OperateID
        {
            get { return int.Parse(ViewState["OperateID"].ToString()); }
            set { ViewState["OperateID"] = value; }
        }
        #endregion 

        #region IsReadOnly
        /// <summary>
        /// 是否为只读
        /// </summary>
        public bool IsReadOnly
        {
            get {
                if (ViewState["IsReadOnly"] != null)
                  return  bool.Parse(ViewState["IsReadOnly"].ToString());
                else
                    return false;
                }
            set { ViewState["IsReadOnly"] = value; }
        }
        #endregion 

        #region Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = LoadData();
                dgEA_ExtendRights.DataSource = dt.DefaultView;
                dgEA_ExtendRights.DataBind();

                if (IsReadOnly)
                {
                    dgEA_ExtendRights.Columns[0].Visible = false;
                    dgEA_ExtendRights.Columns[dgEA_ExtendRights.Columns.Count - 1].Visible = false;
                }
            }
            else
            {
                if (Request.Form["__EVENTTARGET"] == "datarange")
                {
                    DataTable dt = LoadData();
                    dgEA_ExtendRights.DataSource = dt.DefaultView;
                    dgEA_ExtendRights.DataBind();
                }
            }
        }
        #endregion 

        #region btnDelete_Click
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
            foreach (DataGridItem itm in dgEA_ExtendRights.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    Label labRightID = (Label)itm.Cells[1].FindControl("labRightID");
                    string sID = labRightID.Text.Trim();
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        ee.DeleteRecorded(long.Parse(sID));
                    }
                }
            }
            DataTable dt = LoadData();
            dgEA_ExtendRights.DataSource = dt.DefaultView;
            dgEA_ExtendRights.DataBind();
        }
        #endregion 

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData()
        {
            DataTable dt;
            string sWhere = string.Empty;
            sWhere += " And OperateType=" + OperateType.ToString();
            sWhere += " And OperateID=" + OperateID.ToString();
            EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
            dt = ee.GetRightInfo(sWhere); ;
            //ViewState["EA_ExtendRights"] = dt;
            return dt;
        }
        #endregion 

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            //DataTable dt;
            //if (ViewState["EA_ExtendRights"] == null)
            //{
            //    dt = LoadData();
            //}
            //else
            //{
            //    dt = (DataTable)ViewState["EA_ExtendRights"];
            //}
            //dgEA_ExtendRights.DataSource = dt.DefaultView;
            //dgEA_ExtendRights.DataBind();
        }
        #endregion 

        #region dgEA_ExtendRights_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEA_ExtendRights_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count-1; i++)
                {
                    if (i > 0)
                    {
                        j = i;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion 

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DataTable dt = LoadData();
            dgEA_ExtendRights.DataSource = dt.DefaultView;
            dgEA_ExtendRights.DataBind();
        }
    }
}