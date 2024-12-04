/*******************************************************************
 *
 * Description:配置项管理
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月5日
 * *****************************************************************/
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
using System.Xml;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_SchemaItemsMain : BasePage
    {
        #region 属性
        /// <summary>
        /// 是否选择状态
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }


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
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.SchemaItemsMain;
            if (IsSelect)
                this.Master.IsCheckRight = false;
            else
            {
                this.Master.IsCheckRight = true;
                dgEqu_SchemaItems.Columns[5].Visible = this.Master.GetEditRight();
            }
           
         
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.ShowQueryPageButton();
            this.Master.MainID = "1";

            if (IsSelect)  //如果为选择时
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                if (Request["IsChecked"] != null && Request["IsChecked"].ToString() == "True")
                {
                    dgEqu_SchemaItems.Columns[0].Visible = true;
                }
                else
                {
                    dgEqu_SchemaItems.Columns[0].Visible = false;
                }

                dgEqu_SchemaItems.Columns[5].Visible = false;
                dgEqu_SchemaItems.Columns[7].Visible = false;
                dgEqu_SchemaItems.Columns[6].Visible = true;
            }

        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_SchemaItemsEdit.aspx");
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            Bind();
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {

            string StrError = "";
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            foreach (DataGridItem itm in dgEqu_SchemaItems.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    
                    string FieldID = itm.Cells[2].Text;
                    if (chkdel.Checked)
                    {
                        //判断改类型是否用到
                        if (!ee.isEquShiYong(long.Parse(FieldID.ToString())))
                        {
                            ee.DeleteRecorded(long.Parse(sID));
                        }
                        else
                        {
                            StrError += "," + itm.Cells[3].Text.ToString();
                        }
                    }
                    //强制相关缓存失效 
                    HttpRuntime.Cache.Insert("CommCacheValidEquSchemaItem", false);
                }
            }
            Bind();

            if (StrError.Trim(',').Trim() != "")
            {
                PageTool.MsgBox(this, "配置信息名称[" + StrError.Trim(',').ToString() + "]已在资产配置中用到,删除失败!");
            }
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
            cpEqu_SchemaItems.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(Bind);
            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }

                Bind();
            }
            if (Request["IsChecked"] != null && Request["IsChecked"].ToString() == "True")
            {
                trChecked.Visible = true;
            }
            else
            {
                trChecked.Visible = false;
            }
        }
        #endregion

       

        #region  Bind
        /// <summary>
        /// 
        /// </summary>
        private void Bind()
        {
            int iRowCount = 0;
            DataTable dt;
            string sWhere = string.Empty;
            string sOrder = " order by ID";
            if (txtFieldID.Text.Trim() != string.Empty)
            {
                sWhere += " And FieldID like " + StringTool.SqlQ("%" + txtFieldID.Text.Trim() + "%");
            }
            if (txtCHName.Text.Trim() != string.Empty)
            {
                sWhere += " And CHName like " + StringTool.SqlQ("%" + txtCHName.Text.Trim() + "%");
            }
            if (ddltitemType.SelectedValue.Trim() != string.Empty)
            {
                sWhere += " And itemType=" + ddltitemType.SelectedValue.Trim();
            }
               //过滤掉已经在主窗体中存在的
            if (Request["IsChecked"] != null && Request["IsChecked"].ToString() == "True")
            {
                if (Request["subjectedId"] != "")
                {
                    long subjectedId = long.Parse(Request["subjectedId"].ToString());
                    string strFiledId = getFiledId(subjectedId);
                    if (strFiledId != "")
                    {
                        sWhere += " And FieldId not in (" + strFiledId + ")";
                    }
                }
            } 
            Equ_SchemaItemsDP ee = new Equ_SchemaItemsDP();
            dt = ee.GetDataTable(sWhere, sOrder, this.cpEqu_SchemaItems.PageSize, this.cpEqu_SchemaItems.CurrentPage, ref iRowCount); 
            dgEqu_SchemaItems.DataSource = dt.DefaultView;
            dgEqu_SchemaItems.DataBind();
            this.cpEqu_SchemaItems.RecordCount = iRowCount;
            this.cpEqu_SchemaItems.Bind();
        }
        #endregion




        public string getFiledId(long lngSubjectID)
        {
            DataTable dt = Equ_SubjectDP.GetSubjectByID(lngSubjectID);
            //获取表结构
            DataTable dt2 = CreateNullTable();
            if (dt.Rows.Count > 0)
            {
                string valueXml = dt.Rows[0]["configureSchema"].ToString();
                if (valueXml != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(valueXml);

                    AddBaseSchemeDatas(xmldoc, ref dt2);

                    XmlNodeList relnodes = xmldoc.SelectNodes("EquScheme/RelationConfig");

                    foreach (XmlNode relnode in relnodes)
                    {
                        AddRelSchemeDatas(relnode, ref dt2);
                    }

                    relnodes = xmldoc.SelectNodes("EquScheme/Remark");

                    foreach (XmlNode relnode in relnodes)
                    {
                        AddRelRemarkDatas(relnode, ref dt2);
                    }


                }
            }
            string strFiledId = string.Empty;
            if (dt2.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt2.Rows)
                {
                    if (i == 0)
                    {
                        //如是第一次赋值，则不需要逗号
                        strFiledId = dr["ID"].ToString();
                    }
                    else
                    {
                        //给字符串加字符串
                        strFiledId += "," + dr["ID"].ToString();
                    }
                    i++;
                }
            }

            return strFiledId;
        }

        #region 备注信息
        /// <summary>
        /// 创建 datatable结构
        /// </summary>
        /// <returns></returns>
        private DataTable CreateNullTable()
        {
            DataTable dt = new DataTable("Schema");


            dt.Columns.Add("ID");
            dt.Columns.Add("CHName");
            dt.Columns.Add("Default");
            dt.Columns.Add("Group");
            dt.Columns.Add("TypeName");

            return dt;
        }

        private void AddBaseSchemeDatas(XmlDocument xmldoc, ref DataTable dt)
        {
            XmlNodeList bnodes = xmldoc.SelectNodes("EquScheme/Base/BaseItem");
            string strGroup = "";
            string strTypeName = "基础信息";
            foreach (XmlNode node in bnodes)
            {
                strGroup = "";

                if (node.Attributes["Title"] != null)
                {
                    strGroup = node.Attributes["Title"].Value;
                }
                object[] values = new object[5];

                XmlNodeList ns = node.SelectNodes("AttributeItem");

                foreach (XmlNode n in ns)
                {
                    values[0] = (object)n.Attributes["ID"].Value;
                    values[1] = (object)n.Attributes["CHName"].Value;
                    values[2] = (object)n.Attributes["Default"].Value;
                    values[3] = (object)strGroup;
                    values[4] = (object)strTypeName;
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
            object[] values = new object[5];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)strGroup;
                values[4] = (object)strTypeName;
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
            object[] values = new object[5];
            foreach (XmlNode n in ns)
            {
                values[0] = (object)n.Attributes["ID"].Value;
                values[1] = (object)n.Attributes["CHName"].Value;
                values[2] = (object)n.Attributes["Default"].Value;
                values[3] = (object)strGroup;
                values[4] = (object)strTypeName;
                dt.Rows.Add(values);

            }
        }

        #endregion 



        #region  dgEqu_SchemaItems_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgEqu_SchemaItems_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                if (!IsSelect)
                    Response.Redirect("frmEqu_SchemaItemsEdit.aspx?id=" + e.Item.Cells[1].Text.ToString());
                else
                {
                    StringBuilder sbText = new StringBuilder();
                    sbText.Append("<script>");
                    sbText.Append("var arr;");
                    // FieldID
                    sbText.Append("arr ='" + e.Item.Cells[2].Text.Trim() + "';");
               
                    sbText.Append("window.parent.returnValue = arr;");
                    // 关闭窗口
                    sbText.Append("top.close();");
                    sbText.Append("</script>");
                    // 向客户端发送
                    Page.ClientScript.RegisterStartupScript(GetType(),DateTime.Now.ToString(), sbText.ToString());
                    Response.Write(sbText.ToString());
                }
                
            }

            if (e.CommandName == "look")
            {
                Response.Redirect("frmEqu_SameSchemaItem.aspx?isQuery=1&ItemFieldID=" + e.Item.Cells[2].Text.ToString() + "&ItemFieldValue=");
                

            }

        }
        #endregion

        #region dgEqu_SchemaItems_ItemCreated
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_SchemaItems_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                int j = 0;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (Request["IsChecked"] != null && Request["IsChecked"].ToString() == "True")
                    {
                        if (i > 1 && i < e.Item.Cells.Count - 3)
                        {
                            j = i - 2;
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                    else
                    {
                        if (i > 1 && i < e.Item.Cells.Count - 3)
                        {
                            j = i - 2;
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                    }
                    
                }
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgEqu_SchemaItems_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.Cells[4].Text.Trim() == "0")
                {
                    e.Item.Cells[4].Text = "基础信息";
                }
                else if (e.Item.Cells[4].Text.Trim() == "1")
                {
                    e.Item.Cells[4].Text = "关联配置";
                }
                else if (e.Item.Cells[4].Text.Trim() == "2")
                {
                    e.Item.Cells[4].Text = "备注信息";
                }
                else if (e.Item.Cells[4].Text.Trim() == "3")
                {
                    e.Item.Cells[4].Text = "下拉选择";
                }
                else if (e.Item.Cells[4].Text.Trim() == "4")
                {
                    e.Item.Cells[4].Text = "部门信息";
                }
                else if (e.Item.Cells[4].Text.Trim() == "5")
                {
                    e.Item.Cells[4].Text = "用户信息";
                }
                else if (e.Item.Cells[4].Text.Trim() == "6")
                {
                    e.Item.Cells[4].Text = "日期类型";
                }
                else if (e.Item.Cells[4].Text.Trim() == "7")
                {
                    e.Item.Cells[4].Text = "数值类型";
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                e.Item.Attributes.Add("ondblclick", "window.open('../EquipmentManager/frmEqu_SchemaItemsEdit.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");

                if (IsSelect)
                {
                    string value1 = e.Item.Cells[2].Text.Trim();

                    //e.Item.Attributes.Add("ondblclick", "ServerOndblclick('" + value1 + "');");
                }
            }
        }

        protected void BtnChecked_Click(object sender, EventArgs e)
        {
            // 向客户端发送        
            string values = string.Empty;
            int valueNmber = 0;
            foreach (DataGridItem itm in dgEqu_SchemaItems.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||itm.ItemType == ListItemType.Item)
                {   
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    if (chkdel.Checked)
                    {
                        if (valueNmber == 0)
                        {
                            values = itm.Cells[2].Text.Trim();
                        }
                        else
                        {
                            values  +=","+ itm.Cells[2].Text.Trim();
                        }
                        valueNmber++;         
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", Opener_ClientId + "hidTempID", values);
            sbText.AppendFormat("window.opener.document.all.{0}.click();", Opener_ClientId + "btnAddNewItem");

            //sbText.Append("var arr;");
            //sbText.Append("arr ='" + values + "';");  
            //sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), DateTime.Now.ToString(), sbText.ToString());
            //Response.Write(sbText.ToString());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("top.close();");
            sbText.Append("</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }
    }
}
