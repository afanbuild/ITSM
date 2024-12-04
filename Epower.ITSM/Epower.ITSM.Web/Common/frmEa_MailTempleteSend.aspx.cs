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
using System.Text.RegularExpressions;

namespace Epower.ITSM.Web.Common
{
    public partial class frmEa_MailTempleteSend : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.TableVisible = false;
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
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                Ea_MailTempleteDP ee = new Ea_MailTempleteDP();
                ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));
                txtMailTitle.Text = ee.MailTitle.ToString();
                txtMailBody.Text = ee.MailBody.ToString();
            }
        }
        #endregion

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string sRecs = txtRecs.Text.Trim();
            //¶àÓàµÄ¿Õ¸ñ
            sRecs = Regex.Replace(sRecs, @"\ +", ";");
            sRecs = Regex.Replace(sRecs, @"\,+", ";");
            sRecs = Regex.Replace(sRecs, @"\:+", ";");
            sRecs = Regex.Replace(sRecs, @"\;+", ";");

            txtRecs.Text = sRecs;
          
           MailSendDeal.WebMailSend(sRecs, txtMailTitle.Text.Trim(), txtMailBody.Text);
        }

        protected void txtRecs_TextChanged(object sender, EventArgs e)
        {

        }

      
       

      
       
    }
}
