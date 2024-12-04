/*******************************************************************
 * ��Ȩ���У�
 * Description�����������̲�ѯ�ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Xml;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using EpowerGlobal;
	using EpowerCom;
	using Epower.DevBase.BaseTools;
    using Epower.ITSM.SqlDAL;

	/// <summary>
	///		CtrMessageQuery ��ժҪ˵����
	/// </summary>
	public partial class CtrMessageQuery : System.Web.UI.UserControl
	{


		long lngUserDeptID=0;
		long lngUserID=0;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			lngUserDeptID=StringTool.String2Long(Session["UserDeptID"].ToString());
			lngUserID=StringTool.String2Long(Session["UserID"].ToString());

			if(!IsPostBack)
			{
				cboStatus.Items.Add(new ListItem("����״̬","-1"));
				cboStatus.Items.Add(new ListItem("--������",((int)e_MessageStatus.emsHandle).ToString()));
				cboStatus.Items.Add(new ListItem("--��������",((int)e_MessageStatus.emsFinished).ToString()));
				cboStatus.Items.Add(new ListItem("--����",((int)e_MessageStatus.emsWaiting).ToString()));
				cboStatus.Items.Add(new ListItem("--��ͣ",((int)e_MessageStatus.emsStop).ToString()));
				cboStatus.Items[1].Selected=true;

				cboMsgRange.Items.Add(new ListItem("����","0"));
				cboMsgRange.Items.Add(new ListItem("����","1"));
				cboMsgRange.Items.Add(new ListItem("��˾","2"));
				cboMsgRange.Items[0].Selected = true;

                //������ʼ����
                string sQueryBeginDate = "0";
                //if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                //    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    txtMsgDateBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    txtMsgDateBegin.Text = DateTime.Parse(sQueryBeginDate).ToString("yyyy-MM-dd");
                }
				txtMsgDateEnd.Text=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");


				cboApp.DataSource= epApp.GetAllApps().DefaultView;
				cboApp.DataTextField="AppName";
				cboApp.DataValueField="AppID";
				cboApp.DataBind();
				
				ListItem itm=new ListItem("[����Ӧ��]","-1");
				cboApp.Items.Insert(0,itm);
				cboApp.SelectedIndex=0; 

				
			}

		}

		#region GetAllValues

		public XmlDocument GetAllValues()
		{
			XmlDocument xmlDoc = new XmlDocument();
			
			XmlElement xmlRoot = xmlDoc.CreateElement("Fields");

			XmlElement xmlEle;

			#region Status
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","Status");
			if(cboStatus==null || cboStatus.SelectedItem==null)
			{
				xmlEle.SetAttribute("Value",((int)e_MessageStatus.emsHandle).ToString());
			}
			else
			{
				xmlEle.SetAttribute("Value",cboStatus.SelectedItem.Value.ToString());
			}
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region App
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","AppID");
			if(cboApp==null || cboApp.SelectedItem==null)
			{
				xmlEle.SetAttribute("Value","-1");
			}
			else
			{
				xmlEle.SetAttribute("Value",cboApp.SelectedItem.Value.ToString());
			}
			xmlRoot.AppendChild(xmlEle);
			#endregion
			
			#region Subject
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","Subject");
			if (txtSubject==null)
			{
				xmlEle.SetAttribute("Value","");
			}
			else
			{
				xmlEle.SetAttribute("Value",txtSubject.Text);
			}
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region ProcessDate
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","ProcessBegin");
			xmlEle.SetAttribute("Value",txtProcessBegin.Text);
			
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region ProcessEnd
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","ProcessEnd");
			xmlEle.SetAttribute("Value",txtProcessEnd.Text);
			
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region MessageBegin
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","MessageBegin");
			if(txtMsgDateBegin==null || txtMsgDateBegin.Text.Equals(""))
			{
                string sQueryBeginDate = "0";
                if (CommonDP.GetConfigValue("Other", "QueryBeginDate") != null)
                    sQueryBeginDate = CommonDP.GetConfigValue("Other", "QueryBeginDate").ToString();
                if (sQueryBeginDate == "0")
                {
                    xmlEle.SetAttribute("Value", DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));
                }
                else
                {
                    xmlEle.SetAttribute("Value", sQueryBeginDate);
                }
			}
			else
			{
				xmlEle.SetAttribute("Value",txtMsgDateBegin.Text);
			}
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region MessageEnd
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","MessageEnd");
			if(txtMsgDateEnd==null || txtMsgDateEnd.Text.Equals(""))
			{
				xmlEle.SetAttribute("Value",DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
			}
			else
			{
				xmlEle.SetAttribute("Value",txtMsgDateEnd.Text);
			}
			
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region UserID
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","UserID");
			xmlEle.SetAttribute("Value",Session["UserID"].ToString());
			xmlRoot.AppendChild(xmlEle);
			#endregion

			#region UserDeptID
			xmlEle = xmlDoc.CreateElement("Field");
			xmlEle.SetAttribute("FieldName","UserDeptID");
			xmlEle.SetAttribute("Value",Session["UserDeptID"].ToString());
			xmlRoot.AppendChild(xmlEle);
			#endregion

			xmlDoc.AppendChild(xmlRoot);
			return xmlDoc;
		}
		#endregion


		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
