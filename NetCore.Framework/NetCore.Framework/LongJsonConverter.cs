using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NetCore.Framework
{
    public class LongJsonConverter : JsonConverter
    {
        public LongJsonConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            // 只处理long和ulong两种类型的数据
            switch (objectType.FullName)
            {
                case "System.Int64":
                case "System.UInt64":
                    return true;
                default:
                    return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // 取得读到的十六进制字符串
            string hex = reader.Value as string;
            // 调用ToInt64扩展将字符串转换成long型
            // ToInt64扩展方法后附
            long v = hex.ToInt64(NumberStyles.HexNumber, 0L);
            // 将v转换成实际需要的类型 ulong 或 long(不转换)
            return typeof(ulong) == objectType ? (object)(ulong)v : v;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // 由于CanConvert过滤，数据类型只可能是long或ulong
            // 统一转换成long类型处理
            long v = value is ulong ? (long)(ulong)value : (long)value;
            writer.WriteValue(v.ToString());
        }

    }

    public static class StringExtention
    {
        public static long ToInt64(this string me, NumberStyles style,
            long defaultValue)
        {
            long? value = me.ToInt64(style,0L);
            return value == null ? defaultValue : value.Value;
        }
    }
}
