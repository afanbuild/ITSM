/****************************************************************************
 * 
 * description:资产关联
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2008-04-23
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

using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using System.Xml;
using Epower.ITSM.Web.Controls;
using Epower.ITSM.SqlDAL.EquipmentManager;
using System.Text;

namespace Epower.ITSM.Web.EquipmentManager
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmEquRelMain : BasePage
    {
        #region 属性

        protected long lngCatalogID = 0; //资产分类ID
        protected long lngDeptID = 0; //资产维护部门ID

        #region 设备分类ID
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (Request["subjectid"] != null && Request["subjectid"] != "")
                    return Request["subjectid"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region EquID
        /// <summary>
        /// 
        /// </summary>
        protected string EquID
        {
            get { if (Request["ID"] != null) return Request["ID"].ToString(); else return "0"; }
        }
        #endregion

        #endregion

        /// <summary>
        /// 是否打开新窗口,新窗口返回时关闭
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["newWin"] != null) return true; else return false; }
        }

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowBackUrlButton(true);
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (IsNewWin) //关闭窗口的情况
            {
                PageTool.AddJavaScript(this, "window.close();");
                return;
            }
            else if (Request["IsChartAdd"] != null && Request["IsChartAdd"].ToString() == "true")
            {
                PageTool.AddJavaScript(this, "window.close();window.opener.location.reload(); ");
                return;
            }

            Response.Redirect("frmEqu_DeskMain.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region 页面加载 Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (!IsPostBack)
            {
                AjaxWithRelName();

                PageDeal.SetLanguage(this.Controls[0]);
                InitPage();
                LoadData();

                #region 判断是否有资产关联权限 与资产修改相同的判断
                string sEquList = Session["EquLimitList"].ToString() + ",";

                E8Logger.Info(sEquList);
                if (sEquList.Contains("'" + lngCatalogID.ToString() + "',") || DeptDP.IsExistUserByDept(Session["UserID"].ToString(), lngDeptID.ToString()))
                    trNewSave.Visible = true;
                else
                    trNewSave.Visible = false;
                #endregion
            }
        }
        #endregion

        #region ajax call for 资产关联名称
        /// <summary>
        /// 资产关联名称的 Ajax 调用
        /// </summary>
        private void AjaxWithRelName()
        {
            //action:'addrelname', relname
            String strAction = Request.QueryString["action"];
            String strRelName = Request.QueryString["relname"];

            if (String.IsNullOrEmpty(strAction)
                || !strAction.Equals("addrelname"))
                return;
            if (String.IsNullOrEmpty(strRelName))
                return;

            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            long lngRelNameId = equRelNameDP.AddRelName(strRelName);
            Response.Write(lngRelNameId.ToString());

            Response.End();
        }
        #endregion

        #region 页面数据初始化 InitPage
        /// <summary>
        /// 
        /// </summary>
        private void InitPage()
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            Br_MastCustomerDP bmc = new Br_MastCustomerDP();
            ee = ee.GetReCorded(long.Parse(EquID));
            lblName.Text = ee.Name.ToString();//资产名称
            lblCode.Text = ee.Code.ToString();//资产编号
            lblCustom.Text = ee.CostomName.ToString();//所属客户
            lblServiceBeginTime.Text = ee.ServiceBeginTime.ToString("yyyy-MM-dd");//保存开始日期
            lblServiceEndTime.Text = ee.ServiceEndTime.ToString("yyyy-MM-dd");//保存结束日期
            lblEquStatus.Text = ee.EquStatusName.ToString();//资产状态
            lblPartBankName.Text = ee.partBankName;//维护机构
            lblPartBranchName.Text = ee.partBranchName;//维护部门
            lblListName.Text = ee.ListName;//资产目录
            bmc = bmc.GetReCorded(long.Parse(ee.Mastcustid.ToString()));//获取服务单位
            lblMastCust.Text = bmc.ShortName;

            lngCatalogID = (long)ee.CatalogID;
            lngDeptID = long.Parse(ee.partBranchId.ToString() == "" ? "0" : ee.partBranchId.ToString());

        }
        #endregion

        #region 取得详细资料GetDetailItem
        /// <summary>
        /// 取得详细资料
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem()
        {
            DataTable dt = (DataTable)Session["EquRelItemData"];
            DataTable dt2 = dt.Clone();
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string strEquNatureID = ((CtrEquNature)row.FindControl("CtrEquNature1")).Value;     //资产关联属性ID串
                    string strEquNatureName = ((CtrEquNature)row.FindControl("CtrEquNature1")).Text;    //资产关联属性Name串

                    if (strEquNatureID != "")
                    {
                        //关联属性不为空

                        string[] ArrEquNatureID = strEquNatureID.Split(',');                    //资产关联属性ID字符串
                        string[] ArrEquNatureName = strEquNatureName.Split(',');                //资产关联属性Name字符串

                        for (int j = 0; j < ArrEquNatureID.Length; j++)
                        {
                            dr = dt.NewRow();
                            dr["Equ_ID"] = row.Cells[0].Text.ToString();
                            dr["RelID"] = row.Cells[1].Text.ToString();
                            dr["Code"] = row.Cells[3].Text.ToString();
                            dr["Name"] = row.Cells[2].Text.ToString();
                            dr["RelPropID"] = ArrEquNatureID[j].ToString();
                            dr["RelPropName"] = ArrEquNatureName[j].ToString();
                            dr["RelDescription"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogValue;
                            dr["RelDescriptionID"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogID;

                            if (ddlRelName.SelectedIndex > 0)
                            {
                                dr["RelKey"] = ddlRelName.SelectedItem.Text.ToLower();
                            }
                            else
                            {
                                dr["RelKey"] = "default";
                            }


                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        //关联属性为空，则直接添加资产与资产的关联 
                        dr = dt.NewRow();
                        dr["Equ_ID"] = row.Cells[0].Text.ToString();
                        dr["RelID"] = row.Cells[1].Text.ToString();
                        dr["Code"] = row.Cells[3].Text.ToString();
                        dr["Name"] = row.Cells[2].Text.ToString();
                        dr["RelPropID"] = "0";
                        dr["RelPropName"] = "";
                        dr["RelDescription"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogValue;
                        dr["RelDescriptionID"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogID;

                        if (ddlRelName.SelectedIndex > 0)
                        {
                            dr["RelKey"] = ddlRelName.SelectedItem.Text.ToLower();
                        }
                        else
                        {
                            dr["RelKey"] = "default";
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            Session["EquRelItemData"] = dt;
            return dt;
        }
        #endregion

        #region 取得详细资料GetDetailItem2【新增时使用】
        /// <summary>
        /// 取得详细资料【新增时使用】
        /// </summary>
        /// <param name="isAll"></param>
        /// <returns></returns>
        private DataTable GetDetailItem2()
        {
            DataTable dt = (DataTable)Session["EquRelItemData"];
            dt.Rows.Clear();
            int iCostID = 0;
            DataRow dr;
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Controls[0].Controls)
            {
                iCostID++;
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    string strEquNatureID = ((CtrEquNature)row.FindControl("CtrEquNature1")).Value;     //资产关联属性ID串
                    string strEquNatureName = ((CtrEquNature)row.FindControl("CtrEquNature1")).Text;    //资产关联属性Name串

                    if (strEquNatureID != "")
                    {
                        //关联属性不为空

                        string[] ArrEquNatureID = strEquNatureID.Split(',');                    //资产关联属性ID字符串
                        string[] ArrEquNatureName = strEquNatureName.Split(',');                //资产关联属性Name字符串

                        dr = dt.NewRow();
                        dr["Equ_ID"] = row.Cells[0].Text.ToString();
                        dr["RelID"] = row.Cells[1].Text.ToString();
                        dr["Name"] = row.Cells[2].Text.ToString();
                        dr["Code"] = row.Cells[3].Text.ToString();
                        dr["RelPropID"] = ArrEquNatureID[0].ToString();
                        dr["RelPropName"] = ArrEquNatureName[0].ToString();
                        dr["RelDescription"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogValue;
                        dr["RelDescriptionID"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogID;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        //关联属性为空，则直接添加资产与资产的关联 
                        dr = dt.NewRow();
                        dr["Equ_ID"] = row.Cells[0].Text.ToString();
                        dr["RelID"] = row.Cells[1].Text.ToString();
                        dr["Name"] = row.Cells[2].Text.ToString();
                        dr["Code"] = row.Cells[3].Text.ToString();
                        dr["RelPropID"] = "0";
                        dr["RelPropName"] = "";
                        dr["RelDescription"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogValue;
                        dr["RelDescriptionID"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogID;
                        dt.Rows.Add(dr);
                    }
                }
            }
            Session["EquRelItemData"] = dt;
            return dt;
        }
        #endregion

        #region 明细删除事件 dgPro_ProblemAnalyse_ItemCommand
        /// <summary>
        /// 费用明细新增，删除事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            DataTable dt = GetDetailItem2();
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                this.hidCustArrIDold.Value = this.hidCustArrIDold.Value.Replace(e.Item.Cells[1].Text.ToString() + ",", "");
                this.hidCustArrID.Value = this.hidCustArrID.Value.Replace(e.Item.Cells[1].Text.ToString() + ",", "");
            }
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            //btnSave_Click(null, null);
        }
        #endregion

        #region 保存数据 btnSave_Click
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            /* 默认视图 */
            String strEquRelKey = ddlRelName.SelectedItem.Text;
            if (ddlRelName.SelectedIndex <= 0)
                strEquRelKey = "default";

            DataTable dt = GetDetailItem();

            Equ_RelDP.SaveItemNew(dt,
                decimal.Parse(EquID),
                long.Parse(Session["UserID"].ToString()),
                strEquRelKey);


            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;

            PageTool.MsgBox(this, "保存成功！");
        }
        #endregion

        #region 新增 btnAdd_Click
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (hidFlag.Value.ToUpper() == "OK")
            {
                LoadProblemData();
            }
        }
        #endregion

        #region 加载数据 LoadData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            #region 关联名称
            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();
            BindRelKeyList();

            /*增加默认提示*/
            DataRow dr = dt.NewRow();
            dr[0] = -1;
            dr[1] = "请选择";
            dt.Rows.InsertAt(dr, 0);

            ddlRelName.DataSource = dt;
            ddlRelName.DataValueField = "ID";
            ddlRelName.DataTextField = "RelKey";
            ddlRelName.DataBind();

            /* 新增关联名称后, 显示新增的关联名称. */
            Int32 intSelectIndex = -1;
            String strddlRelKeyIndex = Request.QueryString["ddlrelkeyindex"];
            if (!String.IsNullOrEmpty(strddlRelKeyIndex))
            {
                Boolean isOk = Int32.TryParse(strddlRelKeyIndex, out intSelectIndex);
                if (isOk) ddlRelName.SelectedIndex = intSelectIndex;
            }
            /* 如果是新建关联名称 */
            String strddlRelKeyId = Request.QueryString["ddlrelkeyid"];
            if (!String.IsNullOrEmpty(strddlRelKeyId))
            {
                Int32 intRelKeyId;
                Boolean isOk = Int32.TryParse(strddlRelKeyId, out intRelKeyId);
                if (isOk)
                {
                    for (int i = 0; i < ddlRelName.Items.Count; i++)
                    {
                        if (ddlRelName.Items[i].Value.Equals(intRelKeyId.ToString()))
                        {
                            ddlRelName.SelectedIndex = i;
                            BindData(ddlRelName.SelectedIndex);
                            return;
                        }
                    }
                }
            }

            #endregion

            Equ_RelDP ee = new Equ_RelDP();
            string sWhere = " and Equ_ID=" + EquID;

            // 视角名称.
            String strEquRelKey = ddlRelName.SelectedItem.Text.ToLower();
            if (ddlRelName.SelectedIndex <= 0)
                strEquRelKey = "default"; //默认视角


            sWhere += String.Format(" and RelKey = {0}", StringTool.SqlQ(strEquRelKey));
            dt = ee.GetDataTableNew(sWhere, string.Empty, strEquRelKey);

            Session["EquRelItemData"] = dt;
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();


            foreach (DataGridItem item in dgPro_ProblemAnalyse.Items)
            {
                string strEquRelID = item.Cells[1].Text;   //关联资产ID

                //给资产属性赋值
                SerEquProps(EquID, strEquRelID, item, strEquRelKey);
            }

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }
        /// <summary>
        /// 绑定所有视角
        /// </summary>        
        private void BindRelKeyList()
        {
            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();

            if (dt.Rows.Count <= 0)
            {
                literalRelKeyList.Visible = false;
                return;
            }

            StringBuilder sbText = new StringBuilder();
            String strTemplate = literalRelKeyList.Text;

            foreach (DataRow item in dt.Rows)
            {
                sbText.AppendFormat(strTemplate,
                    item["ID"],
                    item["RelKey"]);
            }

            literalRelKeyList.Text = sbText.ToString();
        }

        /// <summary>
        /// 刷新关联资产
        /// </summary>
        /// <param name="intSelectIndex">关联名称编号</param>
        private void BindData(Int32 intSelectIndex)
        {
            Equ_RelDP ee = new Equ_RelDP();
            string sWhere = " and Equ_ID=" + EquID;
            DataTable dt = null;

            // 视角名称.
            String strEquRelKey = ddlRelName.SelectedItem.Text.ToLower();
            if (intSelectIndex <= 0)
                strEquRelKey = "default"; //默认视角

            sWhere += String.Format(" and RelKey = {0}", StringTool.SqlQ(strEquRelKey));
            dt = ee.GetDataTableNew(sWhere, string.Empty, strEquRelKey);

            //DataTable dt = ee.GetDataTableNew(sWhere, string.Empty, ddlRelName.SelectedItem.Text);
            Session["EquRelItemData"] = dt;
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();


            foreach (DataGridItem item in dgPro_ProblemAnalyse.Items)
            {
                string strEquRelID = item.Cells[1].Text;   //关联资产ID

                //给资产属性赋值
                SerEquProps(EquID, strEquRelID, item, strEquRelKey);
            }

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }


        protected void ddlRelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hidCustArrIDold.Value = "";
            this.hidCustArrID.Value = "";
            BindData(ddlRelName.SelectedIndex);
        }
        #endregion

        #region 给资产属性赋值
        /// <summary>
        /// 给资产属性赋值
        /// </summary>
        /// <param name="strEquID">资产ID</param>
        /// <param name="strEquRelID">关联资产ID</param>
        private void SerEquProps(string strEquID, string strEquRelID, DataGridItem item,
        string strRelKey)
        {
            Equ_RelDP ee = new Equ_RelDP();
            string strEquPropID = string.Empty;                     //资产关联属性ID串
            string strEquPropName = string.Empty;                   //资产关联属性Name串


            //根据资产ID和关联资产ID得到关联的属性ID和Name串
            string sWhere = " and Equ_ID=" + strEquID + " and RelID="
                + strEquRelID + " AND RelKey = " + StringTool.SqlQ(strRelKey.ToLower());
            DataTable dt = ee.GetPropsByID(sWhere);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strEquPropID += row["RelPropID"].ToString() + ",";
                    strEquPropName += row["RelPropName"].ToString() + ",";
                }
            }

            if (strEquPropID != "")
            {
                strEquPropID = strEquPropID.Substring(0, strEquPropID.Length - 1);
                strEquPropName = strEquPropName.Substring(0, strEquPropName.Length - 1);
                ((CtrEquNature)item.FindControl("CtrEquNature1")).Value = strEquPropID;
                ((CtrEquNature)item.FindControl("CtrEquNature1")).Text = strEquPropName;
            }
            else
            {
                ((CtrEquNature)item.FindControl("CtrEquNature1")).Value = "";
                ((CtrEquNature)item.FindControl("CtrEquNature1")).Text = "";
            }

        }
        #endregion

        #region 生成总的REFID
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private void CreateTotle(ref string sArrRefID)
        {
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Items)
            {
                sArrRefID += row.Cells[1].Text.Trim() + ",";
            }
        }
        #endregion

        #region 加载问题数据，并整合原有数据 LoadProblemData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadProblemData()
        {
            string sArr = string.Empty;
            string sArrold = this.hidCustArrIDold.Value.Trim();
            string[] arr = this.hidCustArrID.Value.Trim().Split(',');
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (sArrold.IndexOf(arr[i] + ",") == -1)
                {
                    sArr += arr[i] + ",";
                    sArrold += arr[i] + ",";
                }
            }
            this.hidCustArrIDold.Value = sArrold;

            if (!string.IsNullOrEmpty(sArr))
            {
                DataTable dtProblem = Equ_RelDP.GetProblemAnalsys(sArr, EquID);

                DataTable dt = GetDetailItem2();
                dt.Merge(dtProblem);
                dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
                dgPro_ProblemAnalyse.DataBind();

                string sRefID = string.Empty;
                CreateTotle(ref sRefID);
                this.hidCustArrIDold.Value = sRefID;
            }
        }
        #endregion

        #region dgPro_ProblemAnalyse_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < 5)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region dgPro_ProblemAnalyse_ItemDataBound
        /// <summary>
        /// dgPro_ProblemAnalyse_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPro_ProblemAnalyse_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Epower.ITSM.Web.Controls.ctrFlowCataDropList CataDropList = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)e.Item.FindControl("txtRelDescription_RelDescription");
                string RelDescriptionID = DataBinder.Eval(e.Item.DataItem, "RelDescriptionID").ToString();
                if (RelDescriptionID.Trim() != "")
                {

                    try
                    {
                        CataDropList.CatelogID = long.Parse(RelDescriptionID);
                    }
                    catch
                    {

                    }
                }

                #region 给资产属性赋值
                string strEquID = e.Item.Cells[0].Text;    //资产ID
                string strEquRelID = e.Item.Cells[1].Text;    //关联资产ID
                string strEquName = e.Item.Cells[2].Text; //资产名
                string strEquRelKey = ddlRelName.SelectedItem.Text; // 视角名称
                if (ddlRelName.SelectedIndex <= 0)
                {
                    strEquRelKey = "default";// 默认视角.
                }
                //DataRowView dr = e.Item.DataItem as DataRowView;
                //(e.Item.FindControl("txtKey") as TextBox).Text = ObjectIsNull(dr["RelKey"]);
                SerEquProps(strEquID, strEquRelID, e.Item, strEquRelKey);

                e.Item.Cells[4].Text
                    = String.Format("<a href=\"frmEquRelMain.aspx?newWin=true&amp;ID={0}&ddlrelkeyindex={1}\" target=\"_blank\">{2}</a>",
                    strEquRelID,
                    ddlRelName.SelectedIndex,
                    strEquName);
                //frmEquRelMain.aspx?newWin=true&amp;ID={0}
                #endregion
            }
        }
        #endregion

        private String ObjectIsNull(Object o)
        {
            return o == null ? "" : o.ToString();
        }
    }
}
