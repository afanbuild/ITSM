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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmPlanDetail : BasePage
    {
        string strSet = "";
        PlanDetailSet pds;
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            if (Request.QueryString["planset"] != null)
            {
                strSet = Request.QueryString["planset"];
            }
            pds = new PlanDetailSet(strSet);

            if (!Page.IsPostBack)
            {
                LoadPlanValues();
            }
        }


        /// <summary>
        /// ���ø����尴ť�¼�
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.ShowSaveButton(true);
            this.Master.ShowBackUrlButton(true);
            this.Master.ShowSiteMap(false);
           
        }

        void Master_Master_Button_GoHistory_Click()
        {

            Response.Write("<script>window.returnValue='';self.close();</script>");
        }

        void Master_Master_Button_Save_Click()
        {
            string sSet = GetPlanString();
            Response.Write("<script>window.returnValue='" + sSet + "';self.close();</script>");
        }


        private string GetPlanString()
        {
            PlanDetailSet pds = new PlanDetailSet();
            //���
            pds.SetType = (ePlan_SetType)(int.Parse(rblType.SelectedValue));
            

            //���м��
            pds.Interval = int.Parse(txtInterval.Text) * 3600000;
          

            //ָ������ʱ��
            pds.SpecfiedTime = ddlHours.SelectedItem.Text + ":" + ddlMinutes.SelectedItem.Text;
            
            //ʱ�䷶Χ
            pds.BeginTime = ddlHoursBeg.SelectedItem.Text + ":" + ddlMinutesBeg.SelectedItem.Text;
            pds.EndTime = ddlHoursEnd.SelectedItem.Text + ":" + ddlMinutesEnd.SelectedItem.Text;

            //ָ������(ÿ��ִ��)
            pds.Day = ddlDays.SelectedItem.Value;
            //ָ������(ÿ��ִ��)
            pds.WeekDay = ddlWeeks.SelectedItem.Value;
            

            //����ֵ
            string sWeeks = "";
            if (cblWeeks.Items[0].Selected == true)
            {
                sWeeks += "0";
            }

            if (cblWeeks.Items[1].Selected == true)
            {
                sWeeks += "1";
            }

            if (cblWeeks.Items[2].Selected == true)
            {
                sWeeks += "2";
            }

            if (cblWeeks.Items[3].Selected == true)
            {
                sWeeks += "3";
            }

            if (cblWeeks.Items[4].Selected == true)
            {
                sWeeks += "4";
            }

            if (cblWeeks.Items[5].Selected == true)
            {
                sWeeks += "5";
            }

            if (cblWeeks.Items[6].Selected == true)
            {
                sWeeks += "6";
            }
            pds.Weeks = sWeeks;

            return pds.ToSetString();
            
        }

        private void LoadPlanValues()
        {
            //���
            rblType.SelectedValue = ((int)pds.SetType).ToString();
            
            //���м��
            decimal dm = pds.Interval / 3600000;
            txtInterval.Text = dm.ToString();

            //ָ������ʱ��

            string[] specTime = pds.SpecfiedTime.Split(":".ToCharArray());
            for (int i = 0; i < 24; i++)
            {
                ddlHours.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(specTime[0]))
                {
                    ddlHours.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(specTime[1]))
                {
                    ddlMinutes.SelectedIndex = i;
                }
            }

            //ָ������(ÿ��ִ��)
            for (int i = 0; i < 31; i++)
            {
                ddlDays.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(pds.Day))
                {
                    ddlDays.SelectedIndex = i;
                }
            }

            //ָ������(ÿ��ִ��)
            ddlWeeks.Items.Add(new ListItem("������","0"));
            ddlWeeks.Items.Add(new ListItem("����һ", "1"));
            ddlWeeks.Items.Add(new ListItem("���ڶ�", "2"));
            ddlWeeks.Items.Add(new ListItem("������", "3"));
            ddlWeeks.Items.Add(new ListItem("������", "4"));
            ddlWeeks.Items.Add(new ListItem("������", "5"));
            ddlWeeks.Items.Add(new ListItem("������", "6"));
            ddlWeeks.SelectedIndex = int.Parse(pds.WeekDay);

            //ʱ�䷶Χ
            //���쿪ʼʱ��
            string[] beginTime = pds.BeginTime.Split(":".ToCharArray());

            //�������ʱ��
            string[] endTime = pds.EndTime.Split(":".ToCharArray());

            for (int i = 0; i < 24; i++)
            {
                ddlHoursBeg.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(beginTime[0]))
                {
                    ddlHoursBeg.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutesBeg.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(beginTime[1]))
                {
                    ddlMinutesBeg.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 24; i++)
            {
                ddlHoursEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(endTime[0]))
                {
                    ddlHoursEnd.SelectedIndex = i;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                ddlMinutesEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (i == int.Parse(endTime[1]))
                {
                    ddlMinutesEnd.SelectedIndex = i;
                }
            }

            //����ֵ
            if (pds.Weeks.IndexOf("0") >= 0)
            {
                cblWeeks.Items[0].Selected = true;
            }
            else
            {
                cblWeeks.Items[0].Selected = false;
            }

            if (pds.Weeks.IndexOf("1") >= 0)
            {
                cblWeeks.Items[1].Selected = true;
            }
            else
            {
                cblWeeks.Items[1].Selected = false;
            }

            if (pds.Weeks.IndexOf("2") >= 0)
            {
                cblWeeks.Items[2].Selected = true;
            }
            else
            {
                cblWeeks.Items[2].Selected = false;
            }

            if (pds.Weeks.IndexOf("3") >= 0)
            {
                cblWeeks.Items[3].Selected = true;
            }
            else
            {
                cblWeeks.Items[3].Selected = false;
            }

            if (pds.Weeks.IndexOf("4") >= 0)
            {
                cblWeeks.Items[4].Selected = true;
            }
            else
            {
                cblWeeks.Items[4].Selected = false;
            }

            if (pds.Weeks.IndexOf("5") >= 0)
            {
                cblWeeks.Items[5].Selected = true;
            }
            else
            {
                cblWeeks.Items[5].Selected = false;
            }

            if (pds.Weeks.IndexOf("6") >= 0)
            {
                cblWeeks.Items[6].Selected = true;
            }
            else
            {
                cblWeeks.Items[6].Selected = false;
            }

           

        }
    }
}
