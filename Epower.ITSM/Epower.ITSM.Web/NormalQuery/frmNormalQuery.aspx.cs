using System;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
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
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.NormalQuery
{
    public partial class frmNormalQuery : BasePage
    {
        string strXmlTemp = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
        <QuerySeting>
              <!--������������-->
             <SearchArea Title=""ͨ�����̲�ѯ"">
             </SearchArea>
             <Columns>
             </Columns>
             
           </QuerySeting>";

        long lngOFlowModelID = 0;
        long lngFlowModelID = 0;
        long lngOperateID = 0;
        XmlNodeList nodes = null;

        #region ���ø����尴ť�¼�SetParentButtonEvent
        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region ҳ���ʼ��
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FlowModelID"] != null)
            {
                lngFlowModelID = long.Parse(Request["FlowModelID"].ToString());
                lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);
            }

            if (Request["OperateID"] != null)
                lngOperateID = long.Parse(Request["OperateID"].ToString());

            DataTable dt1 = FlowModel.GetCommAppFlowModelFields(lngOFlowModelID);

            DataRow dr1 = null;
            if (dt1.Rows.Count > 0)
            {
                dr1 = dt1.Rows[0];
                strXmlTemp = dr1["queryxml"].ToString();
            }

            SetParentButtonEvent();

            if (strXmlTemp != string.Empty)
                AddColForGrid(strXmlTemp);

            ControlPageIssues.On_PostBack += new EventHandler(ControlPageIssues_On_PostBack);
            ControlPageIssues.DataGridToControl = dgDispatch;

            if (!IsPostBack)
            {
                InitDropList();
                Hashtable ht = null;
                Bind(ht);

                Session["FromUrl"] = Constant.ApplicationPath + "/NormalQuery/frmNormalQuery.aspx?FlowModelID=" + lngFlowModelID.ToString() + "&OperateID=" + lngOperateID.ToString();

                dgDispatch.Columns[dgDispatch.Columns.Count - 1].Visible = false;  //ɾ������Ȩ��
            }


            if (strXmlTemp != string.Empty)
                LoadHtmlControls(strXmlTemp);


        }
        #endregion

        #region ��̬���ر��Ϳؼ�

        private void AddColForGrid(string strXml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(strXml);

            int iinsIndex = 5;

            XmlNodeList cols = xmldoc.DocumentElement.SelectNodes("Columns/Column");

            foreach (XmlNode node in cols)
            {
                BoundColumn col = new BoundColumn();

                col.HeaderText = node.Attributes["CHName"].Value;

                col.DataField = node.Attributes["Name"].Value;

                col.DataFormatString = node.Attributes["DataFormatString"].Value;

                col.ItemStyle.Width = new Unit(node.Attributes["Width"].Value);

                dgDispatch.Columns.AddAt(iinsIndex, col);

                iinsIndex++;
            }
            if (iinsIndex == 6)
            {
                BoundColumn col1 = new BoundColumn();
                col1.HeaderText = "test";
                col1.DataField = "";
                col1.Visible = false;
                col1.DataFormatString = string.Empty;
                dgDispatch.Columns.AddAt(iinsIndex, col1);
            }

        }

        private void LoadHtmlControls(string strXml)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(strXml);

            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell tc = new HtmlTableCell();

            nodes = xmldoc.DocumentElement.SelectNodes("SearchArea/Field");

            int iRow = 0;
            int iCell = 0;
            foreach (XmlNode node in nodes)
            {
                if (iCell == 0)
                {
                    //��Ҫ������� 
                    iRow++;
                    tr = new HtmlTableRow();

                    tr.ID = "tDynamicRow" + iRow;




                }
                tc = AddControlByNode(node, ref tr, ref tc, ref iRow, ref iCell);
            }

            if (iCell == 1)
            {
                //������һ���ؼ�������,���������һ��TC ������
                tc.Attributes.Add("colspan", "3");
                AddControl(tabMain, tr);
            }




        }

        #endregion

        #region ����˽�з���

        /// <summary>
        /// ����XML���ö�̬���ؿؼ�
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ctrl"></param>
        /// <param name="_tc"> ��һ��ִ�й��� cell����</param>
        /// <param name="iRow"></param>
        /// <param name="iCell"></param>
        /// <returns></returns>
        private HtmlTableCell AddControlByNode(XmlNode node, ref HtmlTableRow ctrl, ref HtmlTableCell _tc, ref int iRow, ref int iCell)
        {
            iCell++;
            //��ӱ�ǩTD
            string strNodeType = node.Attributes["Type"].Value;
            if (strNodeType.Equals("datetime"))
            {
                //��ǰ����Ϊ����ʱ����ʱ ��Ҫ������ʾ �������������ڿؼ� 
                if (iCell == 2)
                {
                    //����һ����ӵ�table,��ֱ�� �������
                    _tc.Attributes.Add("colspan", "3");
                    AddControl(tabMain, ctrl);

                    iRow++;
                    ctrl = new HtmlTableRow();

                    ctrl.ID = "tDynamicRow" + iRow;
                    iCell = 0;
                    _tc = AddControlByNode(node, ref ctrl, ref _tc, ref iRow, ref iCell);

                    return _tc;
                }
                else
                {
                    iCell = 2;
                }




            }

            HtmlTableCell tc = new HtmlTableCell();
            tc.ID = "tTitleDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("nowrap", "nowrap");
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("class", "listTitleRight");
            tc.InnerText = node.Attributes["CHName"].Value;

            AddControl(ctrl, tc);
            //��ӿؼ�TD���ؼ�

            tc = new HtmlTableCell();
            tc.ID = "tDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
            tc.Attributes.Add("nowrap", "nowrap");
            tc.Attributes.Add("align", "left");
            tc.Attributes.Add("class", "list");
            if (strNodeType.Equals("datetime"))
            {
                //����һ����ӵ�table,��������ʼ�ͽ�������ʱ��
                tc.Attributes.Add("colspan", "3");
                Control control = GetSettingControl(node, "begin");
                if (control != null)
                    AddControl(tc, control);

                control = new Label();
                ((Label)control).Text = " ~ ";
                control.ID = "tLabelDynamicCell_" + iRow.ToString() + "_" + iCell.ToString();
                AddControl(tc, control);

                control = GetSettingControl(node, "end");
                if (control != null)
                    AddControl(tc, control);
            }
            else
            {

                Control control = GetSettingControl(node, "");
                if (control != null)
                    AddControl(tc, control);
            }
            AddControl(ctrl, tc);

            if (iCell == 2)
            {
                iCell = 0;
                //����е���
                AddControl(tabMain, ctrl);

            }
            return tc;

        }

        /// <summary>
        ///  ��ȡ���õĿؼ�
        /// </summary>
        /// <param name="node"></param>
        /// <param name="strDateType"></param>
        /// <returns></returns>
        private Control GetSettingControl(XmlNode node, string strDateType)
        {
            string strCtrlType = node.Attributes["CtrlType"].Value;
            Control control;
            switch (strCtrlType)
            {
                case "TextBox":
                    control = new TextBox();
                    control.ID = "tDynamic" + "_txt_" + node.Attributes["Name"].Value;
                    ((TextBox)control).Width = new Unit(152);
                    ((TextBox)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "CtrCatalog":
                    control = (Epower.ITSM.Web.Controls.ctrFlowCataDropList)LoadControl("~/controls/ctrFlowCataDropList.ascx"); ;
                    control.ID = "tDynamic" + "_catelog_" + node.Attributes["Name"].Value;
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).RootID = long.Parse(node.Attributes["DicVal"].Value);
                    ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)control).Attributes.Add("Tag", node.Attributes["ID"].Value);
                    break;
                case "CtrDateTime":
                case "CtrDate":

                    control = (Epower.ITSM.Web.Controls.CtrDateAndTimeV2)LoadControl("~/Controls/CtrDateAndTimeV2.ascx");
                    if (strCtrlType.Equals("CtrDateTime"))
                    {
                        control.ID = "tDynamic" + "_datetime_" + node.Attributes["Name"].Value + "_" + strDateType;
                    }
                    else
                    {
                        ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).ShowTime = false;
                        control.ID = "tDynamic" + "_date_" + node.Attributes["Name"].Value + "_" + strDateType;
                    }
                    int iDays = 0;
                    if (strDateType == "begin")
                    {
                        if (Page.IsPostBack == false)
                        {
                            //��һ�μ���ʱ ���ó�ֵ
                            if (node.Attributes["Default"].Value.Length > 0)
                            {
                                iDays = int.Parse(node.Attributes["Default"].Value);
                                ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).dateTime = DateTime.Now.AddDays(iDays);
                            }
                            else
                            {
                                //���ÿ�
                                ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).AllowNull = true;
                            }
                        }
                        else
                        {
                            //���ÿ�
                            ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).AllowNull = true;
                        }
                    }
                    else
                    {
                        if (Page.IsPostBack == false)
                        {
                            //��һ�μ���ʱ ���ó�ֵ
                            if (node.Attributes["Default1"].Value.Length > 0)
                            {
                                iDays = int.Parse(node.Attributes["Default1"].Value);
                                ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).dateTime = DateTime.Now.AddDays(iDays);
                            }
                            else
                            {
                                //���ÿ�
                                ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).AllowNull = true;
                            }
                        }
                        else
                        {
                            //���ÿ�
                            ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).AllowNull = true;
                        }
                    }

                    ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)control).Attributes.Add("Tag", node.Attributes["ID"].Value + "_" + strDateType);
                    break;
                default:
                    control = null;
                    break;

            }

            return control;

        }

        /// <summary>
        /// �����ؼ��ϼ��ӿؼ�
        /// </summary>
        /// <param name="pParentControl">���ؼ�����</param>
        /// <param name="pSubControl">�ӿؼ�����</param>
        protected void AddControl(Control pParentControl, Control pSubControl)
        {
            if (pParentControl != null && pSubControl != null)
            {
                bool bFound = false;
                foreach (Control pControl in pParentControl.Controls)
                {
                    if (pControl.ID == pSubControl.ID)
                    {
                        bFound = true;
                        break;
                    }

                }
                if (!bFound) pParentControl.Controls.Add(pSubControl);
            }
        }


        /// <summary>
        /// �ݹ��ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="ctrRoot"></param>
        /// <param name="list"></param>
        private void ValueCollection(Control ctrRoot, ref Hashtable list)
        {
            string strType = "";
            foreach (Control pControl in ctrRoot.Controls)  /*���������ӽڵ�*/
            {
                if (pControl.ID != null)
                {

                    if (pControl.ID.StartsWith("tDynamic"))
                    {
                        strType = pControl.GetType().Name;
                        switch (strType)
                        {
                            case "HtmlTableRow":
                            case "HtmlTableCell":
                                ValueCollection(pControl, ref list);
                                break;
                            case "TextBox":
                                list.Add(((TextBox)pControl).Attributes["Tag"], ((TextBox)pControl).Text);
                                break;
                            case "controls_ctrflowcatadroplist_ascx":
                                list.Add(((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.ctrFlowCataDropList)pControl).CatelogID.ToString());
                                break;
                            case "controls_ctrdateandtimev2_ascx":
                                list.Add(((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)pControl).Attributes["Tag"], ((Epower.ITSM.Web.Controls.CtrDateAndTimeV2)pControl).dateTimeString);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
        }

        #endregion

        #region ��ʼ�������б�
        protected void InitDropList()
        {
            cboStatus.Items.Add(new ListItem("����״̬", "-1"));
            cboStatus.Items.Add(new ListItem("--���ڴ���", ((int)e_FlowStatus.efsHandle).ToString()));
            cboStatus.Items.Add(new ListItem("--��������", ((int)e_FlowStatus.efsEnd).ToString()));
            cboStatus.Items.Add(new ListItem("--������ͣ", ((int)e_FlowStatus.efsStop).ToString()));
            cboStatus.Items.Add(new ListItem("--������ֹ", ((int)e_FlowStatus.efsAbort).ToString()));

            ctrDateSelectTimeV21.BeginTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ctrDateSelectTimeV21.EndTime = DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region ��ѯ�¼�
        /// <summary>
        /// ��ѯ�¼�
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            Hashtable ht = new Hashtable();
            ValueCollection(tabMain, ref ht);


            ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            Bind(ht);

        }
        #endregion

        #region ������ Bind
        /// <summary>
        /// ������
        /// </summary>
        private void Bind(Hashtable ht)
        {
            #region bind

            string sWhere = string.Empty;

            #region �̶���������
            if (cboStatus.SelectedValue != "-1")
            {
                sWhere += " AND b.status = " + cboStatus.SelectedValue;
            }
            if (DeptPicker1.DeptID != 0)
            {
                sWhere += " AND a.deptid = " + DeptPicker1.DeptID;
            }
            if (UserPicker1.UserID != 0)
            {
                sWhere += " AND a.applyid = " + UserPicker1.UserID;
            }
            if (txtTitle.Text.Trim() != string.Empty)
                sWhere += " and a.flowname like" + StringTool.SqlQ("%" + txtTitle.Text.Trim() + "%");

            if (ctrDateSelectTimeV21.BeginTime.Trim() != string.Empty)
                sWhere += " And a.StartDate >= to_date(" + StringTool.SqlQ(ctrDateSelectTimeV21.BeginTime.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
            if (ctrDateSelectTimeV21.EndTime.Trim() != string.Empty)
                sWhere += " And a.StartDate < to_date(" + StringTool.SqlQ(DateTime.Parse(ctrDateSelectTimeV21.EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59") + ",'yyyy-MM-dd HH24:mi:ss')";

            #endregion

            #region ��̬��������

            if (ht != null)
            {
                foreach (XmlNode node in nodes)
                {
                    //��������������
                    string strID = node.Attributes["ID"].Value;
                    string strType = node.Attributes["Type"].Value;
                    string strCtrlType = node.Attributes["CtrlType"].Value;
                    string strName = node.Attributes["Name"].Value;

                    string strDicVal = node.Attributes["DicVal"].Value;
                    string strValue = "";
                    string strValue1 = "";
                    if (strType.ToLower() != "datetime")
                    {
                        strValue = ht[strID].ToString();
                        if (strType.ToLower() == "varchar")
                        {
                            if (strValue.Trim() != string.Empty)
                                sWhere += " And a." + strName.Trim() + " like " + StringTool.SqlQ("%" + strValue.Trim() + "%");
                        }

                        if (strType.ToLower() == "long")
                        {
                            if (strCtrlType.ToLower() == "ctrcatalog")
                            {
                                if (strValue == strDicVal || strValue == "-1")
                                {
                                    //���Ϊ������ ���ж�
                                    strValue = string.Empty;
                                }
                            }
                            if (strValue.Trim() != string.Empty)
                                sWhere += " And a." + strName.Trim() + " = " + strValue;
                        }
                    }
                    else
                    {
                        strValue = ht[strID + "_begin"].ToString();
                        strValue1 = ht[strID + "_end"].ToString();
                        if (strCtrlType.ToLower() == "ctrdate")
                        {
                            if (strValue.Trim() != "")
                                strValue += " 00:00:01";
                            if (strValue1.Trim() != "")
                                strValue1 += " 23:59:59";
                        }

                        if (strValue != "" && strValue1 != "")
                        {
                            //  between���
                            sWhere += " And a." + strName.Trim() + " BETWEEN to_date(" + StringTool.SqlQ(strValue) + ",'yyyy-MM-dd HH24:mi:ss') AND  to_date(" + StringTool.SqlQ(strValue1) + ",'yyyy-MM-dd HH24:mi:ss')";
                        }
                        else
                        {
                            if (strValue != "")
                            {
                                sWhere += " And a." + strName.Trim() + " >= to_date(" + StringTool.SqlQ(strValue) + ",'yyyy-MM-dd HH24:mi:ss')";
                            }

                            if (strValue1 != "")
                            {
                                sWhere += " And a." + strName.Trim() + " <= to_date(" + StringTool.SqlQ(strValue1) + ",'yyyy-MM-dd HH24:mi:ss')";
                            }
                        }

                    }
                }
            }

            #endregion


            DataTable dt = Epower.ITSM.SqlDAL.NormalAppDP.GetNormalQuery(long.Parse(Session["UserID"].ToString()), long.Parse(Session["UserDeptID"].ToString()), long.Parse(Session["UserOrgID"].ToString()),
                            (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[lngOperateID], sWhere, lngOFlowModelID.ToString());
            Session["DispatchQuery"] = dt;
            ControlPageIssues.DataGridToControl.CurrentPageIndex = 0;
            BindToTable();
            #endregion
        }
        #endregion

        #region ��ҳ�¼�
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPageIssues_On_PostBack(object sender, EventArgs e)
        {
            BindToTable();
        }
        /// <summary>
        /// 
        /// </summary>
        private void BindToTable()
        {
            DataTable dt = (DataTable)Session["DispatchQuery"];

            dgDispatch.DataSource = dt.DefaultView;
            dgDispatch.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");

            dgDispatch.DataBind();
        }
        #endregion

        #region ��ʾҳ���ַ GetUrl
        /// <summary>
        /// ��ʾҳ���ַ
        /// </summary>
        /// <param name="lngNoticeID"></param>
        /// <returns></returns>
        protected string GetUrl(decimal lngFlowID)
        {
            string sUrl = "";
            sUrl = "javascript:window.open('../Forms/frmIssueView.aspx?FlowID=" + lngFlowID.ToString() + "','MainFrame','scrollbars=yes,resizable=yes,top=0,left=0,width=window.availWidth-12,height=window.availHeight-10');";
            return sUrl;
        }
        #endregion

        #region �������� dgDispatch_ItemCreateds
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDispatch_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (i > 2 && i < e.Item.Cells.Count - 2)
                    {
                        int j = i - 3;
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }
        #endregion

        #region ɾ�������¼� dgDispatch_DeleteCommand
        /// <summary>
        /// ɾ�������¼�
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dgDispatch_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            long lngFlowID = long.Parse(e.Item.Cells[1].Text);
            DataTable dt = (DataTable)Session["DispatchQuery"];
            foreach (DataRow r in dt.Rows)
            {
                if (r["flowid"].ToString() == lngFlowID.ToString())
                {
                    dt.Rows.Remove(r);
                    break;
                }
            }

            Session["DispatchQuery"] = dt;

            BindToTable();
        }
        #endregion

        #region ���Ȩ��CheckRight
        /// <summary>
        /// ���Ȩ��
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

    }
}
