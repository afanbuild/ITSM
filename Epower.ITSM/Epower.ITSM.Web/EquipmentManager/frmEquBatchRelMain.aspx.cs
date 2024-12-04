using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Web.Controls;
using System.Xml;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEquBatchRelMain : BasePage
    {
        #region 属性

        #region 配置项ID
        /// <summary>
        /// 配置项ID
        /// </summary>
        protected string ItemFieldID
        {
            get
            {
                if (Request["ItemFieldID"] != null && Request["ItemFieldID"] != "")
                    return Request["ItemFieldID"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region 配置项值
        /// <summary>
        /// 配置项值
        /// </summary>
        protected string ItemFieldValue
        {
            get
            {
                if (Request["ItemFieldValue"] != null && Request["ItemFieldValue"] != "")
                    return Request["ItemFieldValue"].ToString();
                else return "-1";
            }
        }
        #endregion

        #region 获取到配置项相同的资产ID字符串 EquID
        /// <summary>
        /// 
        /// </summary>
        protected string EquID
        {
            get
            {
                if (Request["ID"] != null)
                {
                    HidEqusID.Value = Request["ID"];
                    return Request["ID"].ToString();
                }
                else return "0";
            }
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

            Response.Redirect("frmEqu_SameSchemaItem.aspx?ItemFieldID=" + ItemFieldID + "&ItemFieldValue=" + ItemFieldValue);
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
                PageDeal.SetLanguage(this.Controls[0]);

                LoadData();
            }
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
            dt.Rows.Clear();
            DataRow dr;
            foreach (DataGridItem row in dgPro_ProblemAnalyse.Controls[0].Controls)
            {
                if (row.ItemType == ListItemType.Item || row.ItemType == ListItemType.AlternatingItem)
                {
                    dr = dt.NewRow();
                    //dr["Equ_ID"] = EquID.ToString();
                    dr["RelID"] = row.Cells[0].Text.ToString();
                    dr["Name"] = row.Cells[1].Text.ToString();
                    dr["RelPropID"] = ((DropDownList)row.FindControl("ddlDeskProp")).SelectedValue;
                    dr["RelPropName"] = ((DropDownList)row.FindControl("ddlDeskProp")).SelectedItem.Text;
                    dr["RelDescription"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogValue;
                    dr["RelDescriptionID"] = ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)row.FindControl("txtRelDescription_RelDescription")).CatelogID;
                    dt.Rows.Add(dr);
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
            DataTable dt = GetDetailItem();
            if (e.CommandName == "Delete")
            {
                dt.Rows.RemoveAt(e.Item.ItemIndex);
                this.hidCustArrIDold.Value = this.hidCustArrIDold.Value.Replace(e.Item.Cells[0].Text.ToString() + ",", "");
                this.hidCustArrID.Value = this.hidCustArrID.Value.Replace(e.Item.Cells[0].Text.ToString() + ",", "");
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
            DataTable dt = GetDetailItem();
            Equ_RelDP.SaveItem(dt, EquID, long.Parse(Session["UserID"].ToString()));


            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
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
            Equ_RelDP ee = new Equ_RelDP();
            string sWhere = string.Empty;
            //sWhere = " and 1 <> 1 ";
            DataTable dt = ee.GetDataTable2(EquID, string.Empty);
            Session["EquRelItemData"] = dt;
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            foreach (DataGridItem item in dgPro_ProblemAnalyse.Items)
            {
                string strEquProp = item.Cells[2].Text;                                     //关联属性ID的值 
                DropDownList ddl = (DropDownList)item.FindControl("ddlDeskProp");
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(strEquProp));
            }   

            string sRefID = string.Empty;
            CreateTotle(ref sRefID);
            this.hidCustArrIDold.Value = sRefID;
        }
        #endregion

        #region 给资产属性赋值
        /// <summary>
        /// 给资产属性赋值
        /// </summary>
        /// <param name="strEquID">资产ID</param>
        /// <param name="strEquRelID">关联资产ID</param>
        private void SerEquProps(string strEquID, string strEquRelID, DataGridItem item)
        {
            Equ_RelDP ee = new Equ_RelDP();
            string strEquPropID = string.Empty;                     //资产关联属性ID串
            string strEquPropName = string.Empty;                   //资产关联属性Name串


            //根据资产ID和关联资产ID得到关联的属性ID和Name串
            string sWhere = " and Equ_ID=" + strEquID + " and RelID=" + strEquRelID;
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
                sArrRefID += row.Cells[0].Text.Trim() + ",";
            }
        }
        #endregion

        #region 加载问题数据，并整合原有数据 LoadProblemData
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadProblemData()
        {
            string strImplyID = hidImpEquID.Value;          //影响资产ID

            DataTable dtProblem = Equ_RelDP.GetProblemAnalsys(strImplyID);

            DataTable dt = GetDetailItem();
            dt.Merge(dtProblem);
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();
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
                    if (i > 2 && i < 4)
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

                if (e.Item.Cells[0].Text != "")
                {
                    string strEquID = e.Item.Cells[0].Text;           //资产ID
                    DropDownList ddlDeskProp = ((DropDownList)e.Item.FindControl("ddlDeskProp"));

                    GetPropsByDeskID(strEquID, ddlDeskProp);                       //绑定资产属性下拉列表
                }

                #region 给资产属性赋值
                string strPropID = e.Item.Cells[2].Text;
                ((DropDownList)e.Item.FindControl("ddlDeskProp")).SelectedIndex = ((DropDownList)e.Item.FindControl("ddlDeskProp")).Items.IndexOf(((DropDownList)e.Item.FindControl("ddlDeskProp")).Items.FindByValue(strPropID));


                #endregion
            }
        }
        #endregion    

        #region 根据资产ID获取其对应的资产属性列表
        /// <summary>
        /// 根据资产ID获取其对应的资产属性列表
        /// </summary>
        /// <param name="strEquID"></param>
        private void GetPropsByDeskID(string strEquID, DropDownList ddlDeskProp)
        {
            string strSchemaXml = Equ_DeskDP.GetSchemaXmlByEquID(strEquID);         //资产对应其类别的配置项ID

            DataTable dt = new DataTable();

            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");

            //解析xml，并存放到dt中
            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                //添加基本配置
                XmlNodeList xmlList = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加关联配置
                xmlList = xmlDoc.SelectNodes("EquScheme/RelationConfig/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }

                //添加备注配置
                xmlList = xmlDoc.SelectNodes("EquScheme/Remark/AttributeItem");
                foreach (XmlNode node in xmlList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = node.Attributes["ID"].Value;
                    dr["CHName"] = node.Attributes["CHName"].Value;
                    dt.Rows.Add(dr);
                }
            }

            //绑定到关联属性下拉列表           

            ddlDeskProp.DataSource = dt.DefaultView;
            ddlDeskProp.DataValueField = "ID";
            ddlDeskProp.DataTextField = "CHName";
            ddlDeskProp.DataBind();
            ddlDeskProp.Items.Insert(0, new ListItem("", "0"));
        }
        #endregion
    }
}
