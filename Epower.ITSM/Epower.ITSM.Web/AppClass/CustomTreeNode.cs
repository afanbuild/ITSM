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
using System.Text;
using System.Xml;
using System.IO;

namespace Epower.ITSM.Web
{
    /// <summary>
    /// 这个类只用于flow_sender里的树，
    ///  
    /// </summary>
    public class CustomTreeNode : TreeNode
    {
        private string nodeData = string.Empty;
        private string NodeURL = string.Empty;
        private string nodeBllType;

        public string NodeBllType
        {
            get
            {
                return nodeBllType;
            }
            set
            {
                nodeBllType = value;
                //this.Value = value;
            }
        }

        public string NodeData
        {
            set { nodeData = value; }
            get { return nodeData; }
        }



        public CustomTreeNode()
            : base()
        {
            NavigateUrl = "javascript:void(0);";

        }

        public CustomTreeNode(string text, string value, string nodeType)
            : base(text, value)
        {
            this.NodeBllType = nodeBllType;
            NavigateUrl = "javascript:void(0);";

        }

        public void SetNavigateUrl()
        {

        }

        protected override void RenderPostText(HtmlTextWriter writer)
        {
            base.RenderPostText(writer);

        }

        protected override void RenderPreText(HtmlTextWriter writer)
        {            
            base.RenderPreText(writer);
        }




        protected override object SaveViewState()
        {
            object[] arrState = new object[4];
            arrState[0] = base.SaveViewState();

            arrState[1] = this.nodeData;
            arrState[2] = this.NodeURL;
            arrState[3] = this.nodeBllType;

            return arrState;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] arrState = savedState as object[];

                this.nodeData = (string)arrState[1];
                this.NodeURL = (string)arrState[2];
                this.nodeBllType = (string)arrState[3];

                base.LoadViewState(arrState[0]);
            }
        }

    }
}
