using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    public partial class frm_Hastodoitems : BasePage
    {

        protected string sUserID = "0";     //用户ID
        protected string Issue = "0";       //事件
        protected string Change = "0";      //变更
        protected string Byts = "0";        //问题
        protected string Kb = "0";          //知识
        protected string Release = "0";     //发布
        protected string Other = "0";       //其它     

        #region 界面转换时，用于传递的随机数

        /// <summary>
        /// 界面转换时，用于传递的随机数

        /// </summary>
        protected string sparams
        {
            get { return new Random().Next().ToString(); }
        }
        #endregion

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sUserID = Session["UserID"].ToString();
            }
            frm_HastodoitemsContent_Process_2 cp2 = new frm_HastodoitemsContent_Process_2();

            //事件
            DataTable dt = new DataTable();
            dt = ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 1026, 1);
            dt = cp2.Data_Issue(dt);     
            Issue = dt.Rows.Count.ToString();

            //变更
            DataTable dtchange = ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 420, 1);       
            dtchange = cp2.Data_Change(dtchange);
            Change = dtchange.Rows.Count.ToString();

            //问题
            DataTable dtbyt = new DataTable();
            dtbyt = ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 210, 1);
            Byts = cp2.Data_Byts(dtbyt).Rows.Count.ToString();         

            //知识
            DataTable dtkb = new DataTable();
            dtkb =ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 400, 1);
            Kb =cp2.Data_Kb(dtkb).Rows.Count.ToString();


            Release = ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 1028, 1).ToString();     //发布
            
            //其它
            DataTable dtother = new DataTable();
            dtother = ZHServiceDP.getHastodoitem_FlowWorkNownum(long.Parse(sUserID), 0, 1);
            Other =cp2.Data_Other(dtother).Rows.Count.ToString();        
        }
        #endregion
    }
}
