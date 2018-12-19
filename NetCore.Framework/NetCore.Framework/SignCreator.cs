using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Linq;

namespace NetCore.Framework
{
    public class SignCreator
    {
        #region 生成签名
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CreateSign(object obj ,string signKey)
        {
            string sign = "";
            try
            {
                if (obj != null)
                {
                    var sortDict = ObjectToSortDictionary(obj)?.Where(p => p.Key != "sign");
                    var signStr = string.Concat(string.Join("|", sortDict.Where(p => p.Value!= null && p.Value.ToString().Trim().Length > 0).Select(p => string.Concat(p.Key.ToString()?.ToLower(),"=",p.Value.ToString()))), "|", signKey);
                    sign = EncryptDecrypt.EncryptMD5(signStr);
                }
                return sign;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion

        #region 取出对象的非空属性放入排序字典中
        /// <summary>
        /// 取出对象的非空属性放入排序字典中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static SortedDictionary<string, object> ObjectToSortDictionary(object obj)
        {
            var sortDict = new SortedDictionary<string, object>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(obj, null);
                if (value != null)
                {
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        sortDict.Add(name, value);
                    }
                    else if (item.PropertyType.IsClass || item.PropertyType.IsArray || item.PropertyType.IsAbstract)
                    {
                        JsonSerializerSettings settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Error,
                            Error = delegate (object objt, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                            {
                                args.ErrorContext.Handled = false;
                            }
                        };
                        settings.Converters.Add(new IsoDateTimeConverter
                        {
                            DateTimeStyles = DateTimeStyles.RoundtripKind,
                            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                        });
                        var jsonValue = JsonConvert.SerializeObject(value)?.ToLower();
                        sortDict.Add(name, value?.ToString());
                    }
                    else
                    {
                        sortDict.Add(name, value?.ToString());
                    }
                }
            }

            return sortDict;
        }
        #endregion
    }
}
