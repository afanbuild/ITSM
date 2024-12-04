using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;
using Epower.ITSM.Base;
using System.Collections.Generic;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// frmAppFieldConfig 的摘要说明。
    /// </summary>
    public partial class frmAppFieldConfig : BasePage
    {
        #region 设置父窗体按钮事件 SetParentButtonEvent
        /// <summary>
        /// 设置父窗体按钮事件
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.CommonFlowPageSet;
            this.Master.IsCheckRight = true;
            this.Master.ShowDeleteButton(true);
            this.Master.ShowSaveButton(true);
            this.Master.ShowNewButton(false);
            this.Master.ShowBackUrlButton(false);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_SaveFinish_Click += new Global_BtnClick(Master_Master_Button_SaveFinish_Click);
        }
        #endregion

        #region 保存后事件Master_Master_Button_SaveFinish_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_SaveFinish_Click()
        {
            this.Master.ShowNewButton(false);
            this.Master.ShowBackUrlButton(false);
        }
        #endregion

        #region 删除事件 Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            labMsg.Text = "";
            string sFlowModedID = dpdFlow.SelectedValue.ToString();
            if (sFlowModedID == "0")
                return;
            try
            {
                AppFieldConfigDP.DeleteFiledConfig(sFlowModedID);
                ClearValue();
            }
            catch (Exception ee)
            {
                labMsg.Text = ee.Message.ToString();
            }
        }
        #endregion

        #region 保存事件 Master_Master_Button_Save_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            labMsg.Text = "";
            string sFlowModedID = dpdFlow.SelectedValue.ToString();
            if (sFlowModedID == "0")
                return;
            try
            {
                string strXml = "";

                strXml = GetNormalQueryXml();

                AppFieldConfigDP.SaveFiledsConfig(sFlowModedID, txtDate1.Text, txtDate2.Text, txtDate3.Text, txtDate4.Text,
                    txtDate5.Text, txtDate6.Text, txtDate7.Text, txtDate8.Text,
                    txtString1.Text, txtString2.Text, txtString3.Text, txtString4.Text, txtString5.Text, txtString6.Text, txtString7.Text, txtString8.Text,
                    txtNum1.Text, txtNum2.Text, txtNum3.Text, txtNum4.Text, txtNum5.Text,
                    txtCata1.Text, txtCata2.Text, txtCata3.Text, txtCata4.Text, txtCata5.Text,
                    txtBool1.Text, txtBool2.Text, txtBool3.Text, txtBool4.Text,
                    txtRemark.Text, txtRemark2.Text, txtRemark3.Text, txtRemark4.Text,
                    txtFbox.Text.Trim(),

                    chkDate1.Checked == true ? "1" : "0", chkDate2.Checked == true ? "1" : "0", chkDate3.Checked == true ? "1" : "0", chkDate4.Checked == true ? "1" : "0",
                    chkDate5.Checked == true ? "1" : "0", chkDate6.Checked == true ? "1" : "0", chkDate7.Checked == true ? "1" : "0", chkDate8.Checked == true ? "1" : "0",

                    chkString1.Checked == true ? "1" : "0", chkString2.Checked == true ? "1" : "0", chkString3.Checked == true ? "1" : "0", chkString4.Checked == true ? "1" : "0",
                    chkString5.Checked == true ? "1" : "0", chkString6.Checked == true ? "1" : "0", chkString7.Checked == true ? "1" : "0", chkString8.Checked == true ? "1" : "0",
                    chkNum1.Checked == true ? "1" : "0", chkNum2.Checked == true ? "1" : "0", chkNum3.Checked == true ? "1" : "0", chkNum4.Checked == true ? "1" : "0", chkNum5.Checked == true ? "1" : "0",
                    chkCata1.Checked == true ? "1" : "0", chkCata2.Checked == true ? "1" : "0", chkCata3.Checked == true ? "1" : "0", chkCata4.Checked == true ? "1" : "0", chkCata5.Checked == true ? "1" : "0",

                    CtrFlowCataDropList1.CatelogID.ToString(),
                    CtrFlowCataDropList2.CatelogID.ToString(),
                    CtrFlowCataDropList3.CatelogID.ToString(),
                    CtrFlowCataDropList4.CatelogID.ToString(),
                    CtrFlowCataDropList5.CatelogID.ToString(),

                    chkBool1.Checked == true ? "1" : "0", chkBool2.Checked == true ? "1" : "0", chkBool3.Checked == true ? "1" : "0", chkBool4.Checked == true ? "1" : "0",

                    chkRemark.Checked == true ? "1" : "0",
                    chkRemark2.Checked == true ? "1" : "0",
                    chkRemark3.Checked == true ? "1" : "0",
                    chkRemark4.Checked == true ? "1" : "0",

                    strXml, ftxtDesc.Text.Trim(), ftxtTitle.Text.Trim(), ftxtBottom.Text.Trim(),

                    chkDesc.Checked == true ? "1" : "0",
                    rdblstDesc.SelectedValue.Trim(),
                    chkTitle.Checked == true ? "1" : "0",
                    chkBottom.Checked == true ? "1" : "0",


                    chkDate1M.Checked == true ? "1" : "0", chkDate2M.Checked == true ? "1" : "0", chkDate3M.Checked == true ? "1" : "0", chkDate4M.Checked == true ? "1" : "0",
                    chkDate5M.Checked == true ? "1" : "0", chkDate6M.Checked == true ? "1" : "0", chkDate7M.Checked == true ? "1" : "0", chkDate8M.Checked == true ? "1" : "0",

                    chkString1M.Checked == true ? "1" : "0", chkString2M.Checked == true ? "1" : "0", chkString3M.Checked == true ? "1" : "0", chkString4M.Checked == true ? "1" : "0",
                    chkString5M.Checked == true ? "1" : "0", chkString6M.Checked == true ? "1" : "0", chkString7M.Checked == true ? "1" : "0", chkString8M.Checked == true ? "1" : "0",
                    chkNum1M.Checked == true ? "1" : "0", chkNum2M.Checked == true ? "1" : "0", chkNum3M.Checked == true ? "1" : "0", chkNum4M.Checked == true ? "1" : "0", chkNum5M.Checked == true ? "1" : "0",
                    chkCata1M.Checked == true ? "1" : "0", chkCata2M.Checked == true ? "1" : "0", chkCata3M.Checked == true ? "1" : "0", chkCata4M.Checked == true ? "1" : "0", chkCata5M.Checked == true ? "1" : "0",

                    chkRemarkM.Checked == true ? "1" : "0",
                    chkRemark2M.Checked == true ? "1" : "0",
                    chkRemark3M.Checked == true ? "1" : "0",
                    chkRemark4M.Checked == true ? "1" : "0",

                    chkDate1S.Checked == true ? "1" : "0", chkDate2S.Checked == true ? "1" : "0", chkDate3S.Checked == true ? "1" : "0", chkDate4S.Checked == true ? "1" : "0",
                    chkDate5S.Checked == true ? "1" : "0", chkDate6S.Checked == true ? "1" : "0", chkDate7S.Checked == true ? "1" : "0", chkDate8S.Checked == true ? "1" : "0"
                    );

                this.Master.ShowNewButton(false);
                this.Master.ShowBackUrlButton(false);


                SaveMenuAndOrderbyInfo(long.Parse(sFlowModedID));    // 保存字段的菜单配置 - 2013-11-22 @孙绍棕

            }
            catch (Exception ee)
            {
                labMsg.Text = ee.Message.ToString();
            }
        }




        #region 保存和设置分组以及字段排序的信息 - 2013-11-22 @孙绍棕
        /// <summary>
        /// 保存分组和字段排序的信息
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        private void SaveMenuAndOrderbyInfo(long lngFlowModelID)
        {
            List<List<String>> listFieldInfo = new List<List<string>>();
            foreach (HtmlTableRow row in Table1.Rows)
            {
                if (row.Cells[2].Controls.Count <= 1) continue;

                List<String> listValue = FetchFieldInfo(row.Cells[2]);
                if (listValue.Count > 0) listFieldInfo.Add(listValue);

                if (row.Cells.Count >= 6)
                {
                    listValue = FetchFieldInfo(row.Cells[5]);
                    if (listValue.Count > 0) listFieldInfo.Add(listValue);
                }
            }

            AppFieldConfigDP.SaveFieldMenuAndOrderby(lngFlowModelID, listFieldInfo);
        }

        /// <summary>
        /// 保存分组和字段排序的信息
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        private void SetMenuAndOrderbyInfo(long lngFlowModelID)
        {
            DataTable dtMenu = AppFieldConfigDP.GetFieldMenuInfoByFlowModelID(lngFlowModelID);
            if (dtMenu.Rows.Count > 0)
            {                
                long lngMenuRootID = CatalogDP.GetCatalogParentID(long.Parse(dtMenu.Rows[0]["groupid"].ToString()));
                ctrCateMenuGroup.CatelogID = lngMenuRootID;// 设置对应的菜单项               

                SetMenu(lngMenuRootID, Table1);
            }

            foreach (HtmlTableRow row in Table1.Rows)
            {
                if (row.Cells[2].Controls.Count <= 1) continue;

                SetFieldInfo(row.Cells[2], dtMenu);

                if (row.Cells.Count >= 6)
                {
                    SetFieldInfo(row.Cells[5], dtMenu);
                }
            }
        }

        /// <summary>
        /// 提取表单配置字段信息
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<String> FetchFieldInfo(HtmlTableCell cell)
        {
            List<String> listValue = new List<string>();

            String strFieldName = String.Empty;
            CheckBox _chk = cell.Controls[1] as CheckBox;

            if (_chk == null)
            {
                _chk = cell.Controls[2] as CheckBox;
            }


            if (_chk != null && _chk.Checked)
            {
                strFieldName = _chk.ID.Substring(3).ToLower();    // 字段名

                int menuIndex = 0, orderby = 0;

                //1 15 15 15 15 13 13 13 13 9 9 9 15 15 7 7 9 9 9 9 

                if (strFieldName.Contains("date")
                    || strFieldName.Contains("cata")) //15
                {
                    menuIndex = 11;
                    orderby = 13;
                }
                else if (strFieldName.Contains("string"))
                {
                    menuIndex = 9;
                    orderby = 11;
                }
                else if (strFieldName.Contains("num")
                    || strFieldName.Contains("remark")) // 9
                {
                    menuIndex = 5;
                    orderby = 7;
                }
                else // 7
                {
                    menuIndex = 3;
                    orderby = 5;
                }

                Epower.ITSM.Web.Controls.ctrFlowCataDropListNew cataGroupName = cell.Controls[menuIndex] as Epower.ITSM.Web.Controls.ctrFlowCataDropListNew;
                long lngMenuID = cataGroupName.CatelogID;    // 菜单编号

                Epower.ITSM.Web.Controls.CtrFlowNumeric flowNumberic = cell.Controls[orderby] as Epower.ITSM.Web.Controls.CtrFlowNumeric;
                long lngOrderby;    // 字段排序号

                long.TryParse(flowNumberic.Value, out lngOrderby);

                listValue.Add(strFieldName);
                listValue.Add(lngMenuID.ToString());
                listValue.Add(lngOrderby.ToString());

            }

            return listValue;
        }

        /// <summary>
        /// 设置字段的菜单和排序等信息
        /// </summary>
        /// <param name="dtMenu">配置信息</param>
        /// <returns></returns>
        private void SetFieldInfo(HtmlTableCell cell, DataTable dtMenu)
        {
            CheckBox _chk = cell.Controls[1] as CheckBox;

            if (_chk == null)
            {
                _chk = cell.Controls[2] as CheckBox;
            }

            if (_chk != null && _chk.Checked)
            {
                String strCtrlFieldName = _chk.ID.Substring(3).ToLower();    // 控件的字段名

                foreach (DataRow row in dtMenu.Rows)
                {
                    String strFieldName = row["FIELDNAME"].ToString();    // 字段名                        

                    if (strCtrlFieldName.Equals(strFieldName))
                    {

                        int menuIndex = 0, orderby = 0;
                        //1 15 15 15 15 13 13 13 13 9 9 9 15 15 7 7 9 9 9 9 

                        if (strFieldName.Contains("date")
                            || strFieldName.Contains("cata")) //15
                        {
                            menuIndex = 11;
                            orderby = 13;
                        }
                        else if (strFieldName.Contains("string"))
                        {
                            menuIndex = 9;
                            orderby = 11;
                        }
                        else if (strFieldName.Contains("num")
                            || strFieldName.Contains("remark")) // 9
                        {
                            menuIndex = 5;
                            orderby = 7;
                        }
                        else // 7
                        {
                            menuIndex = 3;
                            orderby = 5;
                        }

                        long lngMenuID = long.Parse(row["GROUPID"].ToString());    // 菜单编号                          
                        String strOrderby = row["ORDERBY"].ToString();    // 字段排序号;

                        Epower.ITSM.Web.Controls.ctrFlowCataDropListNew cataGroupName = cell.Controls[menuIndex] as Epower.ITSM.Web.Controls.ctrFlowCataDropListNew;
                        cataGroupName.CatelogID = lngMenuID;

                        Epower.ITSM.Web.Controls.CtrFlowNumeric flowNumberic = cell.Controls[orderby] as Epower.ITSM.Web.Controls.CtrFlowNumeric;
                        flowNumberic.Value = strOrderby;

                        break;
                    }

                }


            }

        }
        #endregion



        private string GetNormalQueryXml()
        {
            string strXmlTemp = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<QuerySeting>
      <!--搜索条件定制-->
     <SearchArea Title=""通用流程查询"">
        
     </SearchArea>
     <Columns>
         
     </Columns>
     
   </QuerySeting>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXmlTemp);

            XmlNode snode = xmlDoc.DocumentElement.SelectSingleNode("SearchArea");
            XmlNode cnode = xmlDoc.DocumentElement.SelectSingleNode("Columns");

            XmlElement nodetemp;

            xmlDoc.DocumentElement.SetAttribute("Title", dpdFlow.SelectedItem.Text);

            int si = 0;
            int ci = 0;


            #region 添加节点代码段

            if (chkDate1.Checked == true && chkDate1Q.Checked == true && txtDate1.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date1");
                nodetemp.SetAttribute("CHName", txtDate1.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate1F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date1");
                    nodetemp.SetAttribute("CHName", txtDate1.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }

            if (chkDate2.Checked == true && chkDate2Q.Checked == true && txtDate2.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date2");
                nodetemp.SetAttribute("CHName", txtDate2.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate2F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date2");
                    nodetemp.SetAttribute("CHName", txtDate2.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }
            if (chkDate3.Checked == true && chkDate3Q.Checked == true && txtDate3.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date3");
                nodetemp.SetAttribute("CHName", txtDate3.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate3F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date3");
                    nodetemp.SetAttribute("CHName", txtDate3.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }
            if (chkDate4.Checked == true && chkDate4Q.Checked == true && txtDate4.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date4");
                nodetemp.SetAttribute("CHName", txtDate4.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate4F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date4");
                    nodetemp.SetAttribute("CHName", txtDate4.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }
            if (chkDate5.Checked == true && chkDate5Q.Checked == true && txtDate5.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date5");
                nodetemp.SetAttribute("CHName", txtDate5.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate5F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date5");
                    nodetemp.SetAttribute("CHName", txtDate5.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }

            if (chkDate6.Checked == true && chkDate6Q.Checked == true && txtDate6.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date6");
                nodetemp.SetAttribute("CHName", txtDate6.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate5F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date6");
                    nodetemp.SetAttribute("CHName", txtDate6.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }

            if (chkDate7.Checked == true && chkDate7Q.Checked == true && txtDate7.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date7");
                nodetemp.SetAttribute("CHName", txtDate7.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate7F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date7");
                    nodetemp.SetAttribute("CHName", txtDate7.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }

            if (chkDate8.Checked == true && chkDate8Q.Checked == true && txtDate8.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Date8");
                nodetemp.SetAttribute("CHName", txtDate8.Text.Trim());
                nodetemp.SetAttribute("Type", "datetime");
                nodetemp.SetAttribute("Default", "");
                nodetemp.SetAttribute("Default1", "");

                nodetemp.SetAttribute("CtrlType", "CtrDate");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkDate8F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Date8");
                    nodetemp.SetAttribute("CHName", txtDate8.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }
            }

            if (chkString1.Checked == true && chkString1Q.Checked == true && txtString1.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String1");
                nodetemp.SetAttribute("CHName", txtString1.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString1F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String1");
                    nodetemp.SetAttribute("CHName", txtString1.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString2.Checked == true && chkString2Q.Checked == true && txtString2.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String2");
                nodetemp.SetAttribute("CHName", txtString2.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString2F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "Cata5");
                    nodetemp.SetAttribute("Name", "String2");
                    nodetemp.SetAttribute("CHName", txtString2.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString3.Checked == true && chkString3Q.Checked == true && txtString3.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String3");
                nodetemp.SetAttribute("CHName", txtString3.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString3F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String3");
                    nodetemp.SetAttribute("CHName", txtString3.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString4.Checked == true && chkString4Q.Checked == true && txtString4.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String4");
                nodetemp.SetAttribute("CHName", txtString4.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString4F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String4");
                    nodetemp.SetAttribute("CHName", txtString4.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString5.Checked == true && chkString5Q.Checked == true && txtString5.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String5");
                nodetemp.SetAttribute("CHName", txtString5.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString5F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String5");
                    nodetemp.SetAttribute("CHName", txtString5.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString6.Checked == true && chkString6Q.Checked == true && txtString6.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String6");
                nodetemp.SetAttribute("CHName", txtString6.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString6F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String6");
                    nodetemp.SetAttribute("CHName", txtString6.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString7.Checked == true && chkString7Q.Checked == true && txtString7.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String7");
                nodetemp.SetAttribute("CHName", txtString7.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString7F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String7");
                    nodetemp.SetAttribute("CHName", txtString7.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkString8.Checked == true && chkString8Q.Checked == true && txtString8.Text.Trim().Length > 0)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "String8");
                nodetemp.SetAttribute("CHName", txtString8.Text.Trim());
                nodetemp.SetAttribute("Type", "varchar");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "TextBox");
                nodetemp.SetAttribute("DicVal", "");

                snode.AppendChild(nodetemp);

                if (chkString8F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "String8");
                    nodetemp.SetAttribute("CHName", txtString8.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkCata1.Checked == true && chkCata1Q.Checked == true && txtCata1.Text.Trim().Length > 0 && CtrFlowCataDropList1.CatelogID != -1 && CtrFlowCataDropList1.CatelogID != 1012)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Cate1");
                nodetemp.SetAttribute("CHName", txtCata1.Text.Trim());
                nodetemp.SetAttribute("Type", "long");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "CtrCatalog");
                nodetemp.SetAttribute("DicVal", CtrFlowCataDropList1.CatelogID.ToString());

                snode.AppendChild(nodetemp);

                if (chkCata1F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "CateValue1");
                    nodetemp.SetAttribute("CHName", txtCata1.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkCata2.Checked == true && chkCata2Q.Checked == true && txtCata2.Text.Trim().Length > 0 && CtrFlowCataDropList2.CatelogID != -1 && CtrFlowCataDropList2.CatelogID != 1012)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Cate2");
                nodetemp.SetAttribute("CHName", txtCata2.Text.Trim());
                nodetemp.SetAttribute("Type", "long");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "CtrCatalog");
                nodetemp.SetAttribute("DicVal", CtrFlowCataDropList2.CatelogID.ToString());

                snode.AppendChild(nodetemp);

                if (chkCata2F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "CateValue2");
                    nodetemp.SetAttribute("CHName", txtCata2.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }


            if (chkCata3.Checked == true && chkCata3Q.Checked == true && txtCata3.Text.Trim().Length > 0 && CtrFlowCataDropList3.CatelogID != -1 && CtrFlowCataDropList3.CatelogID != 1012)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Cate3");
                nodetemp.SetAttribute("CHName", txtCata3.Text.Trim());
                nodetemp.SetAttribute("Type", "long");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "CtrCatalog");
                nodetemp.SetAttribute("DicVal", CtrFlowCataDropList3.CatelogID.ToString());

                snode.AppendChild(nodetemp);

                if (chkCata3F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "CateValue3");
                    nodetemp.SetAttribute("CHName", txtCata3.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkCata4.Checked == true && chkCata4Q.Checked == true && txtCata4.Text.Trim().Length > 0 && CtrFlowCataDropList4.CatelogID != -1 && CtrFlowCataDropList4.CatelogID != 1012)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Cate4");
                nodetemp.SetAttribute("CHName", txtCata4.Text.Trim());
                nodetemp.SetAttribute("Type", "long");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "CtrCatalog");
                nodetemp.SetAttribute("DicVal", CtrFlowCataDropList4.CatelogID.ToString());

                snode.AppendChild(nodetemp);

                if (chkCata4F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "CateValue4");
                    nodetemp.SetAttribute("CHName", txtCata4.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }

            if (chkCata5.Checked == true && chkCata5Q.Checked == true && txtCata5.Text.Trim().Length > 0 && CtrFlowCataDropList5.CatelogID != -1 && CtrFlowCataDropList5.CatelogID != 1012)
            {
                //需要配置到XML里
                si++;
                nodetemp = xmlDoc.CreateElement("Field");
                nodetemp.SetAttribute("ID", "Field" + si.ToString());
                nodetemp.SetAttribute("Name", "Cate5");
                nodetemp.SetAttribute("CHName", txtCata5.Text.Trim());
                nodetemp.SetAttribute("Type", "long");
                nodetemp.SetAttribute("Default", "");


                nodetemp.SetAttribute("CtrlType", "CtrCatalog");
                nodetemp.SetAttribute("DicVal", CtrFlowCataDropList5.CatelogID.ToString());

                snode.AppendChild(nodetemp);


                if (chkCata5F.Checked == true)
                {
                    //添加字段节点
                    ci++;
                    nodetemp = xmlDoc.CreateElement("Column");
                    nodetemp.SetAttribute("ID", "Field" + ci.ToString());
                    nodetemp.SetAttribute("Name", "CateValue5");
                    nodetemp.SetAttribute("CHName", txtCata5.Text.Trim());
                    nodetemp.SetAttribute("DataFormatString", "");
                    nodetemp.SetAttribute("Width", "");

                    cnode.AppendChild(nodetemp);
                }


            }


            #endregion

            strXmlTemp = xmlDoc.OuterXml;

            return strXmlTemp;
        }
        #endregion

        #region 页面加载Page_Load
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ctrCateMenuGroup.mySelectedIndexChanged += new EventHandler(ctrCateMenuGroup_mySelectedIndexChanged);

            SetParentButtonEvent();
            if (!IsPostBack)
            {
                dpdFlow.Items.Clear();
                DataTable dt = AppFieldConfigDP.GetFlowModelList();
                dpdFlow.DataSource = dt.DefaultView;
                dpdFlow.DataTextField = "FlowName";
                dpdFlow.DataValueField = "oFlowModelID";
                dpdFlow.DataBind();

                dpdFlow.Items.Insert(0, new ListItem("", "0"));
            }


        }
        #endregion

        #region 设置分组菜单要绑定的常用类别编号 - 2013-11-22 @孙绍棕

        /// <summary>
        /// 切换绑定的菜单(常用类别)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrCateMenuGroup_mySelectedIndexChanged(object sender, EventArgs e)
        {
            long lngMenuID = ctrCateMenuGroup.CatelogID;

            SetMenu(lngMenuID, Table1);    // 切换绑定的常用类别
        }

        /// <summary>
        /// 设置分组菜单要绑定的常用类别编号
        /// </summary>
        /// <param name="lngMenuID">常用类别编号</param>
        /// <param name="ctrObj"></param>
        private void SetMenu(long lngMenuID, Control ctrObj)
        {
            if (ctrObj == null) return;

            if (ctrObj.ID != null)
            {
                bool isContain = ctrObj.ID.ToLower().StartsWith("ctrcata");

                if (isContain)
                {
                    Epower.ITSM.Web.Controls.ctrFlowCataDropListNew cataMenu = ctrObj as Epower.ITSM.Web.Controls.ctrFlowCataDropListNew;
                    if (cataMenu == null) return;

                    cataMenu.RootID = lngMenuID;

                }
            }

            for (int index = 0; index < ctrObj.Controls.Count; index++)
            {
                SetMenu(lngMenuID, ctrObj.Controls[index]);
            }
        }

        #endregion

        #region 清空数据 ClearValue
        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearValue()
        {
            PageDeal.ClearPageControls(this.Controls[0]);
        }
        #endregion

        #region  加载已有的流程设置 dpdFlow_SelectedIndexChanged
        /// <summary>
        /// 加载已有的流程设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dpdFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Master.MainID = dpdFlow.SelectedValue.ToString();
            this.lblFlowModelID.Text = dpdFlow.SelectedValue.ToString();

            labMsg.Text = "";

            ClearValue();
            try
            {
                ctrCateMenuGroup.RootID = 10262;

                string sFlowModelID = dpdFlow.SelectedValue.ToString();
                if (sFlowModelID == "0")
                    return;

                DataTable dt = AppFieldConfigDP.GetFieldsConfig(sFlowModelID);

                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];

                    #region Text
                    txtBool1.Text = dr["Bool1"].ToString();
                    txtBool2.Text = dr["Bool2"].ToString();
                    txtBool3.Text = dr["Bool3"].ToString();
                    txtBool4.Text = dr["Bool4"].ToString();

                    txtCata1.Text = dr["Cate1"].ToString();
                    txtCata2.Text = dr["Cate2"].ToString();
                    txtCata3.Text = dr["Cate3"].ToString();
                    txtCata4.Text = dr["Cate4"].ToString();
                    txtCata5.Text = dr["Cate5"].ToString();

                    txtDate1.Text = dr["Date1"].ToString();
                    txtDate2.Text = dr["Date2"].ToString();
                    txtDate3.Text = dr["Date3"].ToString();
                    txtDate4.Text = dr["Date4"].ToString();

                    txtDate5.Text = dr["Date5"].ToString();
                    txtDate6.Text = dr["Date6"].ToString();
                    txtDate7.Text = dr["Date7"].ToString();
                    txtDate8.Text = dr["Date8"].ToString();

                    txtNum1.Text = dr["Number1"].ToString();
                    txtNum2.Text = dr["Number2"].ToString();
                    txtNum3.Text = dr["Number3"].ToString();
                    txtNum4.Text = dr["Number4"].ToString();
                    txtNum5.Text = dr["Number5"].ToString();

                    txtRemark.Text = dr["Remark1"].ToString();
                    txtRemark2.Text = dr["Remark2"].ToString();
                    txtRemark3.Text = dr["Remark3"].ToString();
                    txtRemark4.Text = dr["Remark4"].ToString();

                    txtFbox.Text = dr["FBox"].ToString();

                    txtString1.Text = dr["String1"].ToString();
                    txtString2.Text = dr["String2"].ToString();
                    txtString3.Text = dr["String3"].ToString();
                    txtString4.Text = dr["String4"].ToString();
                    txtString5.Text = dr["String5"].ToString();
                    txtString6.Text = dr["String6"].ToString();
                    txtString7.Text = dr["String7"].ToString();
                    txtString8.Text = dr["String8"].ToString();

                    #endregion

                    #region Check
                    chkBool1.Checked = dr["Bool1Validate"].ToString() == "1" ? true : false;
                    chkBool2.Checked = dr["Bool2Validate"].ToString() == "1" ? true : false;
                    chkBool3.Checked = dr["Bool3Validate"].ToString() == "1" ? true : false;
                    chkBool4.Checked = dr["Bool4Validate"].ToString() == "1" ? true : false;

                    chkCata1.Checked = dr["Cate1Validate"].ToString() == "1" ? true : false;
                    chkCata2.Checked = dr["Cate2Validate"].ToString() == "1" ? true : false;
                    chkCata3.Checked = dr["Cate3Validate"].ToString() == "1" ? true : false;
                    chkCata4.Checked = dr["Cate4Validate"].ToString() == "1" ? true : false;
                    chkCata5.Checked = dr["Cate5Validate"].ToString() == "1" ? true : false;

                    chkDate1.Checked = dr["Date1Validate"].ToString() == "1" ? true : false;
                    chkDate2.Checked = dr["Date2Validate"].ToString() == "1" ? true : false;
                    chkDate3.Checked = dr["Date3Validate"].ToString() == "1" ? true : false;
                    chkDate4.Checked = dr["Date4Validate"].ToString() == "1" ? true : false;

                    chkDate5.Checked = dr["Date5Validate"].ToString() == "1" ? true : false;
                    chkDate6.Checked = dr["Date6Validate"].ToString() == "1" ? true : false;
                    chkDate7.Checked = dr["Date7Validate"].ToString() == "1" ? true : false;
                    chkDate8.Checked = dr["Date8Validate"].ToString() == "1" ? true : false;

                    chkNum1.Checked = dr["Number1Validate"].ToString() == "1" ? true : false;
                    chkNum2.Checked = dr["Number2Validate"].ToString() == "1" ? true : false;
                    chkNum3.Checked = dr["Number3Validate"].ToString() == "1" ? true : false;
                    chkNum4.Checked = dr["Number4Validate"].ToString() == "1" ? true : false;
                    chkNum5.Checked = dr["Number5Validate"].ToString() == "1" ? true : false;

                    chkRemark.Checked = dr["Remark1Validate"].ToString() == "1" ? true : false;
                    chkRemark2.Checked = dr["Remark2Validate"].ToString() == "1" ? true : false;
                    chkRemark3.Checked = dr["Remark3Validate"].ToString() == "1" ? true : false;
                    chkRemark4.Checked = dr["Remark4Validate"].ToString() == "1" ? true : false;

                    chkString1.Checked = dr["String1Validate"].ToString() == "1" ? true : false;
                    chkString2.Checked = dr["String2Validate"].ToString() == "1" ? true : false;
                    chkString3.Checked = dr["String3Validate"].ToString() == "1" ? true : false;
                    chkString4.Checked = dr["String4Validate"].ToString() == "1" ? true : false;
                    chkString5.Checked = dr["String5Validate"].ToString() == "1" ? true : false;
                    chkString6.Checked = dr["String6Validate"].ToString() == "1" ? true : false;
                    chkString7.Checked = dr["String7Validate"].ToString() == "1" ? true : false;
                    chkString8.Checked = dr["String8Validate"].ToString() == "1" ? true : false;

                    chkDate1M.Checked = dr["Date1Must"].ToString() == "1" ? true : false;
                    chkDate2M.Checked = dr["Date2Must"].ToString() == "1" ? true : false;
                    chkDate3M.Checked = dr["Date3Must"].ToString() == "1" ? true : false;
                    chkDate4M.Checked = dr["Date4Must"].ToString() == "1" ? true : false;

                    chkDate5M.Checked = dr["Date5Must"].ToString() == "1" ? true : false;
                    chkDate6M.Checked = dr["Date6Must"].ToString() == "1" ? true : false;
                    chkDate7M.Checked = dr["Date7Must"].ToString() == "1" ? true : false;
                    chkDate8M.Checked = dr["Date8Must"].ToString() == "1" ? true : false;

                    chkNum1M.Checked = dr["Number1Must"].ToString() == "1" ? true : false;
                    chkNum2M.Checked = dr["Number2Must"].ToString() == "1" ? true : false;
                    chkNum3M.Checked = dr["Number3Must"].ToString() == "1" ? true : false;
                    chkNum4M.Checked = dr["Number4Must"].ToString() == "1" ? true : false;
                    chkNum5M.Checked = dr["Number5Must"].ToString() == "1" ? true : false;

                    chkRemarkM.Checked = dr["RemarkMust"].ToString() == "1" ? true : false;
                    chkRemark2M.Checked = dr["Remark2Must"].ToString() == "1" ? true : false;
                    chkRemark3M.Checked = dr["Remark3Must"].ToString() == "1" ? true : false;
                    chkRemark4M.Checked = dr["Remark4Must"].ToString() == "1" ? true : false;

                    chkString1M.Checked = dr["String1Must"].ToString() == "1" ? true : false;
                    chkString2M.Checked = dr["String2Must"].ToString() == "1" ? true : false;
                    chkString3M.Checked = dr["String3Must"].ToString() == "1" ? true : false;
                    chkString4M.Checked = dr["String4Must"].ToString() == "1" ? true : false;
                    chkString5M.Checked = dr["String5Must"].ToString() == "1" ? true : false;
                    chkString6M.Checked = dr["String6Must"].ToString() == "1" ? true : false;
                    chkString7M.Checked = dr["String7Must"].ToString() == "1" ? true : false;
                    chkString8M.Checked = dr["String8Must"].ToString() == "1" ? true : false;
                    chkCata1M.Checked = dr["Cate1Must"].ToString() == "1" ? true : false;
                    chkCata2M.Checked = dr["Cate2Must"].ToString() == "1" ? true : false;
                    chkCata3M.Checked = dr["Cate3Must"].ToString() == "1" ? true : false;
                    chkCata4M.Checked = dr["Cate4Must"].ToString() == "1" ? true : false;
                    chkCata5M.Checked = dr["Cate5Must"].ToString() == "1" ? true : false;
                    chkDate1S.Checked = dr["Date1Show"].ToString() == "1" ? true : false;
                    chkDate2S.Checked = dr["Date2Show"].ToString() == "1" ? true : false;
                    chkDate3S.Checked = dr["Date3Show"].ToString() == "1" ? true : false;
                    chkDate4S.Checked = dr["Date4Show"].ToString() == "1" ? true : false;

                    chkDate5S.Checked = dr["Date5Show"].ToString() == "1" ? true : false;
                    chkDate6S.Checked = dr["Date6Show"].ToString() == "1" ? true : false;
                    chkDate7S.Checked = dr["Date7Show"].ToString() == "1" ? true : false;
                    chkDate8S.Checked = dr["Date8Show"].ToString() == "1" ? true : false;

                    #endregion

                    #region Root
                    CtrFlowCataDropList1.CatelogID = long.Parse(dr["Cate1RootID"].ToString());
                    CtrFlowCataDropList2.CatelogID = long.Parse(dr["Cate2RootID"].ToString());
                    CtrFlowCataDropList3.CatelogID = long.Parse(dr["Cate3RootID"].ToString());
                    CtrFlowCataDropList4.CatelogID = long.Parse(dr["Cate4RootID"].ToString());
                    CtrFlowCataDropList5.CatelogID = long.Parse(dr["Cate5RootID"].ToString());

                    #endregion

                    ftxtDesc.Text = dr["Description"].ToString();
                    ftxtTitle.Text = dr["PrintTitle"].ToString();
                    ftxtBottom.Text = dr["PrintBottom"].ToString();

                    chkDesc.Checked = dr["DescValidate"].ToString() == "1" ? true : false;
                    chkTitle.Checked = dr["TitleValidate"].ToString() == "1" ? true : false;
                    chkBottom.Checked = dr["BottomValidate"].ToString() == "1" ? true : false;
                    if (dr["DescControl"] != null && dr["DescControl"].ToString() != string.Empty)
                        rdblstDesc.SelectedValue = dr["DescControl"].ToString();

                    if (dr["QueryXml"].ToString() != "")
                        setQueryControlValue(dr["QueryXml"].ToString());


                    SetMenuAndOrderbyInfo(long.Parse(sFlowModelID));    // 设置表单字段的菜单和排序信息 - 2013-11-22 @孙绍棕

                }
            }
            catch (Exception ee)
            {
                labMsg.Text = ee.Message.ToString();
            }
        }


        private void setQueryControlValue(string strXml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(strXml);

            XmlNodeList nodes = xmldoc.DocumentElement.SelectNodes("SearchArea/Field");

            foreach (XmlNode node in nodes)
            {
                switch (node.Attributes["Name"].Value.ToLower())
                {
                    case "date1":
                        chkDate1Q.Checked = true;
                        break;
                    case "date2":
                        chkDate2Q.Checked = true;
                        break;
                    case "date3":
                        chkDate3Q.Checked = true;
                        break;
                    case "date4":
                        chkDate4Q.Checked = true;
                        break;
                    case "string1":
                        chkString1Q.Checked = true;
                        break;
                    case "string2":
                        chkString2Q.Checked = true;
                        break;
                    case "string3":
                        chkString3Q.Checked = true;
                        break;
                    case "string4":
                        chkString4Q.Checked = true;
                        break;
                    case "string5":
                        chkString5Q.Checked = true;
                        break;
                    case "string6":
                        chkString6Q.Checked = true;
                        break;
                    case "string7":
                        chkString7Q.Checked = true;
                        break;
                    case "string8":
                        chkString8Q.Checked = true;
                        break;
                    case "cate1":
                        chkCata1Q.Checked = true;
                        break;
                    case "cate2":
                        chkCata2Q.Checked = true;
                        break;
                    case "cate3":
                        chkCata3Q.Checked = true;
                        break;
                    case "cate4":
                        chkCata4Q.Checked = true;
                        break;
                    case "cate5":
                        chkCata5Q.Checked = true;
                        break;
                    default:
                        break;
                }
            }

            nodes = xmldoc.DocumentElement.SelectNodes("Columns/Column");

            foreach (XmlNode node in nodes)
            {
                switch (node.Attributes["Name"].Value.ToLower())
                {
                    case "date1":
                        chkDate1F.Checked = true;
                        break;
                    case "date2":
                        chkDate2F.Checked = true;
                        break;
                    case "date3":
                        chkDate3F.Checked = true;
                        break;
                    case "date4":
                        chkDate4F.Checked = true;
                        break;
                    case "string1":
                        chkString1F.Checked = true;
                        break;
                    case "string2":
                        chkString2F.Checked = true;
                        break;
                    case "string3":
                        chkString3F.Checked = true;
                        break;
                    case "string4":
                        chkString4F.Checked = true;
                        break;
                    case "string5":
                        chkString5F.Checked = true;
                        break;
                    case "string6":
                        chkString6F.Checked = true;
                        break;
                    case "string7":
                        chkString7F.Checked = true;
                        break;
                    case "string8":
                        chkString8F.Checked = true;
                        break;
                    case "catevalue1":
                        chkCata1F.Checked = true;
                        break;
                    case "catevalue2":
                        chkCata2F.Checked = true;
                        break;
                    case "catevalue3":
                        chkCata3F.Checked = true;
                        break;
                    case "catevalue4":
                        chkCata4F.Checked = true;
                        break;
                    case "catevalue5":
                        chkCata5F.Checked = true;
                        break;
                    default:
                        break;
                }
            }

        }

        #endregion
    }
}
