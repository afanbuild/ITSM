/*********************************
 * 创建人： yanghw
 * 创建时间：2011-08-22
 * 类对象说明：Flash图形化 展示报表说明
 * 
 * 
 * *************************************/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FeifanE8.Framework.Flash;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using System.Collections;
using EpowerCom;

namespace Epower.ITSM.SqlDAL.Flash
{
    /// <summary>
    /// flash 控件类

    /// </summary>
    public class FlashCS
    {
        /// <summary>
        /// 构造函数

        /// </summary>
        public FlashCS()
        {
        }


        #region Flash漏斗图
        /// <summary>
        /// Flash漏斗图 FCF_Funnel.swf
        /// </summary>
        /// <returns></returns>
        public static string loudou()
        {
            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append(" <chart isSliced='1' slicingDistance='4' decimalPrecision='0'>");
            xmlData.Append("<set name='Selected' value='41' color='99CC00' alpha='85' /> ");
            xmlData.Append("<set name='Tested' value='84' color='333333' alpha='85' /> ");
            xmlData.Append("<set name='Interviewed' value='126' color='99CC00' alpha='85' /> ");
            xmlData.Append("<set name='Candidates Applied' value='1800' color='333333' alpha='85' /> ");
            xmlData.Append("</chart>");
            return FusionCharts.RenderChartHTML("Flash/FCF_Funnel.swf", "", xmlData.ToString(), "myNext", "100%", "248", false, false);
        }
        #endregion


        #region flash 图标控件 二维数据结构
        /// <summary>
        /// 二维数据结构 2D柱状图/Column2D.swf、3D柱状图/Column3D.swf、2D饼图/Pie2D.swf、3D饼图/Pie3D.swf
        /// </summary>
        /// <param name="dtSour">数据源</param>
        /// <param name="YFeildName">数据源 类别 绑定字段名称 </param>        
        /// <param name="Title">控件标题</param>
        /// <param name="Yname">纵坐标名称</param>
        /// <param name="Xname">横坐标名称</param>
        /// <param name="Flashurl">Flash路径 可传（[Flash/Column2D.swf]、[Flash/Column3D.swf]、[Flash/Line.swf]、[Flash/Pie2D.swf]、[Flash/Pie3D.swf]）</param>
        /// <param name="width">控件呈现的宽度 （填空 则默认100%）</param>
        /// <param name="Height">控件呈现的高度 （填空 则默认248）</param>
        /// <param name="showBorder">是否显示边框，false 没有边框，true 显示边框 </param>
        /// <param name="decimals">显示值展示的小数位数 ,1则显示1位，2则显示2位数，注:必须为大于或等于0的整数</param>
        /// <returns></returns>
        public static string PublicFlashUrl2D(DataTable dtSour, string YFeildName,string FiledValue, string Title, string Yname, string Xname, string Flashurl, string width, string Height, bool showBorder, int decimals)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12' baseFontSize='12' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'>");

            //转化table 
            if (dtSour.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSour.Rows)
                {
                    xmlData.Append("<set label='" + dr[YFeildName].ToString() + "' value='" + dr[FiledValue].ToString() + "' />");
                }
            }
            else
            {
                xmlData.Append("<set label='' value='0' />");
            }

