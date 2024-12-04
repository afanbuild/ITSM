/****************************************************************************
 * 
 * description:知识展示
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-26
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
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.InformationManager
{
    public partial class frmKBShow : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string sKBID
        {
            get
            {
                return Request["KBID"] != null ? Request["KBID"].ToString() : "0";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string UserName
        {
            get
            {
                return Session["PersonName"] != null ? Session["PersonName"].ToString() : string.Empty;
            }
        }

        private decimal KBId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            KBId = decimal.Parse(Request.QueryString["KBID"]);
            Ctrattachment1.ReadOnly = true;
            if (!IsPostBack)
            {
                LoadData();

                this.Master.TableVisible = false;

                BindDataBBS();  //评论

                DealScore();   //评分等级

                ContractInf();   //相关知识

                Inf_InformationDP.AddReadCount(KBId);  //阅读次数
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            
            if (KBId != 0)
            {
                Inf_InformationDP.InsRead(KBId);

                Inf_InformationDP ee = new Inf_InformationDP();
                ee = ee.GetReCorded((long)KBId);

                Ctrattachment1.AttachmentType = eOA_AttachmentType.eKB;
                Ctrattachment1.FlowID = (long)ee.ID;

                this.LblTitle.Text = ee.Title;
                labKey.Text = ee.PKey;
                this.LblWriter.Text = ee.RegUserName;
                this.LblContent.Text = ee.Content;
                //新访问次数
                labReadCount.Text = ee.ReadCount.ToString();    
                //labReadCount.Text = ee.ReadCount.ToString();          //先注释掉：访问次数
                labSource.Text = GetSourceCaption(ee.KBSource,ee.FromID);
                this.LblPubDate.Text = ee.UpdateTime.ToString("yyyy-MM-dd H:mm:ss");

                

                Response.Write("<script language=javascript>document.title='" + this.LblTitle.Text + "';</script>");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kbs"></param>
        /// <param name="fromID"></param>
        /// <returns></returns>
        private string GetSourceCaption(eOA_KBSource kbs,decimal fromID)
        {
            string s = "录入";
            if (kbs == eOA_KBSource.eFromFlow)
            {
                s = "流程归档(<a href='../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + fromID.ToString() + "' target='_blank'><font color=blue>详情</font></a>)";
               // Session["FromUrl"] = "close";
            }
            else if (kbs == eOA_KBSource.eFlow)
            {
                s = "流程审批(<a href='../Forms/frmIssueView.aspx?NewWin=true&FlowID=" + fromID.ToString() + "' target='_blank'><font color=blue>详情</font></a>)";
                //Session["FromUrl"] = "close";
            }
            return s;
        }

        #region  评论处理
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindDataBBS()
        { 
            Inf_BBSDP ee = new Inf_BBSDP();
            string sWhere = " and KBID=" + sKBID;
            string sOrder = " order by RegTime desc";
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            DataList1.DataSource = dt.DefaultView;
            DataList1.DataBind();

            this.lblcount.Text = dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 新增，删除评论
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //删除评论
            if (e.CommandName == "delete")
            {
                Label lblID = (Label)e.Item.FindControl("lblID");
                long lngID = long.Parse(lblID.Text.Trim());
                Inf_BBSDP ee = new Inf_BBSDP();
                ee.DeleteRecorded(lngID);
            }
                //新增评论
            else if (e.CommandName == "add")
            {
                TextBox txtContent = (TextBox)e.Item.FindControl("txtContent");
                Inf_BBSDP ee = new Inf_BBSDP();
                ee.Content = txtContent.Text.Trim();
                ee.KBID = decimal.Parse(sKBID);
                ee.RegTime = DateTime.Now;
                ee.UserID = decimal.Parse(Session["UserID"].ToString());
                ee.UserName = UserName;
                ee.InsertRecorded(ee);
            }

            BindDataBBS();
        }
        
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");
                Label lblUserID = (Label)e.Item.FindControl("lblUserID");
                if (lblUserID.Text.Trim() == Session["UserID"].ToString() || CheckRight(Constant.InfKBQuery))
                {
                    btnDelete.Visible = true;
                }
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        private bool CheckRight(long OperatorID)
        {
            Epower.DevBase.Organization.SqlDAL.RightEntity re = (Epower.DevBase.Organization.SqlDAL.RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }
        #endregion

        #region 评分处理
        /// <summary>
        /// 
        /// </summary>
        protected void DealScore()
        {
            Inf_ScoreDP ee = new Inf_ScoreDP();
            string sWhere = " and KBID=" + sKBID;
            DataTable dt = ee.GetDataTable(sWhere,string.Empty);

            int icount = 0;
            decimal dScore = 0;
            decimal dAvgScore = 0;
            foreach(DataRow dr in dt.Rows)
            {
                dScore += decimal.Parse(dr["Score"].ToString());
                icount++;
            }
            if (dt.Rows.Count > 0)
            {
                dAvgScore = dScore / icount;
                lblScore.Text = dAvgScore.ToString();
            }
            else
            {
                lblScore.Text = "未评分";
            }

            //等级
            string sLevel = "★";
            if (dAvgScore >= 20 && dAvgScore < 40)
                sLevel = "★★";
            else if (dAvgScore >= 40 && dAvgScore < 60)
                sLevel = "★★★";
            else if (dAvgScore >= 60 && dAvgScore < 80)
                sLevel = "★★★★";
            else if (dAvgScore >= 80 && dAvgScore <= 100)
                sLevel = "★★★★★";
            lbllevel.Text = sLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkScore_Click(object sender, EventArgs e)
        {
            DealScore();
        }
        #endregion 

        #region 知识相关
        /// <summary>
        /// 
        /// </summary>
        private void ContractInf()
        {
            Inf_RelDP ee = new Inf_RelDP();
            string sWhere = " and KBID=" + KBId.ToString();
            DataTable dt = ee.GetDataTable(sWhere, string.Empty);
            dgPro_ProblemAnalyse.DataSource = dt.DefaultView;
            dgPro_ProblemAnalyse.DataBind();

            if (dt.Rows.Count == 0)
            {
                Table2.Visible = false;
                Table3.Visible = false;
            }
        }

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
    }
}
