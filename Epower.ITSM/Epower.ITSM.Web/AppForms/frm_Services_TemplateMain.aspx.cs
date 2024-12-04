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
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_Services_TemplateMain : BasePage
    {
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.OperatorID = Constant.serveDefine;
            this.Master.IsCheckRight = true;
         //   dgECustomer.Columns[4].Visible = this.Master.GetEditRight();   


            Epower.DevBase.Organization.SqlDAL.RightEntity re = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[Constant.serveDefine];

            if (re != null)
            {
                dgECustomer.Columns[5].Visible = re.CanModify;
            }

            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Query_Click  +=new Global_BtnClick(Master_Master_Button_Query_Click); 

            this.Master.ShowQueryPageButton();
            //this.Master.ShowQueryButton();   //���ﲻ��Ҫ��ʾ��ѯ��ť
            this.Master.MainID = "1";
        }

      
        /// <summary>
        /// ��������
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frm_Services_Template.aspx");
        }

        #region ��ѯ�¼�
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            LoadData();
        }
        #endregion 

        /// <summary>
        /// ɾ���¼�
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            foreach (DataGridItem itm in dgECustomer.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    string sID = itm.Cells[1].Text;
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkDel");
                    try
                    {
                        labMsg.Visible = false;
                        if (chkdel.Checked)
                        {
                            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
                            ee.DeleteRecorded(long.Parse(sID));

                        }
                    }
                    catch (Exception ee)
                    {
                        labMsg.Visible = true;
                        labMsg.Text = ee.Message.ToString();
                    }
                }
            }
            LoadData();
        }

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();
            cpTemplate.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                LoadDrList();
                //��������
                LoadData();
            }
        }

        /// <summary>
        /// ��һ������Ŀ¼������
        /// </summary>
        private void LoadDrList()
        {
            drParentList.Items.Clear();
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string sWhere = " And IsParent=1 ";
            DataTable dt = ee.GetDataTable(sWhere, "");
            drParentList.DataSource = dt.DefaultView;
            drParentList.DataTextField = "TemplateName";
            drParentList.DataValueField = "TemplateID";
            drParentList.DataBind();

            drParentList.Items.Insert(0, new ListItem("", "0"));
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {

            string  sqlWhere = "";


            if (drParentList.SelectedItem != null && decimal.Parse(drParentList.SelectedItem.Value)>0)
            {
                sqlWhere += " and ServiceLevelID = " + drParentList.SelectedItem.Value;
            }

            if (CtrFTTemplateName.Value.Trim() != "")
            {
                sqlWhere += " and TemplateName  like " + StringTool.SqlQ("%" + CtrFTTemplateName.Value.Trim() + "%");
            }


            int iRowCount = 0;
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            DataTable dt = ee.GetMyTemplaties((long)Session["UserID"], e_ITSMShortCutReqType.eitsmscrtIssue, this.cpTemplate.PageSize, this.cpTemplate.CurrentPage, ref iRowCount, sqlWhere);
            DataView dv = new DataView(dt);
            dgECustomer.DataSource = dv;
            dgECustomer.DataBind();
            this.cpTemplate.RecordCount = iRowCount;
            this.cpTemplate.Bind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPage1_On_PostBack(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                Response.Redirect("frm_Services_Template.aspx?id=" + e.Item.Cells[1].Text.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgECustomer_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                {
                    if (i > 1)
                    {
                        int j = i - 1;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        protected void dgECustomer_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string ServiceLevel = e.Item.Cells[2].Text.Trim();
                if (ServiceLevel == "" || ServiceLevel == "&nbsp;")
                {
                    e.Item.Cells[2].Text = "һ��Ŀ¼";
                }
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");

                //��Ȩ��ʱ�ſ�˫���������˵�
               
               e.Item.Attributes.Add("ondblclick", "window.open('../AppForms/frm_Services_Template.aspx?id=" + e.Item.Cells[1].Text.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');");
               
            }
        }

       
    }
}
