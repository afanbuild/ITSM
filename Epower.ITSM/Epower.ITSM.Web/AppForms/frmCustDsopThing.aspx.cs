using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using EpowerCom;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmCustDsopThing :  BasePage
    {
        protected string sDate = "";
        protected string sUserID = "0";
        protected string UndoMsg = "0";//待办事项
        protected string ReceiveMsg = "0";//待接收事项
        protected string DealMsg = "0";//处理过的事项
        protected string Attention = "0"; //关注事项
        protected string ReadMsg = "0";//阅知事项
        protected string Attention2 = "0"; //我的消息
        protected string Occasion = "0";//机会处理

        /// <summary>
        /// 
        /// </summary>
        protected string sparams
        {
            get { return new Random().Next().ToString() ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {          
            sUserID = Session["UserID"].ToString();
            int iSize = 10000;
            //待办事项
            UndoMsg = MessageCollectionDP.GetUndoMessage(long.Parse(sUserID.ToString()), iSize).Rows.Count.ToString();
            //待接收事项
            ReceiveMsg = ReceiveList.GetReceiveMessageList(long.Parse(sUserID.ToString()), iSize).Tables[0].Rows.Count.ToString();
            //处理过的事项
            //[<font color='red'><%=DealMsg%></font>]
           // DealMsg = MessageCollectionDP.GetHasDoneMessage(long.Parse(sUserID.ToString()), iSize).Rows.Count.ToString();
            //阅知事项
            ReadMsg = MessageCollectionDP.GetUnReadMessage((long)Session["UserID"], iSize).Rows.Count.ToString();

        }

       
    }
}
