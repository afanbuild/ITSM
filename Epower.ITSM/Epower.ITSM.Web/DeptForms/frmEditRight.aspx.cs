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
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmEditRight ��ժҪ˵����
	/// </summary>
	public partial class frmEditRight : BasePage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				if(this.Request.QueryString["rightid"]!=null)
				{
					string sId=this.Request.QueryString["rightid"].ToString();
				
					LoadObjectType_RightRange();
					LoadData(StringTool.String2Long(sId));
				}
			}
		}

		#region ��������[LoadObjectType_RightRange,LoadData]
		/// <summary>
		/// ���ز�����������б�
		/// </summary>
		private void LoadObjectType_RightRange()
		{
			ListItem lt;
			#region �������
			//ListItem lt=new ListItem("","0");
			//dpdObjectType.Items.Add(lt);

			lt=new ListItem("����",((int)eO_RightObject.eDeptRight).ToString());
			dpdObjectType.Items.Add(lt);
				
			lt=new ListItem("��Ա",((int)eO_RightObject.eUserRight).ToString());
			dpdObjectType.Items.Add(lt);

			lt=new ListItem("�û���",((int)eO_RightObject.eActorRight).ToString());
			dpdObjectType.Items.Add(lt);
			#endregion

			#region Ȩ�޷�Χ
			//lt=new ListItem("","0");
			//dpdRightRange.Items.Add(lt);
			
			
			lt=new ListItem("����",((int)eO_RightRange.ePersonal).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("���ڲ���",((int)eO_RightRange.eDeptDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("��������",((int)eO_RightRange.eDept).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("���ڻ���",((int)eO_RightRange.eOrgDirect).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("��������",((int)eO_RightRange.eOrg).ToString());
			dpdRightRange.Items.Add(lt);

			lt=new ListItem("ȫ��",((int)eO_RightRange.eFull).ToString());
			dpdRightRange.Items.Add(lt);


			#endregion
			
		}
		/// <summary>
		/// ����һ����������ϸ����
		/// </summary>
		/// <param name="lngId"></param>
		private void LoadData(long lngId)
		{
			RightEntity re;
			if(lngId==0)
			{
				re=new RightEntity();
			}
			else
			{
				re=new RightEntity(lngId);
			}
			labID.Text=re.RightID.ToString();
			txtObjectId.Text=re.ObjectID.ToString();
			txtObjectName.Text=re.ObjectName ;
			hidDeptList.Value = re.ExtDeptList;
			txtDeptNames.Text = DeptDP.GetDeptListNames(re.ExtDeptList);

			if((int)re.ObjectType==0)
				dpdObjectType.SelectedIndex  =0;
			else
				dpdObjectType.SelectedValue=((int)re.ObjectType).ToString();

			txtOperateId.Text=re.OperateID.ToString();
			txtOperateName.Text=re.OperateName.ToString();
			uRight.RightValue=re.RightValue;

			if((int)re.RightRange==0)
				dpdRightRange.SelectedIndex =0;
			else
				dpdRightRange.SelectedValue=((int)re.RightRange).ToString();
			

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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
            string strRangeID = Session["RangeID"].ToString();

			if(StringTool.String2Long(txtOperateId.Text.ToString()) == 0)
			{
				PageTool.MsgBox(this,"��ѡ�������!");
				return;
			}
			if(StringTool.String2Long(txtObjectId.Text.ToString()) == 0)
			{
				PageTool.MsgBox(this,"��ѡ����Ȩ����!");
				return;
			}
			if(uRight.RightValue == 0)
			{
				PageTool.MsgBox(this,"��ѡ��Ȩ��ֵ!");
				return;
			}
			
			
			RightEntity re=new RightEntity();
			re.ObjectID=StringTool.String2Long(txtObjectId.Text.ToString());
			re.ObjectType=(eO_RightObject)(int.Parse(dpdObjectType.SelectedValue.ToString()));
			re.OperateID=StringTool.String2Long(txtOperateId.Text.ToString());
			re.RightID=StringTool.String2Long(labID.Text.ToString());
			re.RightValue=uRight.RightValue;
			re.RightRange=(eO_RightRange)(int.Parse(dpdRightRange.SelectedValue.ToString()));
			re.ExtDeptList = hidDeptList.Value;
            re.RangeID = strRangeID;
            
			try
			{
				string OtherRightID=RightEntity.Get_RightID(re.OperateID.ToString(),re.ObjectID.ToString(),dpdObjectType.SelectedValue.ToString());
				//if(RightEntity.Is_Record_Exist(re.OperateID.ToString(),re.ObjectID.ToString(),dpdObjectType.SelectedValue.ToString()))
				if(!OtherRightID.Equals("0") && !OtherRightID.Equals(labID.Text.ToString()))
					PageTool.MsgBox(this,"�����ѶԸò������趨Ȩ��,���������!");
				else
				{
					
					
					re.Save();

					long lngUserID = (long)Session["UserID"];
					if(int.Parse(dpdObjectType.SelectedValue.ToString()) == (int)eO_RightObject.eUserRight 
						&& re.ObjectID == lngUserID)
					{
						//������±��˵�Ȩ��,ͬʱ����HEADER
						Session["UserAllRights"] = RightDP.getUserRightTable(lngUserID);
				
						//PageTool.AddJavaScript(this,"window.opener.parent.topFrame.location.reload(); ");
					}
                  //  PageTool.AddJavaScript(this, "window.opener.document.getElementById('cmdAdd').click();");
                    if (Request["TypeFrm"] != null && Request["TypeFrm"] == "frmrightSearch")
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('cmdFind').click();window.close();");
                    }
                    else if (Request["TypeFrm"] != null && Request["TypeFrm"] == "frmright")
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('cmdFind').click();window.close();");
                    }
                    else
                    {
                        PageTool.AddJavaScript(this, "window.opener.location.href=window.opener.location.href;window.opener.document.getElementById('ctrSetUserOtherRight2_dgEA_ExtendRights_ctl01_cmdAdd').click();window.close();");
                    }
				}
			}
			catch(Exception ee)
			{
				PageTool.MsgBox(this,"����Ȩ��ʱ���ִ���,����Ϊ:\n"+ee.Message.ToString());
			}
		}
	}
}
