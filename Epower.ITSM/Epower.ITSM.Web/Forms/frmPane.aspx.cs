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
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web
{
	/// <summary>
	/// frmPane 的摘要说明。
	/// </summary>
	public partial class frmPane : BasePage
	{

		private long lngUserID = 0;

        bool isLimit = true;


        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.MainID = "1";
            if (Request["Limit"] != null)
            {
                this.Master.ShowBackUrlButton(true);
            }
            else
            {
                this.Master.TableVisible = false;
            }
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            string strUrl = Session["MainUrl"].ToString();
            Response.Redirect(strUrl);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            SetParentButtonEvent();
			lngUserID = long.Parse(Session["UserID"].ToString());

            string strType = "";

           

            if (Request["Limit"] != null)
            {
                if (Request["Limit"].ToString() == "false")
                {
                    isLimit = false;

                }
            }

            if (Request["TypeContent"] != null)
            {
                strType = Request["TypeContent"].ToString();
                Session["MainTypeContent"] = strType;
            }
            else
            {
                strType = Session["MainTypeContent"].ToString();
            }

            

			if(!Page.IsPostBack)
			{
                if (strType == "ggtz")
                {
                    ShowNewsInfo();
                }
                else
                {
                    CtrNewsInfolist1.Visible = false;
                    CtrNewsInfolist2.Visible = false;
                } 
			}
		}

		/// <summary>
		/// 显示公告信息
		/// </summary>
		private void ShowNewsInfo()
		{
			CtrNewsInfolist1.Range=(int)eOA_ReadRange.AllRange;
			CtrNewsInfolist1.OrgID=StringTool.String2Long(Session["UserOrgID"].ToString());
			CtrNewsInfolist1.DeptID=StringTool.String2Long(Session["UserDeptID"].ToString());



            if (isLimit == true)
            {
                CtrNewsInfolist1.sum = 10;
                CtrNewsInfolist1.cnt = 10;
                CtrNewsInfolist1.IsAll = false;
            }
            else
            {
                CtrNewsInfolist1.sum = 100000;
                CtrNewsInfolist1.cnt = 100000;
                CtrNewsInfolist1.IsAll = true;
            }
			

			CtrNewsInfolist2.Range=(int)eOA_ReadRange.OrgRange;

            if (isLimit == true)
            {
                CtrNewsInfolist2.sum = 10;
                CtrNewsInfolist2.cnt = 10;
                CtrNewsInfolist2.IsAll = false;
            }
            else
            {
                CtrNewsInfolist2.sum = 100000;
                CtrNewsInfolist2.cnt = 100000;
                CtrNewsInfolist2.IsAll = true;
            }
			CtrNewsInfolist2.OrgID=StringTool.String2Long(Session["UserOrgID"].ToString());
			CtrNewsInfolist2.DeptID=StringTool.String2Long(Session["UserDeptID"].ToString());
			
			lblOrgName.Text="单位公告:";
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
