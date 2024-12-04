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
    public partial class CtrEquNature : System.Web.UI.UserControl
    {

        /// <summary>
        /// 资产ID
        /// </summary>
        public string EquId
        {
            set
            {
                HidEquId.Value = value;

                if (!IsPostBack)
                {
                    btn_onclick.Attributes.Add("onclick", "ShowDivEquChoice(" + HidEquId.Value + "," + DivEquNature.ClientID + "," + HidEuqNature.ClientID + "," + txtEquNature.ClientID + ")");
                }
                else
                {
                    if (btn_onclick.Attributes["onclick"] == null || btn_onclick.Attributes["onclick"].ToString() == "")
                    {
                        btn_onclick.Attributes.Add("onclick", "ShowDivEquChoice(" + HidEquId.Value + "," + DivEquNature.ClientID + "," + HidEuqNature.ClientID + "," + txtEquNature.ClientID + ")");
                    }
                }
                // HidEquId.Value = value;
            }
        }

        /// <summary>
        /// 获取显示值
        /// </summary>
        public string Text
        {
            get {
                return txtEquNature.Text;
            }
            set {
                txtEquNature.Text=value;
            }
        }
        /// <summary>
        /// 获取隐藏值
        /// </summary>
        public string Value
        {
            get {
                return HidEuqNature.Value;
            }
            set {
                HidEuqNature.Value=value;
            }
 
        }

        /// <summary>
        /// load 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
  
        }

        
    }
}