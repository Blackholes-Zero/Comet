using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetCore.Framework
{
    public static class EnumExtend
    {
        private static readonly Dictionary<System.Enum, string> enumDescriptions = new Dictionary<System.Enum, string>();
        private static readonly object syncRoot = new object();

        public static string GetDescription(this System.Enum enumSubitem)
        {
            try
            {
                if (enumDescriptions.ContainsKey(enumSubitem))
                {
                    return enumDescriptions[enumSubitem];
                }
                lock (syncRoot)
                {
                    string name = enumSubitem.ToString();
                    object[] customAttributes = enumSubitem.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                        name = attribute.Description;
                    }
                    enumDescriptions[enumSubitem] = name;
                    return name;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return string.Empty;
            }
        }
    }

}
