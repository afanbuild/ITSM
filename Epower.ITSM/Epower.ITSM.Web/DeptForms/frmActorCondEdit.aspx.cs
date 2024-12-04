/*
 * by duanqs
 * */
using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// frmActorCondEdit 的摘要说明。
    /// </summary>
    public partial class frmActorCondEdit : BasePage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            CtrTitle.Title = "条件人员编辑";

            if (!IsPostBack)
            {
                if (this.Request.QueryString["CondID"] != null)
                {
                    string sCondID = this.Request.QueryString["CondID"].ToString();
                    hidCondID.Value = sCondID;
                    LoadData(StringTool.String2Long(sCondID));
                }
            }
        }

        #region 加载绑定数据
        private void LoadData(long lngCondID)
        {
            string strRootName = Session["RootDeptName"].ToString();
            long lngRootID = (long)Session["RootDeptID"];
            DataTable dt = ActorCondControl.GetActorCondXML(lngCondID, lngRootID, strRootName);



            Session["ACOTRCONDCONDITION"] = dt;
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
            ActorCondEntity ace = new ActorCondEntity(lngCondID);
            txtCondName.Text = ace.CondName.ToString();

            Update_Grid_By_DataTable();

        }

        private void BindData()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["ACOTRCONDCONDITION"];
            dgCondition.DataSource = dt.DefaultView;
            dgCondition.DataBind();
        }
        #endregion


        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {

            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dgCondition.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgCondition_ItemCommand);

        }
        #endregion

        #region 添加\删除\更新数据
        //添加
        protected void cmdAdd_Click(object sender, System.EventArgs e)
        {

            Update_DataTable_By_GridData();// 遍历Grid中的行，更新DataTable

            DataTable dt = (DataTable)Session["ACOTRCONDCONDITION"];
            DataRow dr = dt.NewRow();
            //设定默认值
            dr["ID"] = (dt.Rows.Count + 1).ToString();
            dr["Relation"] = 0;
            dr["CondItem"] = "";
            dr["CondType"] = "0,NULL";
            dr["Operate"] = 0;
            dr["Expression"] = "";
            dr["Tag"] = "";
            dt.Rows.Add(dr);
            Session["ACOTRCONDCONDITION"] = dt;
            BindData();

            Update_Grid_By_DataTable();// 用DataTable中的数据更新Grid

        }



        /// <summary>
        /// 遍历Grid中的行，更新DataTable
        /// </summary>
        private bool Update_DataTable_By_GridData()
        {
            bool breturn = true;
            string sCondType = "";
            string sTag = "";
            string sValue = "";

            DataTable dt = (DataTable)Session["ACOTRCONDCONDITION"];
            DataRow dr;

            //重新获取Grid中的所有值
            for (int i = 0; i < dgCondition.Items.Count; i++)
            {
                dr = dt.Rows[i];
                dr["ID"] = i + 1;
                dr["Relation"] = ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboRelation")).SelectedValue.ToString();
                dr["CondItem"] = ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboItems")).SelectedItem.Text.ToString();

                sCondType = ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboItems")).SelectedValue.ToString();
                sTag = ((HtmlInputHidden)dgCondition.Items[i].Cells[1].FindControl("hidValue")).Value.ToString();
                sValue = ((TextBox)dgCondition.Items[i].Cells[1].FindControl("txtValue")).Text.ToString();

                //人员.部门
                if (sCondType.Split(",".ToCharArray())[0] == ((int)eO_CondActorType.ecatDept).ToString())
                {
                    if (sValue == "") { sTag = ""; }
                }
                else
                {
                    sTag = sValue;
                }


                dr["CondType"] = sCondType;
                dr["Operate"] = ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboOperate")).SelectedValue.ToString();
                dr["Expression"] = sValue;
                dr["Tag"] = sTag;

                if (sValue == string.Empty)
                {
                    breturn = false;
                }
                else if (sCondType.Split(",".ToCharArray())[1] == "DEPT" && sTag == string.Empty)
                {
                    breturn = false;
                }
            }

            Session["ACOTRCONDCONDITION"] = dt;
            return breturn;
        }


        /// <summary>
        /// 用DataTable中的数据更新Grid
        /// </summary>
        private void Update_Grid_By_DataTable()
        {
            string sCondType = "";
            string sTag = "";
            string sValue = "";
            string sOperate = "0";

            DataTable dt = (DataTable)Session["ACOTRCONDCONDITION"];
            DataRow dr;
            DropDownList ddl;

            //对Grid中的每行重新赋值
            for (int i = 0; i < dgCondition.Items.Count; i++)
            {
                dr = dt.Rows[i];

                ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboRelation")).SelectedValue = dr["Relation"].ToString();
                ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboItems")).SelectedItem.Text = dr["CondItem"].ToString();

                sCondType = dr["CondType"].ToString();
                ddl = (DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboOperate");

                sOperate = dr["Operate"].ToString();

                switch (sCondType.Split(",".ToCharArray())[1])
                {
                    case "CHAR":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("以..开头", "3"));
                        ddl.Items.Add(new ListItem("包含", "6"));
                        ddl.Items.Add(new ListItem("不包含", "7"));
                        break;
                    case "DEPT":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));
                        ddl.Items.Add(new ListItem("属于", "6"));
                        ddl.Items.Add(new ListItem("不属于", "7"));
                        break;

                    case "SEX":
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("等于", "0"));
                        ddl.Items.Add(new ListItem("不等于", "1"));

                        break;

                    case "INT":

                        break;

                    case "FLOAT":

                        break;

                    case "BOOL":

                        break;

                    default:

                        break;
                }

                sTag = dr["Tag"].ToString();
                sValue = dr["Expression"].ToString();

                ddl.SelectedValue = sOperate;

                //人员.部门
                if (sCondType.Split(",".ToCharArray())[0] == ((int)eO_CondActorType.ecatDept).ToString())
                {
                    if (sValue == "") { sTag = ""; }
                }
                else
                {
                    sTag = sValue;
                }

                ((DropDownList)dgCondition.Items[i].Cells[1].FindControl("cboItems")).SelectedValue = sCondType;
                ((HtmlInputHidden)dgCondition.Items[i].Cells[1].FindControl("hidValue")).Value = sTag;
                ((TextBox)dgCondition.Items[i].Cells[1].FindControl("txtValue")).Text = sValue;
            }

        }



        //删除\更新
        private void dgCondition_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataTable dt = (DataTable)Session["ACOTRCONDCONDITION"];
            DataRow dr = dt.Rows[e.Item.ItemIndex];
            switch (e.CommandName)
            {
                case "Delete":
                    dt.Rows.Remove(dr);

                    break;
                case "Update":
                    string sCondType = ((DropDownList)e.Item.Cells[1].FindControl("cboItems")).SelectedValue.ToString();
                    string sTag = ((HtmlInputHidden)e.Item.Cells[1].FindControl("hidValue")).Value.ToString();
                    string sValue = ((TextBox)e.Item.Cells[1].FindControl("txtValue")).Text.Trim();

                    //人员.部门
                    if (sCondType == ((int)eO_CondActorType.ecatDept).ToString())
                    {
                        if (sValue == "") { sTag = ""; }
                    }
                    else
                    {
                        sTag = sValue;
                    }

                    if (StringTool.String2Long(sCondType) == 0)
                    {
                        labMsg.Text = "内容项不能为空,更新失败!";
                        return;
                    }
                    else if (sTag.Trim() == "")
                    {
                        labMsg.Text = "比较值不能为空,更新失败";
                        return;
                    }
                    else
                    {
                        labMsg.Text = "";
                    }

                    dr.BeginEdit();
                    dr["Relation"] = ((DropDownList)e.Item.Cells[1].FindControl("cboRelation")).SelectedValue.ToString();

                    dr["CondItem"] = ((DropDownList)e.Item.Cells[1].FindControl("cboItems")).SelectedItem.Text.ToString();
                    dr["CondType"] = sCondType;

                    dr["Operate"] = ((DropDownList)e.Item.Cells[1].FindControl("cboOperate")).SelectedValue.ToString();


                    dr["Expression"] = sValue;
                    dr["Tag"] = sTag;

                    dr.EndEdit();
                    break;
            }
            Session["ACOTRCONDCONDITION"] = dt;
            

            //if (e.CommandName == "cmdPopCommand")
            //{
            //    windowOpengiv(e);
            //}
            BindData();
        }

        //保存数据
        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            ActorCondEntity ace = new ActorCondEntity();

            if (!Update_DataTable_By_GridData()) // 遍历Grid中的行，更新DataTable
            {
                PageTool.MsgBox(this, "条件值不能为空！");
                return;
            }

            DataTable dt = (DataTable)Session["ACOTRCONDCONDITION"];
            if (dt.Rows.Count == 0)
            {
                PageTool.MsgBox(this, "没有可保存的条件！");
                return;
            }
            //if (dt.Rows.Count == 1)
            //{
            //    if (dt.Rows[0]["CondItem"].ToString() == string.Empty)
            //    {
            //        PageTool.MsgBox(this, "请选择内容项！");
            //        return;
            //    }
            //}
            DataTable dt1 = Delete_DuplicateCondition(dt.Copy());
            foreach (DataRow row in dt1.Rows)
            {
                row["CondType"] = row["CondType"].ToString().Split(",".ToCharArray())[0];
            }
            DataSet ds = new DataSet("Conditions");
            ds.Tables.Add(dt1);
            string sXml = ds.GetXml();

            ace.SystemID = (long)Session["SystemID"];
            ace.RangeID = Session["RangeID"].ToString();

            ace.CondId = StringTool.String2Long(hidCondID.Value);
            ace.CondName = txtCondName.Text.ToString();
            ace.Statement = sXml;
            try
            {
                if (ActorCondEntity.Is_Record_Exist(ace.SystemID.ToString(), ace.CondName, ace.Statement))
                    PageTool.MsgBox(this, "该条件已存在,请从新输入!");
                else
                {

                    ace.Save();
                    //返回新的ID防止多次保存
                    hidCondID.Value = ace.CondId.ToString();
                    //PageTool.AddJavaScript(this,"window.opener.location.reload(); self.close()");
                    PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.close();");
                }
            }
            catch (Exception ee)
            {
                PageTool.MsgBox(this, "保存人员条件时出现错误,错误为:" + ee.Message.ToString());
            }

        }

        //清除重复条件
        private DataTable Delete_DuplicateCondition(DataTable dt)
        {
            DataRow dr1, dr2;
            //删除空条件 或空条件

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr1 = dt.Rows[i];
                for (int j = i + 1; j < dt.Rows.Count; j++)
                {
                    dr2 = dt.Rows[j];
                    if ((dr1["CondItem"].ToString() == dr2["CondItem"].ToString() &&
                        dr1["CondType"].ToString() == dr2["CondType"].ToString() &&
                        dr1["Operate"].ToString() == dr2["Operate"].ToString() &&
                        dr1["Expression"].ToString() == dr2["Expression"].ToString()) ||
                        dr2["CondItem"].ToString().Trim().Length == 0 ||
                        dr2["Expression"].ToString().Trim().Length == 0)
                    {
                        dr2.Delete();
                        i--;
                        j--;
                        break;
                    }
                }
            }
            return dt;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgCondition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList cboItems = (DropDownList)e.Item.FindControl("cboItems");
                TextBox txtValue = (TextBox)e.Item.FindControl("txtValue");
                if (cboItems.SelectedValue == "10,DEPT")
                {
                    //txtValue.ReadOnly = true;
                    txtValue.CssClass = "TextBoxReadOnly";
                }
                else
                {
                    //txtValue.ReadOnly = false;
                    txtValue.CssClass = "TextBoxReadWrite";
                }
            }

  
            

        }
        //private void windowOpengiv( DataGridCommandEventArgs e) 
        //{
        //    //==============zxl==============
        //    DropDownList cboDownList = (DropDownList)e.Item.FindControl("cboItems");
        //    HtmlInputHidden hidClientId_ForOpenerPage = (HtmlInputHidden)e.Item.FindControl("hidClientId_ForOpenerPage");

        //    Button cmdPop = (Button)e.Item.FindControl("cmdPop");
        //    if (cboDownList.SelectedValue == "")
        //    {
        //        cboDownList.Items[0].Selected = true;
        //    }
        //    string v = cboDownList.SelectedValue;
        //    if (!string.IsNullOrEmpty(v))
        //    {
        //        string itemId = v.Split(',')[0];
        //        switch (itemId)
        //        {
        //            case "10":

        //                string url = "'frmpopdept.aspx?Opener_ClientId=" + hidClientId_ForOpenerPage.ClientID + "&TypeFrm=frmpopdept'";
        //                string flog = " var popupwindowFlag=1; if($.browser.safari) { popupwindowFlag=0; alert('由于Safari的安全限制，本功能暂不支持Safari浏览器!'); }";
        //                string strs = flog + "if (popupwindowFlag == 1) {  window.open(" + url + ", 'E8OpenWin', 'resizable=1,scrollbars=1,status=no,toolbar=no,menu=no,width=520,height=340,left=150,top=50');}";
        //              //  cmdPop.Attributes.Add("onclick", strs);
        //                 Page.RegisterStartupScript(DateTime.Now.ToString(), "<script>" + strs + "</script>");
        //                break;
        //            case "30"://人员.职位
        //                break;
        //            case "40"://人员.性别
        //                break;
        //            case "50"://人员.学历
        //                break;
        //            case "60"://人员.角色
        //                break;
        //        }
        //    }

        //    //=============zxl===============
        //}
    }
}
