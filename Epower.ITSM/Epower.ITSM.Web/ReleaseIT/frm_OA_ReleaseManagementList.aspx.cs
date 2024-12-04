using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Xml;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EpowerCom;
using EpowerGlobal;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.Service ;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_OA_ReleaseManagementList : BasePage
    {
        long lngUserID = 0;
        long lngDeptID = 0;
        long lngOrgID = 0;
        RightEntity reTrace = null;  //  权限 

        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.ShowQueryPageButton();
            this.Master.ShowDeleteButton(false);
            this.Master.ShowNewButton(true);
            //this.Master.ShowExportExcelButton(true);
            if (Request["NewWin"] != null && Request["NewWin"].ToString().ToLower() == "false")
            {
                this.Master.TableVisible = false;
                tbltitle.Visible = false;
            }

            this.Master.MainID = "1";
        }
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("~/Forms/form_all_flowmodel.aspx?appid=1028");
        }

        void Master_Master_Button_Refresh_Click()
        {
            BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            lngUserID = (long)Session["UserID"];
            lngDeptID = (long)Session["UserDeptID"];
            lngOrgID = (long)Session["UserOrgID"];
            
            //ControlPageIssues.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData); 
            cpCST_Issue.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(BindData);
            //获得 权限
            reTrace = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.SystemManager];
            if (!Page.IsPostBack)
            {
                cboStatus.Items.Add(new ListItem("所有状态", "-1"));
                cboStatus.Items.Add(new ListItem("--正在处理", ((int)e_FlowStatus.efsHandle).ToString()));
                cboStatus.Items.Add(new ListItem("--正常结束", ((int)e_FlowStatus.efsEnd).ToString()));
                cboStatus.Items.Add(new ListItem("--流程暂停", ((int)e_FlowStatus.efsStop).ToString()));
                cboStatus.Items.Add(new ListItem("--流程终止", ((int)e_FlowStatus.efsAbort).ToString()));
                cboStatus.SelectedIndex = 1;

                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    CtrDCreateRegTime.dateTime = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    CtrDCreateRegTime.dateTime = DateTime.Parse(sQueryBeginDate);
                }
                CtrDEndRegTime.dateTime = DateTime.Now;


                #region  删除权限的控制
                if (CheckRight(Constant.admindeleteflow))
                {
                    gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 2].Visible = true;  //删除流程权限
                }
                else
                {
                    RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[Constant.releaseSeach];
                    if (re!= null)
                        gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 2].Visible = re.CanDelete;  //删除流程权限
                    else
                    {
                        gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 2].Visible = false;  //删除流程权限
                    }
                }
                #endregion 

                gridUndoMsg.Columns[gridUndoMsg.Columns.Count - 1].Visible = CheckRight(Constant.EquChangeQuery);
                //Session["FromUrl"] = "close";
                //BindData();

                Control[] arrControl = { txtVersionName, hidTable, txtVersionCode, ctrReleaseScope, cboStatus, ctrVersionKind, ctrVersionType, UserPicker1, CtrDCreateRegTime, CtrDEndRegTime };
                PageDeal.SetPageQueryParam(arrControl, cpCST_Issue, "frm_OA_ReleaseManagementList");
                BindData();

                Session["FromUrl"] = "../ReleaseIT/frm_OA_ReleaseManagementList.aspx";
            }
            Control[] arrControl1 = { txtVersionName, hidTable, txtVersionCode, ctrReleaseScope, cboStatus, ctrVersionKind, ctrVersionType, UserPicker1, CtrDCreateRegTime, CtrDEndRegTime };
            PageDeal.GetPageQueryParam(arrControl1, cpCST_Issue, "frm_OA_ReleaseManagementList");
        }
      

        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            if (Request["NewWin"] != null && Request["NewWin"].ToString().ToLower() == "false")
            {
                sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "','','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            else
            {
                sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            }
            return sUrl;
        }

        private void BindData()
        {
            string sWhere = "";
            int iRowCount = 0;
            
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ReleaseManagementDP.GetReleaseInfor(xmlDoc.InnerXml , StringTool.String2Long(Session["UserOrgID"].ToString()), StringTool.String2Long(Session["UserID"].ToString()), StringTool.String2Long(Session["UserDeptID"].ToString()), reTrace, this.cpCST_Issue.PageSize, this.cpCST_Issue.CurrentPage, ref iRowCount);
            
            gridUndoMsg.DataSource = dt;
            gridUndoMsg.DataBind();

            this.cpCST_Issue.RecordCount = iRowCount;
            this.cpCST_Issue.Bind();
        }

        private XmlDocument GetXmlValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("Fields");
            XmlElement xmlEle;

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "status");
            xmlEle.SetAttribute("Value", cboStatus.SelectedValue.ToString());
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "versionname");
            xmlEle.SetAttribute("Value", txtVersionName.Text.Trim ().ToString());
            xmlRoot.AppendChild(xmlEle);
 
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "versioncode");
            xmlEle.SetAttribute("Value", txtVersionCode.Text.Trim ().ToString ());
            xmlRoot.AppendChild(xmlEle);   
          
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "releasescopeid");
            if (ctrReleaseScope .CatelogValue .ToString () != string.Empty  )
                xmlEle.SetAttribute("Value", ctrReleaseScope.CatelogID.ToString () );
            else
                xmlEle.SetAttribute("Value", "-1");
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "versionkindid");
            if(ctrVersionKind.CatelogValue .ToString ()!= string.Empty )
                xmlEle.SetAttribute("Value", ctrVersionKind.CatelogID.ToString ());
            else
                xmlEle.SetAttribute("Value", "-1");
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "versiontypeid");
            if(ctrVersionType.CatelogValue .ToString () != string.Empty )
                xmlEle.SetAttribute("Value", ctrVersionType .CatelogID .ToString () );
            else
                xmlEle.SetAttribute("Value", "-1");
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "releasepersonid");
            xmlEle.SetAttribute("Value", UserPicker1.UserID.ToString ());
            xmlRoot.AppendChild(xmlEle);
            
            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "RegBegin");
            xmlEle.SetAttribute("Value", CtrDCreateRegTime.dateTime.ToShortDateString ());
            xmlRoot.AppendChild(xmlEle);

            xmlEle = xmlDoc.CreateElement("Field");
            xmlEle.SetAttribute("FieldName", "RegEnd");
            xmlEle.SetAttribute("Value",CtrDEndRegTime .dateTime.AddDays(1).ToShortDateString ());
            xmlRoot.AppendChild(xmlEle);
                      
            xmlDoc.AppendChild(xmlRoot);

            return xmlDoc;
        }
        
        void Master_Master_Button_Query_Click()
        {
            //ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            BindData();
        }

        protected void gridUndoMsg_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //当超过整个流程预计处理时限未处理的，红低显示
                if (int.Parse(e.Item.Cells[1].Text.Trim()) < 0)
                {
                    for (int i = 0; i < e.Item.Cells.Count - 1; i++)
                    {
                        e.Item.Cells[i].ForeColor = Color.Red;
                    }
                }

                if (e.Item.Cells[9].Text.Trim() == "20")
                {
                    e.Item.Cells[9].Text = "正在处理";
                }
                else if (e.Item.Cells[9].Text.Trim() == "30")
                {
                    e.Item.Cells[9].Text = "正常结束";
                }
                else if (e.Item.Cells[9].Text.Trim() == "40")
                {
                    e.Item.Cells[9].Text = "流程暂停";
                }
                else if (e.Item.Cells[9].Text.Trim() == "50")
                {
                    e.Item.Cells[9].Text = "流程终止";
                }

                if (DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == string.Empty || DataBinder.Eval(e.Item.DataItem, "AssociateFlowID").ToString() == "0")
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = true;
                    lblChange.Visible = false;
                }
                else
                {
                    Button btnChange = (Button)e.Item.FindControl("btnAssociate");
                    Label lblChange = (Label)e.Item.FindControl("lblAssociate");
                    btnChange.Visible = false;
                    lblChange.Visible = true;
                }

                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
                string lngFlowID = DataBinder.Eval(e.Item.DataItem, "FlowID").ToString();
                if (Request["NewWin"] != null && Request["NewWin"].ToString().ToLower() == "false")
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + lngFlowID.ToString() + "&randomid='+GetRandom(),'','scrollbars=yes,resizable=yes')");
                }
                else
                {
                    e.Item.Attributes.Add("ondblclick", "window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "&randomid='+GetRandom(),'MainFrame','scrollbars=yes,resizable=yes')");
                }
            }
        } 

        private bool CheckRight(long OperatorID)
        {
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else    
                return re.CanRead;
        }

        protected void gridUndoMsg_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 0 && i < dg.Columns.Count - 3)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        protected void gridUndoMsg_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            BindData();
        }
    }
}
