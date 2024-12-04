using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerCom;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_Services_List : BasePage
    {
        #region 变量申明
        private long lngUserID = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lngUserID = long.Parse(Session["UserID"].ToString());
            if (!IsPostBack)
            {
                //加载数据
                LoadData();
                DoDataBind();
                Session["FromUrl"] = "../AppForms/frm_Services_List.aspx";
                ltlName.Text = Epower.ITSM.SqlDAL.CommonDP.GetConfigValue("SystemName", "PersonServiceName");
                ShowNewsInfo();
            }
        }

        private void LoadData()
        {
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string sWhere = " and ServiceLevelID = -1 ";
            DataTable dt = ee.GetDataTable(sWhere, "order by EA_ServicesTemplate.Templateid");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                row.BeginEdit();
                sWhere = "and ServiceLevelID=" + row["TEMPLATEID"].ToString();
                DataTable dtDtl = ee.GetDataTable(sWhere, "");
                if (dtDtl.Rows.Count == 0)//没有二级目录时
                {
                    int intCanStart = CanUseFlowModel(long.Parse(row["OFlowModelID"].ToString()));
                    if (intCanStart != 0)
                    {
                        dt.Rows[i].Delete();
                    }
                }
                else
                {
                    int num = 0;
                    int num2 = 0;
                    for (int j = 0; j < dtDtl.Rows.Count; j++)
                    {
                        DataRow row2 = dtDtl.Rows[j];
                        sWhere = "and ServiceLevelID=" + row2["TEMPLATEID"].ToString();
                        DataTable dtDtl1 = ee.GetDataTable(sWhere, "");

                        if (dtDtl1.Rows.Count == 0)//没有二级目录时
                        {
                            int intCanStart = CanUseFlowModel(long.Parse(row2["OFlowModelID"].ToString()));
                            if (intCanStart != 0 && num == 0)
                            {
                                dt.Rows[i].Delete();
                            }
                            else
                            {
                                num = num + 1;
                            }
                        }
                        else
                        {
                            num = num + 1;
                            // int CanStartCount = 0;
                            for (int n = 0; n < dtDtl1.Rows.Count; n++)
                            {
                                DataRow rowDtl = dtDtl1.Rows[n];
                                int intCanStart = CanUseFlowModel(long.Parse(rowDtl["OFlowModelID"].ToString()));
                                if (intCanStart == 0)
                                {
                                    num2++;
                                    // CanStartCount++;
                                }
                            }

                        }
                    }
                }

                row.EndEdit();
            }
            dt.AcceptChanges();

            this.tt.DataSource = dt;
            this.tt.DataBind();
        }

        protected string GetImgSrc(object imglogo)
        {
            string img = "../Images/tb_001.gif";
            if (imglogo != null && imglogo.ToString().Trim() != "")
            {
                img = imglogo.ToString();
            }

            return img;
        }

        protected void tt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string sID = DataBinder.Eval(e.Item.DataItem, "TemplateID").ToString();
                HtmlTableCell td = ((HtmlTableCell)e.Item.FindControl("tddown"));
                td.InnerHtml = "";
                DataTable dt = EA_ServicesTemplateDP.GetAttachmentDT(decimal.Parse(sID));
                if (dt != null && dt.Rows.Count > 0)
                {
                    string IDAndName = dt.Rows[0]["FileID"].ToString() + "                                 " + dt.Rows[0]["FileName"].ToString();

                    td.InnerHtml = "<A href='../Forms/flow_Attachment_download.aspx?FileID=" + IDAndName.Trim() + "&Type=eZZ' target='_blank'" + ">" + dt.Rows[0]["FileName"].ToString().Trim() + "</A>";
                }
            }
        }

        #region 判断用户是否可以启动对应的流程模型


        /// <summary>
        /// 判断用户是否可以启动对应的流程模型 两种情况不能启动流程模型： 1、流程模型不是启动状态,返回 -1 2、用户不是流程模型的启动人员 返回 -2
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns>启动状态</returns>
        private int CanUseFlowModel(long lngFlowModelID)
        {
            long lngOFlowModelID = 0;
            long lngNewFlowModelID = 0;
            long lngUserID = long.Parse(Session["UserID"].ToString());

            lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);//原FlowModelID
            lngNewFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID);//获取最新FlowModelID;

            int intCanStart = FlowModel.CanUseFlowModel(lngUserID, lngNewFlowModelID);

            return intCanStart;
        }

        #endregion

        #region 最新知识 ShowNewsInfo
        /// <summary>
        /// 最新知识



        /// </summary>
        private void ShowNewsInfo()
        {
            Inf_InformationDP ee = new Inf_InformationDP();
            DataTable dt = ee.GetNewInf(5);
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                switch (i)
                {
                    case 1:
                        //lblInf1.Text = "当前有" + "<A href=\"javascript:window.open('../Forms/frmContent.aspx','MainFrame');\"><font color=red>" + i.ToString() + "</font></A>" + "条挂起的事项";
                        lblInf1.Text = "<A href='../InformationManager/frmKBShow.aspx?KBID=" + dr["ID"].ToString() + "' target='_blank' style='font-size:10;' title='" + dr["Tags"].ToString() + "'>" + dr["Title"].ToString() + "</A>";
                        lblInf1.ForeColor = System.Drawing.Color.Crimson;
                        break;
                    case 2:
                        lblInf2.Text = "<A href='../InformationManager/frmKBShow.aspx?KBID=" + dr["ID"].ToString() + "' target='_blank' style='font-size:10;' title='" + dr["Tags"].ToString() + "'>" + dr["Title"].ToString() + "</A>";
                        break;
                    case 3:
                        lblInf3.Text = "<A href='../InformationManager/frmKBShow.aspx?KBID=" + dr["ID"].ToString() + "' target='_blank' style='font-size:10;' title='" + dr["Tags"].ToString() + "'>" + dr["Title"].ToString() + "</A>";
                        break;
                    case 4:
                        lblInf4.Text = "<A href='../InformationManager/frmKBShow.aspx?KBID=" + dr["ID"].ToString() + "' target='_blank' style='font-size:10;' title='" + dr["Tags"].ToString() + "'>" + dr["Title"].ToString() + "</A>";
                        break;
                    case 5:
                        lblInf5.Text = "<A href='../InformationManager/frmKBShow.aspx?KBID=" + dr["ID"].ToString() + "' target='_blank' style='font-size:10;' title='" + dr["Tags"].ToString() + "'>" + dr["Title"].ToString() + "</A>";
                        break;
                    default:
                        break;
                }
                i++;
            }
            if (!(dt.Rows.Count > 0))
            {
                lblInf1.Text = "没有最新的知识";
            }
        }
        #endregion

        #region 我的事项 DoDataBind
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void DoDataBind()
        {
            try
            {
                // 待接收事项


                int iRec = ReceiveList.GetReceiveMessageListCount(lngUserID);
                if (iRec > 0)
                {
                    Lab_a.Text = "<A href='../Forms/frmcontent.aspx' target='MainFrame'><font color=red>当前有[" + iRec.ToString() + "]条待接收事项</font></A>";
                    Lab_a.ForeColor = System.Drawing.Color.Crimson;
                    Image1.Visible = true;
                }

                //待办事项
                int iMsgUndo = MessageCollectionDP.GetUndoMessageCount(lngUserID);
                if (iMsgUndo > 0)
                {
                    Lab_b.ForeColor = System.Drawing.Color.Crimson;
                    Lab_b.Text = "<A href='../Forms/frmcontent.aspx' target='MainFrame'><font color=red>当前有[" + iMsgUndo.ToString() + "]条待办事项</font></A>" + "";
                    Image2.Visible = true;
                }

                //阅知事项
                int iMsgRead = MessageCollectionDP.GetUnReadMessageCount(lngUserID);
                if (iMsgRead > 0)
                {
                    Lab_c.Text = "<A href='../Forms/frmcontent.aspx' target='MainFrame'><font color=red>当前有[" + iMsgRead.ToString() + "]条阅知事项</font></A>";
                    Lab_c.ForeColor = System.Drawing.Color.Crimson;
                    Image3.Visible = true;
                }

                //挂起的事项


                int iMsgWaiting = MessageCollectionDP.GetWaitingMessageCount(lngUserID);
                if (iMsgWaiting > 0)
                {
                    Lab_d.Text = "<A href='../Forms/frmcontent.aspx' target='MainFrame'><font color=red>当前有[" + iMsgWaiting.ToString() + "]条挂起的事项</font></A>";
                    Lab_d.ForeColor = System.Drawing.Color.Crimson;
                    Image4.Visible = true;
                }

                //关注事项
                int iAttentions = AttentionDP.GetMyAttentionCount((long)Session["UserID"]);
                if (iAttentions > 0)
                {
                    Lab_e.Text = "<A href='../Forms/frmcontent.aspx' target='MainFrame'><font color=red>当前有[" + iAttentions.ToString() + "]条关注事项</font></A>";
                    Lab_e.ForeColor = System.Drawing.Color.Crimson;
                    Image5.Visible = true;
                }
            }
            catch
            {
                //统一错误展示页面
                throw;
                //Response.Redirect("ErrMessage.aspx?Souce="+e.Source + "&Desc=" + e.ToString());
            }
        }
        #endregion 
    }
}
