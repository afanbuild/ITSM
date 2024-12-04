/*******************************************************************
 * 版权所有：
 * Description：处理动作按钮
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/

using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerGlobal;
using EpowerCom;

namespace Epower.ITSM.Web.Controls
{
	/// <summary>
	///		CtrActions 的摘要说明。
	/// </summary>
	public partial class CtrActions : System.Web.UI.UserControl
	{

		private long lngFlowModelID = 0;
		private long lngNodeModelID = 0;
		private bool blnReadOnly = false;
		private bool blnIsReader = false;

		#region 属性
		/// <summary>
		/// 流程模型编号
		/// </summary>
		public long FlowModelID
		{
			set{lngFlowModelID = value;}
		}

		/// <summary>
		/// 环节模型编号
		/// </summary>
		public long NodeModelID
		{
			set{lngNodeModelID = value;}
		}

		/// <summary>
		/// 是否只读(只读不显示任何动作)
		/// </summary>
		public bool ReadOnly
		{
			set{blnReadOnly = value;}
		}

		/// <summary>
		/// 是否是阅知或协办 沟通 会签 状态(只显示确定按钮)
		/// </summary>
		public bool IsReader
		{
			set{blnIsReader = value;}
		}

#endregion



		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				
				if(blnReadOnly == true )  // 
				{
					ltlActions.Text = "";
				}
				else 
				{
					//阅读／协办时只显示确定按钮
					LoadActionsHtml();
				}
			}
		}


		private void LoadActionsHtml()
		{
			//DataSet ds1 = FlowModel.GetNodeActions(lngFlowModelID,lngNodeModelID);
            DataRow[] drs = FlowModel.GetNodeActionNew(lngFlowModelID, lngNodeModelID);

			StringBuilder sb = new StringBuilder("");
			string strActionName = "";
			string strActionID = "0";
			int iPecent = 0;
			int iButtLen = 40;   // 8 + 字符数　＊16

            //开头标记
            sb.Append("<TABLE id=\"ctrtabactions\" cellSpacing=\"1\" cellPadding=\"1\" width=\"100%\" border=\"0\"><TR>");

            if (drs == null || drs.Length == 0 || blnIsReader == true)
            {
                //默认一个ＩＤ为０的动作
                sb.Append("<TD align=center width=\"100%\"><INPUT class=\"btnClass3\" id=\"ctrcmdAction0" +
                    "\" onclick=\"DoActionsCtr(0,'确定'" +
                    ");\" type=\"button\" value=\" 确定 " +
                    "\" name=\"ctrcmdAction0" + "\" Height=\"24\"></TD>"
                    );
            }
            else
            {
                iPecent = 100 / (drs.Length);
                foreach (DataRow r in drs)
                {

                    strActionName = r["ActionName"].ToString();
                    strActionID = r["ActionID"].ToString();

                    sb.Append("<TD align=center width=\"" + iPecent.ToString() + "%\"><INPUT class=\"btnClass3\" id=\"ctrcmdAction" + strActionID.Trim() +
                            "\"  onclick=\"DoActionsCtr(" + strActionID.Trim() + ",'" + strActionName + "'" +
                            ");\" type=\"button\" value=\"" + strActionName.Trim() +
                            "\" name=\"ctrcmdAction" + strActionID.Trim() + "\" ></TD>"
                            );
                    //不存在缺省动作的影响了
                }
            }

			//结束标记
			sb.Append("</TR></TABLE>");

			ltlActions.Text = sb.ToString();
		}

		private int GetButtonLength(string strName)
		{
			
			int len = strName.Length;

			return (len==0)?40:(8 + 16 * len);

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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
