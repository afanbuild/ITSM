using System;
using System.Collections.Generic;
using System.Text;
using Epower.ITSM.Base;

namespace Epower.ITSM.SqlDAL
{
    public class PlanDetailSet
    {

        private ePlan_SetType _setType = ePlan_SetType.esst_DayOne;
        private ePlan_PeriodType _PeriodType = ePlan_PeriodType.espt_Duration;
        private int _Interval = 3600000;   //不小于1小时
            private string _SpecfiedTime = "00:00";
            private string _BeginTime = "00:00";
            private string _EndTime = "23:59";
            private string _Weeks = "12345";  //周一至周五
        private string _Day = "1";  //每月哪一天
        private string _WeekDay = "1";  //每周哪一天

            private DateTime _LastRunTime;

        private ePlan_Status _Status = ePlan_Status.ess_Null;

        public PlanDetailSet()
        {
        }

            public PlanDetailSet(string sSet)
            {
                string[] sSets;
                if (sSet.Length > 0)
                {
                    sSets = sSet.Split(";".ToCharArray());

                    for (int i = 0; i < sSets.Length; i++)
                    {
                        string sValue = sSets[i];
                        string[] sProperty = sValue.Split("=".ToCharArray());
                        switch (sProperty[0])
                        {
                            //开始运行时间
                            case "BeginTime":
                                this.BeginTime = sProperty[1];
                                break;
                            //运行截止时间
                            case "EndTime":
                                this.EndTime = sProperty[1];
                                break;
                            //运行时间间隔
                            case "Interval":
                                this.Interval = int.Parse(sProperty[1]);
                                break;
                            //周期类别
                            case "PeriodType":
                                this.PeriodType = (ePlan_PeriodType)(int.Parse(sProperty[1]));
                                break;
                            //服务类别
                            case "SetType":
                                this.SetType = (ePlan_SetType)(int.Parse(sProperty[1]));
                                break;
                            //指定时间
                            case "SpecfiedTime":
                                this.SpecfiedTime = sProperty[1];
                                break;
                            //星期设置
                            case "Weeks":
                                this.Weeks = sProperty[1];
                                break;
                            //开始运行时间
                            case "Day":
                                this.Day = sProperty[1];
                                break;
                            //运行截止时间
                            case "WeekDay":
                                this.WeekDay = sProperty[1];
                                break;
                            default:
                                break;
                        }
                    }
                }

            }


        /// <summary>
        /// 返回设置字符串
        /// </summary>
        /// <returns></returns>
        public string ToSetString()
        {
            string strSet = "";

              //开始运行时间
            if(this.BeginTime != "")
            {
                strSet += (strSet == ""?"":";") + "BeginTime=" + this.BeginTime;
            }

            //运行截止时间
            if(this.EndTime != "")
            {
                strSet += (strSet == ""?"":";") + "EndTime=" + this.EndTime;
            }
            //运行时间间隔
            if(this.Interval <=60000)
            {
                strSet += (strSet == ""?"":";") + "Interval=60000";
            }
            else
            {
                strSet += (strSet == ""?"":";") + "Interval=" + this.Interval;
            }
            //周期类别
            strSet += (strSet == ""?"":";") + "PeriodType=" + ((int)this.PeriodType).ToString();
           

                       
           //服务类别
            strSet += (strSet == ""?"":";") + "SetType=" + ((int)this.SetType).ToString();
                      
             //指定时间
            if(this.SpecfiedTime != "")
            {
                strSet += (strSet == ""?"":";") + "SpecfiedTime=" + this.SpecfiedTime;
            } 
            //星期设置
            if(this.Weeks != "")
            {
                strSet += (strSet == ""?"":";") + "Weeks=" + this.Weeks;
            }
            //每月日设置
            if (this.Day != "")
            {
                strSet += (strSet == "" ? "" : ";") + "Day=" + this.Day;
            }
            //每周 设置
            if (this.WeekDay != "")
            {
                strSet += (strSet == "" ? "" : ";") + "WeekDay=" + this.WeekDay;
            }

            return strSet;

        }

        #region 属性定义

        /// <summary>
            /// 星期设置("0123456",周一至周六)
            /// </summary>
            public string Weeks
            {
                get { return _Weeks; }
                set { _Weeks = value; }
            }

            /// <summary>
            /// 上次运行时间
            /// </summary>
            public DateTime LastRunTime
            {
                get
                {
                    return _LastRunTime;
                }
                set
                {
                    _LastRunTime = value;
                }
            }

            /// <summary>
            /// 设置类别
            /// </summary>
            public ePlan_SetType SetType
            {
                get
                {
                    return _setType;
                }
                set
                {
                    _setType = value;
                }
            }

            /// <summary>
            /// 周期类别
            /// </summary>
            public ePlan_PeriodType PeriodType
            {
                get
                {
                    return _PeriodType;
                }
                set
                {
                    _PeriodType = value;
                }
            }

            /// <summary>
            /// 时间间隔，没有指定时缺省为服务时间间隔

            /// </summary>
            /// <value>value</value>
            public int Interval
            {
                get
                {
                    return _Interval;
                }
                set
                {
                    _Interval = value;
                }
            }

            /// <summary>
            /// 指定运行时间，缺省00:00
            /// </summary>
            public string SpecfiedTime
            {
                get
                {
                    return _SpecfiedTime;
                }
                set
                {
                    _SpecfiedTime = value;
                }
            }

            /// <summary>
            /// 每日开始运行时间,缺省00:00
            /// </summary>
            public string BeginTime
            {
                get
                {
                    return _BeginTime;
                }
                set
                {
                    _BeginTime = value;
                }
            }

            /// <summary>
            /// 每日截止运行时间，缺省23:59
            /// </summary>
            public string EndTime
            {
                get
                {
                    return _EndTime;
                }
                set
                {
                    _EndTime = value;
                }
            }

            /// <summary>
            /// 运行状态

            /// </summary>
            /// <value>_Status</value>
            public ePlan_Status ServiceStatus
            {
                get
                {
                    return _Status;
                }
                set
                {
                    _Status = value;
                }
            }

            /// <summary>
            /// 每月哪一天
            /// </summary>
            public string Day
            {
                get
                {
                    return _Day;
                }
                set
                {
                    _Day = value;
                }
            }


            /// <summary>
            /// 每周哪一天
            /// </summary>
        public string WeekDay
            {
                get
                {
                    return _WeekDay;
                }
                set
                {
                    _WeekDay = value;
                }
            }

        #endregion

        }

        
}
