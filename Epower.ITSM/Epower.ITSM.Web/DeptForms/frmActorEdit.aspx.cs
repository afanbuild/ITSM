/*
 *	by duanqs
 * 
 */
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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmActorEdit ��ժҪ˵����
	/// </summary>
	public partial class frmActorEdit : BasePage
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			CtrTitle.Title="�û���ά��";
			cmdDelete.Attributes.Add("onclick", "if(confirm('�Ƿ����Ҫɾ����')){return true;}else{return false;}");
				
			if(!IsPostBack)
			{
				if(this.Request.QueryString["actorId"]!=null)
				{
					string sActorId=this.Request.QueryString["actorId"].ToString();
					hidActorID.Value=sActorId;
                    txtNo.Text = sActorId;
					if(long.Parse(sActorId) < 10000)
					{
						//ϵͳԤ�����ɫ������ɾ���ͱ༭
						cmdDelete.Enabled = false;
						cmdSave.Enabled = false;
					}
					else
					{
						cmdDelete.Enabled = true;
						cmdSave.Enabled = true;
					}
					LoadData(StringTool.String2Long(sActorId));
				}
                if (Session["UserID"].ToString() != "1")
                {
                    DeptPicker1.ContralState = Epower.ITSM.Base.eOA_FlowControlState.eReadOnly;
                }
			}
		}
        private void LoadData(long lngActorID)
        {
            ActorEntity ae = new ActorEntity(lngActorID);
            txtActorDesc.Text = ae.ActorDesc;
            txtActorName.Text = ae.ActorName;

            hidPActorID.Value = ae.ParentID.ToString();
            txtPActorName.Text = new ActorEntity(ae.ParentID).ActorName;

                long lngDeptID = 0;
                string strDeptName = string.Empty;
                DeptDP.GetDeptIDNameByFullID(ae.RangeID, ref lngDeptID, ref strDeptName);
                DeptPicker1.DeptID = lngDeptID;
                DeptPicker1.DeptName = strDeptName;

        }

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
			ActorEntity ae=new ActorEntity(StringTool.String2Long(hidActorID.Value.ToString()));
			
			ae.ActorName=txtActorName.Text.ToString();
			ae.ActorDesc=txtActorDesc.Text;
			//ae.ParentID=StringTool.String2Int(hidPActorID.Value.ToString());
			ae.ActorID=StringTool.String2Int(hidActorID.Value.ToString());
			
			
			if(ae.ActorName.Trim()=="")
			{
				labMsg.Text="�û������Ʋ���Ϊ��!";
				return;
			}
			else
			{
				labMsg.Text="";
			}

			try
			{
				if(ae.ActorID == 0)
				{
					//����ʱ
					ae.SystemID = (long)Session["SystemID"];
					ae.RangeID = Session["RangeID"].ToString();
				}
                ae.SystemID = (long)Session["SystemID"];
                if (DeptPicker1.DeptID != 0 && DeptPicker1.DeptID!=1)
                {
                    ae.RangeID = DeptDP.GetDeptFullID(DeptPicker1.DeptID);
                    //Session["usergroupdeptid"] = DeptPicker1.DeptID.ToString();
                    //Session["usergroupdeptname"] = DeptPicker1.DeptName.ToString();
                }
				ae.Save();

                HttpRuntime.Cache.Insert("EpCacheValidSysActors", false);

                txtNo.Text = ae.ActorID.ToString();
                PageTool.MsgBox(this, "�û��鱣��ɹ���");
				PageTool.AddJavaScript(this,"window.parent.contents.location='frmActorTree.aspx'");
			}
			catch(Exception ee)
			{
                PageTool.MsgBox(this, "��������ʱ���ִ��󣬴���Ϊ��\\n" + ee.Message.ToString());
			}
		}

		protected void cmdAdd_Click(object sender, System.EventArgs e)
		{
			txtActorDesc.Text="";
			txtActorName.Text="";
			txtPActorName.Text="";
			hidActorID.Value="";
            txtNo.Text = string.Empty;
			hidPActorID.Value="";
			cmdDelete.Enabled = true;
			cmdSave.Enabled = true;
            if (Session["UserID"].ToString() != "1")
            {
                long lngDeptID = 0;
                string strDeptName = string.Empty;
                DeptDP.GetDeptIDNameByFullID(Session["RangeID"].ToString(), ref lngDeptID, ref strDeptName);
                DeptPicker1.DeptID = lngDeptID;
                DeptPicker1.DeptName = strDeptName;
            }
		}

		protected void cmdDelete_Click(object sender, System.EventArgs e)
		{
			ActorEntity ae=new ActorEntity();
			ae.ActorID=StringTool.String2Int(hidActorID.Value.ToString());
			try
			{
               // ActorDP.GetUsersForActor(ae.ActorID);
                DataTable dt = ActorDP.GetActorList(ae.ActorID);
                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        ae.Delete();
                        HttpRuntime.Cache.Insert("EpCacheValidSysActors", false);
                        PageTool.AddJavaScript(this, "window.parent.contents.location='frmActorTree.aspx';window.location='about:blank'");
                        PageTool.MsgBox(this, "�û���ɾ���ɹ�!");
                    }
                    else
                    {
                        PageTool.MsgBox(this, "�û�����������Ա!�޷�ɾ��!");
                    }
                }
			}
			catch(Exception ee)
			{
                PageTool.MsgBox(this, "ɾ���û���ʱ���ִ��󣬴���Ϊ��\\n" + ee.Message.ToString());
			}
		}
	}
}
