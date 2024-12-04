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
using Epower.ITSM.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Controls
{
    public partial class UEditor : System.Web.UI.UserControl
    {
        private int _FrameWidth = 800;
        /// <summary>
        /// 获取或设置控件的宽度
        /// </summary>
        public int UEditorFrameWidth
        {
            get { return _FrameWidth; }
            set { _FrameWidth = value; }
        }

        private int _FrameHeight = 320;
        /// <summary>
        /// 获取或设置控件的高度
        /// </summary>
        public int UEditorFrameHeight
        {
            get { return _FrameHeight; }
            set { _FrameHeight = value; }
        }

        private bool _readonly = false;
        /// <summary>
        /// 获取或设置是否只读
        /// </summary>
        public bool UEditorReadOnly
        {
            get { return _readonly; }
            set
            {
                _readonly = value;
            }
        }       
      
        /// <summary>
        /// 给控件 获取或赋值
        /// </summary>
        public string Content
        {
            get { return myEditor.Value; }
            set { myEditor.Value = value; }
        }

        /// <summary>
        /// UEditor的路径
        /// </summary>
        protected string UEditorURL = CommonDP.GetConfigValue("UEditorSet", "UEditorURL");
        /// <summary>
        /// 截图上传时服务器IP或域名
        /// </summary>
        protected string UEditorServer = CommonDP.GetConfigValue("UEditorSet", "UEditorServer");
        /// <summary>
        /// 截图上传时的端口号
        /// </summary>
        protected string UEditorPort = CommonDP.GetConfigValue("UEditorSet", "UEditorPort");

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}