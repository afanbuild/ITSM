/****************************************************************************
 * 
 * description:设备管理主表单
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
using System.Text;

using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using System.Xml;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskMain : BasePage
    {
        #region 属性

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }

        Hashtable ht_sel = null;     //判断是否需要加载动态条件部分
        XmlNodeList nodes = null;    //存放界面初始化时，动态加载的常用条件xml串
        StringBuilder sbText = new StringBuilder();
        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                if (CtrEquCataDropList1.SelectedIndex != string.Empty)
                    return CtrEquCataDropList1.SelectedIndex.ToString();
                return CtrEquCataDropList1.CatelogID.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string FlowID
        {
            get { if (Request["FlowID"] != null) return Request["FlowID"].ToString(); else return "0"; }
        }
        #endregion

        #region 自定义项
        /// <summary>
        /// 
        /// </summary>
        protected void DeleteItem()
        {
            string sSQL = string.Empty;
            StringBuilder sb = new StringBuilder();
            sSQL = "update Equ_DeskDefineItem set Deleted=" + ((int)eRecord_Status.eDeleted).ToString() + " where 1=1 ";
            if (dgEqu_Desk.Items.Count > 0)
            {
                //批量删除
                for (int i = 0; i < dgEqu_Desk.Items.Count; i++)
                {
                    CheckBox chkDel = (CheckBox)dgEqu_Desk.Items[i].Cells[0].FindControl("chkDel");
                    if (chkDel != null && chkDel.Checked)
                    {
                        if (string.IsNullOrEmpty(sb.ToString()))
                            sb.Append(" ContractID=" + dgEqu_Desk.Items[i].Cells[1].Text.Trim());
                        else
                            sb.Append(" or ContractID=" + dgEqu_Desk.Items[i].Cells[1].Text.Trim());
                    }

                }
            }
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                sSQL += " and (" + sb.ToString() + ")";
                CommonDP.ExcuteSql(sSQL.ToString());
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.EquManager;
            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                dgEqu_Desk.Columns[10].Visible = this.Master.GetEditRight();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_ExportExcel_Click += new Global_BtnClick(Master_Master_Button_ExportExcel_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowExportExcelButton(true);
            this.Master.MainID = "1";

            dgEqu_Desk.Columns[14].Visible = false; //事件记录隐藏

            if (IsSelect)  //如果为选择时
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowExportExcelButton(false);
                this.Master.ShowBackUrlButton(true);
                dgEqu_Desk.Columns[0].Visible = false;
                dgEqu_Desk.Columns[11].Visible = false;
                dgEqu_Desk.Columns[12].Visible = false;
                dgEqu_Desk.Columns[13].Visible = false;
                dgEqu_Desk.Columns[14].Visible = false;
                dgEqu_Desk.Columns[15].Visible = false;

                dgEqu_Desk.Columns[10].HeaderText = "选择";
            }
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// Master_Master_Button_GoHistory_Click
        /// </summary>
        private void Master_Master_Button_GoHistory_Click()
        {
            Response.Write("<script>window.close();</script>");
        }
        #endregion

        #region 倒出EXCEL
        /// <summary>
        /// 倒出EXCEL事件
        /// </summary>
        void Master_Master_Button_ExportExcel_Click()
        {
            ViewState["DataTableSpec"] = "True";
            if (CtrEquCataDropList1.CatelogID.ToString().Trim() == "1")
            {
                PageTool.MsgBox(this, "请选择子类别！");
                return;
            }


            string xml = CommonDP.SelectEqu_CategoryconfigureSchema(CtrEquCataDropList1.CatelogID.ToString().Trim());
            //根据xml串获得datetable实例
            DataTable table = createTable(xml);

            DataTable dt = LoadData(CtrEquCataDropList1.CatelogID.ToString().Trim());
            ExportExcel(dt, table);
        }

        /// <summary>
        /// 导出的具体实现
        /// </summary>
        /// <param name="DataTableBase"></param>
        /// <param name="DataTableXml"></param>
        void ExportExcel(DataTable DataTableBase, DataTable DataTableXml)
        {
            try
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                hw.WriteFullBeginTag("html");
                hw.WriteLine();
                hw.WriteFullBeginTag("head");
                hw.WriteLine();
                hw.WriteLine("<meta http-equiv=Content-Type Content=text/html; charset=utf-8>");
                hw.WriteEndTag("head");
                hw.WriteLine();
                hw.WriteFullBeginTag("body");
                hw.WriteLine();

                hw.WriteLine("<table><tr><td align=center><font size=\"3\"><B>资产导出<B></font></td></tr>");
                hw.WriteLine("<tr><td>");
                TableXmlExcel(ref hw, DataTableBase, DataTableXml);
                hw.WriteLine("</td></tr>");
                hw.WriteLine("</table>");


                //this.grdTypeDirection.RenderControl(hw);

                hw.WriteEndTag("body");
                hw.WriteLine();
                hw.WriteEndTag("html");

                //输出到.xls文件
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("资产导出", System.Text.Encoding.UTF8) + ".xls");
                Response.Charset = "utf-8";
                Response.Write(tw.ToString());
                Response.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 导出数据 组织
        /// </summary>
        /// <param name="hw"></param>
        /// <param name="DataTableBase"></param>
        /// <param name="DataTableXml"></param>
        void TableXmlExcel(ref System.Web.UI.HtmlTextWriter hw, DataTable DataTableBase, DataTable DataTableXml)
        {


            Hashtable hashTB = new Hashtable();
            #region 根据xml遍历datatable数据结构
            foreach (DataRow xmlRow in DataTableXml.Rows)
            {
                DataTableBase.Columns.Add(xmlRow["CHName"].ToString());
                string[] str = new string[3];
                str[0] = xmlRow["ID"].ToString();
                str[1] = xmlRow["TypeName"].ToString();
                str[2] = xmlRow["Default"].ToString();
                hashTB.Add(xmlRow["CHName"].ToString(), str);
            }
            #endregion

            #region 取出配置表信息数据 添加到table表中
            foreach (DataRow BaseRow in DataTableBase.Rows)
            {
                List<EQU_deploy> listline = EQU_deploy.getEQU_deployList(long.Parse(BaseRow["ID"].ToString()));

                foreach (DataColumn dc in DataTableBase.Columns)
                {
                    //判断列名是在xml中存在                    
                    if (hashTB[dc.ColumnName] != null)
                    {
                        string[] str = (string[])hashTB[dc.ColumnName];
                        if (str[1].ToString() == "下拉选择")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        BaseRow[dc.ColumnName] = CatalogDP.GetCatalogNamebyID(long.Parse(deploy.Value), long.Parse(str[2].Trim()));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "部门信息")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        //获得部门名称
                                        BaseRow[dc.ColumnName] = DeptDP.GetDeptName(long.Parse(deploy.Value));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "用户信息")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "")
                                    {
                                        //获得部门名称
                                        BaseRow[dc.ColumnName] = UserDP.GetUserName(long.Parse(deploy.Value));
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    }
                                    break;
                                }
                            }
                        }
                        else if (str[1].ToString() == "关联配置")
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    if (deploy.Value.Trim() != "" && deploy.Value.Trim() == "1")
                                    {
                                        //获得部门名称
                                        BaseRow[dc.ColumnName] = "是";
                                    }
                                    else
                                    {
                                        BaseRow[dc.ColumnName] = "否";
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            IEnumerator e = listline.GetEnumerator();
                            while (e.MoveNext())
                            {
                                EQU_deploy deploy = (EQU_deploy)e.Current;
                                if (deploy.FieldID.ToString() == str[0].Trim())
                                {
                                    BaseRow[dc.ColumnName] = deploy.Value.Trim();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            #endregion


            #region 遍历导出内容的表头
            hw.WriteLine("<table width=100% border=1>");
            hw.WriteLine("<tr>");
            foreach (DataColumn dc in DataTableBase.Columns)
            {
                switch (dc.ColumnName.ToLower())
                {
                    case "name":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>资产名称</B></td>");
                        break;
                    case "code":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>资产编号</B></td>");
                        break;
                    case "costomname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>所属客户</B></td>");
                        break;
                    case "partbankname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>维护机构</B></td>");
                        break;
                    case "partbranchname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>维护部门</B></td>");
                        break;
                    case "equstatusname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>资产状态</B></td>");
                        break;
                    case "servicebegintime":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>保修开始日期</B></td>");
                        break;
                    case "serviceendtime":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>保修结束日期</B></td>");
                        break;
                    case "mastcustname":
                        hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>服务单位</B></td>");
                        break;
                    default:
                        if (hashTB[dc.ColumnName] != null)
                        {
                            hw.WriteLine("<td style='background-color:#ccccff;font-family:宋体;font-size:10pt;' align=center><B>" + dc.ColumnName + "</B></td>");
                        }
                        break;
                }
            }
            hw.WriteLine("</tr>");
            #endregion

            #region 遍历导出的数据
            foreach (DataRow BaseRow in DataTableBase.Rows)
            {
                hw.WriteLine("<tr>");
                foreach (DataColumn dc in DataTableBase.Columns)
                {
                    switch (dc.ColumnName.ToLower())
                    {
                        case "name":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "code":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "costomname":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "partbankname":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "partbranchname":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "equstatusname":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "servicebegintime":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "serviceendtime":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        case "mastcustname":
                            hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            break;
                        default:
                            if (hashTB[dc.ColumnName] != null)
                            {
                                hw.WriteLine("<td style='font-family:宋体;font-size:10pt;' align=center>" + BaseRow[dc.ColumnName].ToString().Trim() + "</td>");
                            }
                            break;
                    }
                }
                hw.WriteLine("</tr>");
            }

            hw.WriteLine("</table>");
            #endregion

        }



        //根据xml串 获得内容转化成datatable
        public DataTable createTable(string strXmlTemp)
        {
            DataTable dt = CreateNullTable();

            if (strXmlTemp != "")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(strXmlTemp);
                AddBaseSchemeDatas(xmldoc, ref dt);

                XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");
                foreach (XmlNode relnode in relnodes)
                {
                    AddRelSchemeDatas(relnode, ref dt);
                }

                relnodes = xmldoc.SelectNodes("EquScheme/Remark");
                foreach (XmlNode relnode in relnodes)
                {
                    AddRelRemarkDatas(relnode, ref dt);
                }

            }
            return dt;
        }

        //创建table实例
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("Schema");

            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");
            dt.Columns.Add("Default");
            dt.Columns.Add("IsMust");//是否必填
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");
            dt.Columns.Add("OrderBy");//排序
            dt.Columns.Add("isChack");//是否时间
            dt.Columns.Add("IsSelect");//是否设置为查询条件

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="dt"></param>
        private void AddBaseSchemeDatas(XmlDocument xmldoc, ref DataTable dt)
        {
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/BaseItem");
            string strGroup = "";
            foreach (XmlNode node in bnodes)
            {
                strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup = node.Attributes["Title"].Value;
                }
                object[] values = new object[9];

                XmlNodeList ns = node.SelectNodes("AttributeItem");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["CHName"].Value;
                    values[2] = (object)n.Attributes["Default"].Value;
                    values[3] = (object)n.Attributes["IsMust"].Value;
                    values[4] = (object)strGroup;
                    if (n.Attributes["CtrlType"].Value == "TextBox")
                    {
                        values[5] = (object)"基础信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "DropDownList")
                    {
                        values[5] = (object)"下拉选择";
                    }
                    else if (n.Attributes["CtrlType"].Value == "deptList")
                    {
                        values[5] = (object)"部门信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "UserList")
                    {
                        values[5] = (object)"用户信息";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Time")
                    {
                        values[5] = (object)"日期类型";
                    }
                    else if (n.Attributes["CtrlType"].Value == "Number")
                    {
                        values[5] = (object)"数值类型";
                    }
                    else
                    {
                        values[5] = (object)"基础信息";
                    }
                    values[6] = (object)n.Attributes["OrderBy"].Value;
                    values[7] = (object)n.Attributes["isChack"].Value;
                    values[8] = (object)n.Attributes["IsSelect"].Value;
                    dt.Rows.Add(values);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelSchemeDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "关联配置";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[9];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                dt.Rows.Add(values);

            }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        /// <param name="relnode"></param>
        /// <param name="dt"></param>
        private void AddRelRemarkDatas(XmlNode relnode, ref DataTable dt)
        {
            XmlNodeList ns = null;

            string strGroup = "";
            string strTypeName = "备注信息";

            if (relnode.Attributes["Title"] != null)
            {
                strGroup = relnode.Attributes["Title"].Value;
            }
            ns = relnode.SelectNodes("AttributeItem");
            object[] values = new object[9];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)n.Attributes["IsMust"].Value;
                values[4] = (object)strGroup;
                values[5] = (object)strTypeName;
                values[6] = (object)n.Attributes["OrderBy"].Value;
                values[7] = (object)n.Attributes["isChack"].Value;
                values[8] = (object)n.Attributes["IsSelect"].Value;
                dt.Rows.Add(values);

            }
        }

        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskEdit.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 查询事件
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            //加载动态条件部分
            ht_sel = new Hashtable();
            ValueCollection(Table3, ref ht_sel);

            Bind("");
        }
        #endregion

        #region 递归获取控件的值
        /// <summary>
        /// 递归获取控件的值
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="list"></param>
        private void ValueCollection(Control ctrRoot, ref Hashtable list)
        {
            string strType = string.Empty;              //控件类型
            foreach (Control pControl in ctrRoot.Controls)
            {
                if (pControl.ID != null)
                {
                    if (pControl.ID.StartsWith("tDynamic"))
                    {
                        strType = pControl.GetType().Name;      //控件类型
                        switch (strType)
                        {
                            case "HtmlTableRow":
                            case "HtmlTableCell":
                                ValueCollection(pControl, ref list);
                                break;
                            case "TextBox":
                                //文本、数值
                                list.Add(((TextBox)pControl).Attributes["Tag"], ((TextBox)pControl).Text);
                                break;
                            case "controls_ctrdateandtime_ascx":
                                //日期
                                list.Add(((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.CtrDateAndTime)pControl).dateTimeString);
                                break;
                            case "controls_ctrflowcatadroplist_ascx":
                                //下拉
                                list.Add(((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).CatelogID);
                                break;
                            case "controls_deptpicker_ascx":
                                //部门
                                list.Add(((Epower.ITSM.Web.Controls.DeptPicker)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.DeptPicker)pControl).DeptID);
                                break;
                            case "controls_userpicker_ascx":
                                //用户
                                list.Add(((Epower.ITSM.Web.Controls.UserPicker)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.UserPicker)pControl).UserID);
                                break;
                            case "":
                                list.Add(((CheckBox)pControl).Attributes["Tag"], ((CheckBox)pControl).Checked == true ? "1" : "0");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// Master_Master_Button_Delete_Click
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            string strID = string.Empty;                //保存所有要删除的资产ID组合
            Equ_DeskDP ee = new Equ_DeskDP();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    string strName = itm.Cells[2].Text;         //资产名称
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        strID += sID + ",";
                    }
                }
            }

            strID = strID.Substring(0, strID.Length - 1);           //去掉ID组合字符串最后一个逗号

            ee.DeleteRecorded(strID);

            //删除自定义项
            DeleteItem();

            Bind("");
        }
        #endregion

        #region 得到删除前的提示语言
        /// <summary>
        /// 得到删除前的提示语言 
        /// </summary>
        /// <returns></returns>
        public string GetTipsAndEquIDs()
        {
            string strTip = string.Empty;               //删除时的提示信息
            Equ_DeskDP ee = new Equ_DeskDP();
            foreach (DataGridItem itm in dgEqu_Desk.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    string strName = itm.Cells[2].Text;         //资产名称
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        string strEqus = Equ_DeskDP.GetEquNamesByEquID(sID);        //根据资产ID得到其所影响的资产名称组合
                        strEqus = strEqus.Substring(0, strEqus.Length - 1);         //将最后一个逗号去掉
                        if (strEqus != "")
                        {
                            //如果有影响的资产，则给出提示；是，则删；否则不删
                            strTip += strName + "的删除会影响到:" + strEqus + ";";
                        }
                    }
                }
            }

            strTip += "您确认要删除这些资产吗?";
            return strTip;
        }
        #endregion

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpfECustomerInfo.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(bindGrid);
            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }

                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);
               
                SetHeaderText();

                InitDropDownList("-1");         //初始化根类别

                //if (CommonDP.GetConfigValue("Other", "ItsmCustomerMode") != "0")
                //{
                //绑定服务单位
                string sMastCust = "";
                if (Request["MastCust"] != null)
                    sMastCust = Request["MastCust"].ToString();
                InitMCDropDownList(sMastCust);

                if (Request["EquName"] != null)
                    txtName.Text = Request["EquName"].ToString();
                //if (Request["Cust"] != null)
                //    txtCust.Text = Request["Cust"].ToString();

                if (Request["EquipmentCatalogID"] != null)
                    hidEquipmentCatalogID.Value = Request["EquipmentCatalogID"].ToString() == "" ? "0" : Request["EquipmentCatalogID"].ToString();

                if (Request["subjectid"] != null && Request["subjectid"] != "-1" && Request["subjectid"] != "1")
                {
                    CtrEquCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                    InitDropDownList(Request["subjectid"].ToString());
                    Bind(Request["subjectid"].ToString());
                }
                else
                {
                    InitDropDownList("1");          //初始化根类别
                    Bind("");
                }
            }

            LoadHtmlContl();                //动态加载常用条件
        }
        #endregion


        #region 设置datagrid标头显示 余向前 2013-05-17
        /// <summary>
        /// 设置datagrid标头显示
        /// </summary>
        private void SetHeaderText()
        {
            dgEqu_Desk.Columns[2].HeaderText = PageDeal.GetLanguageValue("Equ_DeskName");
            dgEqu_Desk.Columns[3].HeaderText = PageDeal.GetLanguageValue("Equ_Code");
            dgEqu_Desk.Columns[4].HeaderText = PageDeal.GetLanguageValue("Equ_CostomName");
            dgEqu_Desk.Columns[5].HeaderText = PageDeal.GetLanguageValue("Equ_CatalogName");
            dgEqu_Desk.Columns[6].HeaderText = PageDeal.GetLanguageValue("Equ_SerialNumber");
            dgEqu_Desk.Columns[7].HeaderText = PageDeal.GetLanguageValue("Equ_CatalogName");
            dgEqu_Desk.Columns[8].HeaderText = PageDeal.GetLanguageValue("Equ_Breed");
            dgEqu_Desk.Columns[9].HeaderText = PageDeal.GetLanguageValue("Equ_Model");
        }
        #endregion

        #region 翻页绑定dagagrid
        /// <summary>
        /// 
        /// </summary>
        public void bindGrid()
        {
            Bind("");
        }
        #endregion

        #region InitMCDropDownList
        /// <summary>
        /// 绑定服务单位
        /// </summary>
        /// <param name="sMastCust"></param>
        private void InitMCDropDownList(string sMastCust)
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            string sWhere = string.Empty;
            string sOrder = string.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddltMastCustID.DataSource = dt;
            ddltMastCustID.DataTextField = "ShortName";
            ddltMastCustID.DataValueField = "ID";
            ddltMastCustID.DataBind();
            ddltMastCustID.Items.Insert(0, new ListItem("", ""));
            if (sMastCust.Length > 0)
                ddltMastCustID.SelectedIndex = ddltMastCustID.Items.IndexOf(ddltMastCustID.Items.FindByText(sMastCust));
        }
        #endregion

        #region InitDropDownList
        /// <summary>
        /// 绑定配置项
        /// </summary>
        private void InitDropDownList(string strCatalogID)
        {
            #region 获取配置项

            string strSchemaXml = string.Empty;                                             //配置项xml            
            DataTable dt = Equ_SubjectDP.GetSubjectByID(long.Parse(strCatalogID));          //得到此类别下的配置项xml
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
                }
            }

            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();

            #endregion

            #region 基本配置项
            string sWhere = " AND itemtype = 0 ";
            string sOrder = string.Empty;
            string strFieldID = "(0";                                                       //存放资产类别下各基本配置项FieldID

            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");           //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemJB.DataSource = dt;
            ddlSchemaItemJB.DataTextField = "CHName";
            ddlSchemaItemJB.DataValueField = "FieldID";
            ddlSchemaItemJB.DataBind();
            ddlSchemaItemJB.Items.Insert(0, new ListItem("", ""));
            #endregion

            #region 关联配置项
            sWhere = " AND itemtype = 1 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //存放资产类别下各基本配置项FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/RelationConfig/AttributeItem");     //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemGL.DataSource = dt;
            ddlSchemaItemGL.DataTextField = "CHName";
            ddlSchemaItemGL.DataValueField = "FieldID";
            ddlSchemaItemGL.DataBind();
            ddlSchemaItemGL.Items.Insert(0, new ListItem("", ""));
            #endregion

            #region 下拉配置项
            sWhere = " AND itemtype = 3 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //存放资产类别下各基本配置项FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlPulldownItem.DataSource = dt;
            ddlPulldownItem.DataTextField = "CHName";
            ddlPulldownItem.DataValueField = "FieldID";
            ddlPulldownItem.DataBind();
            ddlPulldownItem.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region 日期配置项
            sWhere = " AND itemtype = 6 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //存放资产类别下各基本配置项FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemDT.DataSource = dt;
            ddlSchemaItemDT.DataTextField = "CHName";
            ddlSchemaItemDT.DataValueField = "FieldID";
            ddlSchemaItemDT.DataBind();
            ddlSchemaItemDT.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region 部门配置项
            sWhere = " AND itemtype = 4 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //存放资产类别下各基本配置项FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemDP.DataSource = dt;
            ddlSchemaItemDP.DataTextField = "CHName";
            ddlSchemaItemDP.DataValueField = "FieldID";
            ddlSchemaItemDP.DataBind();
            ddlSchemaItemDP.Items.Insert(0, new ListItem("", ""));

            #endregion

            #region 用户配置项
            sWhere = " AND itemtype = 5 ";
            sOrder = string.Empty;
            strFieldID = "(0";                                                                                 //存放资产类别下各基本配置项FieldID     
            strFieldID = strFieldID + AnalySchema(strSchemaXml, "EquScheme/BaseItem/AttributeItem");     //解析基本配置项xml，得到各FieldID的集合
            sWhere += " AND FieldID in " + strFieldID + ")";                                                   //加上筛选条件

            dt = ee.GetDataTable(sWhere, sOrder);
            ddlSchemaItemUS.DataSource = dt;
            ddlSchemaItemUS.DataTextField = "CHName";
            ddlSchemaItemUS.DataValueField = "FieldID";
            ddlSchemaItemUS.DataBind();
            ddlSchemaItemUS.Items.Insert(0, new ListItem("", ""));

            #endregion
        }
        #endregion

        #region 初始化动态查询条件
        /// <summary>
        /// 初始化动态查询条件
        /// </summary>
        private void LoadHtmlContl()
        {
            string strSchemaXml = string.Empty;                                             //配置项xml            
            DataTable dt = Equ_SubjectDP.GetSubjectByID(long.Parse(CatalogID));          //得到此类别下的配置项xml
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    strSchemaXml = dt.Rows[0]["ConfigureSchema"].ToString();
                }
            }

            #region 初始化常用查询条件
            if (strSchemaXml != string.Empty && IsSelect == false)
            {
                //如果没有扩展项，则没有高级条件及常用条件
                Table13.Visible = true;
                Table12.Visible = true;

                LoadHtmlControls(strSchemaXml);
            }
            else
            {
                Table13.Visible = false;
                Table12.Visible = false;
            }
            #endregion
        }
        #endregion

        #region 动态加载查询条件
        /// <summary>
        /// 动态加载查询条件
        /// </summary>
        /// <param name="strSchemaXml"></param>
        private void LoadHtmlControls(string strSchemaXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strSchemaXml);

            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();

            nodes = xmlDoc.SelectNodes("EquScheme/BaseItem/AttributeItem");     //基本配置项
            RepeatCataItems(ref tr, ref tc, nodes);
        }
        #endregion

        #region 循环遍历各类配置项
        /// <summary>
        /// 循环遍历各类配置项
        /// </summary>
        private void RepeatCataItems(ref HtmlTableRow tr, ref HtmlTableCell tc, XmlNodeList nodes)
        {
            int iCount = 0;         //计算常用条件的个数，若没有，则不展示常用条件
            int iRow = 0;
            int iCell = 0;

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["IsSelect"].Value == "1")
                {
                    iCount++;
                    //如果配置项的查询属性为真，则循环添加到查询条件中
                    if (iCell == 0)
                    {
                        //如果iCell为1，需要新添加行
                        iRow++;
                        tr = new HtmlTableRow();
                        tr.ID = "tDynamicRow" + iRow;
                    }

                    //依次遍历各结点，添加查询条件
                    tc = AddControlByNode(node, ref tr, ref tc, ref iRow, ref iCell);
                }
            }

            if (iCell == 1)
            {
                //如果td数量为1，表明最后一个查询条件行只有一个；此时，要将最后一个td设置占3个列
                tc.Attributes.Add("colspan", "3");
                AddControl(Table3, tr);
            }

            if (iCount == 0)
            {
                //如果没有常用条件，则不展示常用条件部分
                Table13.Visible = false;
            }
            else
            {
                Table13.Visible = true;
            }
        }
        #endregion

        #region 依次遍历各结点，添加查询条件
        /// <summary>
        /// 依次遍历各结点，添加查询条件
        /// </summary>
        private HtmlTableCell AddControlByNode(XmlNode node, ref HtmlTableRow ctrl, ref HtmlTableCell _tc, ref int iRow, ref int iCell)
        {
            iCell++;                //td数量递增

            //第一个td，标题
            HtmlTableCell tc = new HtmlTableCell();
            tc.ID = "tTitleDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("class", "listTitleRight");
            tc.Attributes.Add("style", "width:12%");
            tc.InnerText = node.Attributes["CHName"].Value;

            AddControl(ctrl, tc);           //添加第一个td到tr中

            //第二个td，内容
            tc = new HtmlTableCell();
            tc.ID = "tDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("class", "list");
            if (iCell == 2)
            {
                //第四个td,不设置宽度
            }
            else
            {
                tc.Attributes.Add("style", "width:35%");
            }

            Control control = GetSettingControl(node);
            if (control != null)
            {
                AddControl(tc, control);
            }
            AddControl(ctrl, tc);           //添加第二个td【第一个txt文本】到tr中

            if (iCell == 2)
            {
                //如果iCell为2，表明一行已经循环结束，需要换行了
                iCell = 0;
                AddControl(Table3, ctrl);
            }

            return tc;
        }
        #endregion

        #region 获取设置的控件
        /// <summary>
        /// 获取设置的控件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="strControlType"></param>
        /// <returns></returns>
        private Control GetSettingControl(XmlNode node)
        {
            string strContrType = node.Attributes["CtrlType"].Value;

            Control control;
            switch (strContrType)
            {
                case "MultiLine":
                //备注
                case "TextBox":
                    //文本框
                    control = new TextBox();
                    control.ID = "tDynamic" + "_txt_" + node.Attributes["ID"].Value;
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "Time":
                    //日期
                    control = (Epower.ITSM.Web.Controls.CtrDateAndTime)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                    control.ID = "tDynamic" + "_DateTime_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).ShowTime = false;
                    ((Epower.ITSM.Web.Controls.CtrDateAndTime)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "DropDownList":
                    //下拉
                    control = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)LoadControl("~/Controls/ctrFlowCataDropList.ascx");
                    control.ID = "tDynamic" + "_cataddl_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).RootID = long.Parse(node.Attributes["Default"].Value == "" ? "0" : node.Attributes["Default"].Value);
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "Number":
                    //数值
                    control = new TextBox();
                    control.ID = "tDynamic" + "_Num_" + node.Attributes["ID"].Value;
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "deptList":
                    //部门
                    control = (Epower.ITSM.Web.Controls.DeptPicker)LoadControl("~/Controls/DeptPicker.ascx");
                    control.ID = "tDynamic" + "_Dept_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.DeptPicker)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "UserList":
                    //用户
                    control = (Epower.ITSM.Web.Controls.UserPicker)LoadControl("~/Controls/UserPicker.ascx");
                    control.ID = "tDynamic" + "_User_" + node.Attributes["ID"].Value;
                    ((Epower.ITSM.Web.Controls.UserPicker)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "":
                    //关联
                    control = new CheckBox();
                    control.ID = "tDynamic" + "_Chb_" + node.Attributes["ID"].Value;
                    ((CheckBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                default:
                    control = null;
                    break;
            }
            return control;
        }
        #endregion

        #region 添加控件td和控件
        /// <summary>
        /// 添加控件td和控件
        /// </summary>
        /// <param name="pParentControl">父控件</param>
        /// <param name="pSubControl">子控件</param>
        private void AddControl(Control pParentControl, Control pSubControl)
        {
            if (pParentControl != null && pSubControl != null)
            {
                bool bFound = false;
                foreach (Control contrl in pParentControl.Controls)
                {
                    if (contrl.ID == pSubControl.ID)
                    {
                        bFound = true;
                        break;
                    }
                }
                if (!bFound)
                {
                    //如果没找到，则添加 
                    pParentControl.Controls.Add(pSubControl);
                }
            }
        }
        #endregion

        #region 解析配置项xml
        /// <summary>
        /// 解析配置项xml
        /// </summary>
        private string AnalySchema(string strSchemaXml, string strLists)
        {
            string strFieldID = string.Empty;

            if (strSchemaXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strSchemaXml);

                XmlNodeList bnodes = xmlDoc.SelectNodes(strLists);
                foreach (XmlNode node in bnodes)
                {
                    strFieldID += "," + node.Attributes["ID"].Value;
                }
            }
            return strFieldID;
        }
        #endregion

        #region Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind(string Strsubjectid)
        {
            #region 定义变更

            int iRowCount = 0;
            string sCalalogID = string.Empty;
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            #endregion

            #region 类别

            if (Strsubjectid != "")
            {
                sCalalogID = Strsubjectid;
                CtrEquCataDropList1.SelectedIndex = sCalalogID;
            }
            else
            {
                sCalalogID = CtrEquCataDropList1.CatelogID.ToString();
            }

            #endregion

            #region 动态条件部分

            if (ht_sel != null && nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["IsSelect"].Value == "1")
                    {
                        //如果在查询条件中
                        string strID = node.Attributes["ID"].Value;             //配置项ID
                        string strName = node.Attributes["CHName"].Value;       //配置项名称
                        string strType = node.Attributes["CtrlType"].Value;     //配置项类型
                        string strValue = string.Empty;                         //配置项的值
                        strValue = ht_sel[strID].ToString().Trim();

                        if (strType == "Time")
                        {
                            ////如果为日期类型，则将字符串转成日期类型
                            if (strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = Convert(Datetime," + StringTool.SqlQ(strValue) + "))";
                        }
                        if (strType == "deptList" || strType == "UserList")
                        {
                            //如果为部门、用户
                            if (strValue != "0" && strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                        }
                        else if (strType == "DropDownList")
                        {
                            //如果为下拉
                            string strRoot = node.Attributes["Default"].Value;          //根分类ID
                            if (strValue != strRoot && strValue != "0" && strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                            + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                        }
                        else
                        {
                            if (strValue != "")
                                sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value like " + StringTool.SqlQ("%" + strValue + "%") + ")";
                        }
                    }
                }
            }

            #endregion

            #region 固定条件部分

            #region 扩展配置项

            //基本配置项
            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            //关联配置项
            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            //下拉配置项
            if (ddlPulldownItem.SelectedItem.Text.Trim() != string.Empty && ddlPulldownValue.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlPulldownItem.SelectedValue.Trim(), ddlPulldownValue.SelectedValue.Trim());
            }

            //日期配置项
            if (ddlSchemaItemDT.SelectedItem.Text.Trim() != string.Empty && dtServiceTime.dateTimeString != "")
            {
                ht.Add(ddlSchemaItemDT.SelectedValue.Trim(), dtServiceTime.dateTime.ToShortDateString().Trim());
            }

            //部门配置项
            if (ddlSchemaItemDP.SelectedItem.Text.Trim() != string.Empty && DeptPicker1.DeptID != 0)
            {
                ht.Add(ddlSchemaItemDP.SelectedValue.Trim(), DeptPicker1.DeptID.ToString());
            }

            //用户配置项
            if (ddlSchemaItemUS.SelectedItem.Text.Trim() != string.Empty && UserPicker1.UserID != 0)
            {
                ht.Add(ddlSchemaItemUS.SelectedValue.Trim(), UserPicker1.UserID.ToString());
            }
            #endregion

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12分两种情况  1 是包含客户信息 或 服务单位 的用一种查询,没有包含客户信息的用另一种查询,
            //if (ShowMastCust.Visible == true)
            //{
            if (ddltMastCustID.SelectedItem != null && ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And MastCustID=" + ddltMastCustID.SelectedValue.Trim();
                blnSpec = true;
            }
            //}
            if (txtCust.Text.Trim() != string.Empty)
            {
                //英文名
                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //中文名
                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //工号
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //电话
                sWhere += " OR nvl(Tel1,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //EMAIL
                sWhere += " OR nvl(Email,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");

                sWhere += ")";

                blnSpec = true;
            }

            if (blnSpec == true)
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }
                //维护机构
                if (DeptPickerbank2.DeptID > 0)
                    sWhere += " and partBankId=" + DeptPickerbank2.DeptID;


                if (decimal.Parse(sCalalogID) > 0)
                {
                    if (chkIncludeSub.Checked == true)
                    {
                        if (sCalalogID != "1")
                        {
                            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                            sWhere += " and CatalogID in (select CatalogID from Equ_Category where FullID like" + StringTool.SqlQ(strFullID + "%") + ")";
                        }
                    }
                    else
                    {
                        sWhere += " and CatalogID = " + sCalalogID;
                    }
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }
            else
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }
                //维护机构
                if (DeptPickerbank2.DeptID > 0)
                    sWhere += " and partBankId=" + DeptPickerbank2.DeptID;

                if (decimal.Parse(sCalalogID) > 0)
                {
                    if (chkIncludeSub.Checked == true)
                    {
                        if (sCalalogID != "1")
                        {
                            string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                            sWhere += " and CatalogID in (select CatalogID from Equ_Category where FullID like" + StringTool.SqlQ(strFullID + "%") + ")";
                        }
                    }
                    else
                    {
                        sWhere += " and CatalogID = " + sCalalogID;
                    }
                }

                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
            }

            #endregion

            dgEqu_Desk.DataSource = dt;
            dgEqu_Desk.DataBind();
            this.cpfECustomerInfo.RecordCount = iRowCount;
            this.cpfECustomerInfo.Bind();
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private DataTable LoadData(string Strsubjectid)
        {
            int iRowCount = 0;
            string sCalalogID = string.Empty;
            if (Strsubjectid != "")
            {
                sCalalogID = Strsubjectid;
            }
            else
            {
                sCalalogID = CtrEquCataDropList1.CatelogID.ToString();
            }
            DataTable dt;
            string sWhere = " 1=1 ";
            string sOrder = " order by ID Desc ";
            Hashtable ht = new Hashtable();

            bool blnSpec = false;

            #region 动态条件部分

            if (ht_sel != null)
            {
                foreach (XmlNode node in nodes)
                {
                    string strID = node.Attributes["ID"].Value;             //配置项ID
                    string strName = node.Attributes["CHName"].Value;       //配置项名称
                    string strType = node.Attributes["CtrlType"].Value;     //配置项类型
                    string strValue = string.Empty;                         //配置项的值
                    strValue = ht[strID].ToString();

                    if (strType == "Time")
                    {
                        ////如果为日期类型，则将字符串转成日期类型
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value = Convert(Datetime," + StringTool.SqlQ(strValue) + "))";
                    }
                    if (strType == "deptList" || strType == "DropDownList" || strType == "UserList")
                    {
                        //如果为下拉、部门、用户
                        sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                                                    + " and CHName = " + StringTool.SqlQ(strName) + " and Value = " + strValue + ")";
                    }
                    else
                    {
                        if (strValue != "")
                            sWhere += " and ID in (select EquID from EQU_deploy where FieldID = " + StringTool.SqlQ(strID)
                                + " and CHName = " + StringTool.SqlQ(strName) + " and Value like " + StringTool.SqlQ("%" + strValue + "%") + ")";
                    }
                }
            }

            #endregion

            #region 扩展配置项

            //基本配置项
            if (ddlSchemaItemJB.SelectedItem.Text.Trim() != string.Empty && txtItemValue.Text.Trim().Length > 0)
            {
                ht.Add(ddlSchemaItemJB.SelectedValue.Trim(), txtItemValue.Text.Trim());
            }

            //关联配置项
            if (ddlSchemaItemGL.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlSchemaItemGL.SelectedValue.Trim(), (chkItemValue.Checked == true ? "1" : "0"));
            }

            //下拉配置项
            if (ddlPulldownItem.SelectedItem.Text.Trim() != string.Empty && ddlPulldownValue.SelectedItem.Text.Trim() != string.Empty)
            {
                ht.Add(ddlPulldownItem.SelectedValue.Trim(), ddlPulldownValue.SelectedValue.Trim());
            }

            //日期配置项
            if (ddlSchemaItemDT.SelectedItem.Text.Trim() != string.Empty && dtServiceTime.dateTimeString != "")
            {
                ht.Add(ddlSchemaItemDT.SelectedValue.Trim(), dtServiceTime.dateTime.ToShortDateString().Trim());
            }

            //部门配置项
            if (ddlSchemaItemDP.SelectedItem.Text.Trim() != string.Empty && DeptPicker1.DeptID != 0)
            {
                ht.Add(ddlSchemaItemDP.SelectedValue.Trim(), DeptPicker1.DeptID.ToString());
            }

            //用户配置项
            if (ddlSchemaItemUS.SelectedItem.Text.Trim() != string.Empty && UserPicker1.UserID != 0)
            {
                ht.Add(ddlSchemaItemUS.SelectedValue.Trim(), UserPicker1.UserID.ToString());
            }
            #endregion

            Equ_DeskDP ee = new Equ_DeskDP();

            //2008-05-12分两种情况  1 是包含客户信息 或 服务单位 的用一种查询,没有包含客户信息的用另一种查询,
            //if (ShowMastCust.Visible == true)
            //{
            if (ddltMastCustID.SelectedItem != null && ddltMastCustID.SelectedItem.Text.Trim() != string.Empty)
            {
                sWhere += " And MastCustID=" + ddltMastCustID.SelectedValue.Trim();
                blnSpec = true;
            }
            //}

            //客户信息
            if (txtCust.Text.Trim() != string.Empty)
            {
                //英文名
                sWhere += " And (nvl(FullName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //中文名
                sWhere += " OR nvl(ShortName,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //工号
                sWhere += " OR nvl(CustomCode,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //电话
                sWhere += " OR nvl(Tel1,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");
                //EMAIL
                sWhere += " OR nvl(Email,'') like " + StringTool.SqlQ("%" + txtCust.Text.Trim() + "%");

                sWhere += ")";

                blnSpec = true;
            }

            if (blnSpec == true)
            {
                //资产名称
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                //资产编号
                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if (sCalalogID == "1" || sCalalogID == "-1")
                { }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                //资产状态
                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                if (ViewState["DataTableSpec"] != null && ViewState["DataTableSpec"].ToString() == "True")
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, 1000000000, 1, ref iRowCount);
                    ViewState["DataTableSpec"] = null;
                }
                else
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
                }
            }
            else
            {
                if (txtName.Text.Trim() != string.Empty)
                {
                    sWhere += " And Name like " + StringTool.SqlQ("%" + txtName.Text.Trim() + "%");
                }

                if (txtCode.Text.Trim() != string.Empty)
                {
                    sWhere += " And Code like " + StringTool.SqlQ("%" + txtCode.Text.Trim() + "%");
                }

                if (sCalalogID == "")
                {
                    sCalalogID = "-1";

                }
                string strFullID = Equ_SubjectDP.GetSubjectFullID(long.Parse(sCalalogID));
                if (sCalalogID == "1" || sCalalogID == "-1")
                { }
                else
                {
                    sWhere += " And FullID = " + StringTool.SqlQ(strFullID);
                }

                //资产状态
                if (!string.IsNullOrEmpty(ctrCataEquStatus.CatelogValue.Trim()))
                    sWhere += " and EquStatusID= " + ctrCataEquStatus.CatelogID.ToString();

                if (ViewState["DataTableSpec"] != null && ViewState["DataTableSpec"].ToString() == "True")
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, 1000000000, 1, ref iRowCount);
                    ViewState["DataTableSpec"] = null;
                }
                else
                {
                    dt = ee.GetDataTableSpec(sWhere, sOrder, ht, this.cpfECustomerInfo.PageSize, this.cpfECustomerInfo.CurrentPage, ref iRowCount);
                }
            }
            return dt;
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
            Bind("");
        }
        #endregion

        #region  dgEqu_Desk_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {            
                if (!IsSelect)
                    Response.Redirect("frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
                else
                {
                    string id = e.Item.Cells[1].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + id;

                    Equ_DeskDP ec = new Equ_DeskDP();
                    DataTable dt = ec.GetDataTable(sWhere, "");
                   
                     sbText.Append("<script>");
                     string requestType = "";
                     if (Request["TypeFrm"] != null) 
                     {
                         requestType = Request.QueryString["TypeFrm"].ToString();
                     }
                     else if (Request["TypeFrom"] != null) {
                         requestType = Request.QueryString["TypeFrom"].ToString();
                     }

                    switch (requestType)
                    {
                        case "frm_Change_AdvancedCondition":
                            AdvancedCondition(dt);                          
                            break;
                        case "frm_ChangeBase":
                            ChangeBase(dt);
                            break;
                        case "DemandBase":
                            DemandBase(dt);
                            break;
                    }
                    sbText.Append("window.close();");


                    //// 关闭窗口
                    //sbText.Append("top.close();");
                    sbText.Append("</script>");
                    // 向客户端发送
                    Page.RegisterStartupScript(DateTime.Now.Ticks.ToString(), sbText.ToString());
                    //Response.Write(sbText.ToString());
                }
            }
            if (e.CommandName == "look")
            {
                if (IsSelect)
                {
                    Response.Redirect("frmEqu_DeskEdit.aspx?IsSelect=1&id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID);
                }
                else
                {
                    Response.Redirect("frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID);
                }
            }
            if (e.CommandName == "link")
            {
                Response.Redirect("frmEquRelMain.aspx?ID=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID);
            }
        }

        private void AdvancedCondition(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEquipmentName", dt.Rows[0]["name"].ToString());   //设备名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["name"].ToString());     //设备名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["id"].ToString()); 
        }

        private void ChangeBase(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddID", dt.Rows[0]["id"].ToString());
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtAddEquName", dt.Rows[0]["name"].ToString());     //设备名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddEquName", dt.Rows[0]["name"].ToString());     //
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "lblAddCode", dt.Rows[0]["code"].ToString());     //
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidAddCode", dt.Rows[0]["code"].ToString());     //资产目录名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "lblDept", dt.Rows[0]["partbranchname"].ToString());     //
                    
        }       
        #endregion

        #region dgEqu_Desk_ItemDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button lnkedit = (Button)e.Item.FindControl("lnkedit");
                LinkButton lnkEquName = (LinkButton)e.Item.FindControl("lnkEquName");
                Label lblEquName = (Label)e.Item.FindControl("lblEquName");
                if (IsSelect)
                {
                    lnkedit.Text = "选择";
                    lnkEquName.Visible = false;
                    lblEquName.Visible = true;
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                if (!IsSelect)
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_DeskEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&subjectid=" + CatalogID + "&randomid='+GetRandom(),'subjectinfo','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
                }
                else
                {
                    string id = e.Item.Cells[1].Text.Replace("&nbsp;", "");
                    string sWhere = " And id = " + id;

                    Equ_DeskDP ec = new Equ_DeskDP();
                    DataTable dt = ec.GetDataTable(sWhere, "");
                    Json json = new Json(dt);

                    string jsonstr = "{record:" + json.ToJson() + "}";
                    //这里是更新代码
                    string requestType="";
                    if (Request["TypeFrm"] != null)
                    {
                        requestType = Request.QueryString["TypeFrm"].ToString();
                    }

                    switch (requestType)
                    {
                        case "CST_Issue_Base":
                           // AdvancedCondition(dt);
                            e.Item.Attributes.Add("onclick", "ServerOndblclick_CST_Issue_Base("+jsonstr+")");
                            break;
                        case "frm_ChangeBase":
                            e.Item.Attributes.Add("onclick", "ServerOndblclickChangeBase("+jsonstr+")");
                            //ChangeBase(dt);
                            break;
                        case "frmProblemSolve":
                            e.Item.Attributes.Add("onclick", "ServerOndblclickfrmProblemSolve("+jsonstr+")");
                            break;
                        case "CST_Issue_Base_Fast":
                            lnkedit.Attributes.Add("onclick", "cst_issue_Base_fastServe(" + jsonstr + ")");
                           // e.Item.Attributes.Add("onclick","cst_issue_Base_fastServe("+jsonstr+")");
                            break;
                        case "frm_Change_AdvancedCondition":
                            lnkedit.Attributes.Add("onclick", "ServerAdvancedCondition(" + jsonstr + ")");
                            break;
                        case "ServerCst_Issue":
                            lnkedit.Attributes.Add("onclick","ServerCst_Issue_Serivce("+jsonstr+");");
                            break;
                        case "CST_Issue_Base_self":
                            lnkedit.Attributes.Add("onclick", "CTS_Issue_Base_Self("+jsonstr+")");
                            break;
                        case "DemandBase":
                            lnkedit.Attributes.Add("onclick", "DemandBase(" + jsonstr + ")");
                            break;
                        default:
                            // 向客户端发送
                            e.Item.Attributes.Add("onclick", "ServerOndblclick(" + jsonstr + ");");
                            break;

                    }
                  
                   

                }

            }

        }
        #endregion

        #region 点击资产名称时，弹出资产详情界面
        /// <summary>
        /// 点击资产名称时，弹出资产详情界面
        /// </summary>
        /// <param name="decEquID"></param>
        protected string GetEquDetail(decimal decEquID)
        {
            string strUrl = string.Empty;

            strUrl = "javascript:openobj.open('" + "frmEqu_DeskEdit.aspx?IsTanChu=true&IsSelect=1&id=" + decEquID.ToString() + "&subjectid=" + CatalogID + "&FlowID=" + FlowID + "','','scrollbars=yes,resizable=yes');event.returnValue = false;";

            return strUrl;
        }
        #endregion

        #region dgEqu_Desk_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_Desk_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 1 && i < 8)
                    {
                        if (IsSelect)
                            j = i - 2;
                        else
                            j = i - 1;
                        if (i == 7)
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                        }
                        else
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                }
            }
        }
        #endregion

        #region 事件记录
        /// <summary>
        /// 事件记录
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetServiceUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('../AppForms/frmIssueList.aspx?NewWin=true&ID=0&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion

        #region 巡检历史
        /// <summary>
        /// 巡检历史
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngID)
        {
            //暂时没处理分页
            string sUrl = "";
            sUrl = "javascript:window.open('frm_Equ_PatrolList.aspx?NewWin=true&EquID=" + lngID.ToString() + "','newwindow','scrollbars=yes,resizable=yes,top=0,left=0,width=800px,height=600');";
            return sUrl;
        }
        #endregion

        #region 下拉配置项名称改变时，改变对应的绑定下拉内容
        /// <summary>
        /// 下拉配置项名称改变时，改变对应的绑定下拉内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPulldownItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //下拉配置项FieldID
            string strItemID = ddlPulldownItem.SelectedItem.Value;

            //根据类别ID获取对应的绑定内容
            DataTable dt = CommonDP.GetCatasByParentID(CatalogID, strItemID);
            ddlPulldownValue.DataSource = dt;
            ddlPulldownValue.DataTextField = "CatalogName";
            ddlPulldownValue.DataValueField = "CatalogID";
            ddlPulldownValue.DataBind();
            ddlPulldownValue.Items.Insert(0, new ListItem("", ""));
        }
        #endregion

        #region 需求管理页面选择资产 余向前 2013-04-27
        /// <summary>
        /// 需求管理页面选择资产
        /// </summary>
        /// <param name="dt"></param>
        private void DemandBase(DataTable dt)
        {
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "txtEqu", dt.Rows[0]["name"].ToString());   //资产名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEquName", dt.Rows[0]["name"].ToString());     //资产名称
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidEqu", dt.Rows[0]["id"].ToString());    //资产ID

            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListName", dt.Rows[0]["listname"].ToString());     //资产目录
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidListID", dt.Rows[0]["listid"].ToString());     //资产目录ID
        }
        #endregion
    }
}

