using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace E8ITSM_Phone.Toos
{
    public class JsonToos
    {
        public static JsonSerializer GetJsonSerializer()
        {
            //设置Json序列化格式
            JsonSerializer js = new JsonSerializer();
            //JSON中的Key名称采用驼峰命名法，且首字母小写
            js.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //设置JSON Date类型转换格式
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            js.Converters.Add(timeConverter);
            return js;
        }
    }
}
