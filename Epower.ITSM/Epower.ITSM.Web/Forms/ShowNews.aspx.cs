using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// ShowNews 的摘要说明。
    /// </summary>
    public partial class ShowNews : BasePage
    {
        private long NewsId;
        private long FlowID = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            NewsId = Convert.ToInt32(Request.QueryString["NewsId"]);
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            if (NewsId != 0)
            {
                DataTable dt = NewsDp.GetNewsList(NewsId);
                if (dt.Rows.Count > 0)
                {
                    this.LblTitle.Text = dt.Rows[0]["Title"].ToString();
                    this.LblWriter.Text = dt.Rows[0]["Writer"].ToString();
                    this.LblContent.Text = dt.Rows[0]["Content"].ToString();
                    this.LblPubDate.Text = dt.Rows[0]["PubDate"].ToString();
                    FlowID = CTools.ToInt64(dt.Rows[0]["FlowID"].ToString(), 0);

                    Response.Write("<script language=javascript>document.title='" + this.LblTitle.Text + "';</script>");

                    this.LblIsFile.Text = "附件：";
                    Ctrattachment1.AttachmentType = eOA_AttachmentType.eNormal;
                    Ctrattachment1.FlowID = FlowID;
                    Ctrattachment1.ReadOnly = true;

                    if (dt.Rows[0]["Photo"].ToString() != "")
                    {
                        this.Image1.ImageUrl = @"../Attfiles/photos/" + dt.Rows[0]["Photo"].ToString();
                        this.Image1.Visible = true;
                    }
                    else
                    {
                        this.Image1.Visible = false;
                    }
                }
            }
        }

    }
}
