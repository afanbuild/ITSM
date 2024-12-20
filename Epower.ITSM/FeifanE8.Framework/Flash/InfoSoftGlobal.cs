using System;
using System.Collections.Generic;
using System.Text;

namespace FeifanE8.Framework.Flash
{
    /// <summary>
    /// Summary description for FusionCharts.
    /// </summary>
    public class FusionCharts
    {
        /// <summary>
        /// encodes the dataURL before it's served to FusionCharts
        /// If you have parameters in your dataURL, you'll necessarily need to encode it
        /// </summary>
        /// <param name="dataURL">dataURL to be fed to chart</param>
        /// <param name="noCacheStr">Whether to add aditional string to URL to disable caching of data</param>
        /// <returns>Encoded dataURL, ready to be consumed by FusionCharts</returns>
        public static string EncodeDataURL(string dataURL, bool noCacheStr)
        {
            string result = dataURL;
            if (noCacheStr)
            {
                result += (dataURL.IndexOf("?") != -1) ? "&" : "?";
                //Replace : in time with _, as FusionCharts cannot handle : in URLs
                result += "FCCurrTime=" + DateTime.Now.ToString().Replace(":", "_");
            }

            return System.Web.HttpUtility.UrlEncode(result);
        }

        /// <summary>
        /// Generate html code for rendering chart
        /// This function assumes that you've already included the FusionCharts JavaScript class in your page
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <param name="registerWithJS">Whether to ask chart to register itself with JavaScript</param>
        /// /// <param name="transparent">Whether transparent chart (true / false)</param>
        /// <returns>JavaScript + HTML code required to embed a chart</returns>
        private static string RenderChartALL(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("<!-- START Script Block for Chart {0} -->" + Environment.NewLine, chartId);
            builder.AppendFormat("<div id='{0}Div' align='center'>" + Environment.NewLine, chartId);
            builder.Append("Chart." + Environment.NewLine);
            builder.Append("</div>" + Environment.NewLine);
            builder.Append("<script type=\"text/javascript\">" + Environment.NewLine);
            builder.AppendFormat("var chart_{0} = new FusionCharts(\"{1}\", \"{0}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\");" + Environment.NewLine, chartId, chartSWF, chartWidth, chartHeight, boolToNum(debugMode), boolToNum(registerWithJS));
            if (strXML.Length == 0)
            {
                builder.AppendFormat("chart_{0}.setDataURL(\"{1}\");" + Environment.NewLine, chartId, strURL);
            }
            else
            {
                builder.AppendFormat("chart_{0}.setDataXML(\"{1}\");" + Environment.NewLine, chartId, strXML);
            }

            //if (transparent == true)
            {
                builder.AppendFormat("chart_{0}.setTransparent({1});" + Environment.NewLine, chartId, "true");
            }

            builder.AppendFormat("chart_{0}.render(\"{1}Div\");" + Environment.NewLine, chartId, chartId);
            builder.Append("</script>" + Environment.NewLine);
            builder.AppendFormat("<!-- END Script Block for Chart {0} -->" + Environment.NewLine, chartId);
            return builder.ToString();
        }

        /// <summary>
        /// Generate html code for rendering chart
        /// This function assumes that you've already included the FusionCharts JavaScript class in your page
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <param name="registerWithJS">Whether to ask chart to register itself with JavaScript</param>
        /// <param name="transparent">Whether transparent chart (true / false)</param>
        /// <returns>JavaScript + HTML code required to embed a chart</returns>
        public static string RenderChart(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {
            return RenderChartALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, transparent);
        }

        /// <summary>
        /// Generate html code for rendering chart
        /// This function assumes that you've already included the FusionCharts JavaScript class in your page
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <param name="registerWithJS">Whether to ask chart to register itself with JavaScript</param>
        /// <returns>JavaScript + HTML code required to embed a chart</returns>
        public static string RenderChart(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS)
        {
            return RenderChart(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, false);
        }

