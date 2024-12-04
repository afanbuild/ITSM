/****************************************************************************
 * 
 * description:AJax图片上传控件
 * 
 * 
 * 
 * Create by: yxq
 * Create Date:2011-09-07
 * *************************************************************************/
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

namespace Epower.ITSM.Web.Controls
{
    public partial class browsepic : System.Web.UI.UserControl
    {


        public string ClientID
        {
            get {
                return file1.ClientID.ToString();
            }
        }

        /// <summary>
        /// 返回文件
        /// </summary>
        public HtmlInputFile File
        {
            get { return file1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}