            xmlData.Append("</chart>");

            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }

            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion 

        #region flash 图标控件 二维数据结构【添加链接参数后】

        /// <summary>
        /// 二维数据结构 2D柱状图/Column2D.swf、3D柱状图/Column3D.swf、2D饼图/Pie2D.swf、3D饼图/Pie3D.swf 
        /// </summary>
        /// <param name="dtSour">数据源</param>
        /// <param name="YFeildName">数据源 类别 绑定字段名称 </param>        
        /// <param name="Title">控件标题</param>
        /// <param name="Yname">纵坐标名称</param>
        /// <param name="Xname">横坐标名称</param>
        /// <param name="Flashurl">Flash路径 可传（[Flash/Column2D.swf]、[Flash/Column3D.swf]、[Flash/Line.swf]、[Flash/Pie2D.swf]、[Flash/Pie3D.swf]）</param>
        /// <param name="width">控件呈现的宽度 （填空 则默认100%）</param>
        /// <param name="Height">控件呈现的高度 （填空 则默认248）</param>
        /// <param name="showBorder">是否显示边框，false 没有边框，true 显示边框 </param>
        /// <param name="decimals">显示值展示的小数位数 ,1则显示1位，2则显示2位数，注:必须为大于或等于0的整数</param>
        /// <param name="strLink">点击柱子后，展示链接的新页面</param>
        /// <returns></returns>
        public static string PublicFlashUrl2D(DataTable dtSour, string YFeildName, string FiledValue, string Title, string Yname, string Xname, string Flashurl, string width, string Height, bool showBorder, int decimals,string strLinkWhere)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12' baseFontSize='12' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF' anchorRadius='3'>");

            //转化table 
            if (dtSour.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSour.Rows)
                {
                    string strWhere = string.Empty;
                    strWhere = dr["Types"].ToString() + "," + strLinkWhere;

                    xmlData.Append("<set label='" + dr[YFeildName].ToString() + "' value='" + dr[FiledValue].ToString() + "'  link='n-frmServiceMonitor_Child.aspx?strArr=" + strWhere + " '/>");
                }
            }
            else
            {
                xmlData.Append("<set label='' value='0'/>");
            }

            xmlData.Append("</chart>");

            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }

            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion

        #region  flash图标控件 多维数据结构
        /// <summary>
        /// 三维数据结构 2D柱状图/MSColumn2D.swf、3D柱状图/MSColumn3D.swf、2D线性图/MSLine.swf、3D线性图/MSColumn3DLineDY.swf
        /// </summary>
        /// <param name="dtSour">数据源</param>
        /// <param name="YFeildName">数据源 类别 绑定字段名称 </param>
        /// <param name="XFeildName">数据源 横坐标（X抽）绑定 字段名称 </param>
        /// <param name="FiledValue">数据源 显示值（value） 绑定字段名称</param>
        /// <param name="Title">控件标题</param>
        /// <param name="Yname">纵坐标名称</param>
        /// <param name="Xname">横坐标名称</param>
        /// <param name="Flashurl">Flash路径 可传（[Flash/MSColumn2D.swf]、[Flash/MSColumn3D.swf]、[Flash/MSColumn3DLineDY.swf]、[Flash/MSLine.swf]）</param>
        /// <param name="width">控件呈现的宽度 （填空 则默认100%）</param>
        /// <param name="Height">控件呈现的高度 （填空 则默认248）</param>
        /// <param name="showBorder">是否显示边框，false 没有边框，true 显示边框 </param>
        /// <param name="decimals">显示值展示的小数位数 ,1则显示1位，2则显示2位数，注:必须为大于或等于0的整数</param>
        /// <returns></returns>
        public static string MSPublicFlashUrl3D(DataTable dtSour, string YFeildName, string XFeildName, string FiledValue, string Title, string Yname, string Xname, string Flashurl,string width,string Height, bool showBorder, int decimals)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;            
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12'  baseFontSize='12' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'><categories>");

            //转化table 
            DataTable dt = rtnTBL(dtSour, YFeildName, XFeildName, FiledValue);

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != YFeildName)
                    xmlData.Append(" <category label='" + dc.ColumnName.ToString() + "'/>");
            }
            xmlData.Append(" </categories>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int countValue = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (countValue == 0)
                        {
                            xmlData.Append(" <dataset seriesName='" + dr[dc.ColumnName.ToString()].ToString() + "'>");
                        }
                        else
                        {
                            string value = dr[dc.ColumnName.ToString()].ToString() == "" ? "0" : dr[dc.ColumnName.ToString()].ToString();
                            xmlData.Append("<set value='" + value + "'/>");
                        }
                        countValue++;
                    }
                    xmlData.Append("</dataset>");

                }
            }
            else
            {
                //当没有数据时的情况处理


                int countValue = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countValue == 0)
                    {
                        xmlData.Append(" <dataset seriesName=''>");
                    }
                    else
                    {
                        xmlData.Append("<set value='" + 0 + "'/>");
                    }
                    countValue++;
                }
                xmlData.Append("</dataset>");
            }
            xmlData.Append("</chart>");


            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }


            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion 

        #region  flash图标控件 多维数据结构
        /// <summary>
        /// 三维数据结构 2D柱状图/MSColumn2D.swf、3D柱状图/MSColumn3D.swf、2D线性图/MSLine.swf、3D线性图/MSColumn3DLineDY.swf  数据显示成百分比 
        /// </summary>
        /// <param name="dtSour">数据源</param>
        /// <param name="YFeildName">数据源 类别 绑定字段名称 </param>
        /// <param name="XFeildName">数据源 横坐标（X抽）绑定 字段名称 </param>
        /// <param name="FiledValue">数据源 显示值（value） 绑定字段名称</param>
        /// <param name="Title">控件标题</param>
        /// <param name="Yname">纵坐标名称</param>
        /// <param name="Xname">横坐标名称</param>
        /// <param name="Flashurl">Flash路径 可传（[Flash/MSColumn2D.swf]、[Flash/MSColumn3D.swf]、[Flash/MSColumn3DLineDY.swf]、[Flash/MSLine.swf]）</param>
        /// <param name="width">控件呈现的宽度 （填空 则默认100%）</param>
        /// <param name="Height">控件呈现的高度 （填空 则默认248）</param>
        /// <param name="showBorder">是否显示边框，false 没有边框，true 显示边框 </param>
        /// <param name="decimals">显示值展示的小数位数 ,1则显示1位，2则显示2位数，注:必须为大于或等于0的整数</param>
        /// <returns></returns>
        public static string MSPublicFlashUrl3DRate(DataTable dtSour, string YFeildName, string XFeildName, string FiledValue, string Title, string Yname, string Xname, string Flashurl, string width, string Height, bool showBorder, int decimals)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12'  baseFontSize='12' numberSuffix='%25' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'><categories>");

            //转化table 
            DataTable dt = rtnTBL(dtSour, YFeildName, XFeildName, FiledValue);

            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != YFeildName)
                    xmlData.Append(" <category label='" + dc.ColumnName.ToString() + "'/>");
            }
            xmlData.Append(" </categories>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int countValue = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (countValue == 0)
                        {
                            xmlData.Append(" <dataset seriesName='" + dr[dc.ColumnName.ToString()].ToString() + "'>");
                        }
                        else
                        {
                            string value = dr[dc.ColumnName.ToString()].ToString() == "" ? "0" : dr[dc.ColumnName.ToString()].ToString();
                            xmlData.Append("<set value='" + value + "'/>");
                        }
                        countValue++;
                    }
                    xmlData.Append("</dataset>");

                }
            }
            else
            {
                //当没有数据时的情况处理



                int countValue = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countValue == 0)
                    {
                        xmlData.Append(" <dataset seriesName=''>");
                    }
                    else
                    {
                        xmlData.Append("<set value='" + 0 + "'/>");
                    }
                    countValue++;
                }
                xmlData.Append("</dataset>");
            }
            xmlData.Append("</chart>");


            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }

            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion 

        #region
        public static string MSPublicFlashUrl3D2(DataTable dtSour, string YFeildName, string XFeildName, string FiledValue, string Title, string Yname, string Flashurl, string width, string Height, bool showBorder, int decimals, string num)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12'  baseFontSize='12' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'><categories>");

            //转化table 
            DataTable dt = rtnTBL1(dtSour, YFeildName, XFeildName, FiledValue, num);
            string[] fieldName = YFeildName.Split(',');
            for (int i = 0; i < dtSour.Rows.Count; i++)
            {
                //  if (dc.ColumnName != YFeildName)
                xmlData.Append(" <category label='" + dtSour.Rows[i][XFeildName].ToString() + "'/>");
            }
            xmlData.Append(" </categories>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int countValue = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (countValue == 0)
                        {
                            xmlData.Append(" <dataset seriesName='" + dr[dc.ColumnName.ToString()].ToString() + "'>");
                        }
                        else
                        {
                            string value = dr[dc.ColumnName.ToString()].ToString() == "" ? "0" : dr[dc.ColumnName.ToString()].ToString();
                            xmlData.Append("<set value='" + value + "'/>");
                        }
                        countValue++;
                    }
                    xmlData.Append("</dataset>");

                }
            }
            else
            {
                //当没有数据时的情况处理



                int countValue = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countValue == 0)
                    {
                        xmlData.Append(" <dataset seriesName=''>");
                    }
                    else
                    {
                        xmlData.Append("<set value='" + 0 + "'/>");
                    }
                    countValue++;
                }
                xmlData.Append("</dataset>");
            }
            xmlData.Append("</chart>");


            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }


            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion

        #region  flash图标控件 多维数据结构
        /// <summary>
        /// 三维数据结构 2D柱状图/MSColumn2D.swf、3D柱状图/MSColumn3D.swf、2D线性图/MSLine.swf、3D线性图/MSColumn3DLineDY.swf
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="YFeildName">数据源 类别 绑定字段名称 </param>        
        /// <param name="Title">控件标题</param>
        /// <param name="Yname">纵坐标名称</param>
        /// <param name="Xname">横坐标名称</param>
        /// <param name="Flashurl">Flash路径 可传（[Flash/MSColumn2D.swf]、[Flash/MSColumn3D.swf]、[Flash/MSColumn3DLineDY.swf]、[Flash/MSLine.swf]）</param>
        /// <param name="width">控件呈现的宽度 （填空 则默认100%）</param>
        /// <param name="Height">控件呈现的高度 （填空 则默认248）</param>
        /// <param name="showBorder">是否显示边框，false 没有边框，true 显示边框 </param>
        /// <param name="decimals">显示值展示的小数位数 ,1则显示1位，2则显示2位数，注:必须为大于或等于0的整数</param>
        /// <returns></returns>
        public static string MSPublicFlashUrl3D(DataTable dt, string YFeildName,  string Title, string Yname, string Xname, string Flashurl, string width, string Height, bool showBorder, int decimals)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            //拼写xml表头
            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' canvasBorderAlpha='30' canvasBorderColor='d0d0d0'  chartRightMargin='50' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12' baseFontSize='12' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'><categories>");


            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != YFeildName)
                    xmlData.Append(" <category label='" + dc.ColumnName.ToString() + "'/>");
            }
            xmlData.Append(" </categories>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int countValue = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (countValue == 0)
                        {
                            xmlData.Append(" <dataset seriesName='" + dr[dc.ColumnName.ToString()].ToString() + "'>");
                        }
                        else
                        {
                            string value = dr[dc.ColumnName.ToString()].ToString() == "" ? "0" : dr[dc.ColumnName.ToString()].ToString();
                            xmlData.Append("<set value='" + value + "'/>");
                        }
                        countValue++;
                    }
                    xmlData.Append("</dataset>");

                }
            }
            else
            {
                //当没有数据时的情况处理



                int countValue = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countValue == 0)
                    {
                        xmlData.Append(" <dataset seriesName=''>");
                    }
                    else
                    {
                        xmlData.Append("<set value='" + 0 + "'/>");
                    }
                    countValue++;
                }
                xmlData.Append("</dataset>");
            }
            xmlData.Append("</chart>");

            //如果传入的宽度和高度伟空时，给出默认值！
            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }

            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        public static string MSPublicFlashUrl3DRate(DataTable dt, string YFeildName, string Title, string Yname, string Xname, string Flashurl, string width, string Height, bool showBorder, int decimals)
        {
            //默认没有边框
            int border = 0;
            if (showBorder)
            {
                border = 1;
            }

            //防止 故意传入小雨0的参数的
            if (decimals < 0)
            {
                decimals = 0;
            }

            //拼写xml表头
            System.Text.StringBuilder xmlData = new System.Text.StringBuilder();
            xmlData.Append("  <chart palette='2' caption='" + Title + "' xAxisName='" + Xname + "' yAxisName='" + Yname + "' showValues='1' decimals='" + decimals.ToString() + "'  outCnvBaseFontSize='12' baseFontSize='12' numberSuffix='%25' formatNumberScale='0' showBorder='" + border.ToString() + "'  bgColor='#FFFFFF'><categories>");


            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName != YFeildName)
                    xmlData.Append(" <category label='" + dc.ColumnName.ToString() + "'/>");
            }
            xmlData.Append(" </categories>");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int countValue = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (countValue == 0)
                        {
                            xmlData.Append(" <dataset seriesName='" + dr[dc.ColumnName.ToString()].ToString() + "'>");
                        }
                        else
                        {
                            string value = dr[dc.ColumnName.ToString()].ToString() == "" ? "0" : dr[dc.ColumnName.ToString()].ToString();
                            xmlData.Append("<set value='" + value + "'/>");
                        }
                        countValue++;
                    }
                    xmlData.Append("</dataset>");

                }
            }
            else
            {
                //当没有数据时的情况处理




                int countValue = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (countValue == 0)
                    {
                        xmlData.Append(" <dataset seriesName=''>");
                    }
                    else
                    {
                        xmlData.Append("<set value='" + 0 + "'/>");
                    }
                    countValue++;
                }
                xmlData.Append("</dataset>");
            }
            xmlData.Append("</chart>");

            //如果传入的宽度和高度伟空时，给出默认值！
            if (width == "")
            {
                width = "100%";
            }
            if (Height == "")
            {
                Height = "248";
            }

            return FusionCharts.RenderChartHTML(Flashurl, "", xmlData.ToString(), "myNext", width, Height, false, false);

        }
        #endregion 

        #region 把table中的“列”转化成 “行”，使得满足flash控件的表结构
        /// <summary>
        /// 把datatable由列转行成行
        /// </summary>
        /// <param name="dt">所传入的datatable</param>
        /// <param name="YFeildname">列的名称</param>
        /// <param name="XFeildName">x被转成横行的字段名称</param>
        /// <param name="FeildValue">显示值value</param>
        /// <returns></returns>
        public static DataTable rtnTBL(DataTable dt ,string YFeildname,string XFeildName,string FeildValue)
        {
            DataTable newTBL = new DataTable();
            newTBL.Columns.Add(YFeildname);
            foreach (DataRow dr in dt.Rows)
            {
                //先判断列名称是否存在
                if (!newTBL.Columns.Contains(dr[XFeildName].ToString().Trim()))
                {                    //不存在则添加一列

                    newTBL.Columns.Add(dr[XFeildName].ToString().Trim());
                }
            }

            Dictionary<string, int> dic = new Dictionary<string, int>();

            int index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (!dic.ContainsKey(dr[YFeildname].ToString().Trim()))
                {
                    dic.Add(dr[YFeildname].ToString().Trim(), index++);

                    DataRow newDr = newTBL.NewRow();
                    newDr[YFeildname] = dr[YFeildname].ToString().Trim();
                    foreach (DataRow dr2 in dt.Select(YFeildname + "=" +StringTool.SqlQ(dr[YFeildname].ToString())))
                    {
                        newDr[dr2[XFeildName].ToString().Trim()] = dr2[FeildValue].ToString().Trim();
                    }
                    newTBL.Rows.Add(newDr);
                }
            }
            return newTBL;
        }
        #endregion 

        #region 把table中的“列”转化成 “行”，使得满足flash控件的表结构1
        /// <summary>
        /// 把datatable由列转行成行
        /// </summary>
        /// <param name="dt">所传入的datatable</param>
        /// <param name="YFeildname">列的名称</param>
        /// <param name="XFeildName">x被转成横行的字段名称</param>
        /// <param name="FeildValue">显示值value</param>
        /// <returns></returns>
        public static DataTable rtnTBL1(DataTable dt, string YFeildname, string XFeildName, string FeildValue, string No)
        {
            DataTable newTBL = new DataTable();
            string[] filedName = YFeildname.Split(',');
            string[] fValue = FeildValue.Split(',');
            newTBL.Columns.Add(filedName[0]);
            foreach (DataRow dr in dt.Rows)
            {
                //先判断列名称是否存在
                if (!newTBL.Columns.Contains(dr[XFeildName].ToString().Trim()))
                {                    //不存在则添加一列


                    newTBL.Columns.Add(dr[XFeildName].ToString().Trim());
                }
            }
            Dictionary<string, int> dic = new Dictionary<string, int>();

            for (int i = 1; i < filedName.Length; i++)
            {
                Dictionary<string, int> dic2 = new Dictionary<string, int>();
                int index = 1;

                DataRow newDr = newTBL.NewRow();
                foreach (DataRow dr in dt.Rows)
                {
                    int num = 1;

                    newDr[filedName[0]] = filedName[i];
                    if (No == "1")
                    {
                        foreach (DataRow dr2 in dt.Select(XFeildName + "=" + StringTool.SqlQ(dr[XFeildName].ToString())))
                        {
                            if (filedName[i] == "处理中数量")
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[0]].ToString().Trim();

                            }
                            if (filedName[i].Contains("未解决数量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[1]].ToString().Trim();

                            }
                            if (filedName[i].Contains("已解决数量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[2]].ToString().Trim();

                            }
                            if (filedName[i].Contains("当月新增数量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[3]].ToString().Trim();

                            }
                            else if (filedName[i].Contains("今日新增数量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[4]].ToString().Trim();

                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr2 in dt.Select(XFeildName + "=" + StringTool.SqlQ(dr[XFeildName].ToString())))
                        {
                            if (filedName[i] == "全年事件量")
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[0]].ToString().Trim();

                            }
                            if (filedName[i].Contains("全年时效"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[1]].ToString().Trim();

                            }
                            if (filedName[i].Contains("当月事件量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[4]].ToString().Trim();

                            }
                            if (filedName[i].Contains("当月时效"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[5]].ToString().Trim();

                            }
                            else if (filedName[i].Contains("当季事件量"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[2]].ToString().Trim();

                            }
                            else if (filedName[i].Contains("当季时效"))
                            {
                                newDr[dr2[XFeildName].ToString().Trim()] = dr2[fValue[3]].ToString().Trim();

                            }
                        }
                    }
                }
                if (!dic.ContainsKey(filedName[i].Trim()))
                {

                    dic.Add(filedName[i].Trim(), index++);

                    newTBL.Rows.Add(newDr);
                }
            }
            return newTBL;
        }
        #endregion 
        
        #region 把table中的“列”转化成 “行”

        /// <summary>
        /// 把datatable由列转行成行
        /// </summary>
        /// <param name="dt">所传入的datatable</param>
        /// <param name="YFeildname">列的名称</param>
        /// <param name="XFeildName">x被转成横行的字段名称</param>
        /// <param name="FeildValue">显示值value</param>
        /// <returns></returns>
        public static DataTable rtnTBL(DataTable dt, string[] YFeildname, string XFeildName, string FeildValue)
        {
            DataTable newTBL = new DataTable();

            #region 拼表头

            //当y轴存在多个的时候，的转化情况              
            foreach (string str in YFeildname)
            {
                newTBL.Columns.Add(str);
            }
            foreach (DataRow dr in dt.Rows)
            { 
                //先判断列名称是否存在
                if (!newTBL.Columns.Contains(dr[XFeildName].ToString().Trim()))
                {                    
                    //不存在则添加一列

                    newTBL.Columns.Add(dr[XFeildName].ToString().Trim());
                }
            }

            #endregion

            #region 得到行数 以及 每列的值

            Dictionary<string, int> dic = new Dictionary<string, int>();

            int index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                DataRow newDr=null;
                
                foreach (string str in YFeildname)
                {   
                    if (!dic.ContainsKey(dr[str].ToString().Trim()))
                    {
                        dic.Add(dr[str].ToString().Trim(), index++);
                        if (newDr == null)
                        {
                            newDr = newTBL.NewRow();
                        }
                    }
                    if (newDr != null)
                    {
                        newDr[str] = dr[str].ToString().Trim();
                    }
                }

                string strWhere = "";
                foreach (string str in YFeildname)
                {
                    if(strWhere.Trim()=="")
                    {
                        strWhere= str + "=" + StringTool.SqlQ(dr[str].ToString());
                    }
                    else
                    {
                        strWhere +=" and " + str + "=" + StringTool.SqlQ(dr[str].ToString());
                    }
                }

                foreach (DataRow dr2 in dt.Select(strWhere))
                {
                    if (newDr!=null)
                    {

                        newDr[dr2[XFeildName].ToString().Trim()] = dr2[FeildValue].ToString().Trim();
                    }
                  
                }

                if (newDr!=null)
                {
                    newTBL.Rows.Add(newDr);
                }


            }
            #endregion
            return newTBL;
        }
        #endregion 

    }
}
