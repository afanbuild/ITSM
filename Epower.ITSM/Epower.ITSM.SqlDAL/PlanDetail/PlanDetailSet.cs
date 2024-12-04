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
        private int _Interval = 3600000;   //��С��1Сʱ
            private string _SpecfiedTime = "00:00";
            private string _BeginTime = "00:00";
            private string _EndTime = "23:59";
            private string _Weeks = "12345";  //��һ������
        private string _Day = "1";  //ÿ����һ��
        private string _WeekDay = "1";  //ÿ����һ��

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
                            //��ʼ����ʱ��
                            case "BeginTime":
                                this.BeginTime = sProperty[1];
                                break;
                            //���н�ֹʱ��
                            case "EndTime":
                                this.EndTime = sProperty[1];
                                break;
                            //����ʱ����
                            case "Interval":
                                this.Interval = int.Parse(sProperty[1]);
                                break;
                            //�������
                            case "PeriodType":
                                this.PeriodType = (ePlan_PeriodType)(int.Parse(sProperty[1]));
                                break;
                            //�������
                            case "SetType":
                                this.SetType = (ePlan_SetType)(int.Parse(sProperty[1]));
                                break;
                            //ָ��ʱ��
                            case "SpecfiedTime":
                                this.SpecfiedTime = sProperty[1];
                                break;
                            //��������
                            case "Weeks":
                                this.Weeks = sProperty[1];
                                break;
                            //��ʼ����ʱ��
                            case "Day":
                                this.Day = sProperty[1];
                                break;
                            //���н�ֹʱ��
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
        /// ���������ַ���
        /// </summary>
        /// <returns></returns>
        public string ToSetString()
        {
            string strSet = "";

              //��ʼ����ʱ��
            if(this.BeginTime != "")
            {
                strSet += (strSet == ""?"":";") + "BeginTime=" + this.BeginTime;
            }

            //���н�ֹʱ��
            if(this.EndTime != "")
            {
                strSet += (strSet == ""?"":";") + "EndTime=" + this.EndTime;
            }
            //����ʱ����
            if(this.Interval <=60000)
            {
                strSet += (strSet == ""?"":";") + "Interval=60000";
            }
            else
            {
                strSet += (strSet == ""?"":";") + "Interval=" + this.Interval;
            }
            //�������
            strSet += (strSet == ""?"":";") + "PeriodType=" + ((int)this.PeriodType).ToString();
           

                       
           //�������
            strSet += (strSet == ""?"":";") + "SetType=" + ((int)this.SetType).ToString();
                      
             //ָ��ʱ��
            if(this.SpecfiedTime != "")
            {
                strSet += (strSet == ""?"":";") + "SpecfiedTime=" + this.SpecfiedTime;
            } 
            //��������
            if(this.Weeks != "")
            {
                strSet += (strSet == ""?"":";") + "Weeks=" + this.Weeks;
            }
            //ÿ��������
            if (this.Day != "")
            {
                strSet += (strSet == "" ? "" : ";") + "Day=" + this.Day;
            }
            //ÿ�� ����
            if (this.WeekDay != "")
            {
                strSet += (strSet == "" ? "" : ";") + "WeekDay=" + this.WeekDay;
            }

            return strSet;

        }

        #region ���Զ���

        /// <summary>
            /// ��������("0123456",��һ������)
            /// </summary>
            public string Weeks
            {
                get { return _Weeks; }
                set { _Weeks = value; }
            }

            /// <summary>
            /// �ϴ�����ʱ��
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
            /// �������
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
            /// �������
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
            /// ʱ������û��ָ��ʱȱʡΪ����ʱ����

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
            /// ָ������ʱ�䣬ȱʡ00:00
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
            /// ÿ�տ�ʼ����ʱ��,ȱʡ00:00
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
            /// ÿ�ս�ֹ����ʱ�䣬ȱʡ23:59
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
            /// ����״̬

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
            /// ÿ����һ��
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
            /// ÿ����һ��
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
