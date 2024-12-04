using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;

namespace Epower.ITSM.Web.Controls.TimeSelect
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DateTimeDay : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string year = context.Request["years"];
            string monht = context.Request["month"];
            string strType = context.Request["Type"];
            context.Response.Write(getDateTable(year, monht, strType));


        }

        public string getDateTable(string year, string month,string strType)
        {
            year = year.Replace("&nbsp;","").Trim();//去掉2头的空格
            month = month.Replace("&nbsp;", "").Trim();//去掉2头的空格
            StringBuilder xmlDateDay = new StringBuilder();
            xmlDateDay.Append("<table width=\"175px\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >");
            xmlDateDay.Append("<tr  style=\"line-height:20px;background-color:#A3C9E1;\">");
            xmlDateDay.Append("<td  style=\"text-align:center;\">日</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">一</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">二</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">三</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">四</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">五</td>");
            xmlDateDay.Append("<td  style=\"text-align:center;\">六</td>");
            xmlDateDay.Append("<tr>");


            string OnOverNodeTopDay=string.Empty;//鼠标移动事件
            string  OnOutNodeTopDay=string.Empty;//鼠标离开事件
            string OnClickTimeDay=string.Empty;//鼠标点击事件
            string TdId = string.Empty;
            if (strType == "Begin")//时间开始时的鼠标移动效果
            {
                OnOverNodeTopDay = "OnOverNodeTopDayBegin(this)";
                OnOutNodeTopDay = "OnOutNodeTopDayBegin(this)";
                TdId = "TD_DivBegin";
                
            }
            else
            {
                OnOverNodeTopDay = "OnOverNodeTopDayEnd(this)";
                OnOutNodeTopDay = "OnOutNodeTopDayEnd(this)";
                TdId = "TD_DivEnd";
                
            }



            string Days=gettimeTR(year,month);
            string[] DaysDT=Days.Split(',');
            int i = 0;            
            foreach (string day in DaysDT)
            {
                
                if (strType == "Begin")//
                {
                    //鼠标点击事件
                    OnClickTimeDay = "OnClickTimeDayBegin('" + year + "','" + month + "','" + day.ToString() + "',this)";
                }
                else
                {
                    OnClickTimeDay = "OnClickTimeDayEnd('" + year + "','" + month + "','" + day.ToString() + "',this)";
                }

                i++;
                if (i == 1)
                {
                    if (day.Trim().ToString() != "")
                    {

                        xmlDateDay.Append("<tr><td id=\""+TdId+"-" + year + "-" + month + "-" + day.ToString() + "\"  onmouseover=\"" + OnOverNodeTopDay + "\" onmouseout=\"" + OnOutNodeTopDay + "\" class=\"DayStyle1\"  style=\"text-align:center;color:Red;height:25px;\" valign=\"middle\" onclick=\"" + OnClickTimeDay + "\">" + day.ToString() + "</td>");
                    }
                    else
                    {
                        xmlDateDay.Append("<tr><td  style=\"text-align:center;bgcolor:#F5F5F5;\">&nbsp;</td>");
                    }
                    
                }
                else if (i == 7)
                {
                    if (day.Trim().ToString() != "")
                    {
                        xmlDateDay.Append("<td id=\"" + TdId + "-" + year + "-" + month + "-" + day.ToString() + "\"  onmouseover=\"" + OnOverNodeTopDay + "\" onmouseout=\"" + OnOutNodeTopDay + "\" class=\"DayStyle1\"  style=\"text-align:center;color:red;height:25px;\" onclick=\"" + OnClickTimeDay + "\">" + day.ToString() + "</td></tr>");
                    }
                    else
                    {
                        xmlDateDay.Append("<td  style=\"text-align:center;bgcolor:#F5F5F5;\">&nbsp;</td></tr>");
                    }
                    i = 0;
                }
                else
                {
                    if (day.Trim().ToString() != "")
                    {
                        xmlDateDay.Append("<td id=\"" + TdId + "-" + year + "-" + month + "-" + day.ToString() + "\"   onmouseover=\"" + OnOverNodeTopDay + "\" onmouseout=\"" + OnOutNodeTopDay + "\" class=\"DayStyle1\"  style=\"text-align:center;color:Black;height:25px;\"  onclick=\"" + OnClickTimeDay + "\">" + day.ToString() + "</td>");
                    }
                    else
                    {
                        xmlDateDay.Append("<td  style=\"text-align:center;bgcolor:#F5F5F5;\">&nbsp;</td>");
                    }
                }
            }
            

            xmlDateDay.Append("</table>");

            return xmlDateDay.ToString();

        }
        public string gettimeTR(string  year, string month)
        {
            int Days = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
            string str="";
            DateTime DtOne = FirstDayOfMonth(year, month);
            DateTime DtEnd = LastDayOfPrdviousMonth(year, month);
            if (DtOne.DayOfWeek.ToString() == "Sunday")//星期日
            {

            }
            else if (DtOne.DayOfWeek.ToString() == "Monday")//星期一
            {
                str = " ,";
            }
            else if (DtOne.DayOfWeek.ToString() == "Tuesday")//星期二
            {
                str = " ," + " ,";
            }
            else if (DtOne.DayOfWeek.ToString() == "Wednesday")//星期三
            {
                str = " ," + " ," + " ,";
            }
            else if (DtOne.DayOfWeek.ToString() == "Thursday")//星期四
            {
                str = " ," + " ," + " ," + " ,";
            }
            else if (DtOne.DayOfWeek.ToString() == "Friday")//星期五
            {
                str = " ," + " ," + " ," + " ," + " ,";
            }
            else if (DtOne.DayOfWeek.ToString() == "Saturday")//星期六
            {
                str = " ," + " ," + " ," + " ," + " ," + " ,";
            }

            for (int i = 1; i <= Days; i++)
            {
                if (i == Days)
                {
                    if (DtEnd.DayOfWeek.ToString() == "Saturday")//星期日
                    {
                        str += i.ToString() + "";
                    }
                    else
                    {
                        str += i.ToString() + ",";
                    }
                }
                else
                {
                    str += i.ToString() + ",";
                }

            }

            if (DtEnd.DayOfWeek.ToString() == "Sunday")//星期日
            {
                str += " ," + " ," + " ," + " ," + " ," + " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Monday")//星期一
            {
                str += " ," + " ," + " ," + " ," + " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Tuesday")//星期二
            {
                str += " ," + " ," + " ," + " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Wednesday")//星期三
            {
                str += " ," + " ," + " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Thursday")//星期四
            {                
                str += " ," + " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Friday")//星期五
            {
                str += " ";
            }
            else if (DtEnd.DayOfWeek.ToString() == "Saturday")//星期六
            {
            }

            return str;



        }



        //获取月的第一天
        private DateTime FirstDayOfMonth(string year,string month)
        {
            string datatimeNow = year.ToString() + "-" + month.ToString() + "-" + "01";
            DateTime datetime = DateTime.Parse(datatimeNow);
            return datetime.AddDays(1 - datetime.Day);
        }

        //获取月的最后一天
        private DateTime LastDayOfPrdviousMonth(string year, string month)
        {
            string datatimeNow = year.ToString() + "-" + month.ToString() + "-" + "01";

            DateTime datetime = DateTime.Parse(datatimeNow);    
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        #region IHttpHandler 成员
        public bool IsReusable
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        #endregion 

    }
}

