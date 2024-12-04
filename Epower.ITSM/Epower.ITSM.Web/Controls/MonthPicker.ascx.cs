/*******************************************************************
 * 版权所有：
 * Description：月份选择控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
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

namespace Epower.ITSM.Web.Controls
{
    public partial class MonthPicker : System.Web.UI.UserControl
    {
        /// <summary>
        /// 取得或设置月份
        /// </summary>
        public int Month
        {
            get
            {
                return int.Parse(drpMonth.SelectedItem.Value);
            }
            set
            {
                month = value;
                drpMonth.SelectedIndex = drpMonth.Items.IndexOf(drpMonth.Items.FindByValue(month.ToString()));
            }
        }

        /// <summary>
        /// 增加项
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="sValue"></param>
        /// <param name="iIndex"></param>
        public void AddItem(string sText, string sValue, int iIndex)
        {
            drpMonth.Items.Insert(iIndex, new ListItem(sText,sValue));
        }

        private int month = DateTime.Now.Month;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpMonth.SelectedIndex = drpMonth.Items.IndexOf(drpMonth.Items.FindByValue(month.ToString()));
            }
        }

        public void Bind()
        {
            if (null != drpMonth.Items.FindByValue(month.ToString()))
            {
                drpMonth.SelectedItem.Selected = false;
                drpMonth.Items.FindByValue(month.ToString()).Selected = true;
            }
        }
    }
}