        /// <summary>
        /// Renders the HTML code for the chart. This
        /// method does NOT embed the chart using JavaScript class. Instead, it uses
        /// direct HTML embedding. So, if you see the charts on IE 6 (or above), you'll
        /// see the "Click to activate..." message on the chart.
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <returns></returns>

        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, false, true);
        }

        /// <summary>
        /// Renders the HTML code for the chart. This
        /// method does NOT embed the chart using JavaScript class. Instead, it uses
        /// direct HTML embedding. So, if you see the charts on IE 6 (or above), you'll
        /// see the "Click to activate..." message on the chart.
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <param name="registerWithJS">Whether to ask chart to register itself with JavaScript</param>
        /// <returns></returns>

        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, true);
        }

        /// <summary>
        /// Renders the HTML code for the chart. This
        /// method does NOT embed the chart using JavaScript class. Instead, it uses
        /// direct HTML embedding. So, if you see the charts on IE 6 (or above), you'll
        /// see the "Click to activate..." message on the chart.
        /// </summary>
        /// <param name="chartSWF">SWF File Name (and Path) of the chart which you intend to plot</param>
        /// <param name="strURL">If you intend to use dataURL method for this chart, pass the URL as this parameter. Else, set it to "" (in case of dataXML method)</param>
        /// <param name="strXML">If you intend to use dataXML method for this chart, pass the XML data as this parameter. Else, set it to "" (in case of dataURL method)</param>
        /// <param name="chartId">Id for the chart, using which it will be recognized in the HTML page. Each chart on the page needs to have a unique Id.</param>
        /// <param name="chartWidth">Intended width for the chart (in pixels)</param>
        /// <param name="chartHeight">Intended height for the chart (in pixels)</param>
        /// <param name="debugMode">Whether to start the chart in debug mode</param>
        /// <param name="registerWithJS">Whether to ask chart to register itself with JavaScript</param>
        /// <param name="transparent">Whether transparent chart (true / false)</param>
        /// <returns></returns>
        public static string RenderChartHTML(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {
            return RenderChartHTMLALL(chartSWF, strURL, strXML, chartId, chartWidth, chartHeight, debugMode, registerWithJS, transparent);
        }

        private static string RenderChartHTMLALL(string chartSWF, string strURL, string strXML, string chartId, string chartWidth, string chartHeight, bool debugMode, bool registerWithJS, bool transparent)
        {
            //Generate the FlashVars string based on whether dataURL has been provided
            //or dataXML.
            StringBuilder strFlashVars = new StringBuilder();
            string flashVariables = String.Empty;
            if (strXML.Length == 0)
            {
                //DataURL Mode
                flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&registerWithJS={3}&DOMId={4}&dataURL={5}", chartWidth, chartHeight, boolToNum(debugMode), (registerWithJS), chartId, strURL);
            }
            else
            //DataXML Mode
            {
                flashVariables = String.Format("&chartWidth={0}&chartHeight={1}&debugMode={2}&registerWithJS={3}&DOMId={4}&dataXML={5}", chartWidth, chartHeight, boolToNum(debugMode), boolToNum(registerWithJS), chartId, strXML);
            }

            strFlashVars.AppendFormat("<!-- START Code Block for Chart {0} -->" + Environment.NewLine, chartId);
            strFlashVars.AppendFormat("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\" width=\"{0}\" height=\"{1}\" name=\"{2}\" id==\"{2}\" >" + Environment.NewLine, chartWidth, chartHeight, chartId);
            
            strFlashVars.Append("<param name=\"allowScriptAccess\" value=\"always\" />" + Environment.NewLine);
            strFlashVars.AppendFormat("<param name=\"movie\" value=\"{0}\"/>" + Environment.NewLine, chartSWF);
            strFlashVars.AppendFormat("<param name=\"FlashVars\" value=\"{0}\" />" + Environment.NewLine, flashVariables);
            strFlashVars.Append("<param name=\"quality\" value=\"high\" />" + Environment.NewLine);


            string strwmode = "";
            //if (transparent == true)
            {
                strFlashVars.Append("<param name=\"wmode\" value=\"transparent\" />" + Environment.NewLine);
                strwmode = "wmode=\"transparent\"";
            }

            strFlashVars.AppendFormat("<embed src=\"{0}\" FlashVars=\"{1}\" quality=\"high\" width=\"{2}\" height=\"{3}\" name=\"{4}\" id=\"{4}\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" {5} />" + Environment.NewLine, chartSWF, flashVariables, chartWidth, chartHeight, chartId, strwmode);
            strFlashVars.Append("</object>" + Environment.NewLine);
            strFlashVars.AppendFormat("<!-- END Code Block for Chart {0} -->" + Environment.NewLine, chartId);
            string FlashXML = "<div id='" + chartId + "Div'>"; FlashXML += strFlashVars.ToString() + "</div>";
            return FlashXML;


        }

        /// <summary>
        /// Transform the meaning of boolean value in integer value
        /// </summary>
        /// <param name="value">true/false value to be transformed</param>
        /// <returns>1 if the value is true, 0 if the value is false</returns>
        private static int boolToNum(bool value)
        {
            return value ? 1 : 0;
        }

    }
}
