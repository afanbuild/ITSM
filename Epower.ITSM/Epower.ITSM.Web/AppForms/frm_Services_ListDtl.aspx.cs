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
    public partial class frm_Services_ListDtl : BasePage
    {
        public static string TemplateId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //加载数据
                LoadData(Request["ServiceLevelID"].ToString());

                Session["FromUrl"] = "../AppForms/frm_Services_ListDtl.aspx?ServiceLevelID=" + Request["ServiceLevelID"].ToString();
                ShowNewsInfo();
            }
        }

        private void LoadData(string strServiceLevelID)
        {
            long lngUserID = long.Parse(Session["UserID"].ToString());
            //获取一级目录信息
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            ee = ee.GetReCorded(long.Parse(strServiceLevelID));

            //获取二级目录
            string sWhere = " And ServiceLevelID= " + strServiceLevelID;
            DataTable dt = ee.GetDataTable(sWhere, "order by EA_ServicesTemplate.Templateid");            

            if (dt.Rows.Count <= 0)
            {
                //如果一级目录是否选择了模板 且 没有子级则直接跳转
                if (ee.OFlowModelID > 0)
                {
                    long lngOFlowModelID2 = FlowDP.GetOFlowModelID(long.Parse(ee.OFlowModelID.ToString()));//原FlowModelID
                    long lngNewFlowModelID2 = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID2);//获取最新FlowModelID; 

                    Response.Redirect("../Forms/OA_AddNew.aspx?flowmodelid=" + lngNewFlowModelID2 + "&ep=" + ee.TemplateID + "&TemplateId=" + TemplateId);
                    Response.End();
                    return;
                }              
            }
            else
            {
                //保存模型Id
                TemplateId = strServiceLevelID;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row2 = dt.Rows[i];
                    sWhere = "and ServiceLevelID=" + row2["TEMPLATEID"].ToString();
                    DataTable dtDtl1 = ee.GetDataTable(sWhere, "order by EA_ServicesTemplate.Templateid");

                    if (dtDtl1.Rows.Count == 0)//没有二级目录时
                    {
                        int intCanStart = CanUseFlowModel(long.Parse(row2["OFlowModelID"].ToString()));
                        if (intCanStart != 0)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    else
                    {

                        int CanStartCount = 0;
                        for (int n = 0; n < dtDtl1.Rows.Count; n++)
                        {
                            DataRow rowDtl = dtDtl1.Rows[n];
                            int intCanStart = CanUseFlowModel(long.Parse(rowDtl["OFlowModelID"].ToString()));
                            if (intCanStart == 0)
                            {
                                CanStartCount++;
                            }
                        }
                        if (CanStartCount == 0)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                }
            }



            dt.AcceptChanges();
            this.tt.DataSource = dt;
            this.tt.DataBind();
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
                // LoadData(sID);
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
    }
}
